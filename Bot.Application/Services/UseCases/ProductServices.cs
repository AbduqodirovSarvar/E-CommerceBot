using Bot.Application.Interfaces.DbInterfaces;
using Bot.Application.Interfaces.FileServiceInterfaces;
using Bot.Application.Interfaces.UseCaseInterfaces;
using Bot.Domain.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO.Abstractions;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Bot.Application.Services.UseCases
{
    public class ProductServices : IProductServices
    {
        private readonly IAppDbContext _context;
        private readonly IFileService _fileService;
        public ProductServices(
            IAppDbContext context,
            IFileService fileService)
        {
            _context = context;
            _fileService = fileService;
        }
        public async Task<Product> CreateProduct(ProductCreateDto productDto, CancellationToken cancellationToken)
        {
            var result = await _context.Products.AnyAsync(x => x.NameEN == productDto.NameEN, cancellationToken);
            if (result)
            {
                throw new Exception();
            }

            var product = new Product()
            {
                NameEN = productDto.NameEN,
                NameRU = productDto.NameRU,
                NameUZ = productDto.NameUZ,
                DescriptionEN = productDto.DescriptionEN,
                DescriptionRU = productDto.DescriptionRU,
                DescriptionUZ = productDto.DescriptionUZ,
                Price = productDto.Price,
                TypeId = productDto.TypeId,
                ImagePath = await _fileService.Save(productDto.Image!)
            };

            await _context.Products.AddAsync(product, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return product;
        }

        public async Task<ProductType> CreateProductType(ProductType productType, CancellationToken cancellationToken)
        {
            var result = await _context.Products.AnyAsync(x => x.NameEN == productType.NameEN, cancellationToken);
            if (result)
            {
                throw new Exception();
            }

            await _context.ProductTypes.AddAsync(productType, cancellationToken);
            await _context.SaveChangesAsync(cancellationToken);
            return productType;
        }

        public async Task<bool> DeleteProduct(int id, CancellationToken cancellationToken)
        {
            var product = await _context.Products.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new Exception();

            _context.Products.Remove(product);
            return (await _context.SaveChangesAsync(cancellationToken)) > 0;
        }

        public async Task<bool> DeleteProductType(int id, CancellationToken cancellationToken)
        {
            var productType = await _context.ProductTypes.FirstOrDefaultAsync(x => x.Id == id, cancellationToken) ?? throw new Exception();

            _context.ProductTypes.Remove(productType);
            return (await _context.SaveChangesAsync(cancellationToken)) > 0;
        }

        public async Task<List<Product>> GetAll(CancellationToken cancellationToken)
        {
            var products = await _context.Products.ToListAsync(cancellationToken);
            return products;
        }

        public async Task<List<ProductType>> GetAllType(CancellationToken cancellationToken)
        {
            var productTypes = await _context.ProductTypes.ToListAsync(cancellationToken);
            return productTypes;
        }
    }

    public class ProductCreateDto
    {
        public string NameUZ { get; set; } = null!;
        public string NameEN { get; set; } = null!;
        public string NameRU { get; set; } = null!;
        public string? DescriptionUZ { get; set; }
        public string? DescriptionEN { get; set; }
        public string? DescriptionRU { get; set; }
        public double Price { get; set; }
        public IFormFile? Image { get; set; }
        public int TypeId { get; set; }
    }
}
