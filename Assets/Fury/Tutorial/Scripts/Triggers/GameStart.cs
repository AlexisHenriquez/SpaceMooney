using System;
using System.Linq;
using System.Collections.Generic;

using UnityEngine;
using Fury.Behaviors;

/// <summary>
/// This is an automatically generated trigger template for the 
/// trigger of type OnMapLoaded.
/// </summary>
public class GameStart : MonoBehaviour
{
	public Fury.Database.Unit WarriorType, MageType;

	public Fury.Database.Token[] WarriorStartingItems;
	public Fury.Database.Token[] MageStartingItems;

	private void Start()
	{
		Manager.Instance.OnMapLoaded += OnMapLoaded;
	}

	private void OnMapLoaded()
	{
		var cmdrOneExists = Manager.Instance.Commanders.Count(c => c.Index == Fury.CommanderIndices.One) > 0;
		var cmdrThreeExists = Manager.Instance.Commanders.Count(c => c.Index == Fury.CommanderIndices.Three) > 0;

		Fury.Behaviors.IUnit warrior, mage;
		var spawnPoint = GameObject.Find("-SpawnPoint").transform.position;

		if (cmdrOneExists && cmdrThreeExists)
		{
			warrior = Manager.Instance.CreateUnit(WarriorType,
				Manager.Instance.Commanders.First(c => c.Index == Fury.CommanderIndices.One),
				spawnPoint + new Vector3(1, 0, 0),
				null);

			mage = Manager.Instance.CreateUnit(MageType,
				Manager.Instance.Commanders.First(c => c.Index == Fury.CommanderIndices.Three),
				spawnPoint - new Vector3(1, 0, 0),
				null);
		}
		else
		{
			var idx = cmdrOneExists ? Fury.CommanderIndices.One : Fury.CommanderIndices.Three;
			var cmdr = Manager.Instance.Commanders.First(c => c.Index == idx);

			warrior = Manager.Instance.CreateUnit(WarriorType, cmdr, spawnPoint + new Vector3(1, 0, 0), null);
			mage = Manager.Instance.CreateUnit(MageType, cmdr, spawnPoint - new Vector3(1, 0, 0), null);
		}

		foreach (var item in MageStartingItems)
			if (item is Consumable)
				// State stands for number of pots we have
				mage.CreateTokenStack(item, new Byte[] { 5 });
			else
				// State = 1 stands for equipped, State = 2 stands for unequipped
				mage.CreateTokenStack(item, new Byte[] { 1 });

		foreach (var item in WarriorStartingItems)
			if (item is Consumable)
				// State stands for number of pots we have
				warrior.CreateTokenStack(item, new Byte[] { 5 });
			else
				// State = 1 stands for equipped, State = 2 stands for unequipped
				warrior.CreateTokenStack(item, new Byte[] { 1 });
		
		// Most triggers will just destroy themselves after executing
		Manager.Instance.OnMapLoaded -= OnMapLoaded;
		GameObject.Destroy(this);
	}
}