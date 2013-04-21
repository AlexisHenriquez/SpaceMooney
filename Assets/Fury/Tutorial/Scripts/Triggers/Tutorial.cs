using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Tutorial : MonoBehaviour
{
	private enum TutorialStages
	{
		None,
		SelectingUnits,
		NavigatingMap,
		AttackDummies,
		FollowPath,
		DestroyTower,
		LootItems,
		Completed
	}

	public GUIStyle MessageStyle, BackgroundStyle;

	public Collider TowerWarning;

	private TutorialStages Stage;

	private Vector3 OldCameraPosition;

	private Boolean IsInterfaceHooked;

	private void Start()
	{
		Fury.Behaviors.Manager.Instance.OnMapLoaded += new Fury.General.Action(OnMapLoaded);
		Fury.Behaviors.Manager.Instance.OnUnitDead += new Fury.General.DUnitDead(OnUnitDead);
		Fury.Behaviors.Manager.Instance.OnTokenStackCreated += new Fury.General.Action<Fury.Behaviors.Stack>(OnTokenStackCreated);
		Fury.Behaviors.Manager.Instance.OnTokenStackStateChanged += new Fury.General.DTokenStackStateChanged(OnTokenStackStateChanged);
		Fury.Behaviors.Manager.Instance.OnUnitEnterTrigger += new Fury.General.DUnitEnterTrigger(OnUnitEnterTrigger);
	}

	private void Update()
	{
		if (SimpleHUD.Instance != null && SimpleHUD.Instance.Selection != null && !IsInterfaceHooked)
		{
			SimpleHUD.Instance.Selection.OnUnitSelected += OnUnitSelected;
			IsInterfaceHooked = true;
		}

		if (Stage == TutorialStages.NavigatingMap &&
			Vector3.Distance(OldCameraPosition, Camera.main.transform.position) > 0.01f)
		{

			Stage++;
		}

		OldCameraPosition = Camera.main.transform.position;
	}

	private void OnGUI()
	{
		var msg = "";
		switch (Stage)
		{
			case TutorialStages.SelectingUnits:
				msg = "To select an individual unit, click it.\nTo select multiple units, drag a box around them.";
				break;

			case TutorialStages.NavigatingMap:
				msg = "You can pan the camera using the [W][A][S][D] keys.\nYou can also pan by moving the mouse to the edge of the screen.";
				break;

			case TutorialStages.AttackDummies:
				msg = "With your unit(s) selected, right click the target dummy to attack it.\nYou may also cast your mage's [Lightning Strike] on the dummy.";
				break;

			case TutorialStages.FollowPath:
				msg = "Follow the path to the left to explore this island.";
				break;

			case TutorialStages.DestroyTower:
				msg = "A tower lies ahead; lead with your warrior to draw the tower's attention.";
				break;

			case TutorialStages.LootItems:
				msg = "The tower had a crate of items inside, use your [Loot] skill on the crate.";
				break;
		}

		if (msg.Length > 0)
		{
			GUILayout.BeginArea(new Rect(0, 0, Screen.width, 80), BackgroundStyle);
			GUILayout.FlexibleSpace();
			GUILayout.Label(msg, MessageStyle);
			GUILayout.FlexibleSpace();
			GUILayout.EndArea();
		}
	}
	
	private void OnMapLoaded()
	{
		Stage = TutorialStages.SelectingUnits;
	}

	private void OnUnitEnterTrigger(Fury.Behaviors.Unit unit, Collider other)
	{
		if (other == TowerWarning && Stage < TutorialStages.DestroyTower)
			Stage = TutorialStages.DestroyTower;
	}

	private void OnTokenStackCreated(Fury.Behaviors.Stack stack)
	{
		if (Stage == TutorialStages.LootItems && stack.Owner is Fury.Behaviors.Unit)
			Stage++;
	}

	private void OnTokenStackStateChanged(Fury.Behaviors.Stack stack, byte index, byte oldVal, byte newVal)
	{
		if (Stage == TutorialStages.LootItems && stack.Owner is Fury.Behaviors.Unit)
			Stage++;
	}

	private void OnUnitDead(Fury.Behaviors.Unit deadUnit, Fury.Behaviors.Unit lastAttacker)
	{
		if (deadUnit.Properties is DummyLogic && Stage == TutorialStages.AttackDummies)
			Stage++;

		if (deadUnit.Properties is Tower && Stage == TutorialStages.DestroyTower)
			Stage++;
	}

	private void OnUnitSelected(Fury.Behaviors.Unit obj)
	{
		if (Stage == TutorialStages.SelectingUnits)
			Stage++;
	}
}