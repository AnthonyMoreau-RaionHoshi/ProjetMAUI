using Microsoft.Maui.Controls.PlatformConfiguration;

namespace ProjetComplet.ViewModel;

public class LoginViewModel : ContentPage
{
    MyModelsService myService;
    DeviceOrientationServices myDOS;
    public LoginViewModel(MyModelsService myService)
    {
        this.myService = myService;
        this.myDOS = new DeviceOrientationServices();
        myDOS.ConfigureScanner();
        //myDOS.SerialBuffer.Changed += AddScannedPokemon;
    }
}