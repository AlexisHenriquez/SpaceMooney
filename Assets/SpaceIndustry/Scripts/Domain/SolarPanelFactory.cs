using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class SolarPanelFactory : Buildable
{
	public const int SILICON_EXTRACT_FACTOR = 10;

	public const int ALUMINUM_EXTRACT_FACTOR = 10;

	public const int IRON_EXTRACT_FACTOR = 10;

	public const int PRICE_SOLAR_PANEL = 10;

	public SolarPanelFactory(GameObject gameObject)
	{
		this.LifeTime = 25;

		this.gameObject = gameObject;
	}

	public int Silicon { get { return SILICON_EXTRACT_FACTOR; } }
	public int Aluminum { get { return ALUMINUM_EXTRACT_FACTOR; } }
	public int Iron { get { return IRON_EXTRACT_FACTOR; } }

	public override int GetDegradationFactor()
	{
		return 1;
	}

	public override int GetConsumeEnergyFactor()
	{
		return 5;
	}

	public int ConsumeSiliconFactor()
	{
		return 1;
	}

	internal int ConsumeAlluminumFactor()
	{
		return 1;
	}

	internal int ConsumeIronFactor()
	{
		return 1;
	}

}
