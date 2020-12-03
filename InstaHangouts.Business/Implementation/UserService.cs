using InstaHangouts.Interfaces;
using InstaHangouts.Interfaces.BusinessInterface;
using InstaHangouts.Models;
using InstaHangouts.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using InstaHangouts.Common.Constants;
using System.Web.Security;
using InstaHangouts.Common.Encryption;
using System.Configuration;
using InstaHangouts.Common.SendMail;
using NLog;

namespace InstaHangouts.Business
{
    public class UserService : IUserService
    {
        #region Private variables
        /// <summary>
        /// Unit of work object
        /// </summary>
        private readonly IUnitOfWorkRepository unitOfWork;

        /// <summary>
        /// The unit of work
        /// </summary>
        private static Logger logger = LogManager.GetCurrentClassLogger();
        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="UserService"/> class.
        /// </summary>
        /// <param name="unitofWork">The unit of work.</param>
        public UserService(IUnitOfWorkRepository unitofWork)
        {
            this.unitOfWork = unitofWork;            
        }

        public CommonResponse ActiveUser(int userId)
        {
            CommonResponse commonResponseModel = new CommonResponse();
            try
            {
                commonResponseModel.IsSuccess = this.unitOfWork.UserRepository.ActiveUser(userId);
                if (commonResponseModel.IsSuccess == true)
                {
                    commonResponseModel.StatusMessage = Messages.ActiveUserMessage;
                }
                else
                {
                    commonResponseModel.StatusMessage = Messages.ActiveUserFailed;
                }
            }
            catch (Exception ex)
            {
                commonResponseModel.IsSuccess = false;
                logger.Error(ex.StackTrace.ToString());
                commonResponseModel.StatusMessage = Messages.Exception;                
            }

            return commonResponseModel;
        }        

        public CommonResponse AddUser(InstaHangouts.Models.UserModel user)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                commonResponse = this.CheckingValidations(user, "Add");
                if (commonResponse.IsSuccess)
                {
                    user.IsActive = false;
                    string password = Membership.GeneratePassword(12, 3);
                    user.IsFirstLogin = false;
                    user.Password = password;
                    user.EventAttender = false;
                    var addResponse = this.unitOfWork.UserRepository.AddUser(user);
                    if (addResponse.IsSuccess)
                    {
                        user.UserId = Convert.ToInt32(addResponse.UserId);
                        ////Mail sending function
                        this.SendMail(user, Messages.NewUser);
                        commonResponse.IsSuccess = true;
                        commonResponse.UserId = addResponse.UserId;
                        user.UserId = Convert.ToInt32(addResponse.UserId);
                        commonResponse.StatusMessage = Messages.AddUserUpdatedMessage;
                        commonResponse.userInfo = user;
                        commonResponse.UserId = addResponse.UserId;
                    }
                    else
                    {
                        commonResponse.IsSuccess = false;
                        commonResponse.StatusMessage = Messages.ActiveUserFailed;
                        commonResponse.userInfo = user;
                        commonResponse.UserId = addResponse.UserId;
                    }
                }
                else if (commonResponse.IsSuccess == false && commonResponse.StatusMessage == Messages.UserexistsWithFalse)
                {
                    user.IsActive = false;
                    string password = Membership.GeneratePassword(12, 3);
                    user.IsFirstLogin = false;
                    user.Password = password;
                    user.EventAttender = false;
                    user.Message = Messages.UserexistsWithFalse;
                    user.UserId = Convert.ToInt32(commonResponse.userInfo.UserId);
                    var addResponse = this.unitOfWork.UserRepository.EditUser(user);
                    if (addResponse.IsSuccess)
                    {
                        user.UserId = Convert.ToInt32(addResponse.UserId);
                        ////Mail sending function
                        this.SendMail(user, Messages.NewUser);
                        commonResponse.IsSuccess = true;
                        commonResponse.UserId = addResponse.UserId;
                        user.UserId = Convert.ToInt32(addResponse.UserId);
                        commonResponse.StatusMessage = Messages.AddUserUpdatedMessage;
                        commonResponse.userInfo = user;
                    }
                    else
                    {
                        commonResponse.IsSuccess = false;
                        commonResponse.StatusMessage = Messages.ActiveUserFailed;
                        commonResponse.userInfo = user;
                        commonResponse.UserId = addResponse.UserId;
                    }
                }

                return commonResponse;
            }
            catch (Exception ex)
            {
                commonResponse.IsSuccess = false;
                commonResponse.StatusMessage = Messages.Exception;
                commonResponse.userInfo = user;
                logger.Error(ex.StackTrace.ToString());
                return commonResponse;
            }            
        }

        public CommonResponse SendReactivationLink(InstaHangouts.Models.UserModel user)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                commonResponse = this.unitOfWork.UserRepository.UpdateEmail(user);
                ////Mail sending function
                this.SendMail(user, Messages.ResendActivation);
                commonResponse.IsSuccess = true;
                commonResponse.StatusMessage = Messages.UserResendMessage;                         
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
                commonResponse.IsSuccess = false;
                commonResponse.StatusMessage = Messages.Exception;
            }

            return commonResponse;
        }

        public CommonResponse DeactiveUser(int userId)
        {
            CommonResponse commonResponseModel = new CommonResponse();
            try
            {
                commonResponseModel.IsSuccess = this.unitOfWork.UserRepository.DeactiveUser(userId);
                if (commonResponseModel.IsSuccess == true)
                {
                    commonResponseModel.StatusMessage = Messages.DeactiveUserMessage;
                }
                else
                {
                    commonResponseModel.StatusMessage = Messages.DeactiveUserFailed;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
                commonResponseModel.IsSuccess = false;
                commonResponseModel.StatusMessage = Messages.UserAlreadyError;
            }
            return commonResponseModel;
        }

        public CommonResponse EditUser(InstaHangouts.Models.UserModel user)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                commonResponse = this.CheckingValidations(user, "Edit");
                if (commonResponse.IsSuccess)
                {   
                    string password = user.Password;                    
                    user.Password = password;
                    user.IsActive = true;
                    user.IsEmailValidated = true;
                    user.EventAttender = false;
                    user.IsFirstLogin = false;
                    commonResponse = this.unitOfWork.UserRepository.EditUser(user);
                    if (commonResponse.IsSuccess)
                    {
                        commonResponse.UserId = user.UserId.ToString();
                        commonResponse.IsSuccess = true;
                        commonResponse.StatusMessage = Messages.EditUserUpdatedMessage;
                    }
                    else
                    {
                        commonResponse.UserId = user.UserId.ToString();
                        commonResponse.IsSuccess = false;
                        commonResponse.userInfo = user;
                        commonResponse.StatusMessage = Messages.EditUserUpdatedFailed;
                    }
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
                commonResponse.IsSuccess = false;
                commonResponse.StatusMessage = Messages.UserAlreadyError;
            }
            return commonResponse;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Returns list of user Model.</returns>
        public List<UserModel> GetAllUsers()
        {
            List<UserModel> userModel = new List<UserModel>();
            try
            {
                logger.Info("get users");
                userModel = this.unitOfWork.UserRepository.GetAllUsers();
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
            }

            return userModel;
        }

        public UserModel GetUserById(int userId)
        {
            UserModel userModel;
            try
            {
                
                userModel = this.unitOfWork.UserRepository.GetUserById(userId);
            }
            catch (Exception ex)
            {
                userModel = null;
                logger.Error(ex.StackTrace.ToString());
            }
            return userModel;
        }

        public UserModel GetUserByIdWithAllDetails(int userId)
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Checking the validations.
        /// </summary>
        /// <param name="newUser">The new user.</param>
        /// <param name="strAction">The string action.</param>
        /// <returns>Common Response Model.</returns>
        public CommonResponse CheckingValidations(UserModel newUser, string strAction)
        {
            CommonResponse commonResponse = new CommonResponse();            
            try
            {
                var value = this.unitOfWork.UserRepository.CheckingValidations(newUser, strAction);
                if (value == null)
                {
                    commonResponse.StatusMessage = Messages.UserNew;
                    commonResponse.IsSuccess = true;                   
                }
                else if (strAction == "Add" && value.IsActive == false)
                {
                    commonResponse.StatusMessage = Messages.UserexistsWithFalse;
                    commonResponse.IsSuccess = false;
                    commonResponse.userInfo = value;
                }
                else
                {
                    commonResponse.StatusMessage = Messages.UserAlready;
                    commonResponse.IsSuccess = false;
                    commonResponse.userInfo = value;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
                commonResponse.IsSuccess = false;
                commonResponse.StatusMessage = Messages.UserAlreadyError;
            }

            return commonResponse;
        }

        /// <summary>
        /// Get user details based on login and password
        /// </summary>
        /// <param name="loginId">login id</param>
        /// <param name="password">user password</param>
        /// <returns>Login Response Model.</returns>
        public UserModel LoginInfo(string emailId, string password)
        {
            UserModel loginResponseModel = new UserModel();
            try
            {
                //var decrptPassword = AESEncryptDecrypt.DecryptStringJS(password);
                loginResponseModel = this.unitOfWork.UserRepository.GetUserByEmailId(emailId);
                //var decrptPassword = AESEncryptDecrypt.DecryptStringJS(loginResponseModel.Password);
                if (loginResponseModel == null)
                {
                    loginResponseModel = new UserModel();
                    loginResponseModel.UserErrorMessage = Messages.EmailNotExists;
                    return loginResponseModel;
                }

                if (loginResponseModel != null)
                {
                    //string passwordvalue = decrptPassword;
                    /*Compare password*/
                    if (loginResponseModel.Password != password)
                    {
                        loginResponseModel.UserErrorMessage = Messages.InvalidPassword;
                        return loginResponseModel;
                    }
                    else
                    {
                        loginResponseModel.UserErrorMessage = Messages.LoginSuccesfuly;
                        return loginResponseModel;
                    }
                }
            }
            catch ( Exception ex)
            {
                loginResponseModel.UserErrorMessage = ex.Message;
                throw ex;                
            }

            return loginResponseModel;
        }

        public CommonResponse ForgotPassword(string emailId)
        {
            CommonResponse commonResponse = new CommonResponse();
            UserModel respone = new UserModel();
            try
            {
                respone = this.unitOfWork.UserRepository.GetUserByEmailId(emailId);
                if (respone == null)
                {
                    commonResponse.userInfo = null;
                    commonResponse.IsSuccess = false;
                    commonResponse.StatusMessage = Messages.EmailIdNotExits;
                    return commonResponse;
                }
                ////Mail sending function
                this.SendMail(respone, Messages.ForgotPassword);
                commonResponse.userInfo = respone;
                commonResponse.IsSuccess = true;
                commonResponse.StatusMessage = Messages.ForgotPassowordSuccess;                
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
                commonResponse.userInfo = respone;
                commonResponse.IsSuccess = false;
                commonResponse.StatusMessage = Messages.UserAlreadyError;
            }
            return commonResponse;
        }
        
        /// <summary>
        /// Sends the mail.
        /// </summary>
        /// <param name="appUser">The application user.</param>
        /// <param name="password">The password.</param>
        /// <param name="templateName">Name of the template.</param>
        private void SendMail(UserModel appUser, string strMailType)
        {
            string strSubject = string.Empty;
            string strBody = string.Empty;
            var emailTemplate = this.unitOfWork.UserRepository.GetEmailTemplate(strMailType);
            string link = string.Format(ConfigurationManager.AppSettings["WebsiteLink"] +"?user_verified=true&userid={0}", appUser.UserId);
            if (strMailType == Messages.NewUser)
            {
                strSubject = "New User Details";
                strBody = string.Format("Thanks for registering, please link and below link to complete the registeration /n {0}?user_verified=true&userid={1}", ConfigurationManager.AppSettings["WebsiteLink"],  appUser.UserId);
            }
            else if (strMailType == Messages.EditUser)
            {
                strSubject = "Completed registerion User Details";
                strBody = string.Format("Thanks for completing full registerion, Please navigate to my service /n {0}/?user_verified=true&userid={1}", ConfigurationManager.AppSettings["WebsiteLink"], appUser.UserId);
            }
            else if (strMailType == Messages.ResendActivation)
            {
                strSubject = "Resend Activation Link";
                strBody = string.Format("Thanks for registering, please link and below link to complete the registeration /n {0}/?user_verified=true&userid={1}", ConfigurationManager.AppSettings["WebsiteLink"], appUser.UserId);
            }
            else if (strMailType == Messages.ForgotPassword)
            {
                link = string.Format(ConfigurationManager.AppSettings["WebsiteLink"] + "?reset=true&userid={0}", appUser.UserId);
                strSubject = "Forgot Password";
                strBody = string.Format("please link and below link to reset the password /n {0}/?user_verified=true&userid={1}", ConfigurationManager.AppSettings["WebsiteLink"], appUser.UserId);
            }

            ////Mail sending function
            string toAddresses = appUser.EmailId;
            Dictionary<string, string> dict = new Dictionary<string, string>();
            dict.Add("FullName", appUser.FullName);
            dict.Add("Date", DateTime.UtcNow.ToLongDateString());
            dict.Add("UserID", appUser.UserId.ToString());            
            dict.Add("InstaHangoutsUrl", link);
            dict.Add("WebsiteUrl", ConfigurationManager.AppSettings["WebsiteLink"]);
            dict.Add("MailTo", ConfigurationManager.AppSettings["FromEmail"]);
            SendMail sm = new SendMail();
            sm.Sendmail(toAddresses, ConfigurationManager.AppSettings["FromEmail"], emailTemplate.MailSubject, appUser, emailTemplate.Template, dict);
        }
    }
}
