namespace ProjetComplet.View;

public partial class UserPage : ContentPage
{
    public UserPage(UserViewModel viewModel)
    {
        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs Args)
    {
        Globals.activePage = "UserPage";
    }
}