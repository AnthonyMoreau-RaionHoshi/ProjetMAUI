namespace ProjetComplet;

public partial class MainPage : ContentPage
{
	public MainPage(MainViewModel viewModel)
	{
		InitializeComponent();
		BindingContext = viewModel;
    }
    protected override void OnNavigatedTo(NavigatedToEventArgs Args)
    {
        Globals.activePage = "MainPage";
    }
}

