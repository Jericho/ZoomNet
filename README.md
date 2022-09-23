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


## Installation

The easiest way to include ZoomNet in your C# project is by adding the nuget package to your project:

```
PM> Install-Package ZoomNet
```

## .NET framework suport

ZoomNet currently supports:
- .NET framework 4.8
- any framework supporting `.NET Standard 2.1` (which includes `.NET Core 3.x`  and `ASP.NET Core 3.x`)
- `.NET 5.0`
- `.NET 6.0`

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
- Zoom generates a "authorization code". This code can be used only once to generate the first access token and refresh token. I CAN'T STRESS THIS ENOUGH: the authorization code can be used only one time. This was the confusing part to me: somehow I didn't understand that this code could be used only one time and I was attempting to use it repeatedly. Zoom would accept the code the first time and would reject it subsequently, which lead to many hours of frustration while trying to figure out why the code was sometimes rejected.
- The access token is valid for 60 minutes and must therefore be "refreshed" periodically.

When you initially add an OAuth application to your Zoom account, you will be issued an "authorization code".
You can provide this autorization code to ZoomNet like so:
```csharp
var clientId = "... your client ID ...";
var clientSecret = "... your client secret ...";
var refreshToken = "... the refresh token previously issued by Zoom ...";
var authorizationCode = "... the code that Zoom issued when you added the OAuth app to your account ...";
var redirectUri = "... the URI you have configured when setting up your OAuth app ..."; // Please note that Zoom sometimes accepts a null value and sometimes rejects it with a 'Redirect URI mismatch' error
var connectionInfo = new OAuthConnectionInfo(clientId, clientSecret, authorizationCode,
    (newRefreshToken, newAccessToken) =>
    {
        /*
            This callback is invoked when the authorization code
            is converted into an access token and also when the
            access token is subsequently refreshed.

            You should use this callback to save the two new tokens
            to a safe place so you can provide them the next time you
            need to instantiate an OAuthConnectionInfo.

            For demonstration purposes, here's how you could use your
            operating system's environment variables to store the tokens:
        */
        Environment.SetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", newRefreshToken, EnvironmentVariableTarget.User);
        Environment.SetEnvironmentVariable("ZOOM_OAUTH_ACCESSTOKEN", newAccessToken, EnvironmentVariableTarget.User);
    },
    redirectUri);
var zoomClient = new ZoomClient(connectionInfo);
```

> **Warning:** This sample I just provided can be used only when Zoom issues a new the autorization code. ZoomNet will take care of converting this code into an access token at which point the autorization code is no longer valid.

Once the autorization code is converted into an access token, you can instantiate a 'connection info' object like so:
```csharp
var clientId = "... your client ID ...";
var clientSecret = "... your client secret ...";
var refreshToken = Environment.GetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", EnvironmentVariableTarget.User);
var accessToken = Environment.GetEnvironmentVariable("ZOOM_OAUTH_ACCESSTOKEN", EnvironmentVariableTarget.User);
var connectionInfo = new OAuthConnectionInfo(clientId, clientSecret, refreshToken, accessToken,
    (newRefreshToken, newAccessToken) =>
    {
        Environment.SetEnvironmentVariable("ZOOM_OAUTH_REFRESHTOKEN", newRefreshToken, EnvironmentVariableTarget.User);
        Environment.SetEnvironmentVariable("ZOOM_OAUTH_ACCESSTOKEN", newAccessToken, EnvironmentVariableTarget.User);
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
var connectionInfo = new OAuthConnectionInfo(clientId, clientSecret, accountId,
	(_, newAccessToken) =>
	{
        /*
            Server-to-Server OAuth does not use a refresh token. That's why I used '_' as the first parameter
            in this delegate declaration. Furthermore, ZoomNet will take care of getting a new access token
            and to refresh it whenever it expires therefore there is no need for you to preserve the token
            like you must do for the 'standard' OAuth authentication.

            In fact, this delegate is completely optional when using Server-to-Server OAuth. Feel free to pass
            a null value in lieu of a delegate.
        */
	});
var zoomClient = new ZoomClient(connectionInfo);
```

The delegate being optional in the server-to-server scenario you can therefore simplify the connection info declaration like so:

```csharp
var connectionInfo = new OAuthConnectionInfo(clientId, clientSecret, accountId, null);
var zoomClient = new ZoomClient(connectionInfo);
```

### Webhook Parser
 
Here's a basic example of a .net 6.0 API controller which parses the webhook from Zoom:
```csharp
using Microsoft.AspNetCore.Mvc;
using StrongGrid;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ZoomWebhooksController : ControllerBase
	{
		[HttpPost]
		[Route("Event")]
		public async Task<IActionResult> ReceiveEvent()
		{
			var parser = new WebhookParser();
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

However, if you want to avoid learning how to perform the validation and you simply want this validation to be conveniently performed for you, ZoomNet can help! The `WebhookParser` class has a method called `VerifyAndParseEventWebhookAsync`which will automatically verify the data and throw a security exception if verification fails. If the verification fails, you should consider the webhook data to be invalid. Here's how it works:

```csharp
using Microsoft.AspNetCore.Mvc;
using StrongGrid;
using System.Security;

namespace WebApplication1.Controllers
{
	[Route("api/[controller]")]
	[ApiController]
	public class ZoomWebhookController : ControllerBase
	{
		[HttpPost]
		public async Task<IActionResult> ReceiveEvent()
		{
			try
			{
				// Get your secret token
				var secretToken = "... your app's secret token ...";

				// Get the signature and the timestamp from the request headers
				var signature = Request.Headers[WebhookParser.SIGNATURE_HEADER_NAME].SingleOrDefault(); // SIGNATURE_HEADER_NAME is a convenient constant provided by ZoomNet so you don't have to remember the name of the header
				var timestamp = Request.Headers[WebhookParser.TIMESTAMP_HEADER_NAME].SingleOrDefault(); // TIMESTAMP_HEADER_NAME is a convenient constant provided by ZoomNet so you don't have to remember the name of the header

				// Parse the event. The signature will be automatically validated and a security exception thrown if unable to validate
				var parser = new WebhookParser();
				var event = await parser.VerifyAndParseEventWebhook(Request.Body, secretToken, signature, timestamp).ConfigureAwait(false);

				// ... do something with the event...
			}
			catch (SecurityException e)
			{
				// ... unable to validate the data ...
			}

			return Ok();
		}
	}
}
```