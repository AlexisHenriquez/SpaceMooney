using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BuildOxygenRefinery : Fury.Database.Ability
{
    public GameObject OxygenRefinery;

    public override object OnBeginCast(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
    {
        return null;
    }

    public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
    {
        var stats = GameObject.Find("PlayerStatsObject");

        var playerStats = stats.GetComponent<PlayerStats>();

        if (playerStats != null &&
            playerStats.HasInInventory(Buyables.OxygenRefinery))
        {
            GameObject clone = Instantiate(this.OxygenRefinery, new Vector3(position.x + 0.0F, position.y + 1F, position.z + 0.0F), new Quaternion(-90, -360, -360, 0)) as GameObject;

			var oxygen = playerStats.AddOxygenRefinery(clone);
			var collisionManager = clone.GetComponent<CollisionManager>();
			collisionManager.Buildable = oxygen;

            playerStats.UpdateAmountInInventory(Buyables.OxygenRefinery, -1);
        }
    }
}
