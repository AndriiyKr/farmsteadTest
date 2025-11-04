using FarmsteadMap.BLL.Data.DTO;
using FluentValidation;

namespace FarmsteadMap.BLL.Validators
{
    public class LoginUserValidator : AbstractValidator<LoginRequestDTO>
    {
        public LoginUserValidator()
        {
            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Ім'я користувача є обов'язковим.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим.");
        }
    }
}