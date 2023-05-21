namespace ProjetComplet.ViewModel;

[QueryProperty(nameof(MyUserID), "Data")]
public partial class RegisterViewModel : ObservableObject
{
    [ObservableProperty]
    string myUserName;
    [ObservableProperty]
    string myPassWord;
    [ObservableProperty]
    string myUserID;


    UserManagementServices myUserDBService;
    public RegisterViewModel()
	{
        myUserDBService = new UserManagementServices();
        myUserDBService.ConfigTools();
    }

    [RelayCommand]
	private async void GoToMainPage()
	{
        if (!(MyUserName == null) || !(MyPassWord == null))
		{
            await myUserDBService.InsertUser(MyUserID, MyUserName, MyPassWord,2);

            await myUserDBService.FillUserTable();
            try
            {
                Globals.currentUser.User_ID = myUserID;
                Globals.currentUser.UserName = MyUserName;
                Globals.currentUser.UserPassword = MyPassWord;
                Globals.currentUser.UserAccessType = 2;
                await Shell.Current.GoToAsync("MainPage", true, new Dictionary<string, object>
                {
                    {"Data", myUserID}
                });
            }
            catch
            (Exception ex)
            {
                await Shell.Current.DisplayAlert("Page router", ex.Message, "OK");
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Errors", "Please, fill in all the gaps", "OK");
        }
    }
}