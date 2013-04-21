using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class HealthPot : Consumable
{
	public Int32 ReplenishAmount = 50;

	public override void OnTokenStackStateChanged(Fury.Behaviors.Stack stack, byte index, short oldVal, short newVal)
	{
		if (newVal < oldVal)
		{
			(stack.Owner as Fury.Behaviors.Unit).ModifyHealth(ReplenishAmount, null, this);
		}
	}
}