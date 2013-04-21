using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class OxygenRefinery : Buildable
{
    public const int EXTRACT_FACTOR = 10;

    public const int PRICE_PER_GALLON = 10;

    public OxygenRefinery(GameObject gameObject)
    {
        this.LifeTime = 25;

        this.gameObject = gameObject;
    }

    public int Oxygen { get { return EXTRACT_FACTOR; } }

    public override int GetDegradationFactor()
    {
        return 1;
    }

	public override int GetConsumeEnergyFactor()
	{
		return 5;
	}

	public int GetSellFactor()
	{
		return PRICE_PER_GALLON;
	}

	internal int getIncome()
	{
		return 2 * this.GetSellFactor();
	}
}
