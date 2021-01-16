using Microsoft.AspNetCore.Mvc;
using Ravency.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency.Components.Cart
{
    public class CartViewComponent : ViewComponent
    {
        private readonly IInMemoryCartService _cartService;
        private readonly IAuthenticationService _authenticationService;

        public CartViewComponent(IInMemoryCartService cartService, IAuthenticationService authenticationService)
        {
            _cartService = cartService;
            _authenticationService = authenticationService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
            => View(await _cartService.GetCart(_authenticationService.User.Id));
    }
}
