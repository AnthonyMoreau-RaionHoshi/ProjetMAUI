
using ProjetComplet.Services;
using ProjetComplet.View;
using System.Collections.ObjectModel;
using System.Data;

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
    UserManagementServices myUserDBService;
    public MainViewModel(MyModelsService myService)
    {
        this.myService = myService;
        myUserDBService = new UserManagementServices();
        myUserDBService.ConfigTools();
        Globals.myDOS.SerialBuffer.Changed += AddScannedPokemon;
        if (Globals.currentUser.UserAccessType == 1)
            myAdminVisible = 60;
        fillScannedPokemons();
    }

    private async void fillScannedPokemons()
    {
        await GetPokemonsFromJson();
        myUserDBService.FillUserTable();
        if (Globals.UserSet.Tables["Owner"].Rows.Count > 0)
        {
            foreach (DataRow myPokemon in Globals.UserSet.Tables["Owner"].Rows)
            {
                if (myPokemon["Owner_ID"].Equals(Globals.currentUser.User_ID))
                {
                    if (Globals.PokemonList.Count != 0)
                    {
                        Pokemon myScannedPokemonTemp = Globals.PokemonList.Where(Pokemon => Pokemon.id == myPokemon["Pokemon_ID"].ToString()).First();
                        ScannedPokemon.Add(myScannedPokemonTemp);
                    }
                }
            }
        }
    }

    async Task GetPokemonsFromJson()
    {
        Globals.PokemonList.Clear();
        IsBusy = true;
        if (Globals.PokemonList.Count <= 0)
        {
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
        
        IsBusy = false;
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
    private async void AddPokemonOnList()
    {
        ScannedPokemon.Add(myScannedPokemon);
        await myUserDBService.InsertPokemon(Globals.currentUser.User_ID,myScannedPokemon.id);
        MyNewPokemonVisible = 0;
        await myUserDBService.ReadOwnerTable();
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