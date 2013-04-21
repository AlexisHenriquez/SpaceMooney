using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using Fury.Database;

public class WarriorWeapon : Weapon
{
	public override void OnAttackComplete(Fury.Behaviors.Unit attacker, Fury.Behaviors.Unit target)
	{
		// Our attack damage is based on strength
		var totalStr = 0;
		foreach (var stack in attacker.Controllers.TokenController.Stacks)
			if (stack.Token is Equipment && stack.States[0] > 0)
				totalStr += (stack.Token as Equipment).Strength;

		target.ModifyHealth(-(Damage + totalStr), attacker, this);
	}
}