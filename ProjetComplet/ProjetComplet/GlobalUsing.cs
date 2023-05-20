global using CommunityToolkit.Mvvm.Input;
global using CommunityToolkit.Mvvm.ComponentModel;

global using ProjetComplet.ViewModel;
global using ProjetComplet.Model;
global using ProjetComplet.Services;

global using System.Text.Json;
using System.Data;

internal class Globals
{
    public static List<Pokemon> PokemonList = new List<Pokemon>();
    public static DataSet UserSet = new DataSet();
    public static DeviceOrientationServices myDOS= new DeviceOrientationServices();
    public static User currentUser = new User();
    public static string activePage="LoginPage";
}
