using System;
using System.Linq;
using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class CreepSpawner : MonoBehaviour
{
	[Serializable]
	public class LootEntry
	{
		public Fury.Database.Token Token;
		public Single Probability;
	}
	
	public Fury.Database.Unit SpawnType = null;
	public Fury.Database.Deposit ChestType = null;

	public List<LootEntry> LootTable = null;

	public Int32 SpawnNum = 3;
	public Single Radius = 5;
	public Single ChaseDistance = 20f;
	public Single AggroDistance = 8f;
	public Single RespawnFrequency = 10f; 
	public Single DecisionFrequency = 1f;

	private Single DecisionTimer = 1;

	private List<Int32> Creeps;
	
	private void Start()
	{
		Fury.Behaviors.Manager.Instance.OnMapLoaded += OnMapLoaded;
		Fury.Behaviors.Manager.Instance.OnUnitDead += new Fury.General.DUnitDead(OnUnitDead);
	}

	private void Update()
	{
		if (Creeps == null) return;

		DecisionTimer -= Time.deltaTime;

		if (DecisionTimer < 0)
		{
			foreach (var id in Creeps)
			{
				var creep = Fury.Behaviors.Manager.Instance.Find<Fury.Behaviors.Unit>(id);

				if (creep != null)
				{
					if (creep.State == Fury.UnitStates.Idle)
					{
						foreach (var commander in Fury.Behaviors.Manager.Instance.Commanders)
							if (commander != creep.Owner)
								foreach (var target in commander.Units.Values)
									if (Vector3.Distance(target.transform.position, creep.transform.position) < AggroDistance)
									{
										creep.Order(target);
									}
					}

					if (Vector3.Distance(creep.transform.position, transform.position) > ChaseDistance)
					{
						creep.Order(GetRandomPosition());
					}
				}
			}

			DecisionTimer = DecisionFrequency;
		}
	}

	private void OnUnitDead(Fury.Behaviors.Unit deadUnit, Fury.Behaviors.Unit lastAttacker)
	{
		if (Creeps.Exists(i => i == deadUnit.Identifier))
		{
			Creeps.Remove(deadUnit.Identifier);

			SpawnLoot(deadUnit.transform.position);

			StartCoroutine("QueueSpawn");
			StartCoroutine("CleanSpawn", deadUnit.gameObject);
		}
	}

	private void OnMapLoaded()
	{
		Creeps = new List<Int32>();
		for (Int32 i = 0; i < SpawnNum; i++)
			CreateSpawn();
	}

	private IEnumerator CleanSpawn(System.Object gameObject)
	{
		yield return new WaitForSeconds(RespawnFrequency / 2);

		GameObject.Destroy(gameObject as GameObject);
	}

	private IEnumerator QueueSpawn()
	{
		yield return new WaitForSeconds(RespawnFrequency);

		CreateSpawn();
	}

	private void SpawnLoot(Vector3 vector3)
	{
		var chest = Fury.Behaviors.Manager.Instance.CreateDeposit(ChestType, vector3, null);

		foreach (var entry in LootTable)
		{
			if (UnityEngine.Random.Range(0f, 1f) < entry.Probability)
			{
				chest.CreateTokenStack(entry.Token, new Byte[] { 1 });
			}
		}
	}

	private void CreateSpawn()
	{
		var createdSpawn = Fury.Behaviors.Manager.Instance.CreateUnit(SpawnType,
			Fury.Behaviors.Manager.Instance.Commanders.First(c => c.Index == Fury.CommanderIndices.Two),
			GetRandomPosition(), null);
		
		Creeps.Add(createdSpawn.Identifier);
	}

	private Vector3 GetRandomPosition()
	{
		return transform.position + new Vector3(UnityEngine.Random.Range(-Radius, Radius), 0, UnityEngine.Random.Range(-Radius, Radius));
	}

	private void OnDrawGizmos()
	{
		Gizmos.color = new Color(0.8f, 0.8f, 0.8f); 
		Gizmos.DrawWireCube(transform.position, new Vector3(Radius * 2, 1, Radius * 2));
	}
}