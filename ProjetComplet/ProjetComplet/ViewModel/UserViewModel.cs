using ProjetComplet.Services;
using System.Collections.ObjectModel;
using System.Data;

namespace ProjetComplet.ViewModel;
public partial class UserViewModel : ObservableObject
{
    UserManagementServices MyDBServices;
    public UserViewModel(UserManagementServices MyDBService)
    {
        this.MyDBServices = MyDBService;
        MyDBServices.ConfigTools();
    }

    public ObservableCollection<User> ShownList { get; set; } = new();

    [ObservableProperty]
    [NotifyPropertyChangedFor(nameof(IsNotBusy))]
    bool isBusy;
    public bool IsNotBusy => !IsBusy;
    [RelayCommand]
    async Task ReadAccess()
    {
        try
        {
            await MyDBServices.ReadAccessTable();
        }
        catch (Exception ex) 
        {
            await Shell.Current.DisplayAlert("DataBase", ex.Message, "ok");
        }
    }

    [RelayCommand]
    async Task FillUsersFromDB()
    {
        IsBusy = true;

        List<User> MyList = new();

        try
        {
            DataRow Access1 = Globals.UserSet.Tables["Access"].NewRow();
            Access1[0] = 1;
            Access1[1] = "Admin";
            Access1[2] = true;
            Access1[3] = true;
            Access1[4] = true;
            Access1[5] = true;

            DataRow Access2 = Globals.UserSet.Tables["Access"].NewRow();
            Access2[0] = 2;
            Access2[1] = "User";
            Access2[2] = true;
            Access2[3] = false;
            Access2[4] = false;
            Access2[5] = false;
            Globals.UserSet.Tables["Access"].Rows.Add(Access1);
            Globals.UserSet.Tables["Access"].Rows.Add(Access2);

            DataRow User1 = Globals.UserSet.Tables["Users"].NewRow();
            User1[0] = 1;
            User1[1] = "aaa";
            User1[2] = "aaa";
            User1[3] = 1;

            DataRow User2 = Globals.UserSet.Tables["Users"].NewRow();
            User2[0] = 2;
            User2[1] = "bbb";
            User2[2] = "bbb";
            User2[3] = 2;

            Globals.UserSet.Tables["Users"].Rows.Add(User1);
            Globals.UserSet.Tables["Users"].Rows.Add(User2);
        }
        catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }

        try
        {
            MyList = Globals.UserSet.Tables["Users"].AsEnumerable().Select(m => new User()
            {
                User_ID = m.Field<string>("User_ID"),
                UserName = m.Field<string>("UserName"),
                UserPassword = m.Field<string>("UserPassword"),
                UserAccessType = m.Field<Int16>("UserAccessType"),
            }).ToList();
        }catch (Exception ex)
        {
            await Shell.Current.DisplayAlert("Database", ex.Message, "OK");
        }

        foreach(var item in MyList) 
        {
            ShownList.Add(item);
        }

        IsBusy = false;
    }
}
