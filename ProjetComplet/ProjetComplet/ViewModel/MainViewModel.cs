using System.Collections.ObjectModel;

namespace ProjetComplet.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    String monText = "Go to details";

    MyModelsService myService;
    public ObservableCollection<Pokemon> Pokemons { get; set; } = new();
    DeviceOrientationServices deviceOrientationServices;
    public MainViewModel(MyModelsService myService)
    {
        deviceOrientationServices = new DeviceOrientationServices();

        deviceOrientationServices.ConfigureScanner();

        this.myService = myService;
    }
    [RelayCommand]
    async Task GoToDetailsPage(string data)
    {
        await Shell.Current.GoToAsync(nameof(DetailsPage),true,new Dictionary<string, object>
        {
            {"Databc",data }
        });
    }

    [RelayCommand]
    async Task GetPokemonsToJson(string data)
    {
        try
        {
            Globals.PokemonList = await myService.GetPokemons();
        }
        catch(Exception ex)
        {
            await Shell.Current.DisplayAlert("Error!", ex.Message, "Ok");
        }
        finally
        {

        }

        foreach (Pokemon pokemon in Globals.PokemonList)
        {
            Pokemons.Add(pokemon);
        }
    }
}