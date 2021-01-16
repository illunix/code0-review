using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Ravency
{
    public class Cart
    {
        public IList<Product> Products { get; init; }
        public decimal TotalPrice
        {
            get
            {
                decimal totalPrice = 0;
                foreach (var product in Products)
                {
                    totalPrice += product.Price * product.Quantity;
                }

                return totalPrice;
            }
        }

        public record Product
        {
            public string Name { get; init; }
            public string ImageUrl { get; init; }
            public decimal Price { get; init; }
            public int Quantity { get; init; }
        }
    }
}
