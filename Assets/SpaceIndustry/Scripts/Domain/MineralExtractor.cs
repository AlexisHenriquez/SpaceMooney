using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class MineralExtractor : Buildable
{
    public const int EXTRACT_FACTOR = 10;

    public const int PRICE_MINERAL_EXTRACTOR = 10;

    public MineralExtractor(GameObject gameObject)
    {
        this.LifeTime = 25;

        this.gameObject = gameObject;
    }

    public int Mineral { get { return EXTRACT_FACTOR; } }

    public override int GetDegradationFactor()
    {
        return 1;
    }

	public override int GetConsumeEnergyFactor()
	{
		return 5;
	}
}
