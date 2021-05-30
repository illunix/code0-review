using System;
using Ravency.Infrastructure.Models;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Categories.Models
{
    public class CategoryLocale : BaseEntity
    {
        public Guid CategoryId { get; private set; }
        public Guid LanguageId { get; private set; }
        public string Name { get; private set; }

        public CategoryLocale(Guid categoryId, Guid languageId, string name)
        {
            CategoryId = categoryId;
            LanguageId = languageId;
            Name = name;
        }
    }
}