using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class InventoryManager : MonoBehaviour
{
	public Texture2D FrameTexture = null;
	public Font OverlayFont = null;

	public void Update()
	{
		var iconSize = 48;
		var spacing = 7;
		var x = Screen.width - 30 - iconSize;
		var hud = SimpleHUD.Instance;
		var gray = new Color(0.5f, 0.5f, 0.5f, 0.5f);
		var yellow = new Color(0.5f, 0.5f, 0f, 0.5f);
		var mPos = Fury.Hud.Position;
		var invertedMousePos = new Vector2(mPos.x, Screen.height - mPos.y);

		if (hud == null || hud.Selection == null) return;

		foreach (var unit in hud.Selection)
			if (unit.Owner.IsLocal)
			{
				foreach (var stack in unit.Controllers.TokenController.Stacks)
				{
					var r = new Rect(x, Screen.height - 30 - iconSize, iconSize, iconSize);
					hud.DrawTexture(stack.Token.Icon, r, gray);

					if (r.Contains(invertedMousePos))
					{
						hud.DrawTexture(FrameTexture, new Rect(r.x - 2, r.y - 2, r.width + 4, r.height + 4), yellow);

						if (Fury.Hud.TriggerLMB)
						{
							UseStack(unit.Identifier, stack.Identifier);
							Fury.Hud.ConsumeLMB();
						}
					}
					else
					{
						hud.DrawTexture(FrameTexture, new Rect(r.x - 2, r.y - 2, r.width + 4, r.height + 4), gray);
					}

					hud.DrawText(stack.States[0].ToString(), OverlayFont, r.x + r.width * 1f - 2, r.y + r.height * 1f, TextAnchor.LowerRight);

					x -= spacing + iconSize;
				}

				break;
			}
	}

	/// <summary>
	/// Use a stack. The effect differs by the type of token in the stack. For example, equipment
	/// is equipped or unequipped while consumables are consumed.
	/// </summary>
	/// <param name="unitId">A unit.</param>
	/// <param name="stackId">A stack the unit has.</param>
	[RPC]
	private void UseStack(Int32 unitId, Int32 stackId)
	{
		// If we're the server, we can employ custom logic to determine what actually 
		// happens when an item stack is used
		if (Network.isServer)
		{
			// Find and validate the unit
			var unit = Fury.Behaviors.Manager.Instance.Find<Fury.Behaviors.Unit>(unitId);
			if (unit == null) return;

			// Find and validate the stack
			var stack = unit.Controllers.TokenController.Stacks.First(s => s.Identifier == stackId);
			if (stack == null) return;

			// Equipment gets equipped (1 = equipped, 0 = unequipped)
			if (stack.Token is Equipment)
			{
				if (stack.States[0] > 0) unit.ModifyTokenStackState(stack, 0, 0);
				else unit.ModifyTokenStackState(stack, 0, 1);
			}

			// Consumables get consumed
			if (stack.Token is Consumable)
			{
				unit.ModifyTokenStackState(stack, 0, (Byte)Math.Max(0, (stack.States[0] - 1)));
			}
		}

		// Clients cannot directly call ModifyTokenStackState functions, we have to pass
		// our request on to the server that runs custom logic to handle item states
		if (Network.isClient)
		{
			networkView.RPC("UseStack", RPCMode.Server, unitId, stackId);
		}
	}
}