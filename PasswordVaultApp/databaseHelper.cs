using System;
using System.Runtime.InteropServices.Marshalling;
using Microsoft.Data.SqlClient;
using Microsoft.Identity.Client;

namespace PasswordVaultApp
{
    public static class DatabaseManager
    {
        private static string connectionString = "Server=DESKTOP-LS4I652;Database=PasswordVault;Trusted_Connection=True;TrustServerCertificate=True;";

        private delegate void dataBaseConfigurationLogic(SqlConnection sqlConnection);

        private static void TalkToDB(dataBaseConfigurationLogic dbModificationLogic)
        {
            using (SqlConnection connection = new SqlConnection(connectionString))
            {
                connection.Open();
                dbModificationLogic(connection);
            }
        }

        #region incrementing commands
        public static void IncrementRecord(Credentials creds, SqlConnection sqlCon)
        {
            Credentials? credsRecord = GetRecord(creds, sqlCon);
            if (credsRecord != null)
            {
                throw new Exception($"credential record with name: {creds.Name}, already exsists");
            }
            string sql = "INSERT INTO Credentials (GroupID, CredName, Username, Password) VALUES (@groupId, @name, @user, @pass)";

            using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
            {
                cmd.Parameters.AddWithValue("@groupId", creds.GroupID);
                cmd.Parameters.AddWithValue("@name", creds.Name);
                cmd.Parameters.AddWithValue("@user", creds.Username);
                cmd.Parameters.AddWithValue("@pass", creds.Password);

                cmd.ExecuteNonQuery();

                Console.WriteLine("New credentials added successfully!");
            }
        }
        public static void IncrementRecord(Group group, SqlConnection sqlCon)
        {
            Group? groupRecord = GetRecord(group, sqlCon);
            if (groupRecord != null)
            {
                throw new Exception($"group record with name: {group.Name}, already exsists");
            }
            string sql = "INSERT INTO Groups (GroupName) VALUES (@groupName)";

            using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
            {
                cmd.Parameters.AddWithValue("@groupName", group.Name);

                cmd.ExecuteNonQuery();

                Console.WriteLine("New groupRecord added successfully!");
            }
        }

        public static void SetMainPassword(MainPassword mainPass, SqlConnection sqlCon)
        {
            MainPassword? mainPasswordRecord = GetRecord(mainPass, sqlCon);
            if (mainPasswordRecord != null)
            {
                throw new Exception($"A main password already exsists");
            }

            string sql = "INSERT INTO MainPassword (MainPasswordHash, Salt) VALUES (@mainHash, @salt)";

            using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
            {
                cmd.Parameters.AddWithValue("@mainHash", mainPass.mainHash);
                cmd.Parameters.AddWithValue("@salt", mainPass.salt);

                cmd.ExecuteNonQuery();

                Console.WriteLine("New groupRecord added successfully!");
            }
        }
        #endregion

        #region reading commands
        public static Credentials? GetRecord(Credentials creds, SqlConnection sqlCon)
        {
            string sqlCommandSyntax = "SELECT CredentialID, GroupID, CredNmae, Username, Password FROM Credentials WHERE CredName = @name";

            using (SqlCommand cmd = new SqlCommand(sqlCommandSyntax, sqlCon))
            {
                cmd.Parameters.AddWithValue("@name", creds.Name);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Credentials
                        {
                            Id = reader.GetInt32(0),
                            GroupID = reader.GetInt32(1),
                            Name = reader.GetString(2),
                            Username = reader.GetString(3),
                            Password = reader.GetString(4)
                        };
                    }
                }
            }
            return null;
        }

        public static Group? GetRecord(Group group, SqlConnection sqlCon)
        {
            string sqlCommandSyntax = "SELECT GroupID, GroupName FROM Groups WHERE GroupName = @name";

            using (SqlCommand cmd = new SqlCommand(sqlCommandSyntax, sqlCon))
            {
                cmd.Parameters.AddWithValue("@name", group.Name);

                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new Group(reader.GetInt32(0), reader.GetString(1));
                    }
                }
            }
            return null;
        }
        public static MainPassword? GetRecord(MainPassword creds, SqlConnection sqlCon)
        {
            string sqlCommandSyntax = "SELECT TOP 1 MainPasswordHash, Salt FROM MainPassword";

            using (SqlCommand cmd = new SqlCommand(sqlCommandSyntax, sqlCon))
            {
                using (SqlDataReader reader = cmd.ExecuteReader())
                {
                    if (reader.Read())
                    {
                        return new MainPassword(reader.GetInt32(0), reader.GetString(1), reader.GetString(2));
                    }
                }
            }
            return null;
        }
        #endregion

        #region updating commands
        public static void EditRecord(Credentials requestedCredsOnDb, Credentials editedCreds, SqlConnection sqlCon)
        {
            Credentials? credsRecord = GetRecord(requestedCredsOnDb, sqlCon);
            if (credsRecord == null)
            {
                throw new Exception($"Credentials Record: [{requestedCredsOnDb.Name}, {requestedCredsOnDb.Username}, {requestedCredsOnDb.Password}], does not exsist on the DB");
            }

            string sql = "UPDATE Credemtials SET GroupID = @newGroupId, CredName = @newCredName, Username = @newUsername, Password = @newPassword WHERE CredentialID = @id";

            using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
            {
                cmd.Parameters.AddWithValue("@newGroupId", editedCreds.GroupID);
                cmd.Parameters.AddWithValue("@newCredName", editedCreds.Name);
                cmd.Parameters.AddWithValue("@newUsername", editedCreds.Username);
                cmd.Parameters.AddWithValue("@newPassword", editedCreds.Password);
                cmd.Parameters.AddWithValue("@id", credsRecord.Value.Id);


                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("Update failed: No rows were modified.");
                }
                Console.WriteLine("update successfull");
            }
        }
        public static void EditRecord(Group requestedGroupOnDb, Group editedGroup, SqlConnection sqlCon)
        {
            Group? groupRecord = GetRecord(requestedGroupOnDb, sqlCon);
            if (groupRecord == null)
            {
                throw new Exception($"Group Record: [{requestedGroupOnDb.Id}, {requestedGroupOnDb.Name}], does not exsist on the DB");
            }
            string sql = "UPDATE Groups SET GroupName = @newName WHERE GroupID = @id";

            using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
            {
                cmd.Parameters.AddWithValue("@newName", editedGroup.Name);
                cmd.Parameters.AddWithValue("@id", groupRecord.Value.Id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("Update failed: No rows were modified.");
                }
            }
        }

        public static void EditRecord(MainPassword requestedMainPassOnDb, MainPassword editedMainPassword, SqlConnection sqlCon)
        {
            MainPassword? mainPasswordRecord = GetRecord(requestedMainPassOnDb, sqlCon);
            if (mainPasswordRecord == null)
            {
                throw new Exception($"Master password Record: [{requestedMainPassOnDb.Id}, {requestedMainPassOnDb.mainHash}, {requestedMainPassOnDb.salt}], does not exsist on the DB");
            }

            string sql = "UPDATE MainPassword SET MainPasswordHash = @newMainPasswordHash, Salt = @newSalt";

            using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
            {
                cmd.Parameters.AddWithValue("@newMainPasswordHash", editedMainPassword.mainHash);
                cmd.Parameters.AddWithValue("@newSalt", editedMainPassword.salt);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception("Update failed: No rows were modified.");
                }
                Console.WriteLine("updated main password successfully");
            }
        }
        #endregion

        #region delete commands
        public static void DeleteRecord(Group group, SqlConnection sqlCon)
        {
            Group? groupRecord = GetRecord(group, sqlCon);
            if (groupRecord == null)
            {
                throw new Exception($"Group record {group.Name}, does not exsist");
            }
            string sql = "DELETE FROM Groups WHERE GroupID = @id";

            using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
            {
                cmd.Parameters.AddWithValue("@id", groupRecord.Value.Id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception($"Delete failed: No record {group.Name}.");
                }

                Console.WriteLine($"Group Record {group.Name} deleted successfully.");
            }
        }
        public static void DeleteRecord(Credentials creds, SqlConnection sqlCon)
        {
            Credentials? credRecord = GetRecord(creds, sqlCon);
            if (credRecord == null)
            {
                throw new Exception($"No credentials with name {creds.Name}");
            }
            string sql = "DELETE FROM Credentials WHERE CredentialID = @id";

            using (SqlCommand cmd = new SqlCommand(sql, sqlCon))
            {
                cmd.Parameters.AddWithValue("@id", credRecord.Value.Id);

                int rowsAffected = cmd.ExecuteNonQuery();

                if (rowsAffected == 0)
                {
                    throw new Exception($"Delete failed: No record {creds.Name}.");
                }

                Console.WriteLine($"Group Record {creds.Name} deleted successfully.");
            }
        }
        #endregion
    }
}