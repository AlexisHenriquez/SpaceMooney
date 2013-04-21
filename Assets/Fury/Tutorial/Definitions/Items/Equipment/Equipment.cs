using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Equipment : Fury.Database.Token
{
	public String ItemModel;

	public String ItemGroup;

	public Int32 Intelligence;

	public Int32 Strength;

	public override void OnTokenStackCreated(Fury.Behaviors.Stack stack)
	{
	}

	public override void OnTokenStackStateChanged(Fury.Behaviors.Stack stack, byte index, short oldVal, short newVal)
	{
		var unit = stack.Owner as Fury.Behaviors.Unit;

		if (index == 0 && newVal > 0)
		{
			var compos = unit.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();

			foreach (var smr in compos)
				if (smr.name == ItemModel)
				{
					smr.enabled = true;
				}
				else if (smr.name.Contains(ItemGroup))
				{
					smr.enabled = false;
				}
		}
		else if (index == 0 && newVal == 0)
		{
			var compos = unit.gameObject.GetComponentsInChildren<SkinnedMeshRenderer>();
			foreach (var smr in compos)
				if (smr.name == ItemModel)
				{
					smr.enabled = false;
				}
				else if (smr.name == "base" + ItemGroup)
				{
					smr.enabled = true;
				}
		}
	}
}