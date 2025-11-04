using FarmsteadMap.BLL.Data.DTO;
using FluentValidation;

namespace FarmsteadMap.BLL.Validators
{
    public class RegisterUserValidator : AbstractValidator<RegisterRequestDTO>
    {
        public RegisterUserValidator()
        {
            RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Поле Email є обов'язковим.")
                .EmailAddress().WithMessage("Неправильний формат email.");

            RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Поле Ім'я користувача є обов'язковим.")
                .MinimumLength(3).WithMessage("Ім'я користувача має містити щонайменше 3 символи.");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле Пароль є обов'язковим.")
                .MinimumLength(6).WithMessage("Пароль має містити щонайменше 6 символів.");

            RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Підтвердження пароля є обов'язковим.")
                .Equal(x => x.Password).WithMessage("Паролі не співпадають.");

            RuleFor(x => x.TermsAccepted)
                .Equal(true).WithMessage("Ви повинні прийняти умови використання.");

            RuleFor(x => x.PersonalDataAccepted)
                .Equal(true).WithMessage("Ви повинні погодитись на обробку персональних даних.");
        }
    }
}