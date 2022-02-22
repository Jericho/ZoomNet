# ZoomNet

[![License](https://img.shields.io/badge/license-MIT-blue.svg)](http://jericho.mit-license.org/)
[![Sourcelink](https://img.shields.io/badge/sourcelink-enabled-brightgreen.svg)](https://github.com/dotnet/sourcelink)

[![Build status](https://ci.appveyor.com/api/projects/status/dak6s7m2b002nuf4/branch/develop?svg=true)](https://ci.appveyor.com/project/Jericho/zoomnet)
[![Tests](https://img.shields.io/appveyor/tests/jericho/zoomnet/master.svg)](https://ci.appveyor.com/project/jericho/zoomnet/build/tests)
[![Coverage Status](https://coveralls.io/repos/github/Jericho/ZoomNet/badge.svg?branch=master)](https://coveralls.io/github/Jericho/ZoomNet?branch=master)
[![CodeFactor](https://www.codefactor.io/repository/github/jericho/zoomnet/badge)](https://www.codefactor.io/repository/github/jericho/zoomnet)

| Release Notes| NuGet (stable) | MyGet (prerelease) |
|--------------|----------------|--------------------|
| [![GitHub release](https://img.shields.io/github/release/jericho/zoomnet.svg)](https://github.com/Jericho/ZoomNet/releases) | [![NuGet Version](http://img.shields.io/nuget/v/ZoomNet.svg)](https://www.nuget.org/packages/ZoomNet/) | [![MyGet Pre Release](https://img.shields.io/myget/jericho/vpre/ZoomNet.svg)](http://myget.org/gallery/jericho) |


## About


## Installation

The easiest way to include ZoomNet in your C# project is by adding the nuget package to your project:

```
PM> Install-Package ZoomNet
```

## .NET framework suport

ZoomNet supports the `4.8` .NET framework, any framework supporting `.NET Standard 2.1` (which includes `.NET Core 3.x`  and `ASP.NET Core 3.x`) as well as `.NET 5.0`.

## Usage

### Connection Information
Before you start using the ZoomNet client, you must decide how you are going to connect to the Zoom API. ZoomNet supports two distinct ways of connecting to Zoom: JWT and OAuth.

#### Connection using JWT
This is the simplest way to connect to the Zoom API. Zoom expects you to use a key and a secret to generate a JSON object with a signed payload and to provide this JSON object with every API request. The good news is that ZoomNet takes care of the intricacies of generating this JSON object: you simply provide the key and the secret and ZoomNet takes care of the rest. Super easy!

As the Zoom documentation mentions, this is perfect `if you’re looking to build an app that provides server-to-server interaction with Zoom APIs`.

Here is an except from the Zoom documentation that explains [how to get your API key and secret](https://marketplace.zoom.us/docs/guides/auth/jwt#key-secret):

> JWT apps provide an API Key and Secret required to authenticate with JWT. To access the API Key and Secret, Create a JWT App on the Marketplace. After providing basic information about your app, locate your API Key and Secret in the App Credentials page.

When you have the API key and secret, you can instantiate a 'connection info' object like so:
```csharp
var apiKey = "... your API key ...";
var apiSecret = "... your API secret ...";
var connectionInfo = new JwtConnectionInfo(apiKey, apiSecret);
```

#### Connection using OAuth
Using OAuth is much more complicated than using JWT but at the same time, it is more flexible because you can define which permissions your app requires. When a user installs your app, they are presented with the list of permissions your app requires and they are given the opportunity to accept. 

The Zoom documentation has a document about [how to create an OAuth app](https://marketplace.zoom.us/docs/guides/build/oauth-app) and another document about the [OAuth autorization flow](https://marketplace.zoom.us/docs/guides/auth/oauth) but I personnality was very confused by the later document so here is a brief step-by-step summary:
- you create an OAuth app, define which permissions your app requires and publish the app in the Zoom marketplace.
- user installs your app. During installation, user is presentd with a screen listing the permissons your app requires. User must click `accept`.
- Zoom generates a "authorization code". This code can be used only once to generate the first access token and refresh token. I CAN'T STRESS THIS ENOUGH: the authorization code can be used only one time. This was the confusing part to me: somehow I didn't understand that this code could be used only one time and I was attempting to use it repeatedly. Zoom would accept the code one time and would reject it subsequently, which lead to many hours of frustration while trying to figure out why the code was sometimes rejected.
- The access token is valid for 60 minutes and must therefore be "refreshed" periodically.

ZoomNet takes care of generating the access token and refresh token but it's your responsability to store these generated values. 

```csharp
var clientId = "... your client ID ...";
var clientSecret = "... your client secret ...";
var refreshToken = "... the refresh token previously issued by Zoom ...";
var accessToken = "... the access token previously issued by Zoom ...";
var connectionInfo = new OAuthConnectionInfo(clientId, clientSecret, refreshToken, accessToken,
	(newRefreshToken, newAccessToken) =>
	{
		/*
            Save the new refresh token and the access token to
            a safe place so you can provide it the next time
            you need to instantiate an OAuthConnectionInfo
        */
	});
```

### Client

You declare your client variable like so:
```csharp
var zoomClient = new ZoomClient(connectionInfo);
```
