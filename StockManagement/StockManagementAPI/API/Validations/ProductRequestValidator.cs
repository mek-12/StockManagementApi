using FluentValidation;
using StockManagementAPI.API.Models;

namespace StockManagementAPI.API.Validations {
    public class ProductRequestValidator : AbstractValidator<ProductRequest> {
        public ProductRequestValidator() {
            RuleFor(x => x.Name)
                .NotEmpty().WithMessage("Ürün ismi boş olamaz.")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("Ürün ismi sadece harflerden oluşmalıdır.");

            RuleFor(x => x.StockQuantity)
                .GreaterThan(0).WithMessage("Stok adedi sıfırdan büyük olmalıdır.");

            RuleFor(x => x.Price)
                .GreaterThan(0).WithMessage("Fiyat sıfırdan büyük olmalıdır.");
        }
    }
}
