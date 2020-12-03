using InstaHangouts.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Interfaces.DataLayerInterface
{
    public interface IGroupDetailRepository
    {
        List<GroupDetailModel> GetAllGroups(int userId);

        List<GroupDetailModel> GetGroupsById(int userId, int groupId);

        GroupDetailModel EditGroups(GroupDetailModel groups);

        GroupDetailModel AddGroups(GroupDetailModel groups);

        GroupDetailModel ImportGroupAttendance(GroupDetailModel groupDetailModels);

        GroupDetailModel DeactiveGroup(GroupDetailModel groups);
    }
}
