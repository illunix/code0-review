using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using GenerateMediator;
using Ravency.Data;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace Ravency.Areas.Panel.SubAreas.Configuration.PaymentMethods
{
    [GenerateMediator]
    public static partial class Index
    {
        public sealed partial record Query { }

        public record Model(IReadOnlyList<Model.PaymentMethod> PaymentMethods)
        {
            public Guid Id { get; init; }
            public string ClientId { get; init; }
            public string ClientSecret { get; init; }
            public bool IsActive { get; init; }

            public record PaymentMethod
            {
                public Guid Id { get; }
                public string Name { get; }
                public string ClientId { get; }
                public string ClientSecret { get; }
                public bool IsActive { get; }

                public PaymentMethod(Domain.PaymentMethod paymentMethod)
                {
                    Id = paymentMethod.Id;
                    Name = paymentMethod.Name;
                    ClientId = paymentMethod.ClientId;
                    ClientSecret = paymentMethod.ClientSecret;
                    IsActive = paymentMethod.IsActive;
                }
            }
        }

        public static async Task<Model> QueryHandler(Query query, ApplicationDbContext context)
        {
            var paymentMethods = context.PaymentMethods
                .OrderBy(paymentMethod => paymentMethod.IsActive)
                .ThenBy(paymentMethod => paymentMethod.Name)
                .AsQueryable();

            return new Model(await paymentMethods.Select(p => new Model.PaymentMethod(p)).ToListAsync());
        }
    }
}