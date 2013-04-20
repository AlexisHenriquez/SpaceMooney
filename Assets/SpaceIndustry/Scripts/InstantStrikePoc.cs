using UnityEngine;
using System.Collections;

public class InstantStrikePoc : Fury.Database.Ability
{

	public int Damage = 200;

	public GameObject Buildable;

	public GameObject Storage;

	public Fury.Database.Status StatusEffect;

	public override object OnBeginCast(Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
	{
		//caster.Animator.CrossFade("cast-begin", 1000, CastTime, QueueMode.PlayNow, WrapMode.ClampForever);

		return null;
	}

	public override void OnEndCast(object tag, Fury.Behaviors.Unit caster, Fury.Behaviors.Targetable target, Vector3 position)
	{
		var storageObject = GameObject.Find("PlayerStorageObject");

		var playerStorage = storageObject.GetComponent<PlayerStorage>();

		if (playerStorage != null &&
			playerStorage.HasPanelesSolares())
		{
			var targetAsUnit = (target as Fury.Behaviors.Unit);

			//targetAsUnit.ModifyHealth(-Damage, caster, this);

			GameObject clone = Instantiate(this.Buildable, new Vector3(position.x + 0.0F, position.y + 5.0F, position.z + 0.0F), caster.transform.rotation) as GameObject;

			if (this.Effect != null)
			{
				clone = Instantiate(this.Effect) as GameObject;

				clone.transform.position = position;
			}

			playerStorage.DisminuirUnidadPanelSolar();
		}

		//if (this.StatusEffect != null &&
		//    targetAsUnit != null)
		//{
		//    targetAsUnit.AddStatus(this.StatusEffect, caster);
		//}
	}
	
}
