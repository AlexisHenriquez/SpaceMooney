using UnityEngine;

public class MicrowaveAntenna : Buildable
{

	public const int BUY_PRICE = 100;

	private int SELL_FACTOR = 10;

	public MicrowaveAntenna(GameObject clone)
	{
		Buyable = new Buyable
		{
			Cost = BUY_PRICE
		};

		this.LifeTime = 200;

		this.gameObject = clone;
	}

	public int Energy { get { return SELL_FACTOR; } }

	public override int GetDegradationFactor()
	{
		return 10;
	}

	public override int GetConsumeEnergyFactor()
	{
		return 5;
	}

	public int GetSellFactor()
	{
		return SELL_FACTOR;
	}

	public int GetIncome()
	{
		return 2 * this.GetSellFactor();
	}

}