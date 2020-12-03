using InstaHangouts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Interfaces.BusinessInterface
{
    public interface IGroupDetailService
    {
        List<GroupDetailModel> GetAllGroups(int userId, out Header header);

        List<GroupDetailModel> GetGroupsById(int userId, int groupId, out Header header);

        GroupDetailModel EditGroups(GroupDetailModel groups, out Header header);

        GroupDetailModel AddGroups(GroupDetailModel groups, out Header header);

        GroupDetailModel DeactiveGroup(GroupDetailModel groups, out Header header);

        GroupDetailModel ImportGroupAttendance(GroupDetailModel groupDetailModels, out Header header);
    }
}
