using Microsoft.Extensions.DependencyInjection;

namespace ProjetComplet;

public static class MauiProgram
{
	public static MauiApp CreateMauiApp()
	{
		var builder = MauiApp.CreateBuilder();
		builder
			.UseMauiApp<App>()
			.ConfigureFonts(fonts =>
			{
				fonts.AddFont("OpenSans-Regular.ttf", "OpenSansRegular");
				fonts.AddFont("OpenSans-Semibold.ttf", "OpenSansSemibold");
			});

		builder.Services.AddSingleton<MainViewModel>(); // transient pour les pages du type unique (Accueil, paramettres, etc..)
        builder.Services.AddSingleton<MainPage>();
		
        builder.Services.AddTransient<DetailsViewModel>(); // transient pour pouvoir réutiliser une view et pouvoir modifier le contenu d'une page sans modifier la page précédente qui utilise la même view
        builder.Services.AddTransient<DetailsPage>();

        builder.Services.AddTransient<MyModelsService>();

        return builder.Build();
	}
}
