![Copyright ev0lve    Digital](https://img.shields.io/badge/Copyright-ev0lve%20Digital-blue)    ![License    AGPL-3.0](https://img.shields.io/github/license/imi-tat0r/GFontProxy.NET) ![Issues](https://img.shields.io/github/issues/imi-tat0r/GFontProxy.NET)    ![Stars](https://img.shields.io/github/stars/imi-tat0r/GFontProxy.NET)    ![Docker    Pulls](https://img.shields.io/docker/pulls/imitat0r/gfontproxy.net)
# GFontProxy.NET
A .NET6 minimal API to act as a proxy in order to cache and provide Google fonts in compliance with GDPR (if hosted in EU)

# How it works
GFontProxy.NET can be self-hosted or accessed via our free demo (no up-time guarantee).  
Simply replace any `https://fonts.googleapis.com/` with `https://your-url.tld/`.  
GFontProxy.NET will then download the corresponding css and fonts from Google and deploy them.  
Since no PII is sent to google, this is GDPR compliant without the need for specific consent.  

# Requirements
In order to use GFontProxy.NET you need to have `Docker` and a webserver which can act as a reverse proxy (e.g. `nginx`).

# Install
1. Run the docker container using this command (CORS_ORIGIN is optional):  
```docker run -d -p {port}:80 -e WEBSITE_URL=https://your-url.tld -e CORS_ORIGIN=https://another-url.tld imitat0r/gfontproxy.net:latest```  
2. Setup a reverse proxy from `https://your-url.tld` to `http://localhost:{port}`  

# Demo
Please note that the demo is currently running behind cloudflare and therefore might not be GDPR compliant. If this changes, this notice will be removed.  
  
If you want to try the demo, simply change every google fonts related <link> from `https://fonts.googleapis.com` with `https://fonts.googleapisproxy.com` and remove the `https://fonts.gstatic.com` link.  
### Before:  
```
<link rel="preconnect" href="https://fonts.gstatic.com">
<link rel="stylesheet" href="https://fonts.googleapis.com/css2?family=Roboto:wght@300;400;500&display=swap">
<link rel="stylesheet" href="https://fonts.googleapis.com/icon?family=Material+Icons">
```
### After:  
```
<link rel="stylesheet" href="https://fonts.googleapisproxy.com/css2?family=Roboto:wght@300;400;500&display=swap">
<link rel="stylesheet" href="https://fonts.googleapisproxy.com/icon?family=Material+Icons">
```
# Thanks
@MrJustreborn - Initial inspiration for this project  
@Fr3shlama - Hosting the Demo
