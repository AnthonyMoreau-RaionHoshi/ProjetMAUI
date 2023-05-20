
using ProjetComplet.Services;
using ProjetComplet.View;
using System.Collections.ObjectModel;
namespace ProjetComplet.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    String monText = "Go to details";
    MyModelsService myService;
    [ObservableProperty]
    private bool isBusy = false;
    public ObservableCollection<Pokemon> Pokemons { get; set; } = new();
    public ObservableCollection<Pokemon> ScannedPokemon { get; set; } = new();
    [ObservableProperty]
    string myPokemonId;
    [ObservableProperty]
    string myPokemonName;
    [ObservableProperty]
    int myAdminVisible = 0;
    [ObservableProperty]
    int myNewPokemonVisible = 0;
    Pokemon myScannedPokemon;
    [ObservableProperty]
    bool isVisible;
    public ObservableCollection<string> AvailableTypes { get; } = new ObservableCollection<string> {"Flying","Grass","Fire","Water","Psychic","Electric","Bug","Normal","Poison","Ground","Rock","Fairy","Ghost","Ice","Fighting","Dark","Dragon","Steel"};
    public MainViewModel(MyModelsService myService)
    {
        this.myService = myService;
        Globals.myDOS.SerialBuffer.Changed += AddScannedPokemon;
        GetPokemonsFromJson();
        if (Globals.currentUser.UserAccessType == 1)
            myAdminVisible = 60;
    }
    async Task GetPokemonsFromJson()
    {
        if (Globals.PokemonList.Count== 0) {
            IsBusy = true;
            try
            {
                Globals.PokemonList = await myService.GetPokemons();
            }
            catch (Exception ex)
            {
                await Shell.Current.DisplayAlert("Error!", ex.Message, "Ok");
            }

            foreach (Pokemon pokemon in Globals.PokemonList)
            {
                Pokemons.Add(pokemon);
            }
        }
        IsBusy= false;
    }
    [RelayCommand]
    private async void SetFilter(string monType)
    {
        Pokemons.Clear();
        if (Globals.PokemonList.Count != 0)
        {
            IsBusy = true;

            if (monType == "ID")
            {
                foreach (Pokemon pokemon in Globals.PokemonList)
                {
                    Pokemons.Add((Pokemon)pokemon);
                }
            }
            else
            {
                foreach (Pokemon pokemon in Globals.PokemonList)
                {
                    if (pokemon.type_0 == monType)
                        Pokemons.Add((Pokemon)pokemon);
                }
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Errors", "No pokemons found! Please load the JSON file", "OK");
        }
        IsBusy = false;
    }
    partial void OnMyPokemonIdChanged(string value)
    {
        if (Globals.PokemonList.Count != 0)
        {
            MyNewPokemonVisible = 100;
            Pokemon myScannedPokemonTemp = Globals.PokemonList.Where(Pokemon => Pokemon.id == value).First();
            if (myScannedPokemonTemp != null)
            {
                MyPokemonName = myScannedPokemonTemp.name_english;
                myScannedPokemon = myScannedPokemonTemp;
            }
        }
        else
        {
            Application.Current.MainPage.DisplayAlert("Errors", "No pokemons found! Please load the JSON file", "OK");
        }
    }
    private void AddScannedPokemon(Object sender, EventArgs myArgs)
    {
        DeviceOrientationServices.QueueBuffer myDOSLocal = (DeviceOrientationServices.QueueBuffer)sender;
        if (Globals.activePage.Equals("MainPage"))
            MyPokemonId = myDOSLocal.Dequeue().ToString();
    }
    [RelayCommand]
    private void AddPokemonOnList()
    {
        ScannedPokemon.Add(myScannedPokemon);
        MyNewPokemonVisible = 0;
    }
    [RelayCommand]
    private async void GoToUserPage()
    {
        try
        {
            await Shell.Current.GoToAsync("UserPage", true);
        }
        catch 
        (Exception ex)
        {
            await Shell.Current.DisplayAlert("Page router", ex.Message, "OK");
        }
    }

    [RelayCommand]
    private async void SimulateScannedPokemon()
    {
        if (Globals.activePage.Equals("MainPage"))
        {
            Random myRdm = new Random();
            int myRdmPokId = myRdm.Next(1, 810);
            MyPokemonId = myRdmPokId + "";
        }
    }
}