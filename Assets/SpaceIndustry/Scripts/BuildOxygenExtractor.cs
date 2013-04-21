using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;


public class BuildOxygenExtractor : Fury.Database.Ability
{
    public GameObject OxygenExtractor;

    public override object OnBeginCast(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
    {
        return null;
    }

    public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
    {
        var stats = GameObject.Find("PlayerStatsObject");

        var playerStats = stats.GetComponent<PlayerStats>();

        if (playerStats != null &&
            playerStats.HasInInventory(Buyables.OxygenExtractor))
        {
            GameObject clone = Instantiate(this.OxygenExtractor, new Vector3(position.x + 0.0F, position.y + 5F, position.z + 0.0F), new Quaternion(-90, -360, -360, 0)) as GameObject;

			var oxygen = playerStats.AddOxygenExtractor(clone);
			var collisionManager = clone.GetComponent<CollisionManager>();
			collisionManager.Buildable = oxygen;

            playerStats.UpdateAmountInInventory(Buyables.OxygenExtractor, -1);
        }
    }
}
