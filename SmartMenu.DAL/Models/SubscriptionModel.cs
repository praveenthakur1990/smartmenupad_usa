using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class SubscriptionModel
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string CustomerId { get; set; }
        public string SubscriptionId { get; set; }
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public decimal? Amount { get; set; }
        public bool? IsCaptured { get; set; }
        public string seller_message { get; set; }
        public string brand { get; set; }
        public string funding { get; set; }
        public string last4 { get; set; }
        public string receipt_url { get; set; }
        public string status { get; set; }
        public string latest_invoice { get; set; }
        public bool? livemode { get; set; }
        public string payment_method { get; set; }
        public string CancellationReason { get; set; }
        public DateTime? CanceledAt { get; set; }
        public bool? IsSubscriptionCancel { get; set; }
        public DateTime? SubscriptionCancelOn { get; set; }
        public DateTime? CreatedDate { get; set; }
    }

    public class SubscriptionInfoModel
    {
        public DateTime? SubscriptionStartDate { get; set; }
        public DateTime? SubscriptionEndDate { get; set; }
        public string PlanName { get; set; }
        public decimal PlanCost { get; set; }
        public string SubscriptionId { get; set; }
    }
}
