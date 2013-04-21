using UnityEngine;

public class SolarPanel : Buildable
{

	public const int PRICE_SOLAR_PANEL = 10;

	public const int LOAD_FACTOR = 1;

	public SolarPanel(GameObject clone)
	{
        Buyable = new Buyable { Cost = PRICE_SOLAR_PANEL };

		this.LifeTime = 20;

		this.gameObject = clone;
	}

	public int Energy { get { return LOAD_FACTOR; } }

	public override int GetDegradationFactor()
	{
		return 1;
	}
}