using System;
using System.Data;

namespace ProjetComplet.Services
{
    public partial class UserManagementServices
    {
        internal partial void ConfigTools();
    }

    public class CreateUserTables
    {
        public CreateUserTables()
        {
            DataTable UserTable = new DataTable();
            DataTable AccessTable = new DataTable();
            DataTable OwnerTable = new DataTable();

            DataColumn User_ID = new DataColumn("User_ID", typeof(string));
            DataColumn UserName = new DataColumn("UserName", typeof(string));
            DataColumn UserPassword = new DataColumn("UserPassword", typeof(string));
            DataColumn AccessType = new DataColumn("UserAccessType", typeof(short));

            DataColumn Owner_ID = new DataColumn("Owner_ID", typeof(string));
            DataColumn Pokemon_ID = new DataColumn("Pokemon_ID", typeof(string));
            DataColumn Register_ID = new DataColumn("Register_ID", typeof(short));


            DataColumn Access_ID = new DataColumn("Access_ID", typeof(short));
            DataColumn AccessName = new DataColumn("AccessName", typeof(string));
            DataColumn CreateObject = new DataColumn("CreateObject", typeof(bool));
            DataColumn DestroyObject = new DataColumn("DestroyObject", typeof(bool));
            DataColumn ModifyObject = new DataColumn("ModifyObject", typeof(bool));
            DataColumn ChangeUserRights = new DataColumn("ChangeUserRights", typeof(bool));

            // UserTable
            UserTable.TableName = "Users";

            User_ID.Unique = true;
            UserTable.Columns.Add(User_ID);
            UserName.Unique = true;
            UserTable.Columns.Add(UserName);
            UserTable.Columns.Add(UserPassword);
            UserTable.Columns.Add(AccessType);

            // AccessTable
            AccessTable.TableName = "Access";

            Access_ID.AutoIncrement = true;
            Access_ID.Unique = true;
            AccessTable.Columns.Add(Access_ID);

            AccessName.Unique = true;
            AccessTable.Columns.Add(AccessName);

            AccessTable.Columns.Add(CreateObject);
            AccessTable.Columns.Add(DestroyObject);
            AccessTable.Columns.Add(ModifyObject);
            AccessTable.Columns.Add(ChangeUserRights);

            // OwnerTable
            OwnerTable.TableName = "Owner";
            Register_ID.AutoIncrement = true;
            Register_ID.Unique = true;

            OwnerTable.Columns.Add(Owner_ID);
            OwnerTable.Columns.Add(Pokemon_ID);
            OwnerTable.Columns.Add(Register_ID);

            Globals.UserSet.Tables.Add(AccessTable);
            Globals.UserSet.Tables.Add(UserTable);
            Globals.UserSet.Tables.Add(OwnerTable);

            DataColumn parentColumn = Globals.UserSet.Tables["Access"].Columns["Access_ID"];
            DataColumn childColumn = Globals.UserSet.Tables["Users"].Columns["UserAccessType"];

            DataRelation relation = new DataRelation("Access2User", parentColumn, childColumn);

            DataColumn UserColumn = Globals.UserSet.Tables["Users"].Columns["User_ID"];
            DataColumn OwnerColumn = Globals.UserSet.Tables["Owner"].Columns["Owner_ID"];

            DataRelation ownerRelation = new DataRelation("User2Owner", UserColumn, OwnerColumn);

            Globals.UserSet.Tables["Owner"].ParentRelations.Add(ownerRelation);
            Globals.UserSet.Tables["Users"].ParentRelations.Add(relation);
        }
    }
}
