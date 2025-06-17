//using Microsoft.EntityFrameworkCore;
//using Microsoft.EntityFrameworkCore.Design;
//using Infrastructure.Data;

//namespace Infrastructure.Data
//{
//    public class StoreContextFactory : IDesignTimeDbContextFactory<StoreContext>
//    {
//        public StoreContext CreateDbContext(string[] args)
//        {
//            var optionsBuilder = new DbContextOptionsBuilder<StoreContext>();

//            // 🔁 Use the same connection string you have in appsettings.json
//            optionsBuilder.UseSqlServer("Server=DESKTOP-FS996RF\\SQLEXPRESS;Database=MS_Store;Trusted_Connection=True;TrustServerCertificate=True;");

//            return new StoreContext(optionsBuilder.Options);
//        }
//    }
//}
