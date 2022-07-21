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
1. Run the docker container using this command:
```docker run -d -p {port}:80 -e WEBSITE_URL=https://your-url.tld imitat0r/gfontproxy.net:latest```
2. Setup a reverse proxy from `https://your-url.tld` to `http://localhost:{port}`

# Demo
coming soon™