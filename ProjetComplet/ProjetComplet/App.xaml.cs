using ProjetComplet.View;

namespace ProjetComplet;

public partial class App : Application
{
	public App()
	{
		InitializeComponent();

		MainPage = new AppShell();

	}
}
