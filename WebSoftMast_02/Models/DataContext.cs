using Microsoft.EntityFrameworkCore;

namespace WebSoftMast_02.Models
{
    public class DataContext : DbContext
    {
        public DataContext(DbContextOptions<DataContext> options) : base(options)
        { }

        public static DataContext Get_DataContext()
        {
            var builder = new ConfigurationBuilder();

            // установка пути к текущему каталогу
            builder.SetBasePath(Directory.GetCurrentDirectory());

            // получаем конфигурацию из файла appsettings.json
            builder.AddJsonFile("appsettings.json");

            // создаем конфигурацию
            var config = builder.Build();

            // получаем строку подключения
            string connectionString = config.GetConnectionString("softMasterConnect");

            var optionsBuilder = new DbContextOptionsBuilder<DataContext>();
            var options = optionsBuilder
                .UseSqlServer(connectionString)
                .Options;


            return new DataContext(options);

        }


        [Obsolete]
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder?.Entity<Detail>()
                .HasIndex(e => e.PositionInTrain)
                .HasName("IND_PositionTrain");

            modelBuilder?.Entity<NatSheet>()
                .HasIndex(e => e.TrainNumber)
                .HasName("IND_TrainNumber");
        }


        public DbSet<NatSheet> NatSheets => Set<NatSheet>();
        public DbSet<Detail> Details => Set<Detail>();
       

    }
}
