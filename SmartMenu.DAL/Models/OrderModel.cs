using System;
using System.Collections.Generic;
using System.Text;
using System.ComponentModel.DataAnnotations;
namespace SmartMenu.DAL.Models
{
    public class OrderModel
    {
        [Key]
        public int Id { get; set; }
        public string OrderNo { get; set; }
        public int CustomerAddressId { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        public string OrderedType { get; set; }
        public DateTime? PickUpDateTime { get; set; }
        public string PickUpAddress { get; set; }
        public DateTime? OrderedDate { get; set; }
        public string OrderedDetails { get; set; }
        public string SpecialInstruction { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal DeliveryCharges { get; set; }
    }

    public class OrderViewModel
    {
        public int Id { get; set; }
        public int CustomerId { get; set; }
        public string CustomerName { get; set; }
        public string OrderNo { get; set; }
        public string Mode { get; set; }
        public string Status { get; set; }
        public string OrderedType { get; set; }
        public DateTime? PickUpDateTime { get; set; }
        public DateTime OrderedDate { get; set; }
        public List<MenuCartModel> OrderedDetails { get; set; }
        public string SpecialInstruction { get; set; }
        public string CapturedId { get; set; }
        public string PaymentMethod { get; set; }
        public DateTime? TransactionDate { get; set; }
        public string Funding { get; set; }
        public decimal CapturedAmt { get; set; }
        public string MobileNumber { get; set; }
        public string CustomerEmail { get; set; }
        public string CustomerAddress { get; set; }
        public string OrderStatus { get; set; }
        public string PaymentStatus { get; set; }
        public string StatusChangedBy { get; set; }
        public DateTime? LastUpdatedDate { get; set; }
        public string LastUpdatedStatus { get; set; }
        public string OrderStatusLogJsonStr { get; set; }
        public List<OrderStatusLogsVM> OrderStatusLogList { get; set; }

        public bool IsCustomerBillCopy { get; set; }
        public DateTime ActualDateTime { get; set; }
        public int TotalRows { get; set; }
        public string PickUpAddress { get; set; }
        public decimal TaxRate { get; set; }
        public decimal TaxAmt { get; set; }
        public decimal DeliveryCharges { get; set; }
    }

    public class OrderStatusLogsVM
    {
        public int OrderId { get; set; }
        public string Status { get; set; }
        public string ChangedBy { get; set; }
        public DateTime ChangedDateTime { get; set; }
    }


}
