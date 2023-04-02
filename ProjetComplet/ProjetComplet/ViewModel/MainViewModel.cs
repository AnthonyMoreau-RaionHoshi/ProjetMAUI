
using System.Collections.ObjectModel;
namespace ProjetComplet.ViewModel;

public partial class MainViewModel : ObservableObject
{
    [ObservableProperty]
    String monText = "Go to details";
    MyModelsService myService;
    DeviceOrientationServices myDOS;
    [ObservableProperty]
    private bool isBusy = false;
    public ObservableCollection<Pokemon> Pokemons { get; set; } = new();
    public ObservableCollection<Pokemon> ScannedPokemon { get; set; } = new();
    public ObservableCollection<string> AvailableTypes { get; } = new ObservableCollection<string> {"Flying","Grass","Fire","Water","Psychic","Electric","Bug","Normal","Poison","Ground","Rock","Fairy","Ghost","Ice","Fighting","Dark","Dragon","Steel"};
    public MainViewModel(MyModelsService myService)
    {
        this.myService = myService;
        this.myDOS = new DeviceOrientationServices();
        myDOS.ConfigureScanner();
        myDOS.SerialBuffer.Changed += AddScannedPokemon;
    }

    [RelayCommand]
    async Task GetPokemonsFromJson()
    {
        if (Globals.PokemonList.Count== 0) {
            isBusy = true;
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
        isBusy= false;
    }
    [RelayCommand]
    private async void SetFilter(string monType)
    {
        Pokemons.Clear();
        if (Globals.PokemonList.Count != 0)
        {
            isBusy = true;
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
        isBusy = false;
    }
    private async void AddScannedPokemon(Object sender, EventArgs myArgs)
    {
        DeviceOrientationServices.QueueBuffer myDOSLocal = (DeviceOrientationServices.QueueBuffer)sender;
        string myPokemonName = myDOSLocal.Dequeue().ToString();

        if (Globals.PokemonList.Count != 0)
        {
            Pokemon myScannedPokemon = Globals.PokemonList.Where(Pokemon => Pokemon.name_english == myPokemonName).First();
            if (myScannedPokemon != null)
            {
                ScannedPokemon.Add(myScannedPokemon);
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Errors", "No pokemons found! Please load the JSON file", "OK");
        }
    }

    [RelayCommand]
    private async void SimulateScannedPokemon()
    {
        
        Random myRdm = new Random();
        int myRdmPokId = myRdm.Next(1, 810);
        if (Globals.PokemonList.Count != 0) 
        {
            Pokemon myScannedPokemon = Globals.PokemonList.Where(Pokemon => Pokemon.id == myRdmPokId.ToString()).First();
            if (myScannedPokemon != null)
            {
                ScannedPokemon.Add(myScannedPokemon);
            }
        }
        else
        {
            await Application.Current.MainPage.DisplayAlert("Errors", "No pokemons found! Please load the JSON file", "OK");
        }

        // Tentative de r�cup�ration des �l�moents XAML mais cela ne fonctionne pas et nous avons chercher pendant 4-5H.
        //Dons sommes donc pass� sur une solution de binder un tableau.
        /*Grid myGrid = (Grid)Application.Current.FindByName("myGrid");
        ScrollView myScrollView = (ScrollView)myGrid.FindByName("myScannedPokemonScrollView");
        FlexLayout myScannedPokemonView = (FlexLayout)myScrollView.FindByName("ScannedPokemonView");
        if (myScannedPokemon != null && myScannedPokemonView!= null)
        {
            this.ScannedPokemon.Add(myScannedPokemon);

            VerticalStackLayout myContentStackLayout = new VerticalStackLayout
            {
                WidthRequest = 125,
                HeightRequest = 150
            };
            Image myScannedPokemonImage = new ()
            {
                HeightRequest = 100,
                WidthRequest = 100,
                Aspect = Aspect.AspectFill,
                Source = myScannedPokemon.Picture,
                HorizontalOptions = LayoutOptions.Center,
            };
            VerticalStackLayout myLabelsStackLayout = new()
            {
                Children =
                {
                    new Label
                    {
                        Text = myScannedPokemon.name_english,
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = Colors.Black
                    },
                    new Label
                    {
                        Text = myScannedPokemon.id,
                        HorizontalOptions = LayoutOptions.Center,
                        TextColor = Colors.Black
                    }
                }
            };
            myContentStackLayout.Add(myScannedPokemonImage);
            myContentStackLayout.Add(myLabelsStackLayout);
            myScannedPokemonView.Add(myContentStackLayout);
        }
        else
        {
            
        }*/
    }
}