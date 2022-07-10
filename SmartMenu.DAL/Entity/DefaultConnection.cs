using SmartMenu.DAL.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Text;

namespace SmartMenu.DAL.Entity
{
    public class DefaultConnection : DbContext
    {
        public virtual DbSet<tb_Tenants> TenantControl { get; set; }
    }
}
