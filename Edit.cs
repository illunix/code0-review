using System;
using System.Threading.Tasks;
using GenerateMediator;
using Ravency.Data;

namespace Ravency.Areas.Panel.SubAreas.Configuration.PaymentMethods
{
    [GenerateMediator]
    public static partial class Edit
    {
        public sealed partial record Command
        { 
            public Guid Id { get; init; }
            public string ClientId { get; init; }
            public string ClientSecret { get; init; }
            public bool IsActive { get; init; }
        }
        
        public static async Task CommandHandler(Command command, ApplicationDbContext context)
        {
            var paymentMethod = await context.PaymentMethods
                .FindAsync(command.Id);

            paymentMethod.ClientId = command.ClientId;
            paymentMethod.ClientSecret = command.ClientSecret;
            paymentMethod.IsActive = command.IsActive;
            
            context.Update(paymentMethod);

            await context.SaveChangesAsync();
        }
    }
}