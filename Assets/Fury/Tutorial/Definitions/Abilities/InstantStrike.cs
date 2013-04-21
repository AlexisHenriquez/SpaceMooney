using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

class InstantStrike : Fury.Database.Ability
{
	public Int32 Damage = 200;

	public Int32 EnergyCost = 75;

	public Fury.Database.Energy EnergyType = null;

	public Fury.Database.Status StatusEffect = null;

	public override bool OnCheckUseOnTarget(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, UnityEngine.Vector3 position)
	{
		if (caster.Controllers.VitalityController.HasEnergy(EnergyType, EnergyCost) && target is Fury.Behaviors.Unit)
			return true;

		return false;
	}

	public override object OnBeginCast(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, UnityEngine.Vector3 position)
	{
		caster.Animator.CrossFade("cast-begin", 1000, CastTime, null, UnityEngine.WrapMode.ClampForever);
		return null;
	}

	public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, UnityEngine.Vector3 position)
	{
		var totalInt = 0;
		foreach (var stack in caster.Controllers.TokenController.Stacks)
			if (stack.Token is Equipment && stack.States[0] > 0)
				totalInt += (stack.Token as Equipment).Intelligence;

		(target as Fury.Behaviors.Unit).ModifyHealth(-(Damage + totalInt), caster, this);
		caster.ModifyEnergy(EnergyType, -EnergyCost);

		if (Effect != null)
		{
			var effect = (UnityEngine.GameObject)UnityEngine.GameObject.Instantiate(Effect);
			effect.transform.position = target.transform.position;
		}

		if (StatusEffect != null)
			(target as Fury.Behaviors.Unit).AddStatus(StatusEffect, caster);
	}
}