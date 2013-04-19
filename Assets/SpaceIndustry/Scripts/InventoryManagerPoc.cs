using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

public class InventoryManagerPoc : MonoBehaviour
{

	public GameObject Player;

	private KeyCode[] Hotkeys = { KeyCode.Q, KeyCode.R, KeyCode.T, KeyCode.Y };

	void Update()
	{
		var unit = Player.GetComponent<Fury.Behaviors.Unit>();

		for (int i = 0; i < Hotkeys.Length; i++)
		{
			if (Input.GetKeyDown(Hotkeys[i]) && i < unit.Controllers.TokenController.Stacks.Count)
			{
				if (unit.Controllers.TokenController.Stacks[i].States[0] > 0)
				{
					unit.ModifyTokenStackState(unit.Controllers.TokenController.Stacks[i], 0, 0);
				}
				else
				{
					unit.ModifyTokenStackState(unit.Controllers.TokenController.Stacks[i], 0, 1);
				}
			}
		}
	}

	public void OnGUI()
	{
		var unit = Player.GetComponent<Fury.Behaviors.Unit>();

		if (unit.Controllers != null)
		{
			GUILayout.BeginArea(new Rect(10F, 10F, 200F, Screen.height - 20F));

			foreach (var item in unit.Controllers.TokenController.Stacks)
			{
				GUILayout.Label(string.Format("{0}: {1}", item.Token.name, item.States[0]));
			}

			GUILayout.EndArea();
		}
	}

}