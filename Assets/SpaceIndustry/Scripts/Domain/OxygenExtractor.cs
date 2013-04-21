using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class OxygenExtractor : Buildable
{
    public const int EXTRACT_FACTOR = 10;

    public const int PRICE_OXYGEN_EXTRACTOR = 10;

    public OxygenExtractor(GameObject gameObject)
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
}
