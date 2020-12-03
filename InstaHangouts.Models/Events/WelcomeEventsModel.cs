using InstaHangouts.Models.Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

namespace InstaHangouts.Models
{
    public class WelcomeEventsModel
    {
        /// <summary>
        /// Gets or sets the user identifier.
        /// </summary>
        /// <value>The user identifier.</value>
        [JsonProperty("UserId")]
        [DataMember]
        public int UserId { get; set; }

        [JsonProperty("InCompletedEvents")]
        [DataMember]
        public int InCompletedEvents { get; set; }

        [JsonProperty("MyHostedEvents")]
        [DataMember]
        public int MyHostedEvents { get; set; }

        [JsonProperty("EventsToAttend")]
        [DataMember]
        public int EventsToAttend { get; set; }

        [JsonProperty("CompletedEvents")]
        [DataMember]
        public int CompletedEvents { get; set; }

        [JsonProperty("GroupDetails")]
        public ICollection<GroupDetailListModel> groupDetails { get; set; }
    }

    public class GroupDetailListModel
    {
        [DataMember]
        public int GroupId { get; set; }

        public int UserId { get; set; }

        public string GroupName { get; set; }

        public int GroupUserId { get; set; }
    }

    public class PlanEventModel
    {
        public int EventId { get; set; }
        public string EventTypeCategory { get; set; }
        public string EventType { get; set; }
        public string EventName { get; set; }
        public System.DateTime EventDate { get; set; }
        public int EventHostUserId { get; set; }
        public bool IncludeGifts { get; set; }
        public bool IncludeGames { get; set; }
        public bool IncludeFood { get; set; }
        public string EventPassword { get; set; }
        public string EventDescription { get; set; }
        public int UserId { get; set; }
        public string TimeZone { get; set; }
        public Nullable<System.DateTime> CreatedDate { get; set; }
        public Nullable<System.DateTime> ModifiedDate { get; set; }
        public Nullable<int> CreatedBy { get; set; }
        public Nullable<int> ModifiedBy { get; set; }

        public List<UserModel> userModels { get; set; }
    }

    public class EventsSummaryDetails
    {
        public ICollection<PlanEventModel> CompletedEvents { get; set; }
        public ICollection<PlanEventModel> InCompletedEvents { get; set; }
        public ICollection<PlanEventModel> EventsToAttend { get; set; }
        public ICollection<PlanEventModel> MyHostedEvents { get; set; }
    }

    public enum EventsSumaary
    {
        CompletedEvents = 1,
        InCompletedEvents =2 ,
        EventsToAttend =3,
        MyHostedEvents = 4
    }

}
