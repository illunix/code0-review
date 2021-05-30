using Ravency.Infrastructure.Models;

namespace Ravency.Areas.Panel.SubAreas.Catalog.Categories.Models
{
    public class Category : BaseEntity
    {
        public string Name { get; private set; }
        public int Gender { get; private set; }
        
        public Category(
            string name,
            int gender
        )
        {
            Name = name;
            Gender = gender;
        }
    }
}