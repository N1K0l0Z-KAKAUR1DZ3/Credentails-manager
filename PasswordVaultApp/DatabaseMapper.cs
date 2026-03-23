using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.SqlServer;

namespace PasswordVaultApp
{
    class CredentialsTable : DbContext
    {
        public DbSet<Credentials> CredentialsEntrys { get; set; }

        string connectionString = "Data Source=.;Initial Catalog=PasswordVault;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        public CredentialsTable(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }
        
    }
}
