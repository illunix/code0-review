using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FluentValidation;
using GenerateMediator;
using Microsoft.EntityFrameworkCore;
using Ravency.Areas.Panel.SubAreas.Catalog.Categories.Models;
using Ravency.Infrastructure.Data;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Categories
{
    [GenerateMediator]
    public static partial class Add
    {
        public sealed partial record Query;

        public record Language(
            Guid Id,
            string Name,
            bool IsDefault
        );

        public record Model(IEnumerable<Language> Languages);

        public static async Task<Model> QueryHandler(ApplicationDbContext context)
        {
            var languages = await context.Languages
                .OrderByDescending(x => x.IsDefault)
                .ThenBy(x => x.Name)
                .Select(p => new Language(
                    p.Id,
                    p.Name,
                    p.IsDefault
                ))
                .ToListAsync();

            return new Model(languages);
        }

        public sealed partial record Command(
            string Name,
            int Gender,
            IEnumerable<Locale> Locales
        )
        {
            public static void AddValidation(AbstractValidator<Command> v)
            {
                v.RuleFor(x => x.Name)
                    .NotEmpty().WithMessage("Please enter name.");

                v.RuleFor(x => x.Gender)
                    .Equal(0).WithMessage("Invalid gender.");

                v.RuleForEach(x => x.Locales).ChildRules(locales =>
                {
                    locales.RuleFor(x => x.Name)
                        .NotEmpty().WithMessage("Please enter name.");
                });
            }   
        }

        public record Locale(
            Guid LanguageId,
            string Name
        );

        public static async Task CommandHandler(
            Command command,
            ApplicationDbContext context
        )
        {
            var category = new Category(command.Name, command.Gender);

            context.Categories
                .Add(category);

            foreach (var locale in command.Locales)
            {
                var categoryLocale = new CategoryLocale(category.Id, locale.LanguageId, locale.Name);

                context.CategoryLocales
                    .Add(categoryLocale);
            }

            await context.SaveChangesAsync();
        }
    }
}