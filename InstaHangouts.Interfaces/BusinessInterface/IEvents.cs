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
    public interface IEvents
    {
        /// <summary>
        /// Gets all users.
        /// </summary>
        /// <returns>List of all user model</returns>
        WelcomeEventsModel WelcomeEventsModel(int userId, out Header header);

        PlanEventModel PlanEvent(PlanEventModel model, out Header header);

        PlanEventModel UpdatePlanEvent(PlanEventModel model, out Header header);

        ICollection<PlanEventModel> GetEvents(int userId,int eventId, out Header header);

        ICollection<PlanEventModel> GetEventsSummaryList(int userId, int summaryId, out Header header);

        PlanEventModel ImportEventsAttendance(PlanEventModel planEventModel, out Header header);

        EventsSummaryDetails GetEventsSummaryListByUserId(int userId, out Header header);
    }
}
