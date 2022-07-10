using System;
using System.Collections.Generic;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class SalesOrderGraphVM
    {
        public string MonthName { get; set; }
        public string Year { get; set; }
        public string OrderDate { get; set; }
        public decimal TotalSaleAmount { get; set; }
        public decimal TotalPickUpAmount { get; set; }
        public decimal TotalDeliveredAmount { get; set; }
        public int TotalCount { get; set; }
        public int TotalPickUpCount { get; set; }
        public int TotalDeliveredCount { get; set; }        
        public int CardPaymentMode { get; set; }
        public int CashPaymentMode { get; set; }
    }

    public class SalesOrderGraphRequestVM
    {
        public string OrderStatus { get; set; }
        public string PaymentMode { get; set; }
        public int MonthId { get; set; }
        public int Year { get; set; }
        public string UserId { get; set; }

    }

    public class DashboardStatDataVM
    {
        public int TotalOrder { get; set; }
        public int TotalCustomer { get; set; }
        public int TotalMenuItem { get; set; }
    }

    
}
