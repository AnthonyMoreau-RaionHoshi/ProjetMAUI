namespace ProjetComplet.ViewModel;


[QueryProperty(nameof(MonTxt),"Databc")]
public partial class DetailsViewModel : ObservableObject
{
	[ObservableProperty]
	string monTxt;
	public DetailsViewModel()
	{

	}
}