using Microsoft.EntityFrameworkCore;

namespace PasswordVaultApp;

class CredentialsHolderDB : DbContext, IVaultDataAccess
{
        string connectionString = "Data Source=.;Initial Catalog=PasswordVault;Integrated Security=True;Connect Timeout=30;Encrypt=True;Trust Server Certificate=True;Application Intent=ReadWrite;Multi Subnet Failover=False";

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlServer(connectionString);
        }

        public DbSet<Credentials> Credentials { get; set; }
        public DbSet<Group> Groups { get; set; }
        public DbSet<MainPassword> MainPassword { get; set; }


    public List<Credentials> GetAllCredentials() => Credentials.ToList();

    public Credentials? GetCredentialById(int id) => Credentials.Find(id);
    public List<Credentials> GetCredentialsByGroup(int groupId) => Credentials.Where(c => c.GroupID == groupId).ToList();
    public List<Group> GetAllGroups() => Groups.ToList();
    public MainPassword? GetMainPassword() => MainPassword.FirstOrDefault();


    public void AddCredentials(Credentials creds)
    {
        if(Credentials.Any(c => c.CredName == creds.CredName))
        {
            throw new Exception($"Credentials with the name {creds.CredName} already exzist");
        }
        Credentials.Add(creds);
        SaveChanges();
    }

    public void AddGroup(Group group)
    {
        if (Groups.Any(c => c.GroupName == group.GroupName))
        {
            throw new Exception($"Group with the name {group.GroupName} already exsists");
        }
        Groups.Add(group);
        SaveChanges();
    }

    public void CreateMainPassword(MainPassword mainPass)
    {
        if (MainPassword.Any())
        {
            throw new Exception("A main password already exists.");
        }
        MainPassword.Add(mainPass);
        SaveChanges();
    }


    public void EditCredentials(Credentials creds)
    {
        var existing = Credentials.Find(creds.CredentialID);
        if (existing is null)
        {
            throw new Exception($"Credential with ID {creds.CredentialID} not found.");
        }
            
        Entry(existing).CurrentValues.SetValues(creds);
        SaveChanges();
    }

    public void EditGroup(Group group)
    {
        var existing = Groups.Find(group.GroupID);
        if (existing is null)
        {
            throw new Exception($"Group with ID {group.GroupID} not found.");
        }
            
        Entry(existing).CurrentValues.SetValues(group);
        SaveChanges();
    }

    public void EditMainPassword(MainPassword mainPass)
    {
        var existing = MainPassword.Find(mainPass.Id);
        if (existing is null)
        {
            throw new Exception($"Main password with ID {mainPass.Id} not found.");
        }
           
        Entry(existing).CurrentValues.SetValues(mainPass);
        SaveChanges();
    }


    public void DeleteCredentials(Credentials creds)
    {
        var existing = Credentials.Find(creds.CredentialID);
        if (existing is null)
            throw new Exception($"Credential with ID {creds.CredentialID} not found.");

        Credentials.Remove(existing);
        SaveChanges();
    }

    public void DeleteGroup(Group group)
    {
        var existing = Groups.Find(group.GroupID);
        if (existing is null)
            throw new Exception($"Group with ID {group.GroupID} not found.");

        Groups.Remove(existing);
        SaveChanges();
    }
}


public class Credentials
{
    public int CredentialID { get; set; }
    public int GroupID { get; set; }
    public string CredName { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public class MainPassword
{
    public int Id { get; set; }
    public string MainPasswordHash { get; private set; }
    public string Salt { get; private set; }

    private MainPassword() { }
    public MainPassword(string password)
    {
        Salt = SecurityHelper.GenerateRandomSalt();
        MainPasswordHash = SecurityHelper.GenerateHash(password, Salt);
    }
}

public class Group
{
    public int GroupID { get; set; }
    public string GroupName { get; set; }
    private Group() { }
    public Group(string groupName)
    {
        GroupName = groupName;
    }
}
