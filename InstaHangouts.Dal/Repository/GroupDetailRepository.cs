using InstaHangouts.Interfaces;
using InstaHangouts.Interfaces.BusinessInterface;
using InstaHangouts.Interfaces.DataLayerInterface;
using InstaHangouts.Models;
using NLog;
using System;
using System.Collections.Generic;
using System.Data.Common.CommandTrees;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Dal.Repository
{
    /// <summary>
    /// Group Details
    /// </summary>
    public class GroupDetailRepository : BaseRepository<GroupDetail>, IGroupDetailRepository
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWorkRepository unitOfWork;
                
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public GroupDetailRepository(IUnitOfWorkRepository unitofWork)
        {
            this.unitOfWork = unitofWork;
        }

        public GroupDetailModel AddGroups(GroupDetailModel groups)
        {
            try
            {
                logger.Info("Add Group Respository");
                var group = dbContext.GroupDetails.Where(x => x.GroupName == groups.GroupName).FirstOrDefault();
                if (group != null)
                {
                    return null;
                }
                else
                {
                    GroupDetail groupDetail = this.ConvertGroupDetailModel(groups);
                    groupDetail.isActive = true;
                    this.Add(groupDetail);
                    groups.GroupId = groupDetail.GroupId;
                    if (groupDetail.GroupId != 0)
                    {
                        foreach (var item in groups.UserInfoList)
                        {
                            var userValidate = this.unitOfWork.UserRepository.CheckingValidationsAndRetrunUser(item);
                            item.UserId = userValidate.UserId;
                            item.UserErrorMessage = userValidate.UserErrorMessage;
                        }
                        if (groups.UserInfoList.Count > 0)
                        {
                            var bulkUpload = this.unitOfWork.UserRepository.BulkUserAddOrUopdate(groups.UserInfoList);

                            foreach (var item in bulkUpload.Where(x => x.isAssocationDeleted == false))
                            {
                                if (item.UserId != 0)
                                {
                                    dbContext.GroupUserAssocs.Add(
                                        new GroupUserAssoc()
                                        {
                                            GroupId = groupDetail.GroupId,
                                            UserId = item.UserId,
                                            CreatedDate = DateTime.UtcNow,
                                            ModifiedDate = DateTime.UtcNow,
                                            IsActive = true,
                                            ModifiedBy = groupDetail.UserId,
                                        });
                                    dbContext.SaveChanges();
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Adding Group Failed");
                    }
                }

                return groups;
            }            
            catch (Exception ex)
            {
                logger.Error("Add Group Respository" + ex.Message);
                this.LogError(ex);
                groups = null;
                throw ex;
            }            
        }

        public GroupDetailModel DeactiveGroup(GroupDetailModel groups)
        {
            try
            {
                logger.Info("Deactive Group");
                GroupDetail item = this.dbContext.GroupDetails.SingleOrDefault(d => d.GroupId == groups.GroupId);
                item.isActive = false;
                this.dbContext.SaveChanges();
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.LogError(ex);
                throw ex;
            }

            return groups;
        }

        public GroupDetailModel EditGroups(GroupDetailModel groups)
        {
            try
            {
                logger.Info("Edit Group Respository");
                var value = dbContext.GroupDetails.Where(x => x.GroupName == groups.GroupName && x.GroupId != groups.GroupId).Count();
                
                if (value == 0)
                {
                    GroupDetail groupdDetailsEntitiy = this.Find(d => d.GroupId == groups.GroupId);
                    GroupDetail groupDetail = this.ConvertGroupDetailModel(groupdDetailsEntitiy, groups);
                    groupDetail.isActive = true;
                    this.Update(groupDetail);
                    groups.GroupId = groupDetail.GroupId;
                    if (groupDetail.GroupId != 0)
                    {
                        foreach (var item in groups.UserInfoList)
                        {
                            var userValidate = this.unitOfWork.UserRepository.CheckingValidationsAndRetrunUser(item);
                            item.UserId = userValidate.UserId;
                            item.UserErrorMessage = userValidate.UserErrorMessage;
                        }

                        if (groups.UserInfoList.Count > 0)
                        {
                            var bulkUpload = this.unitOfWork.UserRepository.BulkUserAddOrUopdate(groups.UserInfoList);

                            foreach (var item in bulkUpload.Where(x => x.isAssocationDeleted == false))
                            {
                                if (item.UserId != 0)
                                {
                                    var assocation = dbContext.GroupUserAssocs.Where(x => x.UserId == item.UserId && x.GroupId == groupDetail.GroupId).FirstOrDefault();
                                    if (assocation == null)
                                    {
                                        dbContext.GroupUserAssocs.Add(
                                            new GroupUserAssoc()
                                            {
                                                GroupId = groupDetail.GroupId,
                                                UserId = item.UserId,
                                                CreatedDate = DateTime.UtcNow,
                                                ModifiedDate = DateTime.UtcNow,
                                                IsActive = true,
                                                ModifiedBy = item.UserId,
                                            });
                                        dbContext.SaveChanges();
                                    }
                                    if (item.isAssocationDeleted)
                                    {
                                        dbContext.GroupUserAssocs.AddOrUpdate(
                                           new GroupUserAssoc()
                                           {
                                               GroupId = groupDetail.GroupId,
                                               UserId = item.UserId,
                                               CreatedDate = DateTime.UtcNow,
                                               ModifiedDate = DateTime.UtcNow,
                                               IsActive = false,
                                               ModifiedBy = item.UserId,
                                           });
                                        dbContext.SaveChanges();
                                    }
                                }
                            }
                        }
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.LogError(ex);
                throw ex;
            }
            return groups;
        }

        public List<GroupDetailModel> GetAllGroups(int userId)
        {
            List<GroupDetailModel> groupDetailListModels = new List<GroupDetailModel>();
            try
            {
                logger.Info("Get All groups Respository");
                var getValues = this.FindAll(d => d.UserId == userId && d.isActive == true);                
                if (getValues != null)
                {
                    foreach (var item in getValues)
                    {
                        GroupDetailModel groupDetailModel = new GroupDetailModel();
                        groupDetailModel.GroupId = item.GroupId;
                        groupDetailModel.GroupName = item.GroupName;
                        groupDetailModel.UserId = userId;
                        groupDetailModel.UserInfoList =
                            (from us in dbContext.UserInfoes
                             join gsa in dbContext.GroupUserAssocs.Where(x=>x.GroupId == item.GroupId) on
                                us.UserId equals gsa.UserId                                                         
                             select new UserModel()
                             {
                                 UserId = us.UserId,
                                 PhoneNumber = us.PhoneNumber,
                                 FullName = us.FullName,
                                 EmailId = us.EmailId,
                             }).ToList();
                        groupDetailModel.GroupUserAssocsList = (from gua in dbContext.GroupUserAssocs.Where(x => x.GroupId == item.GroupId)
                                                                join gd in dbContext.GroupDetails.Where(x => x.GroupId == item.GroupId) on gua.GroupId equals gd.GroupId
                                                                select new GroupUserAssocModel
                                                                {
                                                                    GroupId = gua.GroupId,
                                                                    GroupUserId = gua.GroupUserId
                                                                }).ToList();
                        groupDetailListModels.Add(groupDetailModel);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}:{2}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage, validationError.PropertyName);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                        this.LogInfo(message);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {               
                logger.Error(ex.Message);
                this.LogError(ex);
                throw ex;                
            }

            return groupDetailListModels;
        }

        public GroupDetailModel ImportGroupAttendance(GroupDetailModel groups)
        {
            try
            {   
                logger.Info("Import Group Respository");
                var group = new GroupDetail();
                if (groups.GroupId != 0)
                {
                    group = dbContext.GroupDetails.Where(x => x.GroupId == groups.GroupId).FirstOrDefault();
                }
                else
                {
                    group = dbContext.GroupDetails.Where(x => x.GroupName == groups.GroupName).FirstOrDefault();
                }

                if (group == null)
                {
                    GroupDetail groupdDetailsEntitiy = this.Find(d => d.GroupId == groups.GroupId);
                    GroupDetail groupDetail = this.ConvertGroupDetailModel(groupdDetailsEntitiy, groups);
                    this.Add(groupDetail);
                    groups.GroupId = groupDetail.GroupId;
                    if (groupDetail.GroupId != 0)
                    {
                        foreach (var item in groups.UserInfoList)
                        {
                            var userValidate = this.unitOfWork.UserRepository.CheckingValidationsAndRetrunUser(item);
                            item.UserId = userValidate.UserId;
                            item.UserErrorMessage = userValidate.UserErrorMessage;
                        }
                        if (groups.UserInfoList.Count > 0)
                        {
                            var bulkUploadUserList = this.unitOfWork.UserRepository.BulkUserAddOrUopdate(groups.UserInfoList.Where(x => x.isAssocationDeleted == false).ToList());

                            foreach (var item in bulkUploadUserList)
                            {
                                if (item.UserId != 0)
                                {
                                    dbContext.GroupUserAssocs.Add(
                                        new GroupUserAssoc()
                                        {
                                            GroupId = groupDetail.GroupId,
                                            UserId = item.UserId,
                                            CreatedDate = DateTime.UtcNow,
                                            ModifiedDate = DateTime.UtcNow,
                                            IsActive = true,
                                            ModifiedBy = groupDetail.UserId,
                                        });
                                }
                            }
                        }
                    }
                    else
                    {
                        throw new Exception("Group id not exits");
                    }
                }
                else
                {
                    GroupDetail groupDetail = this.ConvertGroupDetailModel(groups);
                    groupDetail.isActive = true;
                    this.Add(groupDetail);
                    groups.GroupId = groupDetail.GroupId;
                    if (groupDetail.GroupId != 0)
                    {
                        foreach (var item in groups.UserInfoList)
                        {
                            var userValidate = this.unitOfWork.UserRepository.CheckingValidationsAndRetrunUser(item);
                            item.UserId = userValidate.UserId;
                            item.UserErrorMessage = userValidate.UserErrorMessage;
                        }

                        if (groups.UserInfoList.Count > 0)
                        {
                            var bulkUpload = this.unitOfWork.UserRepository.BulkUserAddOrUopdate(groups.UserInfoList.Where(x => x.isAssocationDeleted == false).ToList());

                            foreach (var item in bulkUpload)
                            {
                                if (item.UserId != 0)
                                {
                                    var assocation = dbContext.GroupUserAssocs.Where(x => x.UserId == item.UserId && x.GroupId == groupDetail.GroupId).FirstOrDefault();
                                    if (assocation == null)
                                    {
                                        dbContext.GroupUserAssocs.Add(
                                            new GroupUserAssoc()
                                            {
                                                GroupId = groupDetail.GroupId,
                                                UserId = item.UserId,
                                                CreatedDate = DateTime.UtcNow,
                                                ModifiedDate = DateTime.UtcNow,
                                                IsActive = true,
                                                ModifiedBy = item.UserId,
                                            });
                                    }
                                    if (item.isAssocationDeleted)
                                    {
                                        dbContext.GroupUserAssocs.AddOrUpdate(
                                           new GroupUserAssoc()
                                           {
                                               GroupId = groupDetail.GroupId,
                                               UserId = item.UserId,
                                               CreatedDate = DateTime.UtcNow,
                                               ModifiedDate = DateTime.UtcNow,
                                               IsActive = false,
                                               ModifiedBy = item.UserId,
                                           });
                                    }
                                }
                            }
                        }
                    }
                }
                return groups;
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}:{2}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage, validationError.PropertyName);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                        this.LogInfo(message);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                logger.Error("import Group Respository" + ex.Message);
                this.LogError(ex);
                groups = null;
                throw ex;
            }
        }

        public List<GroupDetailModel> GetGroupsById(int userId, int groupId)
        {
            List<GroupDetailModel> groupDetailListModels = new List<GroupDetailModel>();
            try
            {
                var getValues = this.FindAll(x => x.UserId == userId && x.GroupId == groupId && x.isActive == true).ToList();

                if (getValues != null)
                {
                    foreach (var item in getValues)
                    {
                        GroupDetailModel groupDetailModel = new GroupDetailModel();
                        groupDetailModel.GroupId = item.GroupId;
                        groupDetailModel.GroupName = item.GroupName;
                        groupDetailModel.UserId = userId;
                        groupDetailModel.UserInfoList =
                            (from us in dbContext.UserInfoes
                             join gsa in dbContext.GroupUserAssocs.Where(x=>x.GroupId == item.GroupId) on
                                us.UserId equals gsa.UserId
                             select new UserModel()
                             {
                                 UserId = us.UserId,
                                 PhoneNumber = us.PhoneNumber,
                                 FullName = us.FullName,
                                 EmailId = us.EmailId,
                             }).ToList();
                        groupDetailModel.GroupUserAssocsList = (from gua in dbContext.GroupUserAssocs.Where(x => x.GroupId == item.GroupId)
                                                                join gd in dbContext.GroupDetails.Where(x => x.GroupId == item.GroupId) on gua.GroupId equals gd.GroupId
                                                                select new GroupUserAssocModel
                                                                {
                                                                    GroupId = gua.GroupId,
                                                                    GroupUserId = gua.GroupUserId
                                                                }).ToList();
                        groupDetailListModels.Add(groupDetailModel);
                    }
                }
                else
                {
                    return null;
                }
            }
            catch (System.Data.Entity.Validation.DbEntityValidationException dbEx)
            {
                Exception raise = dbEx;
                foreach (var validationErrors in dbEx.EntityValidationErrors)
                {
                    foreach (var validationError in validationErrors.ValidationErrors)
                    {
                        string message = string.Format("{0}:{1}:{2}",
                            validationErrors.Entry.Entity.ToString(),
                            validationError.ErrorMessage, validationError.PropertyName);
                        // raise a new exception nesting  
                        // the current instance as InnerException  
                        raise = new InvalidOperationException(message, raise);
                        this.LogInfo(message);
                    }
                }
                throw raise;
            }
            catch (Exception ex)
            {
                logger.Error(ex.Message);
                this.LogError(ex);
                throw ex;
            }
            return groupDetailListModels;
        }

        private GroupDetail ConvertGroupDetailModel(GroupDetailModel item)
        {
            return new GroupDetail
            {       
                GroupName = item.GroupName,                  
                CreatedOn = DateTime.UtcNow,
                ModifiedOn = DateTime.UtcNow,
                UserId = item.UserId,
                CreatedBy = item.UserId,
                ModifiedBy = item.UserId,
            };
        }

        private GroupDetail ConvertGroupDetailModel(GroupDetail item, GroupDetailModel model)
        {
            item.GroupId = item.GroupId;
            item.GroupName = model.GroupName;
            item.ModifiedOn = DateTime.UtcNow;            
            item.UserId = model.UserId;            
            item.ModifiedBy = model.UserId;
            return item;
        }
    }
}
