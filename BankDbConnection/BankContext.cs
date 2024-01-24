using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;
using Models;
using Models.Enums;

namespace BankDbConnection
{
    public class BankContext : DbContext
    {  
        public DbSet<Client> Client { get; set; }
        public DbSet<Employee> Employee { get; set; }
        public DbSet<Account> Account { get; set; }
        public DbSet<Currency> Currency { get; set; }
        public DbSet<Passport> Passport { get; set; }
        public DbSet<Contract> Contract { get; set; }
        public BankContext() 
        {
            AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseNpgsql("Host=localhost;Port=5432;Database=bank;Username=postgres;Password=");
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Client>();
            modelBuilder.Entity<Employee>();
            modelBuilder.Entity<Account>();
            modelBuilder.Entity<Currency>();
            modelBuilder.Entity<Passport>();
            modelBuilder.Entity<Contract>();
            ApplyEnumConverterToString<Employee, JobPosition>(modelBuilder, "JobPositionType");
            ApplyEnumConverterToString<Currency, CurrencyType>(modelBuilder, "TypeCurrency");
            ApplyEnumConverterToString<Passport, GenderType>(modelBuilder, "Gender");
        }
        private static void ApplyEnumConverterToString<TEntity, TEnum>(ModelBuilder modelBuilder, string propertyName)
            where TEntity : class
            where TEnum : Enum
        {
            var entity = modelBuilder.Entity<TEntity>();
            var property = entity.Metadata.FindProperty(propertyName);

            if (property != null && property.ClrType == typeof(TEnum))
            {
                property.SetValueConverter(
                    new ValueConverter<TEnum, string>(
                        v => v.ToString(),
                        v => (TEnum)Enum.Parse(typeof(TEnum), v)
                    )
                );
            }
        }
    }
}
