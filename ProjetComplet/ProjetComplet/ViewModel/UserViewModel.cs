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
        FillUsersFromDB();
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
            MyList = Globals.UserSet.Tables["Users"].AsEnumerable().Select(m => new User()
            {
                User_ID = m.Field<string>("User_ID"),
                UserName = m.Field<string>("UserName"),
                UserPassword = m.Field<string>("UserPassword"),
                UserAccessType = m.Field<Int16>("UserAccessType"),
            }).ToList();
            User myUserCurrent = MyList.Where(User => User.User_ID == Globals.currentUser.User_ID).First();
            MyList.Remove(myUserCurrent);
        }
        catch (Exception ex)
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
