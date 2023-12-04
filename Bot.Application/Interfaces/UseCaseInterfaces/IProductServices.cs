using Bot.Application.Services.UseCases;
using Bot.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Interfaces.UseCaseInterfaces
{
    public interface IProductServices
    {
        Task<List<Product>> GetAll(CancellationToken cancellationToken);
        Task<List<ProductType>> GetAllType(CancellationToken cancellationToken);
        Task<ProductType> CreateProductType(ProductType productType, CancellationToken cancellationToken);
        Task<Product> CreateProduct(ProductCreateDto product, CancellationToken cancellationToken);
        Task<bool> DeleteProduct(int id, CancellationToken cancellationToken);
        Task<bool> DeleteProductType(int id, CancellationToken cancellationToken);
    }
}
