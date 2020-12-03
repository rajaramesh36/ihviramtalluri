namespace InstaHangouts.Interfaces.BusinessInterface
{
    using InstaHangouts.Models;
    using InstaHangouts.Models.Common;
    using System;
    using System.Collections.Generic;
    using System.IO;

    /// <summary>
    /// Interface IUserService
    /// </summary>
    public interface IUserService
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of all user model</returns>
        List<UserModel> GetAllUsers();

        /// <summary>
        /// Edits the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns><c>true</c> if XXXX, <c>false</c> otherwise.</returns>
        CommonResponse EditUser(UserModel user);

        /// <summary>
        /// Adds the user.
        /// </summary>
        /// <param name="user">The user.</param>
        /// <returns>bool user.</returns>
        CommonResponse AddUser(UserModel user);

        /// <summary>
        /// De active the user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>bool Value.</returns>
        CommonResponse DeactiveUser(int userId);

        /// <summary>
        /// De active the user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>bool Value.</returns>
        CommonResponse ActiveUser(int userId);

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userId">The identifier.</param>
        /// <returns>User Model</returns>
        UserModel GetUserById(int userId);

        /// <summary>
        /// Gets the user by identifier with all details.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <returns>User Model.</returns>
        UserModel GetUserByIdWithAllDetails(int userId);

        CommonResponse CheckingValidations(UserModel newUser, string strAction);

        UserModel LoginInfo(string emailId, string password);

        CommonResponse SendReactivationLink(InstaHangouts.Models.UserModel user);

        CommonResponse ForgotPassword(string emailId);
    }
}
