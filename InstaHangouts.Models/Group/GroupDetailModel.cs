using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Models
{
    public class GroupDetailModel
    {
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public string GroupName { get; set; }
        public Nullable<System.DateTime> CreatedOn { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public List<UserModel> UserInfoList { get; set; }        
        public List<GroupUserAssocModel> GroupUserAssocsList { get; set; }
    }

    public class GroupUserAssocModel
    {
        public int GroupUserId { get; set; }
        public int GroupId { get; set; }
        public int UserId { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public bool IsActive { get; set; }
        public Nullable<int> ModifiedBy { get; set; }
        public GroupDetailModel GroupDetail { get; set; }
        public UserModel UserInfo { get; set; }
    }
}
