using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Interfaces.BusinessInterface
{
    public interface IBusinessServiceUnit
    {
        /// <summary>
        /// Gets the user service.
        /// </summary>
        /// <value>The user service.</value>
        IUserService UserService { get; }

        IEvents EventsService { get; }

        IGroupDetailService GroupDetailService { get; }
    }
}
