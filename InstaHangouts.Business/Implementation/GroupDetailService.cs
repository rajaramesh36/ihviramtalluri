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
using System.Web.ModelBinding;
using InstaHangouts.Common;

namespace InstaHangouts.Business.Implementation
{
   public class GroupDetailService : IGroupDetailService
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
        public GroupDetailService(IUnitOfWorkRepository unitofWork)
        {
            this.unitOfWork = unitofWork;
        }

        public List<GroupDetailModel> GetAllGroups(int userId, out Header header)
        {
            try
            {
                header = GlobalHelper.ConstructHeader();                
                var groups = this.unitOfWork.GroupDetailRepository.GetAllGroups(userId);
                return groups;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.ExceptionLoad);
                logger.Error(ex.StackTrace.ToString());
                return null;
            }
        }

        public List<GroupDetailModel> GetGroupsById(int userId, int groupId, out Header header)
        {
            try
            {
                header = GlobalHelper.ConstructHeader();
                var groups = this.unitOfWork.GroupDetailRepository.GetGroupsById(userId, groupId);
                return groups;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.ExceptionLoad);
                logger.Error(ex.StackTrace.ToString());
                return null;
            }
        }

        public GroupDetailModel EditGroups(GroupDetailModel groups, out Header header)
        {
            GroupDetailModel groupDetailModel = new GroupDetailModel();
            try
            {
                header = GlobalHelper.ConstructHeader(Messages.AddUserUpdatedMessage);
                groupDetailModel = this.unitOfWork.GroupDetailRepository.EditGroups(groups);
                if (groupDetailModel == null)
                {
                    header = GlobalHelper.ConstructHeader(Messages.GroupAlready);
                }
                return groupDetailModel;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.Exception);
                logger.Error(ex.StackTrace.ToString());
                return groupDetailModel;
            }
        }

        public GroupDetailModel AddGroups(GroupDetailModel groups, out Header header)
        {
            GroupDetailModel groupDetailModel = new GroupDetailModel();
            try
            {
                header = GlobalHelper.ConstructHeader(Messages.GroupAddSucces);
                groupDetailModel = this.unitOfWork.GroupDetailRepository.AddGroups(groups);
                if (groupDetailModel == null)
                {
                    header = GlobalHelper.ConstructHeader(Messages.GroupAlready);
                }
                return groupDetailModel;
            }            
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
                if (ex.Message.Contains("Adding Group Failed"))
                {
                    header = GlobalHelper.ConstructHeader(ex, Messages.GroupAddFailed);
                }
                else
                {
                    header = GlobalHelper.ConstructHeader(ex, Messages.Exception);
                }
                
                return groupDetailModel;
            }
        }

        public GroupDetailModel DeactiveGroup(GroupDetailModel groups, out Header header)
        {
            GroupDetailModel groupDetailModel = new GroupDetailModel();
            try
            {
                header = GlobalHelper.ConstructHeader();
                groupDetailModel = this.unitOfWork.GroupDetailRepository.DeactiveGroup(groups);
                return groupDetailModel;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.DeleteData);
                logger.Error(ex.StackTrace.ToString());
                return groupDetailModel;
            }
        }

        public GroupDetailModel ImportGroupAttendance(GroupDetailModel lstgroups, out Header header)
        {   
            try
            {
                header = GlobalHelper.ConstructHeader(Messages.GroupAddSucces);
                lstgroups = this.unitOfWork.GroupDetailRepository.ImportGroupAttendance(lstgroups);
                if (lstgroups == null)
                {
                    header = GlobalHelper.ConstructHeader(Messages.Failed);
                }
                return lstgroups;
            }
            catch (Exception ex)
            {
                logger.Error(ex.StackTrace.ToString());
                header = GlobalHelper.ConstructHeader(ex, Messages.Exception);
                return lstgroups;
            }
        }
    }
}
