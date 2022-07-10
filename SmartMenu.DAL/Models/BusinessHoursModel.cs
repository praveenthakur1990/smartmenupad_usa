using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class BusinessHoursModel
    {
        [Key]
        public int Id { get; set; }
        public int? WeekDayId { get; set; }
        public TimeSpan? OpenTime { get; set; }
        public TimeSpan? CloseTime { get; set; }
        public bool? IsClosed { get; set; }
    }

    public class BusinessHoursModelVM
    {
        public int Id { get; set; }
        public int? WeekDayId { get; set; }
        public string OpenTime { get; set; }
        public string CloseTime { get; set; }
        public string OpenTime12Hour { get; set; }
        public string CloseTime12Hour { get; set; }
        public bool? IsClosed { get; set; }
        public string CurrentTime { get; set; }
        public string CreatedBy { get; set; }
    }
}
