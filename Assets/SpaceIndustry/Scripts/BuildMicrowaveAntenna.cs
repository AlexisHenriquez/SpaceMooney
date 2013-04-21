using UnityEngine;
using System.Collections;

public class BuildMicrowaveAntenna : Fury.Database.Ability
{

	public GameObject MicrowaveAntenna;
	
	public override object OnBeginCast(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
	{
		return null;
	}

	public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
	{
		var stats = GameObject.Find("PlayerStatsObject");

		var playerStats = stats.GetComponent<PlayerStats>();

		if (playerStats != null &&
			playerStats.HasInInventory(Buyables.MicrowaveAntenna))
		{
			GameObject clone = Instantiate(this.MicrowaveAntenna, new Vector3(position.x + 0.0F, position.y + 5F, position.z + 0.0F), new Quaternion(0, 0, 0, 0)) as GameObject;

			var antenna = playerStats.AddMicrowaveAntenna(clone);
			var collisionManager = clone.GetComponent<CollisionManager>();
			collisionManager.Buildable = antenna;

			playerStats.UpdateAmountInInventory(Buyables.MicrowaveAntenna, -1);
		}
	}
	
}
