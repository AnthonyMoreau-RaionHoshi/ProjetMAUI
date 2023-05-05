
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
    [ObservableProperty]
    string myPokemonId;
    [ObservableProperty]
    string myPokemonName;
    [ObservableProperty]
    int myNewPokemonVisible = 0;
    Pokemon myScannedPokemon;
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
    }
    private void AddScannedPokemon(Object sender, EventArgs myArgs)
    {
        DeviceOrientationServices.QueueBuffer myDOSLocal = (DeviceOrientationServices.QueueBuffer)sender;
        MyPokemonId = myDOSLocal.Dequeue().ToString();
    }

    [RelayCommand]
    private void AddPokemonOnList()
    {
        ScannedPokemon.Add(myScannedPokemon);
        MyNewPokemonVisible = 0;
    }

    [RelayCommand]
    private async void SimulateScannedPokemon()
    {
        
        Random myRdm = new Random();
        int myRdmPokId = myRdm.Next(1, 810);
        MyPokemonId = myRdmPokId+"";
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

        // Tentative de récupération des élémoents XAML mais cela ne fonctionne pas et nous avons chercher pendant 4-5H.
        //Dons sommes donc passé sur une solution de binder un tableau.
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