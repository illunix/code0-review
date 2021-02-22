using MediatR;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency.Areas.Panel.SubAreas.Configuration.PaymentMethods.Components.EditPaymentMethodModal
{
    public partial class EditPaymentMethodModalViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
            => View(new Edit.Command());
    }
}
