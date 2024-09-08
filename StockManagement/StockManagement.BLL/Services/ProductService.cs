using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using StockManagement.CCC.Models;
using StockManagement.CCC.Entities;
using StockManagement.DAL.Interfces;
using StockManagement.BLL.Interfaces;
using AutoMapper;
using Hangfire;
using Microsoft.Extensions.Configuration;
using System.Reflection.Metadata;
using StockManagement.CCC;

namespace StockManagement.BLL.Services {
    public class ProductService : IProductService {
        private readonly IEmailService _emailService;
        private readonly int _cacheDuration;
        private readonly ICacheService _cacheService;
        private readonly IProductRepository _productRepository;
        private readonly IMapper _mapper;
        private readonly IConfiguration _configuration;
        private int HangfireScheduleTime {
            get {
                return _configuration.GetValue<int>(Constants.HANGFIRE_SCHEDULE_TIME);
            }
        }
        public ProductService(IEmailService emailService, IProductRepository productRepository, ICacheService cacheService, IOptions<CacheSettings> cacheSettings, IMapper mapper, IConfiguration configuration) {
            _productRepository = productRepository;
            _emailService = emailService;
            _cacheDuration = cacheSettings.Value.DefaultCacheDuration;
            _cacheService = cacheService;
            _mapper = mapper;
            _configuration = configuration;
        }

        public async Task<ProductResponse> CreateProductAsync(ProductRequest request) {
            var product = new Product
            {
                Name = request.Name,
                StockQuantity = request.StockQuantity,
                Price = request.Price,
                PriceHistories = new List<PriceHistory>()
            };

            await _productRepository.AddAsync(product);
            await _productRepository.SaveChangesAsync();

            return _mapper.Map<ProductResponse>(product);
        }

        public async Task<UpdatePriceResponse?> UpdateProductPriceAsync(int id, UpdatePriceRequest request) {
            var product = await _productRepository.AsQueryable().Include(p => p.PriceHistories).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;

            var priceHistory = new PriceHistory {
                ProductId = id,
                OldPrice = product.Price,
                NewPrice = request.NewPrice,
                ChangeDate = DateTime.UtcNow.AddMinutes(5) // TO DO Bu süre configden okunabilir.
            };

            product.PriceHistories?.Add(priceHistory);
            //_ = Task.Run(async () => {
            //    await Task.Delay(TimeSpan.FromMinutes(HangfireScheduleTime));
            //    product.Price = request.NewPrice;

            //    await _productRepository.SaveChangesAsync();
            //    await _emailService.SendEmailAsync("diazinfo@example.com", "Price Update", $"The price for {product.Name} has been updated to {product.Price}");
            //});
            BackgroundJob.Schedule(() => UpdatePriceAndSendEmailAsync(product.Id, request.NewPrice, product.Name), TimeSpan.FromMinutes(HangfireScheduleTime));

            var response = _mapper.Map<UpdatePriceResponse>(product);
            response.ScheduledUpdate = priceHistory.ChangeDate;

            return response;
        }

        public async Task UpdatePriceAndSendEmailAsync(int productId, decimal newPrice, string? productName) {
            var product = await _productRepository.FindAsync(productId);
            if (product != null) {
                product.Price = newPrice;
                await _productRepository.SaveChangesAsync();

                // E-posta gönderme işlemi
                await _emailService.SendEmailAsync("diazinfo@example.com", "Price Update", $"The price for {productName} has been updated to {product.Price}");
            }
        }

        public async Task<IEnumerable<ProductResponse>?> GetProductsAsync(decimal? minPrice, decimal? maxPrice, int? minStock) {
            var cacheKey = $"products-{minPrice}-{maxPrice}-{minStock}";
            var cachedProducts = _cacheService.Get<List<ProductResponse>>(cacheKey);
            if (cachedProducts == null) {
                var query = _productRepository.AsQueryable();

                if (minPrice.HasValue)
                    query = query.Where(p => p.Price >= minPrice.Value);

                if (maxPrice.HasValue)
                    query = query.Where(p => p.Price <= maxPrice.Value);

                if (minStock.HasValue)
                    query = query.Where(p => p.StockQuantity >= minStock.Value);

                var products = await query.Include(p => p.PriceHistories).ToListAsync();

                cachedProducts = _mapper.Map<List<ProductResponse>>(products);

                _cacheService.Set(cacheKey, cachedProducts, TimeSpan.FromSeconds(_cacheDuration));
            }

            return cachedProducts;
        }

        public async Task<ProductResponse> GetProductByIdAsync(int id) {
            var product = await _productRepository.AsQueryable().Include(p => p.PriceHistories).FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
                return null;

            return _mapper.Map<ProductResponse>(product);
        }

    }
}
