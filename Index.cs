using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using GenerateMediator;
using Microsoft.EntityFrameworkCore;
using Ravency.Infrastructure.Data;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Categories
{
    [GenerateMediator]
    public static partial class Index
    {
        public sealed partial record Query;

        public record Category(
            Guid Id,
            string Name,
            int Gender
        );

        public record Model(IEnumerable<Category> Categories);
        
        public static async Task<Model> QueryHandler(ApplicationDbContext context)
        {
            var categories = await context.Categories
                .Select(
                    p => new Category(
                        p.Id,
                        p.Name,
                        p.Gender
                    )
                )
                .ToListAsync();
            
            return new Model(categories);
        }
    }
}