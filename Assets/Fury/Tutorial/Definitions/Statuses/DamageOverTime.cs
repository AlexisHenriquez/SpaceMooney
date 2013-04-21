using System;
using System.Collections.Generic;

public class DamageOverTime : Fury.Database.Status, Fury.Database.Status.IPeriodic
{
	public Single Period = 1f;
	public Int32 Amount = 10;
	public UnityEngine.GameObject EffectPrefab = null;

	float Fury.Database.Status.IPeriodic.Period { get { return Period; } }

	object Fury.Database.Status.IPeriodic.OnUpdate(object tag, Fury.Behaviors.Unit unit, Fury.Behaviors.Unit from)
	{
		if (EffectPrefab != null)
		{
			var effect = (UnityEngine.GameObject)UnityEngine.GameObject.Instantiate(EffectPrefab);
			effect.transform.position = unit.collider.bounds.center;
			effect.transform.parent = unit.transform;
		}

		unit.ModifyHealth(Amount, from, this);
		return null;
	}
}