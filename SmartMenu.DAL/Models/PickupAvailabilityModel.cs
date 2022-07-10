using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class PickupAvailabilityModel
    {
        public string PickupDate { get; set; }
        public string HourOpenTime { get; set; }
        public string HourCloseTime { get; set; }
        public bool IsAvailable { get; set; }
    }

    public class PickUpDateVM
    {
        public string PickUpDate { get; set; }
        public int WeekDayId { get; set; }
    }

    public class PickUpTimeVM
    {
        public string PickUpTime { get; set; }
        public int WeekDayId { get; set; }
    }
}
