using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class RangedWeapon : Fury.Database.Weapon
{
	public GameObject ArrowPrefab = null;

	public Single Speed = 20f;

	public override void OnAttackComplete(Fury.Behaviors.Unit attacker, Fury.Behaviors.Unit target)
	{
		var arrow = (GameObject)GameObject.Instantiate(ArrowPrefab);
		var projectile = arrow.AddComponent<Projectile>();

		projectile.FlightDuration = Vector3.Distance(attacker.transform.position, target.transform.position) / Speed;
		projectile.StartPoint = attacker.transform.FindChild("-turret").position;
		projectile.WeaponSource = this;
		projectile.AttackingUnit = attacker;
		projectile.TargetUnit = target;
	}
}