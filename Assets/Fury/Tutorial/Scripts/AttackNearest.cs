using System;
using System.Collections.Generic;
using UnityEngine;

public class AttackNearest : MonoBehaviour
{
	private Single Timer;

	private void Update()
	{
		Timer -= Time.deltaTime;

		if (Fury.Behaviors.Manager.Instance == null || Fury.Behaviors.Manager.Instance.GameState != Fury.GameStates.Playing)
			return;

		if (Timer < 0)
		{
			Timer = 0.25f;

			var self = GetComponent<Fury.Behaviors.Unit>();

			if (self == null || self.IsDestroyed) return;
			if (self.Properties.Weapon == null) return;

			Fury.Behaviors.Unit closest = null;
			var minDist = self.Properties.Weapon.Range * 0.9f;

			foreach (var cmdr in Fury.Behaviors.Manager.Instance.Commanders)
				if (!self.IsTeamOrNeutral(cmdr))
					foreach (var unit in cmdr.Units.Values)
					{
						var dist = Vector3.Distance(unit.transform.position, self.transform.position);
						if (dist < minDist)
						{
							minDist = dist;
							closest = unit;
						}
					}

			if (closest != null)
			{
				self.Order(closest);
			}
		}
	}
}