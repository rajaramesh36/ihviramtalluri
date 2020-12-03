using InstaHangouts.Dal.Repository;
using InstaHangouts.Interfaces;
using InstaHangouts.Interfaces.DataLayerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Dal.UnitOfWork
{
    public class UnitOfWorkRepository : IUnitOfWorkRepository
    {

        public UnitOfWorkRepository()
        {

        }        

        /// <summary>
        /// The user repository
        /// </summary>
        private IUserRepository userRepository;

        /// <summary>
        /// The user repository
        /// </summary>
        private IEventsRepository eventsRepository;

        private IGroupDetailRepository groupDetailsRepository;

        /// <summary>
        /// Gets the user repository.
        /// </summary>
        /// <value>The user repository.</value>
        IUserRepository IUnitOfWorkRepository.UserRepository
        {
            get
            {
                if (this.userRepository == null)
                {
                    this.userRepository = new UserRepository();
                }

                return this.userRepository;
            }
        }

        /// <summary>
        /// Gets the user repository.
        /// </summary>
        /// <value>The user repository.</value>
        IEventsRepository IUnitOfWorkRepository.EventsRepository
        {
            get
            {
                if (this.eventsRepository == null)
                {
                    this.eventsRepository = new EventsRepository();
                }

                return this.eventsRepository;
            }
        }

        /// <summary>
        /// Gets the user repository.
        /// </summary>
        /// <value>The user repository.</value>
        IGroupDetailRepository IUnitOfWorkRepository.GroupDetailRepository
        {
            get
            {
                if (this.groupDetailsRepository == null)
                {
                    this.groupDetailsRepository = new GroupDetailRepository(new UnitOfWorkRepository());
                }

                return this.groupDetailsRepository;
            }
        }

    }
}
