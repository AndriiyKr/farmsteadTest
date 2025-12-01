// <copyright file="ChangePasswordRequestDTO.cs" company="PlaceholderCompany">
// Copyright (c) PlaceholderCompany. All rights reserved.
// </copyright>

namespace FarmsteadMap.BLL.Data.DTO
{
    /// <summary>
    /// Data Transfer Object used for processing password change requests.
    /// </summary>
    public class ChangePasswordRequestDTO
    {
        /// <summary>
        /// Gets or sets the user's current password.
        /// </summary>
        required public string OldPassword { get; set; }

        /// <summary>
        /// Gets or sets the new password.
        /// </summary>
        required public string NewPassword { get; set; }

        /// <summary>
        /// Gets or sets the confirmation of the new password.
        /// </summary>
        required public string ConfirmNewPassword { get; set; }
    }
}