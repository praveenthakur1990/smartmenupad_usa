using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class RequestDemoModel
    {
        public string FullName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string RestaurantName { get; set; }
        public int NoOfLocation { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class TalkToExpertModel
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string PhoneNumber { get; set; }
        public string EmailAddress { get; set; }
        public string ChoosePlan { get; set; }
        public DateTime CreatedDate { get; set; }
    }

    public class NewsLetterSubscriberModel
    {
        public string EmailAddress { get; set; }
        public DateTime CreatedDate { get; set; }
    }
}
