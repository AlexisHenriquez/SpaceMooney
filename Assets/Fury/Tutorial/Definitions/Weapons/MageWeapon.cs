using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;
using Fury.Database;
// using Fury.Behaviors;

using Unit = Fury.Behaviors.Unit;
using Target = Fury.Behaviors.Targetable;

public class MageWeapon : Weapon
{
	public UnityEngine.GameObject BurnEffect = null;

	public override void OnAttackComplete(Fury.Behaviors.Unit attacker, Fury.Behaviors.Unit target)
	{		
		// Our attack damage is based on strength
		var tInt = 0;
		foreach (var stack in attacker.Controllers.TokenController.Stacks)
			if (stack.Token is Equipment && stack.States[0] > 0)
				tInt += (stack.Token as Equipment).Intelligence;
		
		// Create the effect
		var effect = (GameObject)
			GameObject.Instantiate(BurnEffect);
		
		// Position the effect
		var position = target.transform.position;
		effect.transform.position = position;
		effect.transform.parent = target.transform;

		// Reduce the target's health
		target.ModifyHealth(-(Damage + tInt / 3), 
			attacker, 
			this);
	}
}