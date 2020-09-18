using Darooha.Data.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Darooha.Data.DatabaseContext
{
    public class DaroohaDbContext : IdentityDbContext<Tbl_User, Role, string,
    IdentityUserClaim<string>, UserRole, IdentityUserLogin<string>,
    IdentityRoleClaim<string>, IdentityUserToken<string>>
    {
        public DaroohaDbContext()
        {
        }
        public DaroohaDbContext(DbContextOptions<DaroohaDbContext> opt) : base(opt)
        {

        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(@"data source=.;initial catalog=Darooha;integrated security=True;multipleactiveresultsets=true");
            ///optionsBuilder.UseSqlServer(@"Server=185.10.75.8;Database=darooha_db;User Id=darooha.main.db;Password=1741034681.Shm;");
            //optionsBuilder.UseSqlServer(@"data source=185.10.75.8;initial catalog=Darooha;integrated security=True;multipleactiveresultsets=true");
        }

        public DbSet<Tbl_Setting> Tbl_Settings { get; set; }
        public DbSet<Token> Tokens { get; set; }
        public DbSet<Notification> Notifications { get; set; }
        public DbSet<Tbl_UserAddress> Tbl_UserAddresses { get; set; }
        public DbSet<Tbl_Wallet> Tbl_Wallets { get; set; }
        public DbSet<Ticket> Tickets { get; set; }
        public DbSet<TicketContent> TicketContents { get; set; }
        public DbSet<Tbl_Order> Tbl_Orders { get; set; }
        public DbSet<Tbl_OrderItem> Tbl_OrderItems { get; set; }
        public DbSet<Tbl_Product> Tbl_Products { get; set; }
        public DbSet<Tbl_ProductImage> Tbl_ProductImages { get; set; }
        public DbSet<Tbl_Menu> Tbl_Menues { get; set; }
        public DbSet<Tbl_SubMenu> Tbl_SubMenues { get; set; }
        public DbSet<Tbl_HomeFirstSlider> Tbl_HomeFirstSlider { get; set; }
        public DbSet<Tbl_Comment> Tbl_Comments { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            base.OnModelCreating(builder);


            builder.Entity<UserRole>(userRole =>
            {
                userRole.HasKey(ur => new { ur.UserId, ur.RoleId });

                userRole.HasOne(ur => ur.Role)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.RoleId)
                    .IsRequired();

                userRole.HasOne(ur => ur.User)
                    .WithMany(r => r.UserRoles)
                    .HasForeignKey(ur => ur.UserId)
                    .IsRequired();
            });
        }
    }
}



//            Add-Migration init -context DaroohaDbContext -OutPutDir Migrations\MainDataBase
//                           Update-Database -context DaroohaDbContext



/*builder.Entity<Tbl_OrderAddress>(orderAddress =>
        {
            orderAddress.HasKey(oa => oa.Order );

            orderAddress.HasOne(oa => oa.Order)
                .WithMany(r => r.Tbl_OrderAddresss)
                //.HasForeignKey(oa => oa.OrderId)
                .IsRequired();

            orderAddress.HasOne(oa => oa.Address)
                .WithMany(r => r.Tbl_OrderAddresss)
                //.HasForeignKey(oa => oa.AddressId)
                .IsRequired();
        });*/

//builder.Entity<Models.Tbl_UserAddress>(userRole =>
//{
//    //userRole.HasKey(ur => new { ur., ur.ID });
//    userRole.HasKey(ur => ur.ID);

//    userRole.HasMany(ur => ur.Tbl_Orders)
//        .WithOne(r => r.UserAddresses)
//        .OnDelete(DeleteBehavior.NoAction)
//        .IsRequired();

//    userRole.HasOne(ur => ur.User)
//    .WithMany(u => u.Tbl_UserAddresses)
//    .OnDelete(DeleteBehavior.Cascade);
//});

//builder.Entity<Tbl_Order>(order => 
//{
//    order.HasOne(o => o.User)
//    .WithMany(u => u.Tbl_Orders)
//    .OnDelete(DeleteBehavior.NoAction);
//});