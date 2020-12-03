using InstaHangouts.Models;
using InstaHangouts.Models.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Interfaces.DataLayerInterface
{
    public interface IEventsRepository
    {
        WelcomeEventsModel WelcomeEventsModel(int userId);

        PlanEventModel PlanEvent(PlanEventModel model);

        PlanEventModel UpdatePlanEvent(PlanEventModel model);

        ICollection<PlanEventModel> GetEvents(int userId, int eventId);

        ICollection<PlanEventModel> GetEventsSummaryList(int userid, int summaryId);

        PlanEventModel ImportEventsAttendance(PlanEventModel planEventModel);

        EventsSummaryDetails GetEventsSummaryListByUserId(int userid);
    }
}
