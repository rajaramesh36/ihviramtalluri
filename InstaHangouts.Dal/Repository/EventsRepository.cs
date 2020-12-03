using InstaHangouts.Interfaces.DataLayerInterface;
using InstaHangouts.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
using System.Linq;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;
using InstaHangouts.Dal.Converters;
using InstaHangouts.Models.Common;
using System.Net.Configuration;
using NLog;

namespace InstaHangouts.Dal.Repository
{
    /// <summary>
    /// Events Repository
    /// </summary>
    public class EventsRepository : BaseRepository<PlanEvent>, IEventsRepository
    {
        private static Logger logger = LogManager.GetCurrentClassLogger();

        public WelcomeEventsModel WelcomeEventsModel(int userId)
        {
            WelcomeEventsModel model = new WelcomeEventsModel();
            try
            {
                var dbEventsData = dbContext.PlanEvents.Where(x => x.UserId == userId).ToList();
                model.UserId = userId;
                model.CompletedEvents = dbEventsData.Where(x => x.EventDate <= DateTime.UtcNow).Count();
                model.InCompletedEvents = dbEventsData.Where(x => x.EventDate <= DateTime.UtcNow).Count();
                model.EventsToAttend = dbEventsData.Where(x => x.EventDate >= DateTime.UtcNow).Count();
                model.MyHostedEvents = dbEventsData.Where(x => x.EventHostUserId == userId).Count();

                model.groupDetails = new List<GroupDetailListModel>();
                var groupData = (from b in dbContext.GroupDetails where b.UserId == userId select b).Take(5)
                                          .ToList();
                model.groupDetails = groupData.Convert();
            }
            catch (Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }

            return model;
        }

        public PlanEventModel PlanEvent(PlanEventModel model)
        {  
            try
            {
                PlanEvent planEvent = this.ConvertPlanEventModel(model);
                this.Add(planEvent);
                model.EventId = planEvent.EventId;
                return model;
            }
            catch (Exception ex)
            {
                model = null;
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);                
                throw ex;
            }

            return model;
        }

        public PlanEventModel UpdatePlanEvent(PlanEventModel model)
        {   
            try
            {
                PlanEvent planEventModel = this.Find(d => d.EventId == model.EventId);
                PlanEvent planEvent = this.ConvertPlanEventModel(planEventModel, model);
                model.EventId = planEvent.EventId;
                return model;
            }
            catch (Exception ex)
            {
                model = null;
                this.LogError(ex);
                logger.Error("Respository" + ex.Message);
                throw ex;
            }

            return model;
        }

        public PlanEventModel ImportEventsAttendance(PlanEventModel model)
        {
            try
            {
                PlanEvent planEventModel = this.Find(d => d.EventId == model.EventId);
                PlanEvent planEvent = this.ConvertPlanEventModel(planEventModel, model);
                model.EventId = planEvent.EventId;
                return model;
            }
            catch (Exception ex)
            {
                model = null;
                this.LogError(ex);
                logger.Error("Respository" + ex.Message);
                throw ex;
            }

            return model;
        }

        public ICollection<PlanEventModel> GetEvents(int userid, int eventId)
        {
            try
            {
                var planEvents =  dbContext.PlanEvents.Where(x=>x.UserId == userid && x.EventId == eventId).ToList();
                return planEvents.Convert();
            }
            catch (Exception ex)
            {   
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }           
        }

        public ICollection<PlanEventModel> GetEventsSummaryList(int userid, int summaryId)
        {   
            try
            {
                if (summaryId == (int) EventsSumaary.CompletedEvents)
                {
                    var planEvents = dbContext.PlanEvents.Where(x => x.UserId == userid && x.EventDate <= DateTime.UtcNow).ToList();
                    return planEvents.Convert();
                }
                else if (summaryId == (int)EventsSumaary.InCompletedEvents)
                {
                    var planEvents = dbContext.PlanEvents.Where(x => x.UserId == userid && x.EventDate >= DateTime.UtcNow).ToList();
                    return planEvents.Convert();
                }
                else if (summaryId == (int)EventsSumaary.EventsToAttend)
                {
                    var planEvents = dbContext.PlanEvents.Where(x => x.UserId == userid && x.EventDate >= DateTime.UtcNow).ToList();
                    return planEvents.Convert();
                }
                else if (summaryId == (int)EventsSumaary.MyHostedEvents)
                {
                    var planEvents = dbContext.PlanEvents.Where(x => x.UserId == userid).ToList();
                    return planEvents.Convert();
                }                
                else
                {
                    var planEvents = dbContext.PlanEvents.Where(x => x.UserId == userid).ToList();
                    return planEvents.Convert();
                }               
            }
            catch (Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;                                               
            }
        }

        public EventsSummaryDetails GetEventsSummaryListByUserId(int userid)
        {
            EventsSummaryDetails eventsSummaryDetails = new EventsSummaryDetails();
            try
            {
                var planEventsByUserId = dbContext.PlanEvents.Where(x => x.UserId == userid).ToList();

                var CompletedEvents = planEventsByUserId.Where(x => x.UserId == userid && x.EventDate <= DateTime.UtcNow).ToList();
                eventsSummaryDetails.CompletedEvents = CompletedEvents.Convert();

                var InCompletedEvents = planEventsByUserId.Where(x => x.UserId == userid && x.EventDate >= DateTime.UtcNow).ToList();
                eventsSummaryDetails.InCompletedEvents = InCompletedEvents.Convert();

                var EventsToAttend = planEventsByUserId.Where(x => x.UserId == userid && x.EventDate >= DateTime.UtcNow).ToList();
                eventsSummaryDetails.EventsToAttend = EventsToAttend.Convert();

                var MyHostedEvents = planEventsByUserId.Where(x => x.UserId == userid).ToList();
                eventsSummaryDetails.MyHostedEvents = EventsToAttend.Convert();
                
            }
            catch (Exception ex)
            {
                logger.Error("Respository" + ex.Message);
                this.LogError(ex);
                throw ex;
            }
            return eventsSummaryDetails;
        }

        private PlanEvent ConvertPlanEventModel(PlanEventModel item)
        {
            return new PlanEvent
            {
                EventDate = item.EventDate,
                EventDescription = item.EventDescription,
                EventName = item.EventName,
                TimeZone = item.TimeZone,
                EventHostUserId = item.UserId,
                EventType = item.EventType,
                EventTypeCategory = item.EventTypeCategory,
                IncludeFood = item.IncludeFood,
                IncludeGames = item.IncludeGames,
                WantFood = item.IncludeGifts,
                CreatedDate = DateTime.UtcNow,
                ModifiedDate = DateTime.UtcNow,
                UserId = item.UserId,
                CreatedBy = item.UserId,
                ModifiedBy = item.UserId,
            };
        }

        private PlanEvent ConvertPlanEventModel(PlanEvent item, PlanEventModel model)
        {
            item.EventName = model.EventName;
            item.EventDescription = model.EventDescription;
            item.EventDate = model.EventDate;
            item.TimeZone = model.TimeZone;
            item.EventHostUserId = model.EventHostUserId;
            item.EventType = model.EventType;
            item.EventTypeCategory = model.EventTypeCategory;
            item.IncludeFood = model.IncludeFood;
            item.IncludeGames = model.IncludeGames;
            item.UserId = model.UserId;
            item.WantFood = model.IncludeGifts;
            item.ModifiedDate = DateTime.UtcNow;
            item.ModifiedBy = model.UserId;            
            return item;
        }
    }
}
