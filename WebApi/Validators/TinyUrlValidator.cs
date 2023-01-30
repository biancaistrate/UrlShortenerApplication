using FluentValidation;
using WebApi.Models;

namespace WebApi.Validators
{
    public class TinyUrlValidator: AbstractValidator<TinyUrlDTO>
    {
        public TinyUrlValidator()
        {
            RuleFor(url => url.Alias).Length(0, 16).WithMessage("Specify a shorter alias");
            RuleFor(url => url.OriginalUrl).NotNull().NotEmpty().MaximumLength(300).WithMessage("Write a url shorter than 300 caracters");
        }
    }
}
