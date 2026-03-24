using System;
using System.Collections.Generic;
using System.Text;

namespace PasswordVaultApp
{
    internal interface IVaultDataAccess
    {
        List<Credentials> GetAllCredentials();
        Credentials? GetCredentialById(int id);
        List<Credentials> GetCredentialsByGroup(int groupId);
        List<Group> GetAllGroups();
        MainPassword? GetMainPassword();

        void AddCredentials(Credentials creds);
        void AddGroup(Group group);
        void CreateMainPassword(MainPassword mainPass);

        void EditCredentials(Credentials creds);
        void EditGroup(Group group);
        void EditMainPassword(MainPassword mainPass);

        void DeleteCredentials(Credentials creds);
        void DeleteGroup(Group group);
    }
}
