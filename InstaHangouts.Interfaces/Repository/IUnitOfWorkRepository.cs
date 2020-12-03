using InstaHangouts.Interfaces.DataLayerInterface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Interfaces
{
    public interface IUnitOfWorkRepository
    {
        /// <summary>
        /// Gets the user repository.
        /// </summary>
        /// <value>The user repository.</value>
        IUserRepository UserRepository { get; }

        IEventsRepository EventsRepository { get; }

        IGroupDetailRepository GroupDetailRepository { get; }
    }
}
