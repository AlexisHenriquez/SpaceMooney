using UnityEngine;
using System.Collections;

public class BuildSolarPanel : Fury.Database.Ability
{

	public GameObject SolarPanel;
	
	public override object OnBeginCast(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
	{
		return null;
	}

	public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
	{
		var stats = GameObject.Find("PlayerStatsObject");

		var playerStats = stats.GetComponent<PlayerStats>();

		if (playerStats != null &&
			playerStats.HasInInventory(Buyables.SolarPanel))
		{
			GameObject clone = Instantiate(this.SolarPanel, new Vector3(position.x + 0.0F, position.y + 5F, position.z + 0.0F), new Quaternion(0, 0, 0, 0)) as GameObject;

			var panel = playerStats.AddSolarPanel(clone);
			var collisionManager = clone.GetComponent<CollisionManager>();
			collisionManager.Buildable = panel;

			playerStats.UpdateAmountInInventory(Buyables.SolarPanel, -1);
		}
	}
	
}
