using System;
using System.Collections.Generic;

using UnityEngine;

class DummyLogic : Fury.Database.Unit
{
	public override void OnDead(Fury.Behaviors.Unit corpse, Fury.Behaviors.Unit killer)
	{
		var colliders = corpse.GetComponentsInChildren<Collider>();

		foreach (var collider in colliders)
		{
			if (collider != corpse.collider)
			{
				collider.enabled = true;
				var body = collider.gameObject.AddComponent<Rigidbody>();
				body.WakeUp();
				body.AddExplosionForce(1f, Vector3.zero, 0.1f);
			}
		}

		GameObject.Destroy(corpse.collider);
	}
}