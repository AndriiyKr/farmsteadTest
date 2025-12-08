// <copyright file="LoginSuccessEventArgs.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.WPF
{
    using System;

    /// <summary>
    /// Містить дані для події успішного входу в систему.
    /// </summary>
    public class LoginSuccessEventArgs : EventArgs
    {
        /// <summary>
        /// Gets or sets the ID користувача.
        /// </summary>
        public long UserId { get; set; }

        /// <summary>
        /// Gets or sets the ім'я користувача (username).
        /// </summary>
        public string Username { get; set; } = string.Empty;
    }
}