using System.Data;
using System.Data.OleDb;
using System.Drawing;
using System.Xml.Linq;

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
        string insertUserCommandText = "INSERT INTO DB_Users (User_ID, UserName, UserPassword,UserAccessType) VALUES (@User_ID,@UserName, @UserPassword, @UserAccessType);";
        string DeleteUserCommandText = "DELETE FROM DB_Users WHERE User_ID = @User_ID;"; 
        string SelectUserCommandText = "SELECT * FROM DB_Users ORDER BY User_ID";
        string UpdateUserCommandText = "UPDATE DB_Users SET UserPassword = @UserPassword, UserAccessType = @UserAccessType WHERE UserName = @UserName";

        //UsersAdapter.InsertCommand(); Insertion de données
        //UsersAdapter.DeleteCommand(); suppression de données
        //UsersAdapter.SelectCommand(); Selection de données
        //UsersAdapter.UpdateCommand(); Modification de données

        //Ajout des commandes dans de véritables commandes

        OleDbCommand Insert_User_Command = new OleDbCommand(insertUserCommandText, Connexion);
        OleDbCommand Delete_User_Command = new OleDbCommand(DeleteUserCommandText, Connexion);
        OleDbCommand Select_User_Command = new OleDbCommand(SelectUserCommandText, Connexion);
        OleDbCommand Update_User_Command = new OleDbCommand(UpdateUserCommandText, Connexion);

        //Ajout des commandes dans les commandes de l'adapter

        UsersAdapter.InsertCommand = Insert_User_Command;
        UsersAdapter.DeleteCommand = Delete_User_Command; 
        UsersAdapter.SelectCommand = Select_User_Command;
        UsersAdapter.UpdateCommand = Update_User_Command;


        //Mapper les variables utilisé ici depuis les tables

        UsersAdapter.TableMappings.Add("DB_Users","Users");

        UsersAdapter.InsertCommand.Parameters.Add("@User_ID", OleDbType.VarChar, 40, "User_ID");
        UsersAdapter.InsertCommand.Parameters.Add("@UserName", OleDbType.VarChar, 40, "UserName");
        UsersAdapter.InsertCommand.Parameters.Add("@UserPassword", OleDbType.VarChar, 40, "UserPassword");
        UsersAdapter.InsertCommand.Parameters.Add("@UserAccessType", OleDbType.Numeric, 100, "UserAccessType");

        UsersAdapter.DeleteCommand.Parameters.Add("@User_ID", OleDbType.VarChar, 40, "User_ID");

        UsersAdapter.UpdateCommand.Parameters.Add("@UserName", OleDbType.VarChar, 40, "UserName");
        UsersAdapter.UpdateCommand.Parameters.Add("@UserPassword", OleDbType.VarChar, 40, "UserPassword");
        UsersAdapter.UpdateCommand.Parameters.Add("@UserAccessType", OleDbType.VarChar, 100, "UserAccessType");

    }
    //"abbord read acces et puis User
    public async Task ReadAccessTable()
    {
        Globals.UserSet.Tables["Access"].Clear();
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
    public async Task ReadOwnerTable()
    {
        Globals.UserSet.Tables["Owner"].Clear();
        OleDbCommand SelectCommand = new ("SELECT * FROM DB_Owner ORDER BY Register_ID", Connexion);

        try
        {
            Connexion.Open();

            OleDbDataReader myReader = SelectCommand.ExecuteReader();

            if(myReader.HasRows)
            {
                while(myReader.Read())
                {
                    Globals.UserSet.Tables["Owner"].Rows.Add(new object[] { myReader[1], myReader[2], myReader[0]});
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
        Globals.UserSet.Tables["Owner"].Clear();
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
            await ReadOwnerTable();
        }
    }
    public async Task InsertPokemon(string Owner_ID, string Pokemon_ID)
    {
        try
        {
            OleDbCommand InsertCommand = new OleDbCommand("INSERT INTO DB_Owner (Owner_ID, Pokemon_ID) VALUES (@Owner_ID, @Pokemon_ID)", Connexion);
            InsertCommand.Parameters.AddWithValue("@Owner_ID", Owner_ID);
            InsertCommand.Parameters.AddWithValue("@Pokemon_ID", short.Parse(Pokemon_ID));
            Connexion.Open();
            if (InsertCommand.ExecuteNonQuery()!= 0)
            {
                await Shell.Current.DisplayAlert("Database", "Pokemon Successfully captured", "Ok");
            }
            else
            {
                await Shell.Current.DisplayAlert("Database", "Error ! Can't captured pokemon", "Ok");
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
    public async Task InsertUser(string ID, string Name, string Passwd, Int16 Access)
    {
        try
        {
            UsersAdapter.InsertCommand.Parameters[0].Value = ID;
            UsersAdapter.InsertCommand.Parameters[1].Value = Name;
            UsersAdapter.InsertCommand.Parameters[2].Value = Passwd;
            UsersAdapter.InsertCommand.Parameters[3].Value = Access;
            Connexion.Open();

            if (UsersAdapter.InsertCommand.ExecuteNonQuery() != 0)
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
    public async Task DeletePokemon(short ID)
    {
        try
        {
            OleDbCommand DeleteCommand = new("DELETE FROM DB_Owner WHERE Register_ID = @Register_ID; ", Connexion);
            DeleteCommand.Parameters.Add("@Register_ID", OleDbType.VarChar, 40, "Register_ID");
            DeleteCommand.Parameters[0].Value = ID;
            if (DeleteCommand.ExecuteNonQuery() == 0)
                await Shell.Current.DisplayAlert("Database", "Error ! Can't Delete Pokemon", "Ok");
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "Ok");
        }
    }
    public async Task DeleteUser(string ID)
    {
        try
        {
            Connexion.Open();
            UsersAdapter.DeleteCommand.Parameters[0].Value = ID;
            if (Globals.UserSet.Tables["Owner"].Rows.Count > 0)
            {
                foreach (DataRow myPokemon in Globals.UserSet.Tables["Owner"].Rows)
                {
                    if (myPokemon["Owner_ID"].Equals(ID))
                        DeletePokemon(short.Parse(myPokemon["Register_ID"].ToString()));
                }
                await Shell.Current.DisplayAlert("Database", "Pokemons are free now", "Ok");
            }     
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