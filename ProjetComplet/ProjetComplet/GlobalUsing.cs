global using CommunityToolkit.Mvvm.Input;
global using CommunityToolkit.Mvvm.ComponentModel;

global using ProjetComplet.ViewModel;
global using ProjetComplet.View;
global using ProjetComplet.Model;
global using ProjetComplet.Services;

global using System.Text.Json;

internal class Globals
{
    public static List<Pokemon> PokemonList = new List<Pokemon>();
    internal static Queue<string> SerialBuffer = new Queue<string>();
}
