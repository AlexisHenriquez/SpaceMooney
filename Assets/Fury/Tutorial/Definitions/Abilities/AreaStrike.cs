using System;
using System.Collections.Generic;

using UnityEngine;

/// <summary>
/// This is a template for an area of effect ability. Make sure to uncheck "RequiresTarget"
/// in order to freely use this ability on the ground.
/// </summary>
public class AreaStrike : Fury.Database.Ability
{
	/// <summary>
	/// The damage caused by the ability.
	/// </summary>
	public Int32 Damage = 200;

	/// <summary>
	/// The radius of the ability's area of effect.
	/// </summary>
	public Single Radius = 1f;

	/// <summary>
	/// The amount of energy the ability costs.
	/// </summary>
	public Int32 EnergyCost = 75;

	/// <summary>
	/// The type of energy the ability costs.
	/// </summary>
	public Fury.Database.Energy EnergyType = null;

	/// <summary>
	/// The status that is applied to units caught in the effect.
	/// </summary>
	public Fury.Database.Status StatusEffect = null;

	/// <summary>
	/// Check if the caster has enough energy.
	/// </summary>
	public override bool OnCheckUseOnTarget(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, UnityEngine.Vector3 position)
	{
		// Check if caster has enough energy of the given type
		return caster.Controllers.VitalityController.HasEnergy(EnergyType, EnergyCost);
	}

	/// <summary>
	/// Start casting the ability.
	/// </summary>
	public override object OnBeginCast(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, UnityEngine.Vector3 position)
	{
		// TODO: Start casting animation, effect, etc
		return null;
	}

	/// <summary>
	/// Finish casting the ability, damage units in the ability's area of effect.
	/// </summary>
	public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, UnityEngine.Vector3 position)
	{
		// Reduce the caster's energy
		caster.ModifyEnergy(EnergyType, -EnergyCost);

		// Iterate through all unfriendly commanders
		foreach (var cmdr in Fury.Behaviors.Manager.Instance.Commanders)
			if (!caster.IsTeamOrNeutral(cmdr))
				// Iterate through all units of unfriendly commanders
				foreach (var unit in cmdr.Units.Values)
					// Check if unit is inside the radius
					if (Vector3.Distance(unit.transform.position, position) < Radius)
					{
						// Damage the unit
						unit.ModifyHealth(-Damage, caster, this);

						// Apply status effect if set
						if (StatusEffect != null)
							unit.AddStatus(StatusEffect, caster);
					}

		// Create an effect where the ability is used
		if (Effect != null)
		{
			var effect = (UnityEngine.GameObject)UnityEngine.GameObject.Instantiate(Effect);
			effect.transform.position = position;
		}
	}
}