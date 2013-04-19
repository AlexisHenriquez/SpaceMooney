using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class DamageOverTimePoc : Fury.Database.Status, Fury.Database.Status.IPeriodic
{

	public float Period = 1F;

	public int Amount = 10;

	public GameObject EffectPrefab;

	float Fury.Database.Status.IPeriodic.Period
	{
		get
		{
			return this.Period;
		}
	}
	object Fury.Database.Status.IPeriodic.OnUpdate(object tag, Fury.Behaviors.Unit unit, Fury.Behaviors.Unit from)
	{
		unit.ModifyHealth(Amount, from, this);

		return null;
	}

}