using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using SlickOne.Biz.Entity;

namespace SlickOne.Biz.Service
{
    /// <summary>
    /// product service interface
    /// </summary>
    public interface IProductService
    {
        ProductEntity Get(int id);
        List<ProductEntity> GetProductList();
        List<ProductEntity> Query(ProductQuery query);
        ProductEntity Save(ProductEntity entity);
        void Delete(int id);
    }
}
