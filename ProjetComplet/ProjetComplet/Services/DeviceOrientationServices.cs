using System.Collections;

namespace ProjetComplet.Services;
public partial class DeviceOrientationServices // partial car on a une deuxi�me d�finition 
{
	public QueueBuffer SerialBuffer;
	public DeviceOrientationServices()
	{}
	public partial void ConfigureScanner();// partial car le contenu va �tre d�v dans la partie sp�cifique � la platforme utilis�

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