using Microsoft.Maui.Controls.PlatformConfiguration;
using System.Collections.ObjectModel;
using System.Data;

namespace ProjetComplet.ViewModel;

public partial class LoginViewModel : ObservableObject
{
    MyModelsService myService;
    UserManagementServices myUserDBService;

    [ObservableProperty]
    string myUserID;

    public LoginViewModel(MyModelsService myService)
    {
        this.myService = myService;
        Globals.myDOS.ConfigureScanner();
        Globals.myDOS.SerialBuffer.Changed += onCartScanned;
        CreateUserTables myTableCreator = new();
        myUserDBService = new UserManagementServices();
        myUserDBService.ConfigTools();
        //myDOS.SerialBuffer.Changed += AddScannedPokemon;
    }
    private void onCartScanned(Object sender, EventArgs myArgs)
    { 
        DeviceOrientationServices.QueueBuffer myDOSLocal = (DeviceOrientationServices.QueueBuffer)sender;
        if (Globals.activePage.Equals("LoginPage"))
            MyUserID = myDOSLocal.Dequeue().ToString();
    }
    [RelayCommand]
    private async void LogInAction(string ID)
    {
        DataRow UserData = null;
        if ((MyUserID != null) && !(MyUserID.Equals("")))
        {
            Dictionary<string, object> myDictionary = new Dictionary<string, object>
            {
                {"Data", ID}
            };
            string myCurrentUser = null;
            if (Globals.UserSet.Tables["Access"].Rows.Count <= 0)
                await myUserDBService.ReadAccessTable();
            if (Globals.UserSet.Tables["Users"].Rows.Count <= 0)
                await myUserDBService.FillUserTable();

            if (Globals.UserSet.Tables["Users"].Rows.Count > 0)
            {
                foreach (DataRow myUserData in Globals.UserSet.Tables["Users"].Rows)
                {
                    if (myUserData["User_ID"].Equals(MyUserID))
                    {
                        myCurrentUser = myUserData["User_ID"].ToString();
                        UserData = myUserData;
                    }
                }
            }

            if (myCurrentUser != null)
            {
                try
                {
                    Globals.currentUser.User_ID = UserData["User_ID"].ToString();
                    Globals.currentUser.UserName = UserData["UserName"].ToString();
                    Globals.currentUser.UserPassword = UserData["UserPassword"].ToString();
                    Globals.currentUser.UserAccessType = (short.Parse(UserData["UserAccessType"].ToString()));
                    await Shell.Current.GoToAsync("MainPage", true, myDictionary);
                }
                catch
                (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Page router", ex.Message, "OK");
                }
            }
            else
            {
                try
                {
                    await Shell.Current.GoToAsync("RegisterPage", true, myDictionary);
                }
                catch
                (Exception ex)
                {
                    await Shell.Current.DisplayAlert("Page router", ex.Message, "OK");
                }
            }
        }
        else
        {
            await Shell.Current.DisplayAlert("Error","Should enter ID number card", "OK");
        }
    }
}