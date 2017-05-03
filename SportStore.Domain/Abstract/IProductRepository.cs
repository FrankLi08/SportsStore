using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SportStore.Domain.Entities;

namespace SportStore.Domain.Abstract
{
 public  interface IProductRepository
    {
        IEnumerable<Product> Products  { get; }
        void SaveProduct(Product product);
        Product DeleteProduct(int productID);
    }
}
