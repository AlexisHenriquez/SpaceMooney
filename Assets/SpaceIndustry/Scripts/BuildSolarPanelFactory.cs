using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


	public class BuildSolarPanelFactory : Fury.Database.Ability
{

	public GameObject solarPanelFactory;
	
	public override object OnBeginCast(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
	{
		return null;
	}

	public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
	{
		var stats = GameObject.Find("PlayerStatsObject");

		var playerStats = stats.GetComponent<PlayerStats>();

		if (playerStats != null &&
			playerStats.HasInInventory(Buyables.SolarPanelFactory))
		{
			GameObject clone = Instantiate(this.solarPanelFactory, new Vector3(position.x + 0.0F, position.y + 5F, position.z + 0.0F), new Quaternion(0, 0, 0, 0)) as GameObject;

			var solarPanelFactory = playerStats.AddSolarPanelFactory(clone);
			var collisionManager = clone.GetComponent<CollisionManager>();
			collisionManager.Buildable = solarPanelFactory;

			playerStats.UpdateAmountInInventory(Buyables.SolarPanelFactory, -1);
		}
	}
}
