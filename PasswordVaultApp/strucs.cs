
using System.Configuration;

namespace PasswordVaultApp;

struct Credentials

{
    public string Name { get; private set; }
    public string Username { get; private set; }
    public string Password { get; private set; }
    public int GroupID { get; private set; }

    public Credentials(string name, string user, string pass, int groupId)
    {
        Name = name;
        Username = user;
        Password = pass;
        GroupID = groupId;
    }
}

struct MainPassword
{
    public string mainHash { get; private set; }
    public string salt { get; private set; }

    public MainPassword(string password)
    {
        salt = SecurityHelper.GenerateRandomSalt();
        mainHash = SecurityHelper.GenerateHash(password, salt);
    }

    public MainPassword(string password, string inputSalt)
    {
        salt = inputSalt;
        mainHash = SecurityHelper.GenerateHash(password, salt);
    }

}

struct Group
{
    public int Id{get; private set;}
    public string Name{get; private set;}

    public Group(string name)
    {
        Name = name;
    }

    public Group(string name, int id)
    {
        Name = name;
        Id = id;
    }
}