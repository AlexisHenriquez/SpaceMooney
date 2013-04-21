using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class Robonauta : Buildable
{

	public Robonauta()
	{
		this.LifeTime = 1000;
	}

	public override int GetConsumeEnergyFactor()
	{
		return 5;
	}

	public override int GetDegradationFactor()
	{
		return 0;
	}

}
