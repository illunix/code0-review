using AutoMapper;
using Convey.CQRS.Commands;
using Convey.CQRS.Queries;
using Microsoft.EntityFrameworkCore;
using Ravency.Data;
using Ravency.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Products
{
    public class Add
    {
        public record Command : ICommand
        {
            public IReadOnlyList<Language<Product>> Languages { get; init; }

            public decimal Price { get; init; }
            public Guid SubCategoryId { get; init; }

            public record Product
            {
                public string Name { get; init; }
                public string Description { get; init; }
            }
        }

        private class CommandHandler : ICommandHandler<Command>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;

            public CommandHandler(ApplicationDbContext db, IMapper mapper)
            {
                _db = db;
                _mapper = mapper;
            }

            public async Task HandleAsync(Command command)
            {
                var productId = new Guid();

                foreach (var language in command.Languages)
                {
                    if (language.IsDefault)
                    {
                        var product = _mapper.Map<Language<Command.Product>, Product>(language);

                        _mapper.Map(command, product);

                        _db.Products
                            .Add(product);

                        productId = product.Id;
                    }
                    else
                    {
                        var productLocale = _mapper.Map<Language<Command.Product>, ProductLocale>(language);

                        productLocale.ProductId = productId;

                        _db.ProductsLocales
                            .Add(productLocale);
                    }
                }

                await _db.SaveChangesAsync();
            }
        }
    }
}
