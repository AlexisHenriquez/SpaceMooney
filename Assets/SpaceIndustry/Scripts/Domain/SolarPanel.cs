
public class SolarPanel : Buildable
{

	public const int PRICE_SOLAR_PANEL = 10;

	public SolarPanel()
	{
		Buyable = new Buyable
		{
			Cost = PRICE_SOLAR_PANEL
		};
	}

}