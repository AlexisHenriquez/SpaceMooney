
public class SolarPanel : Buildable
{

	public const int PRICE_SOLAR_PANEL = 10;

	public const int LOAD_FACTOR = 1;

	public SolarPanel()
	{
		Buyable = new Buyable
		{
			Cost = PRICE_SOLAR_PANEL
		};
	}

	public int Energy { get { return LOAD_FACTOR; } }
	
}