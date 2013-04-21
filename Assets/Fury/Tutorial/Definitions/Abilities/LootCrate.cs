using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class LootCrate : Fury.Database.Ability
{
	public override bool OnCheckUseOnTarget(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, UnityEngine.Vector3 position)
	{
		return target is Fury.Behaviors.Deposit;
	}

	public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, UnityEngine.Vector3 position)
	{
		var targetDeposit = target as Fury.Behaviors.Deposit;

		foreach (var tokenStack in targetDeposit.Controllers.TokenController.Stacks)
		{
			var lootingHandled = false;
			foreach(var existingStack in caster.Controllers.TokenController.Stacks)
				if (existingStack.Token == tokenStack.Token && tokenStack.Token is Consumable)
				{
					caster.ModifyTokenStackState(existingStack, 0, (Byte)Math.Min(existingStack.States[0] + tokenStack.States[0], 255));
					lootingHandled = true;
				}

			if (!lootingHandled)
				caster.CreateTokenStack(tokenStack.Token, tokenStack.States.ToArray());
		}

		targetDeposit.Kill();
	}
}