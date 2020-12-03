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
    public class EventsService : IEvents
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
        public EventsService(IUnitOfWorkRepository unitofWork)
        {
            this.unitOfWork = unitofWork;
        }


        public WelcomeEventsModel WelcomeEventsModel(int userId, out Header header)
        {
            WelcomeEventsModel model = new WelcomeEventsModel();
            try
            {
                header = GlobalHelper.ConstructHeader();
                model = this.unitOfWork.EventsRepository.WelcomeEventsModel(userId);
                return model;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.ExceptionLoad);
                logger.Error(ex.StackTrace.ToString());
                model = null;
                return model;
            }           
        }

        public PlanEventModel PlanEvent(PlanEventModel model, out Header header)
        {
            PlanEventModel planEventModel = new PlanEventModel();
            try
            {
                header = GlobalHelper.ConstructHeader();
                planEventModel = this.unitOfWork.EventsRepository.PlanEvent(model);
                return planEventModel;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.Exception);
                logger.Error(ex.StackTrace.ToString());
                return planEventModel;
            }            
        }

        public PlanEventModel UpdatePlanEvent(PlanEventModel model, out Header header)
        {
            PlanEventModel planEventModel = new PlanEventModel();
            try
            {
                header = GlobalHelper.ConstructHeader();
                planEventModel = this.unitOfWork.EventsRepository.UpdatePlanEvent(model);
                return planEventModel;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.Exception);
                logger.Error(ex.StackTrace.ToString());
                return planEventModel;
            }
        }

        public PlanEventModel ImportEventsAttendance(PlanEventModel model, out Header header)
        {
            PlanEventModel planEventModel = new PlanEventModel();
            try
            {
                header = GlobalHelper.ConstructHeader();
                planEventModel = this.unitOfWork.EventsRepository.ImportEventsAttendance(model);
                return planEventModel;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.Exception);
                logger.Error(ex.StackTrace.ToString());
                return planEventModel;
            }
        }

        public ICollection<PlanEventModel> GetEvents(int userId, int eventId, out Header header)
        {
            try
            {
                header = GlobalHelper.ConstructHeader();
                var events = this.unitOfWork.EventsRepository.GetEvents(userId, eventId);
                return events;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.ExceptionLoad);
                logger.Error(ex.StackTrace.ToString());
                return null;
            }
        }

        public ICollection<PlanEventModel> GetEventsSummaryList(int userId, int summaryId, out Header header)
        {
            try
            {
                header = GlobalHelper.ConstructHeader();
                var events = this.unitOfWork.EventsRepository.GetEventsSummaryList(userId, summaryId);
                return events;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.ExceptionLoad);
                logger.Error(ex.StackTrace.ToString());
                return null;
            }
        }

        public EventsSummaryDetails GetEventsSummaryListByUserId(int userId, out Header header)
        {
            try
            {
                header = GlobalHelper.ConstructHeader();
                var events = this.unitOfWork.EventsRepository.GetEventsSummaryListByUserId(userId);
                return events;
            }
            catch (Exception ex)
            {
                header = GlobalHelper.ConstructHeader(ex, Messages.ExceptionLoad);
                logger.Error(ex.StackTrace.ToString());
                return null;
            }
        }
    }
}
