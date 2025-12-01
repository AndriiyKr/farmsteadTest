// <copyright file="RegisterUserValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Validators
{
    using FarmsteadMap.BLL.Data.DTO;
    using FluentValidation;

    /// <summary>
    /// Validator for user registration requests.
    /// </summary>
    public class RegisterUserValidator : AbstractValidator<RegisterRequestDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="RegisterUserValidator"/> class.
        /// Configures validation rules for registration requests.
        /// </summary>
        public RegisterUserValidator()
        {
            this.RuleFor(x => x.Email)
                .NotEmpty().WithMessage("Поле Email є обов'язковим.")
                .EmailAddress().WithMessage("Неправильний формат email.");

            this.RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Поле Ім'я користувача є обов'язковим.")
                .MinimumLength(3).WithMessage("Ім'я користувача має містити щонайменше 3 символи.");

            this.RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Поле Пароль є обов'язковим.")
                .MinimumLength(6).WithMessage("Пароль має містити щонайменше 6 символів.");

            this.RuleFor(x => x.ConfirmPassword)
                .NotEmpty().WithMessage("Підтвердження пароля є обов'язковим.")
                .Equal(x => x.Password).WithMessage("Паролі не співпадають.");

            this.RuleFor(x => x.TermsAccepted)
                .Equal(true).WithMessage("Ви повинні прийняти умови використання.");

            this.RuleFor(x => x.PersonalDataAccepted)
                .Equal(true).WithMessage("Ви повинні погодитись на обробку персональних даних.");
        }
    }
}