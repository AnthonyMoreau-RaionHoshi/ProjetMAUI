using System.Data.OleDb;
using System.Drawing;

namespace ProjetComplet.Services;

public partial class UserManagementServices
{
    internal OleDbDataAdapter UsersAdapter = new();
    internal OleDbConnection Connexion = new();

    internal partial void ConfigTools()
    {
        //Définir le collection String
        Connexion.ConnectionString = "Provider=Microsoft.ACE.OLEDB.16.0;"+
                                     "Data Source="+Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.Desktop),"QualitySources", "UserAccounts.accdb") +
                                     ";Persist Security Info=False;";


        //Définir les commandes
        string insertCommandText = "INSERT INTO DB_Users (User_ID, UserName, UserPassword,UserAccessType) VALUES (@User_ID,@UserName, @UserPassword, @UserAccessType);";
        string DeleteCommandText = "DELETE FROM DB_Users WHERE UserName = @UserName;"; 
        string SelectCommandText = "SELECT * FROM DB_Users ORDER BY User_ID";
        string UpdateCommandText = "UPDATE DB_Users SET UserPassword = @UserPassword, UserAccessType = @UserAccessType WHERE UserName = @UserName";

        //UsersAdapter.InsertCommand(); Insertion de données
        //UsersAdapter.DeleteCommand(); suppression de données
        //UsersAdapter.SelectCommand(); Selection de données
        //UsersAdapter.UpdateCommand(); Modification de données

        //Ajout des commandes dans de véritables commandes

        OleDbCommand Insert_Command = new OleDbCommand(insertCommandText, Connexion);
        OleDbCommand Delete_Command = new OleDbCommand(DeleteCommandText, Connexion);
        OleDbCommand Select_Command = new OleDbCommand(SelectCommandText, Connexion);
        OleDbCommand Update_Command = new OleDbCommand(UpdateCommandText, Connexion);

        //Ajout des commandes dans les commandes de l'adapter

        UsersAdapter.InsertCommand = Insert_Command;
        UsersAdapter.DeleteCommand = Delete_Command; 
        UsersAdapter.SelectCommand = Select_Command;
        UsersAdapter.UpdateCommand = Update_Command;


        //Mapper les variables utilisé ici depuis les tables

        UsersAdapter.TableMappings.Add("DB_Users","Users");

        UsersAdapter.InsertCommand.Parameters.Add("@User_ID", OleDbType.VarChar, 40, "User_ID");
        UsersAdapter.InsertCommand.Parameters.Add("@UserName", OleDbType.VarChar, 40, "UserName");
        UsersAdapter.InsertCommand.Parameters.Add("@UserPassword", OleDbType.VarChar, 40, "UserPassword");
        UsersAdapter.InsertCommand.Parameters.Add("@UserAccessType", OleDbType.Numeric, 100, "UserAccessType");

        UsersAdapter.DeleteCommand.Parameters.Add("@UserName", OleDbType.VarChar, 40, "UserName");

        UsersAdapter.UpdateCommand.Parameters.Add("@UserName", OleDbType.VarChar, 40, "UserName");
        UsersAdapter.UpdateCommand.Parameters.Add("@UserPassword", OleDbType.VarChar, 40, "UserPassword");
        UsersAdapter.UpdateCommand.Parameters.Add("@UserAccessType", OleDbType.VarChar, 100, "UserAccessType");
    }
    //"abbord read acces et puis User
    public async Task ReadAccessTable()
    {
        OleDbCommand SelectCommand = new ("SELECT * FROM DB_Access ORDER BY Access_ID", Connexion);

        try
        {
            Connexion.Open();

            OleDbDataReader myReader = SelectCommand.ExecuteReader();

            if(myReader.HasRows)
            {
                while(myReader.Read())
                {
                    Globals.UserSet.Tables["Access"].Rows.Add(new object[] { myReader[0], myReader[1], myReader[2], myReader[3], myReader[4], myReader[5] });
                }
            }

            myReader.Close();
        }
        catch (Exception ex) 
        {
            await Shell.Current.DisplayAlert("Database",ex.Message, "Ok"); 
        }
        finally
        {
            Connexion.Close();
        }
    }
    public async Task FillUserTable()
    {
        Globals.UserSet.Tables["Users"].Clear();

        try
        {
            Connexion.Open();

            UsersAdapter.Fill(Globals.UserSet.Tables["Users"]);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "Ok");
        }
        finally
        {
            Connexion.Close();
        }
    }
    public async Task InsertUser(string ID, string Name, string Passwd, Int16 Access)
    {
        try
        {
            Connexion.Open();
            UsersAdapter.InsertCommand.Parameters[0].Value = ID;
            UsersAdapter.InsertCommand.Parameters[1].Value = Name;
            UsersAdapter.InsertCommand.Parameters[2].Value = Passwd;
            UsersAdapter.InsertCommand.Parameters[3].Value = Access;

            if (UsersAdapter.InsertCommand.ExecuteNonQuery()!= 0)
            {
                await Shell.Current.DisplayAlert("Database", "User Successfully insterted", "Ok");
            }
            else
            {
                await Shell.Current.DisplayAlert("Database", "Error ! Can't insert user", "Ok");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "Ok");
        }
        finally
        {
            Connexion.Close();
        }
    }
    public async Task DeleteUser(string Name)
    {

        try
        {
            Connexion.Open();
            UsersAdapter.DeleteCommand.Parameters[0].Value = Name;

            if (UsersAdapter.DeleteCommand.ExecuteNonQuery() != 0)
            {
                await Shell.Current.DisplayAlert("Database", "User Successfully Deleted", "Ok");
            }
            else
            {
                await Shell.Current.DisplayAlert("Database", "Error ! Can't Delete user", "Ok");
            }
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "Ok");
        }
        finally
        {
            Connexion.Close();
        }
    }
    public async Task UpdatetUser()
    {
        try
        {
            Connexion.Open();

            UsersAdapter.Update(Globals.UserSet.Tables["Users"]);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "Ok");
        }
        finally
        {
            Connexion.Close();
        }
    }
}