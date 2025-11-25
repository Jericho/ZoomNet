# ZoomNet integration tests

Integration tests allow you to run live tests against the Zoom system to test the ZoomNet library. There are three main scenarios (we call them "suites"):
- General API tests
- Chatbot tests
- WebSocket client tests


## Configuration

Before you can run the integration tests, there are a few settings you must configure in addition to a few environment variables you must set.


### Step 1: configure your proxy tool

```csharp
// Do you want to proxy requests through a tool such as Fiddler? Very useful for debugging.
private static readonly bool _useProxy = true;
```

Line 43 (shown above) in `Program.cs` allows you to indicate whether you want to send all HTTP requests through a proxy running on your machine such as [Telerik's Fiddler](https://www.telerik.com/fiddler) for example. 
I mention Fiddler simply because it's my preferred proxy tool, but feel free to use an alternative tool if you prefer.
By the way, there are two versions of Fiddler available for download on Telerik's web site: "Fiddler Everywhere" and "Fiddler Classic". 
Both are very capable proxys but I personally prefer the classic version, therefore this is the one I recommend.
A proxy tool is very helpful because it allows you to see the content of each request and the content of their corresponding response from the Zoom API. 
This is helpful to investigate situations where you are not getting the result you were expecting.

```csharp
// By default Fiddler Classic uses port 8888 and Fiddler Everywhere uses port 8866
private static readonly int _proxyPort = 8888;
```

Line 46 (shown above) in `Program.cs` allows you to specify the port number used by your proxy tools. For instance, Fiddler classic uses port 8888 by default while Fiddler Everywhere uses 8886 by default.
Most proxys allow you to customize the port number if, for some reason, you are not satisfied with their default. Feel free to customize this port number if desired but make sure to update the `_proxyPort` value accordingly.

>  :warning: You will get a `No connection could be made because the target machine actively refused it` exception when attempting to run the integration tests if you configure `_useProxy` to `true` and your proxy is not running on you machine or the `_proxyPort` value does not correspond to the port used by your proxy. 


### Step 2: configure which test "suite" you want to run

```csharp
// What tests do you want to run
private static readonly TestType _testType = TestType.Api;
```

Line 49 (shown above)  in `Program.cs` allows you to specify which of the test suites you want to run. 
As of this writing, there are three suites to choose from:
- the first only allows you to invoke a multitude of endpoints in the Zoom REST API
- the second one allows you to test API calls that are intended to be invoked by a Chatbot app
- and the third one allows you to receive and parse webhook sent to you by Zoom via websockets.


### Step 3: configure which authentication flow you want to use

```csharp
// Which connection type do you want to use?
private static readonly ConnectionType _connectionType = ConnectionType.OAuthServerToServer;
```

Line 52 (shown above)  in `Program.cs` allows you to specify which authentication flow you want to use. 


### Step 4: environment variables

The ZoomNet integration tests rely on various environment variables to store values that are specific to your environment, such as your client id and client secret for example. The exact list and name of these environment variables vary depending on the authentication flow you selected in "Step 3". The chart below lists all the environment variables for each connection type:

| Connection Type | Environment variables |
|----------|-------------------|
| OAuthAuthorizationCode | ZOOM_OAUTH_CLIENTID<br/>ZOOM_OAUTH_CLIENTSECRET<br/>ZOOM_OAUTH_AUTHORIZATIONCODE<br/>**Note** the authorization code is intended to be used only once therefore the environment variable is cleared after the first use and a refresh token is used subsequently |
| OAuthRefreshToken | ZOOM_OAUTH_CLIENTID<br/>ZOOM_OAUTH_CLIENTSECRET<br/>ZOOM_OAUTH_REFRESHTOKEN<br/>**Note** The refresh token is initially created when an authorization code is used |
| OAuthClientCredentials | ZOOM_OAUTH_CLIENTID<br/>ZOOM_OAUTH_CLIENTSECRET<br/>ZOOM_OAUTH_CLIENTCREDENTIALS_ACCESSTOKEN *(optional)*<br/>**Note** If the access token is omitted, a new one will be requested and the environment variable will be updated accordingly |
| OAuthServerToServer | ZOOM_OAUTH_CLIENTID<br/>ZOOM_OAUTH_CLIENTSECRET<br/>ZOOM_OAUTH_ACCOUNTID<br/>ZOOM_OAUTH_SERVERTOSERVER_ACCESSTOKEN *(optional)*<br/>**Note** If the access token is omitted, a new one will be requested and the environment variable will be updated accordingly |

In addition to the environment variables listed in the table above, there are a few environment variable that are specific to each test suite that you selected in "Step 1":

| Connection Type | Environment variables |
|----------|-------------------|
| Api      | *no additional environment variable necessary* |
| WebSockets | ZOOM_WEBSOCKET_SUBSCRIPTIONID |
| Chatbot | ZOOM_OAUTH_ACCOUNTID<br/>ZOOM_CHATBOT_ROBOTJID (this is your Chatbot app's JID)<br/>ZOOM_CHATBOT_TOJID (this is the JID of the user who will receive the messages sent during the integration tests) |

Here's a convenient sample PowerShell script that demonstrates how to set some of the necessary environment variables:

```powershell
[Environment]::SetEnvironmentVariable("ZOOM_OAUTH_CLIENTID", "<insert your client id>.", "User")
[Environment]::SetEnvironmentVariable("ZOOM_OAUTH_CLIENTSECRET", "<insert your client secret>", "User")
```
