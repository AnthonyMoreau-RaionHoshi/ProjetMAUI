using System.Collections;

namespace ProjetComplet.Services;
public partial class DeviceOrientationServices // partial car on a une deuxième définition 
{
	public QueueBuffer SerialBuffer;
	public DeviceOrientationServices()
	{}
	public partial void ConfigureScanner();// partial car le contenu va être dév dans la partie spécifique à la platforme utilisé

	public class QueueBuffer : Queue
	{
		public event EventHandler Changed;

		protected virtual void OnChanged()
		{
			Changed?.Invoke(this, EventArgs.Empty);
		}

        public override void Enqueue(object obj)
        {
			base.Enqueue(obj);
			OnChanged();
        }
    }
}