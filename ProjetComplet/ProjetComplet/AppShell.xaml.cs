namespace ProjetComplet;

public partial class AppShell : Shell
{
	public AppShell()
	{
		InitializeComponent();

		Routing.RegisterRoute(nameof(DetailsPage), typeof(DetailsPage));
	}

    private void Button_Clicked(object sender, EventArgs e)
    {

    }
}
