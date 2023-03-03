namespace ProjetComplet.Services;
using System.IO.Ports;
public partial class DeviceOrientationServices
{
	SerialPort mySerialPorts;
    public partial void ConfigureScanner() // partial car le contenu va être dév dans la partie spécifique à la platforme utilisé
    {
		this.mySerialPorts = new SerialPort();

        mySerialPorts.PortName = "COM6";
        mySerialPorts.BaudRate = 9600;
        mySerialPorts.Parity = Parity.None;
        mySerialPorts.DataBits = 8;
        mySerialPorts.StopBits = StopBits.One;

        mySerialPorts.ReadTimeout= 1000;
        mySerialPorts.WriteTimeout= 1000;

        mySerialPorts.DataReceived += new SerialDataReceivedEventHandler(DataHandler);
        try
        {
            mySerialPorts.Open();
        }

        catch (Exception ex)
        {
            Shell.Current.DisplayAlert("Error!", ex.Message, "Ok");
        }
    }
    private void DataHandler(object sender, SerialDataReceivedEventArgs e)
    {
        SerialPort sp = (SerialPort)sender;
        string data = "";

        data = sp.ReadTo("\r");

        Globals.SerialBuffer.Enqueue(data);
    }
}