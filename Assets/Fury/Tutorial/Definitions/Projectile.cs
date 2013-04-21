using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class Projectile : MonoBehaviour
{
	public Vector3 StartPoint;
	public Fury.Behaviors.Unit TargetUnit, AttackingUnit;
	public Fury.Database.Weapon WeaponSource;
	public Single FlightDuration;

	private Single TimeElapsed;

	public void Update()
	{
		TimeElapsed += Time.deltaTime;

		if (TargetUnit.IsDestroyed)
			GameObject.Destroy(gameObject);

		var age = Mathf.Clamp01(TimeElapsed / FlightDuration);
		var pos = Vector3.Lerp(StartPoint, TargetUnit.collider.bounds.center, age);

		gameObject.transform.rotation = Quaternion.LookRotation(pos - transform.position);
		gameObject.transform.position = pos;

		if (age > 0.99f)
		{
			TargetUnit.ModifyHealth(-WeaponSource.Damage, AttackingUnit, WeaponSource);
			GameObject.Destroy(gameObject);
		}
	}
}