
namespace InstaHangouts.Interfaces
{
    using System.Collections.Generic;
    using InstaHangouts.Models;
    using InstaHangouts.Models.Common;

    /// <summary>
    /// Interface IUserRepository
    /// </summary>    
    public interface IUserRepository
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
        bool DeactiveUser(int userId);

        /// <summary>
        /// De active the user.
        /// </summary>
        /// <param name="userId">The user Id.</param>
        /// <returns>bool Value.</returns>
        bool ActiveUser(int userId);

        /// <summary>
        /// Gets the user by identifier.
        /// </summary>
        /// <param name="userId">The identifier.</param>
        /// <returns>User Model</returns>
        UserModel GetUserById(int userId);

        UserModel CheckingValidations(UserModel newUser, string strAction);

        UserModel GetUserByEmailId(string emailId);

        UserModel CheckingValidationsAndRetrunUser(UserModel newUser);

        CommonResponse UpdateEmail(UserModel userModel);

        List<UserModel> BulkUserAddOrUopdate(List<UserModel> lstuser);

        EmailTemplateModel GetEmailTemplate(string code);
    }
}
