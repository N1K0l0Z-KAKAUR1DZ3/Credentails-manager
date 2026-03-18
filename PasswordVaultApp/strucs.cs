
using System.Configuration;

namespace PasswordVaultApp;

public struct Credentials

{
    public int Id {get; set;}
    public int GroupID { get; set; }
    public string Name { get; set; }
    public string Username { get; set; }
    public string Password { get; set; }
}

public struct MainPassword
{
    public int Id{get; set;}
    public string mainHash { get; private set; }
    public string salt { get; private set; }

    public MainPassword(string password)
    {
        salt = SecurityHelper.GenerateRandomSalt();
        mainHash = SecurityHelper.GenerateHash(password, salt);
    }

    public MainPassword(int id, string hashedPassword, string inputSalt)
    {
        Id = id;
        salt = inputSalt;
        mainHash = hashedPassword;
    }

}

public struct Group
{
    public int Id{get; private set;}
    public string Name{get; private set;}

    public Group(string name)
    {
        Name = name;
    }

    public Group(int id, string name)
    {
        Name = name;
        Id = id;
    }
}