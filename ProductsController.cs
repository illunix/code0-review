using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Ravency.Infrastructure;
using Ravency.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Products
{
    [Area("Panel")]
    [SubArea("Catalog")]
    public class ProductsController : Controller
    {
        private readonly IDispatcher _dispatcher;

        public ProductsController(IDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        public async Task<IActionResult> Index(Index.Query query)
            => View(await _dispatcher.Query(query));

        public async Task<IActionResult> Add(Add.Command command)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            await _dispatcher.Send(command);

            TempData["ToastrSuccess"] = "Successfully added product.";

            return RedirectToAction(nameof(Index));
        }

        public async Task<IActionResult> AddImage(IFormFile file)
        {
            await _dispatcher.Send(new AddImages.Command { Image = file });

            return Ok();
        }
    }
}
