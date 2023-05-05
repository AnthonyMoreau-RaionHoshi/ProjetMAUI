using Microsoft.Extensions.DependencyInjection;
using ProjetComplet.View;

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
				fonts.AddFont("Pokemon_Solid.ttf", "PokemonFont");

            });

        builder.Services.AddSingleton<LoginPage>();
        builder.Services.AddSingleton<LoginViewModel>();
        builder.Services.AddSingleton<MainViewModel>();
		builder.Services.AddSingleton<MainPage>();

        builder.Services.AddTransient<MyModelsService>();

        return builder.Build();
	}
}
