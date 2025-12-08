// <copyright file="LoginUserValidator.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Validators
{
    using FarmsteadMap.BLL.Data.DTO;
    using FluentValidation;

    /// <summary>
    /// Validator for user login requests.
    /// </summary>
    public class LoginUserValidator : AbstractValidator<LoginRequestDTO>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoginUserValidator"/> class.
        /// Configures validation rules for login requests.
        /// </summary>
        public LoginUserValidator()
        {
            this.RuleFor(x => x.Username)
                .NotEmpty().WithMessage("Ім'я користувача є обов'язковим.");

            this.RuleFor(x => x.Password)
                .NotEmpty().WithMessage("Пароль є обов'язковим.");
        }
    }
}