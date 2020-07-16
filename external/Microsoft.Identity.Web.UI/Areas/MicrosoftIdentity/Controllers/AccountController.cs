﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.OpenIdConnect;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Microsoft.Identity.Web.UI.Areas.MicrosoftIdentity.Controllers
{
    [NonController]
    [AllowAnonymous]
    [Area("MicrosoftIdentity")]
    [Route("[area]/[controller]/[action]")]
    public class AccountController : Controller
    {
        private readonly IOptionsMonitor<MicrosoftIdentityOptions> _options;

        public AccountController(IOptionsMonitor<MicrosoftIdentityOptions> azureADOptions)
        {
            _options = azureADOptions;
        }

        [HttpGet("{scheme?}")]
        public IActionResult SignIn([FromRoute] string scheme)
        {
            string luisState = "default";
            if (!String.IsNullOrEmpty(HttpContext.Request.Query["luis_state"]))
            {
                luisState = HttpContext.Request.Query["luis_state"];
            }
            
            scheme = scheme ?? OpenIdConnectDefaults.AuthenticationScheme;
            var redirectUrl = Url.Content("~/luisstate");
            var props = new AuthenticationProperties
            {
                RedirectUri = redirectUrl,
                Items = { { "test", "test" } },
                Parameters = { { "luis_state", luisState } }
            };
            var cr = Challenge(props, scheme);

            return cr;
        }

        [HttpGet("{scheme?}")]
        public IActionResult SignOut([FromRoute] string scheme)
        {
            scheme = scheme ?? OpenIdConnectDefaults.AuthenticationScheme;
            var callbackUrl = Url.Page("/Account/SignedOut", pageHandler: null, values: null, protocol: Request.Scheme);
            return SignOut(
                 new AuthenticationProperties
                 {
                     RedirectUri = callbackUrl
                 },
                CookieAuthenticationDefaults.AuthenticationScheme,
                scheme);
        }

        [HttpGet("{scheme?}")]
        public IActionResult ResetPassword([FromRoute] string scheme)
        {
            scheme = scheme ?? OpenIdConnectDefaults.AuthenticationScheme;

            var redirectUrl = Url.Content("~/");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            properties.Items["policy"] = _options.CurrentValue?.ResetPasswordPolicyId;
            return Challenge(properties, scheme);
        }

        [HttpGet("{scheme?}")]
        public async Task<IActionResult> EditProfile([FromRoute] string scheme)
        {
            scheme = scheme ?? OpenIdConnectDefaults.AuthenticationScheme;
            var authenticated = await HttpContext.AuthenticateAsync(scheme);
            if (!authenticated.Succeeded)
            {
                return Challenge(scheme);
            }

            var redirectUrl = Url.Content("~/");
            var properties = new AuthenticationProperties { RedirectUri = redirectUrl };
            properties.Items["policy"] = _options.CurrentValue?.EditProfilePolicyId;
            return Challenge(properties, scheme);
        }
    }
}
