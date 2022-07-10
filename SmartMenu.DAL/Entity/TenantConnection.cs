using SmartMenu.DAL.Models;
using System;
using System.Collections.Concurrent;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.SqlClient;
namespace SmartMenu.DAL.Entity
{
    public partial class TenantConnection : DbContext
    {
        private TenantConnection(DbConnection connection, DbCompiledModel model) : base(connection, model, contextOwnsConnection: false)
        {
        }
      
        public virtual DbSet<DbErrorModel> DbErrorModel { get; set; }
        public virtual DbSet<APIKeyModel> APIKeyModel { get; set; }
        public virtual DbSet<BusinessHoursModel> BusinessHoursModel { get; set; }
        public virtual DbSet<CategoryModel> CategoryModel { get; set; }
        public virtual DbSet<ContactUsModel> ContactUsModel { get; set; }
        public virtual DbSet<CustomerModel> CustomerModel { get; set; }
        public virtual DbSet<CustomerAddressesModel> CustomerAddresses { get; set; }
        public virtual DbSet<MenuItemModel> MenuItemModel { get; set; }
        public virtual DbSet<OrderModel> OrderModel { get; set; }
        public virtual DbSet<OrderStatusLogModel> OrderStatusLogModel { get; set; }
        public virtual DbSet<PaymentModel> PaymentModel { get; set; }
        public virtual DbSet<QuickPagesModel> QuickPagesModel { get; set; }
        public virtual DbSet<RestaurantModel> RestaurantModel { get; set; }       
        public virtual DbSet<SocialMediaModel> SocialMediaModel { get; set; }

        public virtual DbSet<SubscriptionModel> SubscriptionModel { get; set; }        


        private static ConcurrentDictionary<Tuple<string, string>, DbCompiledModel> modelCache = new ConcurrentDictionary<Tuple<string, string>, DbCompiledModel>();
        public static TenantConnection Create(string tenantSchema, DbConnection connection)
        {
            try
            {
                var compiledModel = modelCache.GetOrAdd(
               Tuple.Create(connection.ConnectionString, tenantSchema),
               t =>
               {
                   var builder = new DbModelBuilder();
                   builder.Conventions.Remove<IncludeMetadataConvention>();
                   builder.Entity<DbErrorModel>().ToTable("DB_Errors", tenantSchema);
                   builder.Entity<APIKeyModel>().ToTable("tb_APIKey", tenantSchema);
                   builder.Entity<BusinessHoursModel>().ToTable("tb_BusinessHours", tenantSchema);
                   builder.Entity<CategoryModel>().ToTable("tb_Categories", tenantSchema);
                   builder.Entity<CustomerModel>().ToTable("tb_Customer", tenantSchema);
                   builder.Entity<ContactUsModel>().ToTable("tb_ContactUs", tenantSchema);
                   builder.Entity<CustomerAddressesModel>().ToTable("tb_CustomerAddresses", tenantSchema);
                   builder.Entity<MenuItemModel>().ToTable("tb_MenuItem", tenantSchema);
                   builder.Entity<OrderModel>().ToTable("tb_Orders", tenantSchema);
                   builder.Entity<OrderStatusLogModel>().ToTable("tb_OrderStatusLogs", tenantSchema);
                   builder.Entity<PaymentModel>().ToTable("tb_Payments", tenantSchema);
                   builder.Entity<QuickPagesModel>().ToTable("tb_QuickLinks", tenantSchema);
                   builder.Entity<RestaurantModel>().ToTable("tb_Restaurants", tenantSchema);
                   builder.Entity<SocialMediaModel>().ToTable("tb_SocialMedia", tenantSchema);
                   builder.Entity<SubscriptionModel>().ToTable("tb_Subscription", tenantSchema);
                   var model = builder.Build(connection);
                   return model.Compile();
               });
                return new TenantConnection(connection, compiledModel);
            }
            catch(Exception ex)
            {
                return null;
            }
           
        }

        /// <summary>
        /// Creates the database and/or tables for a new tenant
        /// </summary>
        public static void ProvisionTenant(string tenantSchema, DbConnection connection)
        {
            using (var ctx = Create(tenantSchema, connection))
            {
                if (!ctx.Database.Exists())
                {
                    ctx.Database.Create();
                }

                CreateStoreProcedureCopy(ctx.Database.Connection.Database);
                //else{
                //var createScript = ((IObjectContextAdapter)ctx).ObjectContext.CreateDatabaseScript();
                //if (createScript != "")
                //    ctx.Database.ExecuteSqlCommand(createScript);
            }
        }
        public static void CreateStoreProcedureCopy(string tenantDbName)
        {
            using (SqlConnection connection = new SqlConnection(ConfigurationManager.ConnectionStrings["DefaultConnection"].ConnectionString))
            {
                try
                {
                    connection.Open();
                    using (SqlCommand command = new SqlCommand("Exec [dbo].[USPCopySP] @Name", connection))
                    {
                        command.Parameters.AddWithValue("@Name", tenantDbName);
                        SqlDataReader reader = command.ExecuteReader();
                    }
                    connection.Close();
                }
                catch (Exception ex)
                {
                }
            }
        }
    }
}

