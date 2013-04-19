using UnityEngine;
using System.Collections;

public class ItemPoc : Fury.Database.Token
{

	public string ItemName;

	public override void OnTokenStackCreated(Fury.Behaviors.Stack stack)
	{
	}

	public override void OnTokenStackStateChanged(Fury.Behaviors.Stack stack, byte index, short oldVal, short newVal)
	{
		var unit = stack.Owner as Fury.Behaviors.Unit;

		if (index == 0 && newVal > 0)
		{
			var compos = unit.gameObject.GetComponents<SkinnedMeshRenderer>();

			if (compos != null)
			{
				foreach (var item in compos)
				{
					item.enabled = true;
				}
			}
		}
		else
			if (index == 0)
			{
				var composs = unit.gameObject.GetComponents<SkinnedMeshRenderer>();

				if (composs != null)
				{
					foreach (var item in composs)
					{
						item.enabled = true;
					}
				}
			}
	}

}