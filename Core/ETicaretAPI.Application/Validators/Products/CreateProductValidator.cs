using ETicaretAPI.Application.ViewModels.Products;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ETicaretAPI.Application.Validators.Products
{
    public class CreateProductValidator : AbstractValidator<VM_Create_Product>
    {
        public CreateProductValidator()
        {
            RuleFor(p => p.Name)
              .NotEmpty()
              .NotNull().WithMessage("Lütfen Ürün Adını Boş Geçmeyiniz")
              .MaximumLength(150).MinimumLength(2).WithMessage("Ürün Adını 2 ile 150 Karakter Arası Giriniz");

            RuleFor(p => p.Stock)
              .NotEmpty()
              .NotNull().WithMessage("Lütfen Stok Bilgisini Boş Geçmeyiniz")
              .Must(s => s >= 0).WithMessage("Stok Bilgisi Sıfırın Altında Olamaz");

            RuleFor(p => p.Price)
             .NotEmpty()
             .NotNull().WithMessage("Lütfen Fiyat Bilgisini Boş Geçmeyiniz")
             .Must(p => p >= 0).WithMessage("Fiyat Bilgisi Sıfırın Altında Olamaz");
        }
    }
}
