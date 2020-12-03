using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Models.Common
{
    public class CommonResponse
    {
        public bool IsSuccess { get; set; }
        public int StatusCode { get; set; }

        public string StatusMessage { get; set; }
        
        public string UserId { get; set; }

        public string EventId { get; set; }

        public string Details { get; set; }

        public UserModel userInfo { get; set; }
    }
}
