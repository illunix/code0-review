using AutoMapper;
using Convey.CQRS.Queries;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Ravency.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Products
{
    public class Index
    {
        public record Query : IQuery<Model>
        {

        }

        public record Result
        {
            public IReadOnlyList<Product> Products { get; init; }
            public IReadOnlyList<Language<Product>> Languages { get; init; }
            public IReadOnlyList<SelectListItem> ProductCategories { get; init; }

            public record Product
            {
                public Guid Id { get; init; }
                public string Name { get; init; }
                public string Description { get; init; }
                public decimal Price { get; init; }
                public ProductCategory Category { get; set; }
                public bool MissData { get; set; }
            }

            public record ProductCategory
            {
                public Guid Id { get; init; }
                public string Name { get; init; }
            }
        }

        public record Model : Result
        {
            public Guid CategoryId { get; init; }
            public decimal Price { get; init; }
        }

        public class QueryHandler : IQueryHandler<Query, Model>
        {
            private readonly ApplicationDbContext _db;
            private readonly IConfigurationProvider _configuration;

            public QueryHandler(ApplicationDbContext db, IConfigurationProvider configuration)
            {
                _db = db;
                _configuration = configuration;
            }

            public async Task<Model> HandleAsync(Query query)
            {
                var products = await _db.Products
                    .Include(x => x.Category)
                    .ProjectToListAsync<Result.Product>(_configuration);

                var languagesCount = await _db.Languages
                    .Where(language => language.IsActive && language.IsDefault == false)
                    .CountAsync();

                foreach (var product in products)
                {
                    var productLocalesCount = await _db.ProductCategoriesLocales
                        .Where(categoryLocale => categoryLocale.CategoryId == product.Id)
                        .CountAsync();

                    if (productLocalesCount != languagesCount)
                    {
                        product.MissData = true;
                    }
                }

                var languages = await _db.Languages
                    .Where(language => language.IsActive)
                    .OrderByDescending(x => x.IsDefault)
                    .ThenBy(language => language.Name)
                    .ProjectToListAsync<Language<Result.Product>>(_configuration);

                var productCategories = await _db.ProductCategories
                    .ProjectToListAsync<SelectListItem>(_configuration);

                return new Model
                {
                    Products = products,
                    Languages = languages,
                    ProductCategories = productCategories
                };
            }
        }
    }
}
