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
			GameObject clone = Instantiate(this.SolarPanel, new Vector3(position.x + 0.0F, position.y, position.z + 0.0F), caster.transform.rotation) as GameObject;

			playerStats.AddSolarPanel();

			if (this.Effect != null)
			{
				clone = Instantiate(this.Effect) as GameObject;

				clone.transform.position = position;
			}

			playerStats.UpdateAmountInInventory(Buyables.SolarPanel, -1);
		}
	}
	
}
