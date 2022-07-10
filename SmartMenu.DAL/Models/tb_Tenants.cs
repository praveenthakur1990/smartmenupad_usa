using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace SmartMenu.DAL.Models
{
    public class tb_Tenants
    {
        [Key]
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TenantName { get; set; }
        public string TenantSchema { get; set; }
        public string TenantConnection { get; set; }
        public string TenantDomain { get; set; }
        public string CreatedBy { get; set; }
       
    }

    public class TenantsVM
    {
        public int Id { get; set; }
        public string UserId { get; set; }
        public string TenantName { get; set; }
        public string TenantSchema { get; set; }
        public string TenantConnection { get; set; }
        public string TenantDomain { get; set; }
        public string CreatedBy { get; set; }
        public string RoleName { get; set; }
        public string AddedByName { get; set; }
    }
}
