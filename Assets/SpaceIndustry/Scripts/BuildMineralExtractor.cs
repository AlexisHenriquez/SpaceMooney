using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class BuildMineralExtractor : Fury.Database.Ability
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
            playerStats.HasInInventory(Buyables.MineralExtractor))
        {
            GameObject clone = Instantiate(this.OxygenExtractor, new Vector3(position.x + 0.0F, position.y + 1F, position.z + 0.0F), new Quaternion(0, 0, 0, 0)) as GameObject;

            var mineral = playerStats.AddMineralExtractor(clone);
            var collisionManager = clone.GetComponent<CollisionManager>();
            collisionManager.Buildable = mineral;

            playerStats.UpdateAmountInInventory(Buyables.MineralExtractor, -1);
        }
    }
}
