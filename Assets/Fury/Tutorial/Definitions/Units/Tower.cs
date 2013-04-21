using System;
using System.Collections.Generic;

using UnityEngine;

class Tower : Fury.Database.Unit
{
	public GameObject RubblePrefab = null;
	
	public Fury.Database.Deposit CratePrefab = null;

	public Fury.Database.Token[] LootTable = null;

	public override void OnDead(Fury.Behaviors.Unit corpse, Fury.Behaviors.Unit killer)
	{
		if (RubblePrefab != null)
		{
			var rubble = (GameObject)GameObject.Instantiate(RubblePrefab);
			rubble.transform.position = corpse.transform.position;
		}

		if (CratePrefab != null && LootTable != null)
		{
			var crate = Fury.Behaviors.Manager.Instance.CreateDeposit(CratePrefab, corpse.transform.position, null);
			foreach (var item in LootTable)
				crate.CreateTokenStack(item, new[] { (Byte)1 });
		}

		GameObject.Destroy(corpse.gameObject);
	}
}