namespace ProjetComplet.View;

public partial class RegisterPage : ContentPage
{
	public RegisterPage(RegisterViewModel viewModel)
	{

        InitializeComponent();
        BindingContext = viewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs Args)
    {
        Globals.activePage = "RegisterPage";
    }
}