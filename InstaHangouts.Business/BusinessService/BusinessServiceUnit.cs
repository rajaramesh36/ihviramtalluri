using InstaHangouts.Business.Implementation;
using InstaHangouts.Interfaces;
using InstaHangouts.Interfaces.BusinessInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Unity;

namespace InstaHangouts.Business.BusinessService
{
    public class BusinessServiceUnit : IBusinessServiceUnit
    {
        /// <summary>
        /// The unit of work
        /// </summary>
        private readonly IUnitOfWorkRepository unitOfWork;
 
        /// <summary>
        /// The user service
        /// </summary>
        private IUserService userService;

        /// <summary>
        /// The user service
        /// </summary>
        private IEvents eventsService;

        private IGroupDetailService groupDetailService;

        #region Constructor
        /// <summary>
        /// Initializes a new instance of the <see cref="BusinessServiceUnit"/> class.
        /// </summary>
        /// <param name="unitOfWork">The unit of work.</param>        
        public BusinessServiceUnit(IUnitOfWorkRepository unitOfWork)
        {
            this.unitOfWork = unitOfWork;            
        }
        #endregion

        /// <summary>
        /// Gets the user service.
        /// </summary>
        /// <value>The user service.</value>
        public IUserService UserService
        {
            get
            {
                if (this.userService == null)
                {
                    this.userService = new UserService(this.unitOfWork);
                }

                return this.userService;
            }
        }


        /// <summary>
        /// Gets the user service.
        /// </summary>
        /// <value>The user service.</value>
        public IEvents EventsService
        {
            get
            {
                if (this.eventsService == null)
                {
                    this.eventsService = new EventsService(this.unitOfWork);
                }

                return this.eventsService;
            }
        }

        public IGroupDetailService GroupDetailService
        {
            get
            {
                if (this.groupDetailService == null)
                {
                    this.groupDetailService = new GroupDetailService(this.unitOfWork);
                }

                return this.groupDetailService;
            }
        }
    }
}
