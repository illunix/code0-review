using System.Threading.Tasks;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Ravency.Infrastructure;

namespace Ravency.Areas.Panel.SubAreas.Configuration.PaymentMethods
{
    [Area("Panel")]
    [SubArea("Configuration")]
    [PrimaryConstructor]
    public partial class PaymentMethodsController : Controller
    {
        private readonly IMediator _mediator;

        public async Task<IActionResult> Index(Index.Query query)
            => View(await _mediator.Send(query));

        [HttpPost]
        public async Task<IActionResult> Edit(Edit.Command command)
        {
            if (!ModelState.IsValid)
            {
                return RedirectToAction(nameof(Index));
            }

            await _mediator.Send(command);
            
            TempData["ToastrSuccess"] = "Successfully updated payment method";

            return RedirectToAction(nameof(Index));
        }
    }
}