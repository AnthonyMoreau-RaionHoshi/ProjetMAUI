namespace ProjetComplet.View;

public partial class LoginPage : ContentPage
{
    LoginViewModel myLoginviewModel;
    public LoginPage(LoginViewModel viewModel)
    {
        InitializeComponent(); 
        myLoginviewModel = viewModel;
        BindingContext = viewModel;

    }
    protected override void OnNavigatedTo(NavigatedToEventArgs Args)
    {
        Globals.activePage = "LoginPage";
    }
}