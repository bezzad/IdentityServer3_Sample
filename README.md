# IdentityServer3 Sample
Sample authentication service project of IdentityServer3 for implements the OAuth2 client credential grant types.

The [Central Authentication Service (CAS)](https://en.wikipedia.org/wiki/Central_Authentication_Service) is a [single sign-on](https://en.wikipedia.org/wiki/Single_sign-on) protocol for the [web](https://en.wikipedia.org/wiki/World_Wide_Web). Its purpose is to permit a user to access multiple applications while providing their credentials (such as userid and password) only once. It also allows web applications to authenticate users without gaining access to a user's security credentials, such as a password.

<br/>

Authenticate service is an [Web API](https://docs.microsoft.com/en-us/aspnet/web-api/overview/getting-started-with-aspnet-web-api/tutorial-your-first-web-api) service at all, which controls another services authorization safety with [IdentityServer3](https://identityserver.github.io/Documentation) project. IdentityServer also is an [OpenID Connect][1] Provider and [OAuth 2.0][2] Authorization Server Framework for ASP.NET 4.x/Katana.
So, first you need to know what is the [OpenID Connect][1] protocol ?

<hr/>
<br/>

## [OpenID Connect][1] Protocol

 [OpenID Connect][1] is a simple identity layer on top of the [OAuth 2.0][2]  protocol. It allows Clients to verify the identity of the End-User based on the authentication performed by an Authorization Server, as well as to obtain basic profile information about the End-User in an inter-operable and REST-like manner.

OpenID Connect allows clients of all types, including Web-based, mobile, and JavaScript clients, to request and receive information about authenticated sessions and end-users. The specification suite is extensible, allowing participants to use optional features such as encryption of identity data, discovery of OpenID Providers, and session management, when it makes sense for them. OpenID Connect defines optional mechanisms for robust signing and encryption. Whereas integration of OAuth 1.0a and OpenID 2.0 required an extension, in OpenID Connect, [OAuth 2.0][2]   capabilities are integrated with the protocol itself.

The OpenID Connect specifications, implementer’s guides, and specifications they are built upon are shown in the diagram below.

![](http://openid.net/wordpress-content/uploads/2014/02/OpenIDConnect-Map-4Feb2014.png)

For more information go to [reference][1]

<hr/>
<br/>

## [Introduction to OAuth 2][2]

__OAuth 2__ is an authorization framework that enables applications to obtain limited access to user accounts on an HTTP service, such as Facebook or GitHub. It works by delegating user authentication to the service that hosts the user account, and authorizing third-party applications to access the user account. __OAuth 2__ provides authorization flows for web and desktop applications, and mobile devices.

This informational guide is geared towards application developers, and provides an overview of __OAuth 2__ roles, authorization grant types, use cases, and flows.

<br/>

### OAuth Roles

<blockquote>

* ```Resource Owner: User``` <br/>
    <blockquote>
    The resource owner is the user who authorizes an application to access their account. The application's access to the user's account is limited to the "scope" of the authorization granted (e.g. read or write access).
    </blockquote>
* ```Client: API```  <br/>
    <blockquote>
    The client is the application that wants to access the user's account. Before it may do so, it must be authorized by the user, and the authorization must be validated by the API.
    </blockquote>
* ```Resource Server: API```  <br/>
    <blockquote>
    The resource server hosts the protected user accounts.
    </blockquote>
* ```Authorization Server: Application```  <br/>
    <blockquote>
    The authorization server verifies the identity of the user then issues access tokens to the application.
    </blockquote>

![abstract flow](https://assets.digitalocean.com/articles/oauth/abstract_flow.png)

Here is a more detailed explanation of the steps in the diagram:

1. The application requests authorization to access service resources from the user
2. If the user authorized the request, the application receives an authorization grant
3. The application requests an access token from the authorization server (API) by presenting authentication of its own identity, and the authorization grant
4. If the application identity is authenticated and the authorization grant is valid, the authorization server (API) issues an access token to the application. Authorization is complete.
5. The application requests the resource from the resource server (API) and presents the access token for authentication
6. If the access token is valid, the resource server (API) serves the resource to the application

The actual flow of this process will differ depending on the authorization grant type in use, but this is the general idea. We will explore different grant types in a later section.

</blockquote>

<br/>

### Application Registration

<blockquote>

Before using __OAuth__ with your application, you must register your application with the service. This is done through a registration form in the "developer" or "API" portion of the service's website, where you will provide the following information (and probably details about your application):

* Application Name
* Application Website
* Redirect URI or Callback URL

The redirect URI is where the service will redirect the user after they authorize (or deny) your application, and therefore the part of your application that will handle authorization codes or access tokens.</blockquote>

<br/>

### Client ID and Client Secret

<blockquote>

Once your application is registered, the service will issue `client credentials` in the form of a client identifier and a client secret. The Client ID is a publicly exposed string that is used by the service API to identify the application, and is also used to build authorization URLs that are presented to users. The Client Secret is used to authenticate the identity of the application to the service API when the application requests to access a user's account, and must be kept private between the application and the API.

</blockquote>

<br>

### Authorization Grants

<blockquote>

In the Abstract Protocol Flow above, the first four steps cover obtaining an authorization grant and access token. The authorization grant type depends on the method used by the application to request authorization, and the grant types supported by the API. __OAuth 2__ defines four grant types, each of which is useful in different cases:

* __Authorization Code__: `used with server-side Applications`
<br/><br/>
The __authorization code__ grant type is the most commonly used because it is optimized for server-side applications, where source code is not publicly exposed, and Client Secret confidentiality can be maintained. This is a redirection-based flow, which means that the application must be capable of interacting with the user-agent (i.e. the user's web browser) and receiving API authorization codes that are routed through the user-agent. <br/><br/>
![auth code flow](https://assets.digitalocean.com/articles/oauth/auth_code_flow.png)

<br/><br/>
* __Implicit__: `used with Mobile Apps or Web Applications (applications that run on the user's device)`
<br/><br/>
The __implicit__ grant type is used for mobile apps and web applications (i.e. applications that run in a web browser), where the client secret confidentiality is not guaranteed. The implicit grant type is also a redirection-based flow but the access token is given to the user-agent to forward to the application, so it may be exposed to the user and other applications on the user's device. Also, this flow does not authenticate the identity of the application, and relies on the redirect URI (that was registered with the service) to serve this purpose. <br/><br/>
The implicit grant type does not support refresh tokens. <br/>
The implicit grant flow basically works as follows: the user is asked to authorize the application, then the authorization server passes the access token back to the user-agent, which passes it to the application.
<br/><br/>
![implicit flow](https://assets.digitalocean.com/articles/oauth/implicit_flow.png)

<br/><br/>
* __Resource Owner Password Credentials__: `used with trusted Applications, such as those owned by the service itself`  
<br/><br/>
With the __resource owner password credentials__ grant type, the user provides their service credentials (username and password) directly to the application, which uses the credentials to obtain an access token from the service. This grant type should only be enabled on the authorization server if other flows are not viable. Also, it should only be used if the application is trusted by the user (e.g. it is owned by the service, or the user's desktop OS).

<br/><br/>
* __Client Credentials__: `used with Applications API access`
<br/><br/>
The __client credentials__ grant type provides an application a way to access its own service account. Examples of when this might be useful include if an application wants to update its registered description or redirect URI, or access other data stored in its service account via the API.

</blockquote>

For more information go to [reference][2]

<hr/>
<br/>

## [Identity Server 3][3]

__IdentityServer__ is a framework and a hostable component that allows implementing single sign-on and access control for modern web applications and APIs using protocols like OpenID Connect and OAuth2. It supports a wide range of clients like mobile, web, SPAs and desktop applications and is extensible to allow integration in new and existing architectures.

Most modern applications look more or less like this:

![appArch](http://identityserver.github.io/Documentation/assets/images/appArch.png)

The typical interactions are:

* Browsers communicate with web applications

* Web applications communicate with web APIs (sometimes on their own, sometimes on behalf of a user)

* Browser-based applications communicate with web APIs

* Native applications communicate with web APIs

* Server-based applications communicate with web APIs

* Web APIs communicate with web APIs (sometimes on their own, sometimes on behalf of a user)

Typically each and every layer (front-end, middle-tier and back-end) has to protect resources and implement authentication and/or authorization – and quite typically against the same user store.

This is why we don’t implement these fundamental security functions in the business applications/endpoints themselves, but rather outsource that critical functionality to a service - the security token service.

This leads to the following security architecture and usage of protocols:

![protocols](http://identityserver.github.io/Documentation/assets/images/protocols.png)

This divides the security concerns into two parts.

<br/><br/>
#### __Authentication__

<blockquote>

Authentication is needed when an application needs to know about the identity of the current user. Typically these applications manage data on behalf of that user and need to make sure that this user can only access the data he is allowed to. The most common example for that is (classic) web applications – but native and JS-based applications also have need for authentication.

<br/>

The most common authentication protocols are SAML2p, WS-Federation and OpenID Connect – SAML2p being the most popular and the most widely deployed.

<br/>

OpenID Connect is the newest of the three, but is generally considered to be the future because it has the most potential for modern applications. It was built for mobile application scenarios right from the start and is designed to be API friendly.

</blockquote>

#### __API Access__

<blockquote>

Applications have two fundamental ways with which they communicate with APIs – using the application identity, or delegating the user’s identity. Sometimes both ways need to be combined.

<br/>

__OAuth2__ is a protocol that allows applications to request access tokens from a security token service and use them to communicate with APIs. This reduces complexity on both the client applications as well as the APIs since authentication and authorization can be centralized.

</blockquote>

#### __OpenID Connect and OAuth2 – better together__

<blockquote>

OpenID Connect and OAuth2 are very similar – in fact OpenID Connect is an extension on top of OAuth2. This means that you can combine the two fundamental security concerns – authentication and API access into a single protocol – and often a single round trip to the security token service.

<br/>

This is why we believe that the combination of OpenID Connect and OAuth2 is the best approach to secure modern applications for the foreseeable future. IdentityServer3 is an implementation of these two protocols and is highly optimized to solve the typical security problems of today’s mobile, native and web applications.

</blockquote>

<br/>

### __High Level Features__

##### __Authentication as a Service__

<blockquote>

Centralized login logic and workflow at a single & well-secured place.
</blockquote>

#### __Single Sign-on / Sign-out__

<blockquote>
Single sign-on (and out) over multiple application types like web or mobile.
</blockquote>

#### __Access Control for APIs__

<blockquote>
Issue access tokens for APIs for various types of clients, e.g. server to server, web applications, SPAs and native/mobile apps.
</blockquote>

#### __Federation__

<blockquote>

Support for external social identity providers like Google, Facebook etc, as well as integration for enterprise identity management systems via SAML and WS-Federation.

</blockquote>

#### __Customization everywhere__

<blockquote>

The most important part - every aspect of IdentityServer can be customized to fit your needs. Since IdentityServer is a framework, you can write code to adapt the system in a way it makes sense for your scenarios.
</blockquote>

<br/>

### __Terminology__

The specs, documentation and object model use a certain terminology that you should be aware of.

![terminology](http://identityserver.github.io/Documentation/assets/images/terminology.png)

#### __OpenID Connect Provider (OP)__

<blockquote>

__IdentityServer__ is an OpenID Connect provider - it implements the OpenID Connect protocol (and __OAuth2__ as well).
<br/>
Different literature uses different terms for the same role - you probably also find security token service, identity provider, authorization server, ```IP-STS``` and more.
<br/>
But they are in a nutshell all the same: a piece of software that issues security tokens to clients.
__IdentityServer__ has a number of jobs and features - including: 
<br/>
* authenticate users using a local account store or via an external identity provider
* provide session management and single sign-on
* manage and authenticate clients
* issue identity and access tokens to clients
* validate tokens

</blockquote>

#### __Client__

<blockquote>

A client is a piece of software that requests tokens from IdentityServer - either for authenticating a user or for accessing a resource (also often called a relying party or RP). A client must be registered with the OP.
<br/>
Examples for clients are web applications, native mobile or desktop applications, SPAs, server processes etc.

</blockquote>

#### __User__

<blockquote>

A user is a human that is using a registered client to access his or her data.

</blockquote>

#### __Scope__

<blockquote>

Scopes are identifiers for resources that a client wants to access. This identifier is sent to the OP during an authentication or token request.
By default every client is allowed to request tokens for every scope, but you can restrict that.
They come in two flaw-ours.
<br/>
* __Identity scopes__<br/>
Requesting identity information (aka claims) about a user, e.g. his name or email address is modeled as a scope in OpenID Connect.
<br/>

  There is e.g. a scope called `profile` that includes first name, last name, preferred username, gender, profile picture and more. You can read about the standard scopes [here](http://openid.net/specs/openid-connect-core-1_0.html#ScopeClaims) and you can create your own scopes in IdentityServer to model your own requirements.
<br/><br/>

* __Resource scopes__<br/>

  Resource scopes identify web APIs (also called resource servers) - you could have e.g. a scope named `calendar` that represents your calendar API.
</blockquote>

#### __Claims__

<blockquote>

List of user claims that should be included in the identity (identity scope) or access token (resource scope). 
Scope can also specify claims that go into the corresponding token - the `ScopeClaim` class has the following properties:
* Name
  + Name of the claim
* Description
  + Description of the claim
* AlwaysIncludeInIdToken
  + Specifies whether this claim should always be present in the identity token (even if an access token has been requested as well). Applies to identity scopes only. Defaults to `false`.
<br/>

Example of a `role` identity scope:

<br/>

```cs
var roleScope = new Scope
{
    Name = "roles",
    DisplayName = "Roles",
    Description = "Your organizational roles",
    Type = ScopeType.Identity,

    Claims = new List<ScopeClaim>
    {
        new ScopeClaim(Constants.ClaimTypes.Role, alwaysInclude: true)
    }
};
```

<br/>

The ‘__AlwaysIncludeInIdentityToken__’ property specifies that a certain claim should always be part of the identity token, even when an access token for the userinfo endpoint is requested.
</blockquote>

#### __Authentication/Token Request__

<blockquote>

Clients request tokens from the OP. Depending on the scopes requested, the OP will return an identity token, an access token, or both.

</blockquote>

#### __Identity Token__

<blockquote>

An identity token represents the outcome of an authentication process. It contains at a bare minimum an identifier for the user (called the sub aka subject claim). It can contain additional information about the user and details on how the user authenticated at the OP.

</blockquote>

#### __Access Token__

<blockquote>

An access token allows access to a resource. Clients request access tokens and forward them to an API. Access tokens contain information about the client and the user (if present). APIs use that information to authorize access to their data.

</blockquote>

<br/>

For more information go to the [documentation for the latest version](https://identityserver.github.io/Documentation/docsv2)

<hr/>
<br/>

## Authentication Service Methodology

Authentication service is included four project which implements the __OAuth2__ client credential grant types with below figures:

#### Introduction to [OAuth 2 Client Credential Grant Type](https://www.digitalocean.com/community/tutorials/an-introduction-to-oauth-2#grant-type-client-credentials)

<blockquote>

This project use [IdentityServer3][3] by OAuth __client credential__ grant types for authenticate all clients as API or end point clients. <br/><br/>
The __client credentials__ grant type provides an application a way to access its own service account. Examples of when this might be useful include if an application wants to update its registered description or redirect __URI__, or access other data stored in its service account via the __API__.
<br/><br/>
IdentityServer publishes a [discovery document](http://localhost:5005/.well-known/openid-configuration) where you can find metadata and links to all the endpoints, key material, etc, like the below data:
<br/>

```json
// http://localhost:5005/.well-known/openid-configuration

{
  "issuer": "http://localhost:5005",
  "jwks_uri": "http://localhost:5005/.well-known/jwks",
  "authorization_endpoint": "http://localhost:5005/connect/authorize",
  "token_endpoint": "http://localhost:5005/connect/token",
  "userinfo_endpoint": "http://localhost:5005/connect/userinfo",
  "end_session_endpoint": "http://localhost:5005/connect/endsession",
  "check_session_iframe": "http://localhost:5005/connect/checksession",
  "revocation_endpoint": "http://localhost:5005/connect/revocation",
  "introspection_endpoint": "http://localhost:5005/connect/introspect",
  "frontchannel_logout_supported": true,
  "frontchannel_logout_session_supported": true,
  "scopes_supported": [
    "service"
  ],
  "claims_supported": [
    
  ],
  "response_types_supported": [
    "code",
    "token",
    "id_token",
    "id_token token",
    "code id_token",
    "code token",
    "code id_token token"
  ],
  "response_modes_supported": [
    "form_post",
    "query",
    "fragment"
  ],
  "grant_types_supported": [
    "authorization_code",
    "client_credentials",
    "password",
    "refresh_token",
    "implicit"
  ],
  "subject_types_supported": [
    "public"
  ],
  "id_token_signing_alg_values_supported": [
    "RS256"
  ],
  "code_challenge_methods_supported": [
    "plain",
    "S256"
  ],
  "token_endpoint_auth_methods_supported": [
    "client_secret_post",
    "client_secret_basic"
  ]
}
```

<br/>

In the [IdentityServer meta data documentation](http://localhost:5005/.well-known/openid-configuration) you can see the authentication server addresses and meta data like `token request` address by name "[token_endpoint](http://localhost:5005/connect/token)" or supported scopes name by name "__scopes_supported__" or public key and other security data by name "__jwks_uri__" and etc.

<br/>

* __Client Credentials Flow__
<br/>

  The application requests an access token by sending its credentials, its __client ID__ and __client secret__, to the authorization server. An example POST request might look like the following:
<br/>

  ```
    POST            http://localhost:5005/connect/token
    Content-Type:   application/x-www-form-urlencoded
    Authorization:  Basic(base64)  username(clientId):password(clientSecret)
    Body:   
    {
        grant_type:  client_credentials
        scope:   scope name (exp. service)
    }
  ```

  If the application credentials check out, the authorization server returns an access token to the application like the following reponse:

<br/>

  ```json
    {
    "access_token": "eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IlBmUWh5ZTBvVDVJclBnbXZYZFEwLVYzWWNFTSIsImtpZCI6IlBmUWh5ZTBvVDVJclBnbXZYZFEwLVYzWWNFTSJ9.eyJpc3MiOiJodHRwOi8vYXV0aC50YWFnaGNoZS5pciIsImF1ZCI6Imh0dHA6Ly9hdXRoLnRhYWdoY2hlLmlyL3Jlc291cmNlcyIsImV4cCI6MTQ5NDMxMzE3OSwibmJmIjoxNDk0MzA5NTc5LCJjbGllbnRfaWQiOiJhZG1pbiIsInJvbGUiOlsiYWNjb3VudDpnZXQiLCJhY2NvdW50OmFkZCIsImFjY291bnQ6Y2hhbmdlIiwiZGV2aWNlOmdldCIsImVtYWlsOnNlbmQiLCJkZXZpY2U6Y2hhbmdlIiwiZG93bmxvYWQ6ZG93bmxvYWQiLCJzZWFyY2g6Z2V0Iiwic2VhcmNoOnN0YXR1cyIsInNlYXJjaDpjaGFuZ2UiLCJkYXRhOmdldCJdLCJzY29wZSI6InNlcnZpY2UifQ.jUVMjyzkiDO_9Zcrjhmv9WW9YrtCECyZ0ddmIWVFpWSB7WW2Mcg2NtNmLuxL6OOU8Aat8rnpYSEcNQ_2yCE1Gn7c6hsgdlbTimo2Rh64jiBAUL0em5oGW5_5t02B_elkHBh2UFre_4YZ1gA_xXrpU9o4WhRld2_1dXuvECySqCQzxbN6G0mr8csm5eCx3M1dfUUU_ihV9oGmW99BXiqtFfe9QRb-mJU2Qmbilzb3nb-XY-MZsxuA5BHJOz82JKpv2ALyc1AQndJBBQGOeS2u1c-vYDsSo1Z2BSfeDAxYzs9U_jeKFBLdbib2reGqvNwD_ir9kmSviHOEvhSRNvqPzQ",
    "expires_in": 3600,
    "token_type": "Bearer"
    }
  ```

  Now the application is authorized to use its own account! <br/>

* __Access Token Usage__
<br/>

  Once the application has an access token, it may use the token to access the user's account via the __API__, limited to the scope of access, until the token expires or is revoked.
<br/>
An access token is a data of [JWT (JSON Web Token)][4] that you can to open or validate it from reference web site's for manually decoding.
[JWT][4] are an open, industry standard RFC 7519 method for representing claims securely between two parties.
<br/>
[JWT.IO][4] allows you to decode, verify and generate JWT.
<br/><br/>
__What is the JSON Web Token structure?__ 

<blockquote>

JSON Web Tokens consist of three parts separated by dots (```.```), which are:
<br/>
  + Header
  + Payload
  + Signature

  Therefore, a JWT typically looks like the following.
  <br/>

  ```jwt
  xxxx.yyyyy.zzzzz
  ```

   Let's break down the different parts.
<br/><br/>
__Header__
<br/>
The header typically consists of two parts: the type of the token, which is JWT, and the hashing algorithm being used, such as HMAC SHA256 or RSA.
<br/>
For example:
<br/>

  ```json
  {
      "alg": "RS256",
      "typ": "JWT"
  }
  ```

   <br/>

   Then, this JSON is __[Base64 Url](https://www.base64decode.org)__ encoded to form the first part of the JWT.
<br/><br/>
   __Payload__
<br/>
The second part of the token is the payload, which contains the claims. Claims are statements about an entity (typically, the user) and additional metadata. There are three types of claims: reserved, public, and private claims. <br/>

  * __Reserved claims__: These is a set of predefined claims which are not mandatory but recommended, to provide a set of useful, interoperable claims. Some of them are: __iss__ (issuer), __exp__ (expiration time), __sub__ (subject), __aud__ (audience), and others.
<br/>

> Notice that the claim names are only three characters long as JWT is meant to be compact.
<br/>

  * __Public claims__: These can be defined at will by those using JWTs. But to avoid collisions they should be defined in the IANA JSON Web Token Registry or be defined as a URI that contains a collision resistant namespace.
<br/>

  * __Private claims__: These are the custom claims created to share information between parties that agree on using them.

  An example of payload could be:
<br/>

  ```json
  {
    "iss": "http://localhost:5005",
    "aud": "http://localhost:5005/resources",
    "exp": 1494313179,
    "nbf": 1494309579,
    "client_id": "admin",
    "role": [
        "account:get",
        "account:add",
        "account:change",
        "device:get",
        "email:send",
        "device:change",
        "download:download",
        "search:get",
        "search:status",
        "search:change",
        "data:get"
    ],
    "scope": "service"
  }
  ```

   The payload is then __[Base64 Url](https://www.base64decode.org)__ encoded to form the second part of the JSON Web Token.
   <br/><br/>

  __Signature__
<br/>
To create the signature part you have to take the encoded header, the encoded payload, a secret, the algorithm specified in the header, and sign that.
<br/>
For example if you want to use the HMAC SHA256 algorithm, the signature will be created in the following way:
<br/>

  ```cs
  HMACSHA256(
    base64UrlEncode(header)   + "." +
    base64UrlEncode(payload),
    secret)
  ```

  The signature is used to verify that the sender of the JWT is who it says it is and to ensure that the message wasn't changed along the way.
<br/><br/>
__Putting all together__
<br/>
The output is three Base64 strings separated by dots that can be easily passed in HTML and HTTP environments, while being more compact when compared to XML-based standards such as SAML.
<br/>
The following shows a JWT that has the previous header and payload encoded, and it is signed with a secret.
<br/><br/>
![encoded-jwt3](https://cdn.auth0.com/content/jwt/encoded-jwt3.png)
<br/><br/>
If you want to play with JWT and put these concepts into practice, you can use [jwt.io Debugger][4] to decode, verify, and generate JWTs.
<br/><br/>
![legacy-app-auth-5](https://cdn.auth0.com/blog/legacy-app-auth/legacy-app-auth-5.png)
<br/><br/>

  * __How do JSON Web Tokens work?__ <br/>
  In authentication, when the user successfully logs in using their credentials, a JSON Web Token will be returned and must be saved locally (typically in local storage, but cookies can be also used), instead of the traditional approach of creating a session in the server and returning a cookie.
  Whenever the user wants to access a protected route or resource, the user agent should send the JWT, typically in the __Authorization__ header using the __Bearer__ schema. The content of the header should look like the following:

  ```
   Authorization: Bearer <token>
  ```

  This is a stateless authentication mechanism as the user state is never saved in server memory. The server's protected routes will check for a valid JWT in the Authorization header, and if it's present, the user will be allowed to access protected resources. As JWTs are self-contained, all the necessary information is there, reducing the need to query the database multiple times.<br/><br/>
  This allows you to fully rely on data APIs that are stateless and even make requests to downstream services. It doesn't matter which domains are serving your APIs, so Cross-Origin Resource Sharing (CORS) won't be an issue as it doesn't use cookies.
<br/>
The following diagram shows this process:
<br/><br/>
![jwt-diagram](https://cdn.auth0.com/content/jwt/jwt-diagram.png)
<br/><br/>

</blockquote>

Here is an example of an __API__ request, using ```curl```. Note that it includes the access token:
<br/>

  ```curl -X POST -H 'Authorization: Bearer ACCESS_TOKEN' 'https://localhost:5005/v1/$OBJECT'```
<br/><br/>
Assuming the access token is valid, the __API__ will process the request according to its __API__ specifications. If the access token is expired or otherwise invalid, the __API__ will return an "invalid_request" error.
<br/><br/>


* __Access Token Validation__
<br/>
OWIN Middleware to validate access tokens from IdentityServer v3. <br/>
You can either validate the tokens locally (JWTs only) or use the IdentityServer's access token validation endpoint (JWTs and reference tokens).
<br/>

  ```cs
  app.UseIdentityServerBearerTokenAuthentication(
      new IdentityServerBearerTokenAuthenticationOptions
      {
          Authority = "http://localhost:5005",
          ValidationMode = ValidationMode.Both, // Use local validation for JWTs and the validation endpoint for reference tokens.
      });
  ```

  The middleware can also do the scope validation in one go.<br/>

  ```cs
  app.UseIdentityServerBearerTokenAuthentication(new IdentityServerBearerTokenAuthenticationOptions
      {
          Authority = "https://identity.identityserver.io",
          ValidationMode = ValidationMode.Local,
          RequiredScopes = new[] { "api1", "api2" }
      });
  ```

  We can to validate access token without local checking and use self authentication server instead that. For this job's we must post the __AccessToken__ to one of authentication server API's methods by name __accesstokenvalidation__ like following: <br/>

  ```
  POST            http://localhost:5005/connect/accesstokenvalidation
  Content-Type:   application/x-www-form-urlencoded
  Body:   
  {
        token:  eyJ0eXAiOiJKV1QiLCJhbGciOiJSUzI1NiIsIng1dCI6IlBmUWh5Z...
  }
  ```

  If the __AccessToken__ is valid then response message is like this: <br/>

  ```json
  {
    "iss": "http://localhost:5005",
    "aud": "http://localhost:5005/resources",
    "exp": "1494415019",
    "nbf": "1494411419",
    "client_id": "admin",
    "role": [
        "account:get",
        "account:add",
        "account:change",
        "device:get",
        "email:send",
        "device:change",
        "download:download",
        "search:get",
        "search:status",
        "search:change",
        "data:get"
    ],
    "scope": "service"
  }
  ```

  And for invalid response message is:

  ```json
  {
    "Message": "invalid_token"
  }
  ```

* __Refresh Token Flow__<br/>
<br/>

  After an access token expires, using it to make a request from the __API__ will result in an "Invalid Token Error". At this point, if a refresh token was included when the original access token was issued, it can be used to request a fresh access token from the authorization server.
<br/>

  Here is an example __POST__ request, using a refresh token to obtain a new access token: <br/><br/>
  `https://localhost:5005/v1/oauth/token?grant_type=refresh_token&client_id=CLIENT_ID&client_secret=CLIENT_SECRET&refresh_token=REFRESH_TOKEN`

<br/>

</blockquote>

* __Sample.IdentityServer__

<blockquote>

Identity server placed in [http://localhost:5005](http://localhost:5005). It is based on [identity server 3][3]. It implemented Oauth 2 and [OpenId connect protocols ][1].
Identity server has a console for managing clients and scopes. It can only be reached on localhost (remote localhost:5005 server). All data on identity server is persisted on sql server.

<br/>

IdentityServer publishes a [discovery document](http://localhost:5005//.well-known/openid-configuration) where you can find metadata and links to all the endpoints, key material, etc.

<br/>

IdentityServer allows users to view and revoke application permissions previously granted to client applications.
There are other more advanced walk-throughs in the docs that you could do afterwards.

<br/>

  + __Setting up IdentityServer__
<br/>
First we will create a console host and set up IdentityServer.
Start by creating a standard console application and add IdentityServer via nuget:
<br/>

  ```
    install-package identityserver3
  ```

<br/>

  + __Registering the API__ <br/>
APIs are modeled as scopes - you need to register all APIs that you want to be able to request access tokens for. For that we create a class that returns a list of ```Scope``` at __Config/Scopes.cs__: <br/>

  ```cs
    using IdentityServer3.Core.Models;

    static class Scopes
    {
        public static List<Scope> Get()
        {
            return new List<Scope>
            {
                new Scope
                    {
                        Name = "api1",
                        DisplayName = "",
                        Type = ScopeType.Resource,
                    },
            };
        }
    }
  ```

<br/>

* __Registering the Client__
For now we want to register a single client. This client will be able to request a token for the ```api1``` scope. For our first iteration, there will be no human involved and the client will simply request the token on behalf of itself (think machine to machine communication). Later we will add a user to the picture.
<br/><br/>
For this client we configure the following things at __Config/Clients.cs__:
<br/><br/>
Display name and id (unique name)
  + The client secret (used to authenticate the client against the token endpoint)
  + The flow (client credentials flow in this case)
  + Usage of so called reference tokens. Reference tokens do not need a signing certificate.
  + Access to the ```api1``` scope

  ```cs
    using IdentityServer3.Core.Models;

    static class Clients
    {
        public static List<Client> Get()
        {
            return new List<Client>
            {
            // no human involved
                new Client
                    {
                        ClientName = "Test",
                        Enabled = true,
                        ClientId = "tester",
                        ClientSecrets = new List<Secret>
                        {
                            new Secret("ORGKXlWsGM-xrcD34eBsx".Sha256())
                        },
                        Flow = Flows.ClientCredentials, // Must require auth_keys.pfx file to IdentityServerOptions.SigningCertificate
                        Claims = new List<Claim>
                        {
                            new Claim(Constants.ClaimTypes.Role, "account:get"),
                            new Claim(Constants.ClaimTypes.Role, "account:add"),
                            new Claim(Constants.ClaimTypes.Role, "account:change"),
                            new Claim(Constants.ClaimTypes.Role, "device:get"),
                            new Claim(Constants.ClaimTypes.Role, "email:send"),
                            new Claim(Constants.ClaimTypes.Role, "device:change")
                        },
                        AccessTokenType = AccessTokenType.Jwt, // by default is Jwt
                        AllowAccessToAllScopes = true,
                        PrefixClientClaims = false,
                        AllowedScopes = new List<string>
                        {
                            "api1"
                        }
                    },
                new Client
                    {
                        ClientName = "Test 2",
                        ClientId = "tester2",
                        ClientSecrets = new List<Secret>
                        {
                            new Secret("ORGKXlWsGM-xrcD34eBsx".Sha256())
                        },
                        Flow = Flows.ResourceOwner,                   
                        AccessTokenType = AccessTokenType.Reference, // by default is Jwt
                        AllowAccessToAllScopes = true,
                        PrefixClientClaims = false,
                        AllowedScopes = new List<string>
                        {
                            "api1"
                        }
                    }
            };
        }
    }
  ```

> `Note:` If the client use __ClientCredentials__ flow then must be set the __SigningCertificate__ property of __IdentityServerOptions__ instance at identity server __Startup.cs__ file's to __private_key_file.pfx__ files.
<br/>

* __Configuring IdentityServer__ <br/>
IdentityServer is implemented as OWIN middleware. It is configured in the `Startup` class using the `UseIdentityServer` extension method. The following snippets sets up a bare bones server with our scopes and clients. We also set up an empty list of users - we will add users later.
<br/>

```cs
using Owin;
using System.Collections.Generic;
using IdentityServer3.Core.Configuration;
using IdentityServer3.Core.Services.InMemory;

namespace IdSrv
{
    class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            var options = new IdentityServerOptions
            {
                SiteName = "Identity Server",
                SigningCertificate = Certificate.Get(),
                Factory = Factory.Configure(Settings.Default.DatabaseConnectionString),
                RequireSsl = false,
                LoggingOptions = {EnableHttpLogging = false, EnableKatanaLogging = false}
            };

            app.UseIdentityServer(options);
        }
    }
}
```

</blockquote>
</blockquote>

* __Sample.Authentication Client__

<blockquote>

A class library project to add in clients project that will request an access token and use that to authenticate with the API. This project in fact is authorization interfacing between client applications and authentication server. 
<br/>
This package used [RestSharp](https://github.com/restsharp/RestSharp) rest clients and request to communicate with other services which needs authentication.
<br/>
Authenticator is a tool just for getting tokens from auth server.
<br/>
There is an implementation which get __clientId__ and __clientSecret__ and do the jobs for authentication with an Oauth2 server. IRestRequest from [RestSharp](https://github.com/restsharp/RestSharp) package is parameters for RestClient.
<br/>
> This package is ready to use by Dependency injector containers
<br/>

__How to implement client authenticator__
<br/>
First install a nuget package for an OAuth2 client helper library:
<br/>

```
install-package IdentityModel
```

<br/>
The first code snippet requests the access token using the client credentials:
<br/>

```cs
using IdentityModel.Client;

namespace Sample.AuthenticationClient
{
    public sealed class Authenticator : IDisposable
    {
         static TokenResponse GetClientToken()
         {
                 var client = new TokenClient(
                     address: "http://localhost:5005/token",
                     clientId: "tester",
                     clientSecret: "ORGKXlWsGM-xrcD34eBsx",
                     style: AuthenticationStyle.BasicAuthentication  // default value
                 );

                 return client.RequestClientCredentialsAsync("api1").Result;
         }
        ...
    }
}
```

<br/>
The second code snippet calls the API using the access token:
<br/>

```cs
static void CallApi(TokenResponse response)
{
    var client = new HttpClient();
    client.SetBearerToken(response.AccessToken);

    Console.WriteLine(client.GetStringAsync("http://localhost:14869/test").Result);
}
```

<br/>
If you call both snippets, you should see below result: <br/> <br/>

```json
{"message":"OK computer","client":"tester"}
```

</blockquote>
</blockquote>

* __Sample.AuthenticationServer__

<blockquote>

This package contains tools for validating request in [Owin](http://owin.org). There is a middle ware that do the jobs for this. In this part we will create a middleware to add it at all web APIs that is configured to require an access token from the IdentityServer we just set up.<br/>
In the other words, This project has a extension class as __[Katana Access Token Validation Middleware](https://identityserver.github.io/Documentation/docsv2/consuming/overview.html)__.
<br/>
Consuming IdentityServer access tokens in web APIs is easy, you simply drop in our token validation middleware into your Katana pipeline and set the URL to IdentityServer. All configuration and validation is done for you. 
<br/>
You can get the middleware here: [nuget](https://www.nuget.org/packages/IdentityServer3.AccessTokenValidation) or [source code](https://github.com/IdentityServer/IdentityServer3.AccessTokenValidation). Necessary nuget packages for this middleware project's:
<br/>

```
install-package Microsoft.Owin.Host.SystemWeb
install-package Microsoft.AspNet.WebApi.Owin
install-package IdentityServer3.AccessTokenValidation
```

<br/>
The typical use case is, that you provide the URL to IdentityServer and the scope name of the API:
<br/>

```cs
public static class Extensions
{
        public static IAppBuilder UseAuth(this IAppBuilder app, string authenticationServer, bool localValidation = true)
        {
            app.UseIdentityServerBearerTokenAuthentication(
                new IdentityServerBearerTokenAuthenticationOptions
                {
                    Authority = authenticationServer,
                    RoleClaimType = "role",
                    NameClaimType = "client_id",
                    ValidationMode = localValidation ? ValidationMode.Local : ValidationMode.ValidationEndpoint,
                    AuthenticationMode = AuthenticationMode.Active,
                    EnableValidationResultCache = true
                });
            return app;
        }
}
```

The middleware will first inspect the token - if it is a [JWT][4], token validation will be done locally (using the issuer name and key material found in the discovery document). If the token is a reference token, the middleware will use the access token validation [endpoint](https://identityserver.github.io/Documentation/docsv2/endpoints/accessTokenValidation.html) on IdentityServer (or the [introspection endpoint](https://identityserver.github.io/Documentation/docsv2/endpoints/introspection.html) is credentials are configured).

<br/>

So, when an API want to use another API in secure level, should sent self __Access Token__ to that. Then token receiver API's validate the __Access Token__ by this middleware without sending token to main authentication server to validate the token is true or not.

<br/>

High level features: 

* Support for validating both JWT and reference tokens
* Support for validating scopes
* Built-in configurable in-memory cache for caching reference token validation results
* Cache implementation can be replaced

</blockquote>

<br/>

For more information about how to [creating the simplest OAuth2 Authorization Server, Client and API](https://identityserver.github.io/Documentation/docsv2/overview/simplestOAuth.html) go to [IdentityServer3 document reference](http://identityserver.github.io/Documentation/docsv2).

[1]: http://openid.net/connect
[2]: https://www.digitalocean.com/community/tutorials/an-introduction-to-oauth-2
[3]: https://github.com/IdentityServer/IdentityServer3
[4]: https://jwt.io
