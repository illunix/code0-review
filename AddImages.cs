using AutoMapper;
using Convey.CQRS.Commands;
using Microsoft.AspNetCore.Http;
using Ravency.Data;
using Ravency.Domain;
using Ravency.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Products
{
    public class AddImages
    {
        public record Command : ICommand
        {
            public Guid Id { get; init; }
            public IFormFile Image { get; set; }
            public bool MainImage { get; init; }
        }

        public class CommandHandler : ICommandHandler<Command>
        {
            private readonly ApplicationDbContext _db;
            private readonly IMapper _mapper;
            private readonly IBlobContainerService _blobContainerService;

            public CommandHandler(ApplicationDbContext db, IMapper mapper, IBlobContainerService blobContainerService)
            {
                _db = db;
                _mapper = mapper;
                _blobContainerService = blobContainerService;
            }

            public async Task HandleAsync(Command command)
            {
                var productImage = _mapper.Map<Command, ProductImage>(command);

                _db.ProductsImages
                    .Add(productImage);

                await _blobContainerService.Upload(command.Image);
            }
        }
    }
}
