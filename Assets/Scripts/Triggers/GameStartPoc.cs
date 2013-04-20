using System;
using System.Collections.Generic;

using UnityEngine;
using Fury.Behaviors;

/// <summary>
/// This is an automatically generated trigger template for the 
/// trigger of type OnMapLoaded.
/// </summary>
public class GameStartPoc : MonoBehaviour
{
	/// <summary>
	/// For triggers with multiple copies, this property will return all scene instances of the trigger.
	/// </summary>
	public static GameStartPoc[] All
	{
		get
		{
			return (GameStartPoc[])GameObject.FindSceneObjectsOfType(typeof(GameStartPoc));
		}
	}

	/// <summary>
	/// Most triggers will only have one instance active, and this static property
	/// can be used to retrieve a reference to it.
	/// </summary>
	public static GameStartPoc Last { get; private set; }

	/// <summary>
	/// If true, the trigger will process events.
	/// </summary>
	public Boolean IsTriggerEnabled { get; set; }

	public GameObject Player;

	//public ItemPoc[] StartingItems;

	/// <summary>
	/// Called by the Unity engine when the behaviour starts up.
	/// </summary>
	private void Start()
	{
		Last = this;
		IsTriggerEnabled = true;
		Manager.Instance.OnMapLoaded += OnMapLoaded;
	}

	/// <summary>
	/// Called by the Unity engine when the behaviour is destroyed.
	/// </summary>
	private void OnDestroy()
	{
		Manager.Instance.OnMapLoaded -= OnMapLoaded;
	}

	/// <summary>
	/// Called by the Fury Framework every time the event occurs.
	/// </summary>
	private void OnMapLoaded()
	{
		// Only process events if the trigger is enabled
		if (!IsTriggerEnabled) return;

		// TODO: Most triggers will need to verify some condition of the event
		if (true)
		{
			//var unit = this.Player.GetComponent<Fury.Behaviors.Unit>();

			//foreach (var item in StartingItems)
			//{
			//    //if (item is ItemPoc)
			//    //{
			//        unit.CreateTokenStack(item, new byte[] { 0 });
			//    //}
			//}

			// Most triggers will just destroy themselves after executing
			GameObject.Destroy(this);
		}
	}
}