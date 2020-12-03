using InstaHangouts.Common.Constants;
using InstaHangouts.Dal.Converters;
using InstaHangouts.Interfaces;
using InstaHangouts.Models;
using InstaHangouts.Models.Common;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using System.Data.Entity.Core.EntityClient;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Dal.Repository
{
    public class UserRepository : BaseRepository<UserInfo>, IUserRepository
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public bool ActiveUser(int userId)
        {
            bool active;
            try
            {
                UserInfo item = this.dbContext.UserInfoes.SingleOrDefault(d => d.UserId == userId);
                item.IsActive = true;
                this.dbContext.SaveChanges();
                active = true;
            }
            catch (Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;                
            }

            return active;
        }

        public CommonResponse AddUser(UserModel user)
        {
            try
            {
                CommonResponse commonResponse = new CommonResponse();
                user.IsEmailValidated = false;
                UserInfo userDetails = this.ConvertUserModel(user);                
                this.Add(userDetails);
                user.UserId = userDetails.UserId;
                commonResponse.IsSuccess = true;
                commonResponse.UserId = userDetails.UserId.ToString();
                commonResponse.userInfo = user;
                return commonResponse;
            }
            catch (Exception ex)
            {

                throw ex;
            }
        }

        public List<UserModel> BulkUserAddOrUopdate(List<UserModel> lstuser)
        {
            //update
            try
            {
                List<UserModel> lstUserModel = new List<UserModel>();
                ICollection<UserInfo> lstUserInfo = lstuser.Convert();
                dbContext.Configuration.AutoDetectChangesEnabled = false;
                foreach (var item in lstUserInfo.Where(x => x.UserId == 0))
                {
                    item.IsActive = false;
                    dbContext.Set<UserInfo>().Add(item);
                }

                if (lstUserInfo.Where(x => x.UserId == 0).Count() > 0)
                {
                    dbContext.SaveChanges();
                }

                foreach (var userInfo in lstUserInfo.Where(x => x.UserId != 0))
                {
                    //disable detection of changes to improve performance
                    dbContext.Configuration.AutoDetectChangesEnabled = false;
                    dbContext.Entry(userInfo).State = System.Data.Entity.EntityState.Modified;
                    dbContext.Entry(userInfo).Property(x => x.PhoneNumber).IsModified = true;
                    dbContext.Entry(userInfo).Property(x => x.ZipCode).IsModified = true;
                    dbContext.UserInfoes.Attach(userInfo);                                     
                }

                if (lstUserInfo.Where(x => x.UserId == 0).Count() > 0)
                {
                    dbContext.SaveChanges();
                }

                foreach (var item in lstUserInfo)
                {
                    lstUserModel.Add(GetUserByEmailId(item.EmailId));
                }
                dbContext.Configuration.AutoDetectChangesEnabled = true;
                return lstUserModel;
            }
            catch (Exception ex)
            {
                dbContext.Configuration.AutoDetectChangesEnabled = true;
                throw ex;
            }
        }

        public UserModel CheckingValidations(UserModel newUser, string strAction)
        {
            UserInfo getUserDetails;
            if (strAction == "Add")
            {
                getUserDetails = this.Find(d => d.EmailId == newUser.EmailId);
            }
            else
            {
                getUserDetails = this.Find(d => d.EmailId == newUser.EmailId && d.IsActive == true && d.UserId != d.UserId);
            }

            if (getUserDetails == null)
            {
                return getUserDetails.ConvertUser();
            }
            else
            {
                return getUserDetails.ConvertUser();
            }
        }

        public UserModel CheckingValidationsAndRetrunUser(UserModel newUser)
        {
            UserInfo getUserDetails = this.Find(d => d.EmailId == newUser.EmailId);

            if (getUserDetails == null)
            {   
                return newUser;
            }            
            else
            {
                newUser.UserId = getUserDetails.UserId;
                newUser.UserErrorMessage = "Updating with exsiting User" + getUserDetails.UserId;
                return newUser;
            }
        }

        public bool DeactiveUser(int userId)
        {
            bool isDeactive;
            try
            {
                UserInfo item = this.dbContext.UserInfoes.SingleOrDefault(d => d.UserId == userId);
                item.IsActive = false;
                this.dbContext.SaveChanges();
                isDeactive = true;
            }
            catch (Exception ex)
            {
                this.LogError(ex);
                isDeactive = false;
            }

            return isDeactive;
        }

        public CommonResponse EditUser(UserModel userModel)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                UserInfo userinfo = this.Find(d => d.UserId == userModel.UserId);
                if (userinfo.FullName != userModel.FullName)
                {
                    userinfo.OldName = userinfo.FullName;
                    userinfo.FullName = userModel.FullName;
                }
                else
                {
                    userinfo.FullName = userModel.FullName;
                }

                userinfo.EmailId = userModel.EmailId;
                userinfo.Password = userModel.Password;
                if (userModel.Message == Messages.UserexistsWithFalse)
                {
                    userinfo.IsActive = false;
                    userinfo.IsEmailValidated = false;
                    userinfo.IsFirstLogin = false;
                }
                else
                {
                    userinfo.IsEmailValidated = true;
                    userinfo.IsActive = true;
                    userinfo.IsFirstLogin = true;
                }                
                userinfo.ModifiedBy = userModel.CreatedBy;
                userinfo.Modifiedon = DateTime.UtcNow;                           
                this.Update(userinfo);                
                commonResponse.IsSuccess = true;
                var userDetails = userinfo.ConvertUser();
                commonResponse.userInfo = userDetails;
                commonResponse.UserId = userinfo.UserId.ToString();
            }
            catch (Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }

            return commonResponse;
        }

        public CommonResponse UpdateEmail(UserModel userModel)
        {
            CommonResponse commonResponse = new CommonResponse();
            try
            {
                UserInfo userinfo = this.Find(d => d.UserId == userModel.UserId);
                userinfo.OldEmail = userinfo.EmailId;
                userinfo.EmailId = userModel.EmailId;
                userinfo.Modifiedon = DateTime.UtcNow;
                this.Update(userinfo);
                commonResponse.IsSuccess = true;
                var userinfoDetails = userinfo.ConvertUser();
                commonResponse.userInfo = userinfoDetails;
            }
            catch (Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }

            return commonResponse;
        }

        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>Returns list of user Model.</returns>
        public List<UserModel> GetAllUsers()
        {
            try
            {
                var getValues = dbContext.UserInfoes.ToList();

                List<UserModel> userModel = new List<UserModel>();
                foreach (var item in getValues.ToList())
                {
                    UserModel um = new UserModel();
                    um.UserId = item.UserId;
                    um.FullName = item.FullName;
                    um.EmailId = item.EmailId;
                    um.Password = item.Password;
                    um.IsActive = item.IsActive;
                    um.IsFirstLogin = item.IsFirstLogin;
                    um.IsEmailValidated = item.IsEmailValidated;
                    um.IsActive = item.IsActive;
                    um.CreatedBy = item.CreatedBy.Value;
                    um.ModifiedBy = item.ModifiedBy.Value;
                    um.Createdon = item.Createdon.Value;
                    um.Modifiedon = item.Modifiedon.Value;
                    um.PhoneNumber = item.PhoneNumber;
                    um.ZipCode = item.ZipCode;
                    userModel.Add(um);
                }
                return userModel;
            }
            catch(Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }
        }
        public UserModel GetUserById(int userId)
        {
            try
            {
                var item = this.Find(d => d.UserId == userId);
                if (item != null)
                {
                    return new UserModel()
                    {
                        UserId = item.UserId,
                        FullName = item.FullName,
                        EmailId = item.EmailId,
                        Password = item.Password,
                        IsActive = item.IsActive,
                        IsFirstLogin = item.IsFirstLogin,
                        IsEmailValidated = item.IsEmailValidated,
                        CreatedBy = item.CreatedBy.Value,
                        ModifiedBy = item.ModifiedBy.Value,
                        Createdon = item.Createdon.Value,
                        Modifiedon = item.Modifiedon.Value,
                        PhoneNumber = item.PhoneNumber,
                        ZipCode = item.ZipCode,
                    };
                }
                else
                {
                    return new UserModel();
                }
            }
            catch(Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }
        }

        public UserModel GetUserByEmailId(string emailId)
        {
            try
            {
                var item = this.Find(d => d.EmailId == emailId);
                if (item == null)
                {
                    return null;
                }
                else
                {
                    return new UserModel()
                    {
                        UserId = item.UserId,
                        FullName = item.FullName,
                        EmailId = item.EmailId,
                        Password = item.Password,
                        IsActive = item.IsActive,
                        IsFirstLogin = item.IsFirstLogin,
                        IsEmailValidated = item.IsEmailValidated,
                        CreatedBy = item.CreatedBy.Value,
                        ModifiedBy = item.ModifiedBy.Value,
                        PhoneNumber = item.PhoneNumber,
                        ZipCode = item.ZipCode,
                    };
                }
            }
            catch(Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }
        }

        public EmailTemplateModel GetEmailTemplate(string code)
        {
            try
            {
                return this.DbExecute(db =>
                {
                    var emailTemplate = (from a in db.MailTemplates
                                         select new EmailTemplateModel()
                                         {
                                             MailID = a.MailID,
                                             MailCode = a.MailCode,
                                             Description = a.Description,
                                             MailSubject = a.MailSubject,
                                             Template = a.Template,
                                             ToWhome = a.ToWhome,
                                             BccWhome = a.BccWhome,
                                             CcWhome = a.CcWhome
                                         }).Where(e => e.MailCode == code).ToList().FirstOrDefault();
                    return emailTemplate;
                });
            }
            catch (Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }
        }

        /// <summary>
        /// Converts the user model.
        /// </summary>
        /// <param name="userModel">The user model.</param>
        /// <returns>Returns User details.</returns>
        private UserInfo ConvertUserModel(UserModel item)
        {
            return new UserInfo
            {
                UserId = item.UserId,
                FullName = item.FullName,
                EmailId = item.EmailId,
                Password = item.Password,
                IsActive = item.IsActive,
                IsFirstLogin = item.IsFirstLogin,
                IsEmailValidated = item.IsEmailValidated,
                CreatedBy = item.UserId,
                ModifiedBy = item.UserId,
                Createdon = DateTime.UtcNow,
                Modifiedon = DateTime.UtcNow,
                PhoneNumber = item.PhoneNumber,
                ZipCode = item.ZipCode,
            };
        }
    }
}
