using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ravency.Infrastructure;
using Ravency.Infrastructure.Extensions;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Categories
{
    [SubArea("Catalog")]
    public partial class CategoriesController : BaseController
    {
        private readonly IMediator _mediator;

        public async Task<IActionResult> Index(Index.Query query)
            => View(await _mediator.Send(query));

        public async Task<IActionResult> Add(Add.Query query)
            => Ok(await _mediator.Send(query));
        
        [HttpPost]
        public async Task<IActionResult> Add(Add.Command command)
        {
            if (!ModelState.IsValid)
            {
                return this.RedirectToActionJson(nameof(Index));
            }

            await _mediator.Send(command);

            TempData["ToastrSuccess"] = "Successfully added new category";

            return this.RedirectToActionJson(nameof(Index));
        }
    }
}