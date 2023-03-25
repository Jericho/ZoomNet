# ZoomNet

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](https://jericho.mit-license.org/)
[![Sourcelink](https://img.shields.io/badge/sourcelink-enabled-brightgreen.svg)](https://github.com/dotnet/sourcelink)

[![Build status](https://ci.appveyor.com/api/projects/status/dak6s7m2b002nuf4/branch/develop?svg=true)](https://ci.appveyor.com/project/Jericho/zoomnet)
[![Tests](https://img.shields.io/appveyor/tests/jericho/zoomnet/master.svg)](https://ci.appveyor.com/project/jericho/zoomnet/build/tests)
[![Coverage Status](https://coveralls.io/repos/github/Jericho/ZoomNet/badge.svg?branch=master)](https://coveralls.io/github/Jericho/ZoomNet?branch=master)
[![CodeFactor](https://www.codefactor.io/repository/github/jericho/zoomnet/badge)](https://www.codefactor.io/repository/github/jericho/zoomnet)

| Release Notes| NuGet (stable) | MyGet (prerelease) |
|--------------|----------------|--------------------|
| [![GitHub release](https://img.shields.io/github/release/jericho/zoomnet.svg)](https://github.com/Jericho/ZoomNet/releases) | [![NuGet Version](https://img.shields.io/nuget/v/ZoomNet.svg)](https://www.nuget.org/packages/ZoomNet/) | [![MyGet Pre Release](https://img.shields.io/myget/jericho/vpre/ZoomNet.svg)](https://myget.org/gallery/jericho) |


## About

The ZoomNet library simplifies connecting with the Zoom.us API and interacting with the various endpoints. Additionaly, the library includes a parser that allows you to process inbound webhook messages sent to you by the Zoom API.


## Installation

The easiest way to include ZoomNet in your C# project is by adding the nuget package to your project:

```
PM> Install-Package ZoomNet
```

## .NET framework suport

ZoomNet currently supports:
- .NET framework 4.8
- any framework supporting `.NET Standard 2.1` (which includes `.NET Core 3.x`  and `ASP.NET Core 3.x`)
- `.NET 6.0`
- `.NET 7.0`

The last version of ZoomNet that supported `.NET 4.6.1`, `.NET 4.7.2` and `.NET Standard 2.0` was 0.35.0


## Usage

### Connection Information
Before you start using the ZoomNet client, you must decide how you are going to connect to the Zoom API. ZoomNet supports three ways of connecting to Zoom: JWT, OAuth and Server-to-Server OAuth.

#### Connection using JWT
This is the simplest way to connect to the Zoom API. Zoom expects you to use a key and a secret to generate a JSON object with a signed payload and to provide this JSON object with every API request. The good news is that ZoomNet takes care of the intricacies of generating this JSON object: you simply provide the key and the secret and ZoomNet takes care of the rest. Super easy!

As the Zoom documentation mentions, this is perfect `if you're looking to build an app that provides server-to-server interaction with Zoom APIs`.

Here is an except from the Zoom documentation that explains [how to get your API key and secret](https://marketplace.zoom.us/docs/guides/auth/jwt#key-secret):

> JWT apps provide an API Key and Secret required to authenticate with JWT. To access the API Key and Secret, Create a JWT App on the Marketplace. After providing basic information about your app, locate your API Key and Secret in the App Credentials page.

When you have the API key and secret, you can instantiate a 'connection info' object like so:
```csharp
var apiKey = "... your API key ...";
var apiSecret = "... your API secret ...";
var connectionInfo = new JwtConnectionInfo(apiKey, apiSecret);
var zoomClient = new ZoomClient(connectionInfo);
```

> **Warning:** <a href="https://marketplace.zoom.us/docs/guides/build/jwt-app/jwt-faq/">Zoom has announced</a> that this authentication method would be obsolete in June 2023. The recommendation is to swith to Server-to-Server OAuth.


#### Connection using OAuth
Using OAuth is much more complicated than using JWT but at the same time, it is more flexible because you can define which permissions your app requires. When a user installs your app, they are presented with the list of permissions your app requires and they are given the opportunity to accept. 

The Zoom documentation has a document about [how to create an OAuth app](https://marketplace.zoom.us/docs/guides/build/oauth-app) and another document about the [OAuth autorization flow](https://marketplace.zoom.us/docs/guides/auth/oauth) but I personnality was very confused by the later document so here is a brief step-by-step summary:
- you create an OAuth app, define which permissions your app requires and publish the app in the Zoom marketplace.
- user installs your app. During installation, user is presentd with a screen listing the permissons your app requires. User must click `accept`.
- Zoom generates an "authorization code". This code can be used only once to generate the first access token and refresh token. I CAN'T STRESS THIS ENOUGH: the authorization code can be used only one time. This was the confusing part to me: somehow I didn't understand that this code could be used only one time and I was attempting to use it repeatedly. Zoom would accept the code the first time and would reject it subsequently, which lead to many hours of frustration while trying to figure out why the code was sometimes rejected.
- The access token is valid for 60 minutes and must therefore be "refreshed" periodically.

When you initially add an OAuth application to your Zoom account, you will be issued an "authorization code".
You can provide this autorization code to ZoomNet like so:
```csharp
var clientId = "... your client ID ...";
var clientSecret = "... your client secret ...";
var authorizationCode = "... the code that Zoom issued when you added the OAuth app to your account ...";
var redirectUri = "... the URI you have configured when setting up your OAuth app ..."; // Please note that Zoom sometimes accepts a null value and sometimes rejects it with a 'Redirect URI mismatch' error
var connectionInfo = OAuthConnectionInfo.WithAuthorizationCode(clientId, clientSecret, authorizationCode,
    (newRefreshToken, newAccessToken) =>
    {
        /*
            This callback is invoked when the authorization code
            is converted into an access token and also when the
            access token is subsequently refreshed.

            You should use this callback to save the refresh token
            to a safe place so you can provide it the next time you
            need to instantiate an OAuthConnectionInfo.
            
            The access token on the other hand does not need to be
            preserved because it is ephemeral (meaning it expires
            after 60 minutes). Even if you preserve it, it is very
            likely to be expired (and therefore useless) before the
            next time you need to instantiate an OAuthConnectionInfo.

            For demonstration purposes, here's how you could use your
            operating system's environment variables to store the token:
        */
        Environment.SetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", newRefreshToken, EnvironmentVariableTarget.User);
    },
    redirectUri);
var zoomClient = new ZoomClient(connectionInfo);
```

> **Warning:** This sample I just provided can be used only when Zoom issues a new the autorization code. ZoomNet will take care of converting this code into an access token at which point the autorization code is no longer valid.

Once the autorization code is converted into an access token and a refresh token, you can instantiate a 'connection info' object like so:
```csharp
var clientId = "... your client ID ...";
var clientSecret = "... your client secret ...";
var refreshToken = Environment.GetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", EnvironmentVariableTarget.User);
var connectionInfo = OAuthConnectionInfo.WithRefreshToken(clientId, clientSecret, refreshToken,
    (newRefreshToken, newAccessToken) =>
    {
        /*
            As previously stated, it's important to preserve the refresh token.
        */
        Environment.SetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", newRefreshToken, EnvironmentVariableTarget.User);
    });
var zoomClient = new ZoomClient(connectionInfo);
```

#### Connection using Server-to-Server OAuth

This authentication method is the replacement for JWT authentication which Zoom announced will be made obsolete in June 2023.

From Zoom's documentation:
> A Server-to-Server OAuth app enables you to securely integrate with Zoom APIs and get your account owner access token without user interaction. This is different from the OAuth app type, which requires user authentication. See Using OAuth 2.0 for details.

ZoomNet takes care of getting a new access token and it also refreshes a previously issued token when it expires (Server-to-Server access token are valid for one hour).

```csharp
var clientId = "... your client ID ...";
var clientSecret = "... your client secret ...";
var accountId = "... your account id ...";
var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId,
    (_, newAccessToken) =>
    {
        /*
            Server-to-Server OAuth does not use a refresh token. That's why I used '_' as the first parameter
            in this delegate declaration. Furthermore, ZoomNet will take care of getting a new access token
            and to refresh it whenever it expires therefore there is no need for you to preserve it.

            In fact, this delegate is completely optional when using Server-to-Server OAuth. Feel free to pass
            a null value in lieu of a delegate.
        */
    });
var zoomClient = new ZoomClient(connectionInfo);
```

The delegate being optional in the server-to-server scenario you can therefore simplify the connection info declaration like so:

```csharp
var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);
var zoomClient = new ZoomClient(connectionInfo);
```

#### Mutliple instances of your application in Server-to-Server OAuth scenarios

One important detail about Server-to-Server OAuth which is not widely known is that requesting a new token automatically invalidates a previously issued token EVEN THOUGH IT HASN'T REACHED ITS EXPIRATION DATE/TIME. This will affect you if you have multiple instances of your application running at the same time. To illustrate what this means, let's say that you have two instances of your application running at the same time. What is going to happen is that instance number 1 will request a new token which it will successfully use for some time until instance number 2 requests its own token. When this second token is issued, the token for instance 1 is invalidated which will cause instance 1 to request a new token. This new token will invalidate token number 2 which will cause instance 2 to request a new token, and so on. As you can see, instance 1 and 2 are fighting each other for a token.

There are a few ways you can overcome this problem:

Solution number 1:
You can create mutiple OAuth apps in Zoom's management dashboard, one for each instance of your app. This means that each instance will have their own clientId, clientSecret and accountId and therefore they can independently request tokens without interfering with each other.

This puts the onus is on you to create and manage these Zoom apps. Additionally, you are responsible for ensuring that the `OAuthConnectionInfo` in your C# code is initialized with the appropriate values for each instance.

This is a simple and effective solution when you have a relatively small number of instances but, in my opinion, it becomes overwhelming when your number of instances becomes too large.


Solution number 2:
Create a single Zoom OAuth app. Contact Zoom support and request additional "token indices" (also known as "group numbers") for this OAuth app. Subsequently, new tokens can be "scoped" to a given index which means that a token issued for a specific index does not invalidate token for any other index. Hopefully, Zoom will grant you enough token indices and you will be able to dedicate one index for each instance of your application and you can subsequently modify your C# code to "scope"" you OAuth connection to a desired index, like so:

```csharp
// you initialize the connection info for your first instance like this:
var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId, 0);

// for your second instance, like this:
var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId, 1);

// instance number 3:
var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId, 2);

... and so on ...
```

Just like solution number 1, this solution works well for scenarios where you have a relatively small number of instances and where Zoom has granted you enough indices.


### Webhook Parser
 
Here's a basic example of a .net 6.0 API controller which parses the webhook from Zoom:
```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoomWebhooksController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ReceiveEvent()
        {
            var parser = new ZoomNet.WebhookParser();
            var event = await parser.ParseEventWebhookAsync(Request.Body).ConfigureAwait(false);

            // ... do something with the event ...

            return Ok();
        }
    }
}
```

### Ensuring that webhooks originated from Zoom before parsing

It is possible for you to verify that a webhook is legitimate and originated from Zoom.
Any webhook that fails to be verified should be considered suspicious and should be discarded.

To get started, you need to make sure that a `Secret Token` is associated with your Zoom Marketplace app (click the 'Regenerate' button if your app doesn't have such a token already) as demonstrated in this screenshot.
![Screenshot](https://user-images.githubusercontent.com/112710/187087437-277e1e6f-e0c9-4046-b368-45e6612a781c.png)

When your Marketplace app has a `Secret Token`, Zoom will include two additional headers in the request posted to your endpoint and you must use the values in these headers to verify that the content your received is legitimate.
If you want to know what to do with these two values to determine if a webhook is legitimate or not, please review [this page in the documentation](https://marketplace.zoom.us/docs/api-reference/webhook-reference/#verify-webhook-events).
But, ZoomNet strives to make your life easier so we have already implemented this logic.

The `WebhookParser` class has a method called `VerifyAndParseEventWebhookAsync`which will automatically verify the data and throw a security exception if verification fails. If the verification fails, you should consider the webhook data to be invalid. Here's how it works:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoomWebhookController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ReceiveEvent()
        {
            // Your webhook app's secret token
            var secretToken = "... your app's secret token ...";

            // Get the signature and the timestamp from the request headers
            // SIGNATURE_HEADER_NAME and TIMESTAMP_HEADER_NAME are two convenient constants provided by ZoomNet so you don't have to remember the actual names of the headers
            var signature = Request.Headers[ZoomNet.WebhookParser.SIGNATURE_HEADER_NAME].SingleOrDefault();
            var timestamp = Request.Headers[ZoomNet.WebhookParser.TIMESTAMP_HEADER_NAME].SingleOrDefault();

            var parser = new ZoomNet.WebhookParser();

            // The signature will be automatically validated and a security exception thrown if unable to validate
            var zoomEvent = await parser.VerifyAndParseEventWebhookAsync(Request.Body, secretToken, signature, timestamp).ConfigureAwait(false);

            // ... do something with the event...

            return Ok();
        }
    }
}
```

### Responding to requests from Zoom to validate your webhook endpoint

When you initially configure the URL where you want Zoom to post the webhooks, Zoom will send a request to this URL and you are expected to respond to this validation challenge in a way that can be validated by the Zoom API. Zoom calls this a "Challenge-Response check (CRC)". Assuming this initial validation is successful, the Zoom API will repeat this validation process every 72 hours. You can of course manually craft this reponse by following Zoom's [instructions](https://marketplace.zoom.us/docs/api-reference/webhook-reference/#validate-your-webhook-endpoint).
However, if you want to avoid learning the intricacies of the reponse expected by Zoom and you simply want this response to be conveniently generated for you, ZoomNet can help! 
The `EndpointUrlValidationEvent` class has a method called `GenerateUrlValidationResponse` which will generate the string that you must include in your HTTP200 response.
Here's how it works:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoomWebhooksController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ReceiveEvent()
        {
            // Your webhook app's secret token
            var secretToken = "... your app's secret token ...";

            var parser = new ZoomNet.WebhookParser();
            var event = await parser.ParseEventWebhookAsync(Request.Body).ConfigureAwait(false);

            var endpointUrlValidationEvent = zoomEvent as EndpointUrlValidationEvent;
            var responsePayload = endpointUrlValidationEvent.GenerateUrlValidationResponse(secretToken);
            return Ok(responsePayload);
        }
    }
}
```

### The ultimate webhook controller

Here's the "ultimate" webhook controller which combines all the above features:

```csharp
using Microsoft.AspNetCore.Mvc;

namespace WebApplication1.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ZoomWebhooksController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> ReceiveEvent()
        {
            // Your webhook app's secret token
            var secretToken = "... your app's secret token ...";

            // SIGNATURE_HEADER_NAME and TIMESTAMP_HEADER_NAME are two convenient constants provided by ZoomNet so you don't have to remember the actual name of the headers
            var signature = Request.Headers[ZoomNet.WebhookParser.SIGNATURE_HEADER_NAME].SingleOrDefault();
            var timestamp = Request.Headers[ZoomNet.WebhookParser.TIMESTAMP_HEADER_NAME].SingleOrDefault();

            var parser = new ZoomNet.WebhookParser();
            Event zoomEvent;

            if (!string.IsNullOrEmpty(signature) && !string.IsNullOrEmpty(timestamp))
            {
                try
                {
                    zoomEvent = await parser.VerifyAndParseEventWebhookAsync(Request.Body, secretToken, signature, timestamp).ConfigureAwait(false);
                }
                catch (SecurityException e)
                {
                    // Unable to validate the data. Therefore you should consider the request as suspicious
                    throw;
                }
            }
            else
            {
                zoomEvent = await parser.ParseEventWebhookAsync(Request.Body).ConfigureAwait(false);
            }

            if (zoomEvent.EventType == EventType.EndpointUrlValidation)
            {
                // It's important to include the payload along with your HTTP200 response. This is how you let Zoom know that your URL is valid
                var endpointUrlValidationEvent = zoomEvent as EndpointUrlValidationEvent;
                var responsePayload = endpointUrlValidationEvent.GenerateUrlValidationResponse(secretToken);
                return Ok(responsePayload);
            }
            else
            {
                // ... do something with the event ...

                return Ok();
            }
        }
    }
}
```

### Webhooks over websockets

As of this writing (October 2022), webhooks over websocket is in public beta testing and you can signup if you want to participate in the beta (see [here](https://marketplace.zoom.us/docs/api-reference/websockets/)). 

ZoomNet offers a convenient client to receive and process webhooks events received over a websocket connection. This websocket client will automatically manage the connection, ensuring it is re-established if it's closed for some reason. Additionaly, it will manage the OAuth token and will automatically refresh it when it expires.

Here's how to use it in a C# console application:

```csharp
using System.Net;
using ZoomNet;
using ZoomNet.Models.Webhooks;

var clientId = "... your client id ...";
var clientSecret = "... your client secret ...";
var accountId = "... your account id ...";
var subscriptionId = "... your subscription id ..."; // See instructions below how to get this value

// This is the async delegate that gets invoked when a webhook event is received
var eventProcessor = new Func<Event, CancellationToken, Task>(async (webhookEvent, cancellationToken) =>
{
    if (!cancellationToken.IsCancellationRequested)
    {
        // Add your custom logic to process this event
    }
});

// Configure cancellation (this allows you to press CTRL+C or CTRL+Break to stop the websocket client)
var cts = new CancellationTokenSource();
var exitEvent = new ManualResetEvent(false);
Console.CancelKeyPress += (s, e) =>
{
    e.Cancel = true;
    cts.Cancel();
    exitEvent.Set();
};

// Start the websocket client
var connectionInfo = OAuthConnectionInfo.ForServerToServer(clientId, clientSecret, accountId);
using (var client = new ZoomWebSocketClient(connectionInfo, subscriptionId, eventProcessor, proxy, logger))
{
    await client.StartAsync(cts.Token).ConfigureAwait(false);
    exitEvent.WaitOne();
}
```

#### How to get your websocket subscription id

When you configure your webhook over websocket in the Zoom Marketplace, Zoom will generate a URL like you can see in this screenshot:

![Screenshot](https://user-images.githubusercontent.com/112710/196733937-7813abdd-9cb5-4a35-ad69-d5f6ac9676e4.png)

Your subscription Id is the last part of the URL. In the example above, the generated URL is similar to `wss://api.zoom.us/v2/webhooks/events?subscription_id=1234567890` and therefore the subscription id is `1234567890`.