using System.Globalization;
using System.Text.Json;

namespace ProjetComplet.Services;

public class MyModelsService : ContentPage
{
    List<Pokemon> Pokemons;

    public async Task<List<Pokemon>> GetPokemons()
    {
        using var stream = await FileSystem.OpenAppPackageFileAsync("Pokedex.json");
        using var reader = new StreamReader(stream);
        var contents = await reader.ReadToEndAsync();
        Pokemons = JsonSerializer.Deserialize<List<Pokemon>>(contents);

        return Pokemons;
    }
}