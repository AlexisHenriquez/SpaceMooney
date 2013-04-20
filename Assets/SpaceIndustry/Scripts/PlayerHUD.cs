using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;

public class PlayerHUD : MonoBehaviour
{

	public PlayerStats PlayerStats;

	internal bool ShowBuyMenu;

	internal bool ShowInventory;

	internal bool ShippingInProgress;

	internal const float ShippingArrivalTime = 5F;

	internal float ArrivalTime;

	public void OnGUI()
	{
		GUI.Button(new Rect(10, 10, 80, 30), "(C)omprar");

		GUI.Button(new Rect(95, 10, 80, 30), "(I)nventario");

		this.HandleBuy();

		this.HandleInventory();

		this.HandleShipping();
	}

	public void HandleInventory()
	{
		if (ShowInventory)
		{
			GUI.Button(new Rect(95, 50, 120, 100), string.Empty);

			GUI.Button(new Rect(100, 60, 110, 30), string.Format("Panel Solar: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.SolarPanel).ToString()));
		}
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.C) &&
			!ShippingInProgress)
		{
			ShowBuyMenu = !ShowBuyMenu;

			if (!ShowBuyMenu)
			{
				this.PlayerStats.ResetBuyables();
			}
		}

		if (ShowBuyMenu)
		{
			this.HandleBuySolarPanel();
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			ShowInventory = !ShowInventory;
		}

		this.HandleBuyConfirm();
	}

	public void HandleBuyConfirm()
	{
		if (Input.GetKeyDown(KeyCode.Return))
		{
			ShowBuyMenu = false;

			ShippingInProgress = true;

			ArrivalTime = ShippingArrivalTime;
		}
	}

	public void HandleBuy()
	{
		if (ShowBuyMenu &&
			!ShippingInProgress)
		{
			GUI.Button(new Rect(10, 50, 120, 100), string.Empty);

			GUI.Button(new Rect(15, 60, 110, 30), string.Format("(P)anel Solar: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.SolarPanel).ToString()));
		}
	}

	public void HandleShipping()
	{
		if (ShippingInProgress)
		{
			GUI.Button(new Rect(10, 60, 160, 30), string.Format("Tiempo de llegada {0}", ArrivalTime.ToString("F0")));

			ArrivalTime -= Time.deltaTime;

			if (ArrivalTime <= 0)
			{
				ShippingInProgress = false;

				this.HandleShippingArrival();
			}
		}
	}

	public void HandleShippingArrival()
	{
		foreach (var item in PlayerStats.BuyingInventory)
		{
			this.PlayerStats.UpdateAmountInInventory(item.Key, this.PlayerStats.GetBuyingAmountOf(item.Key));
		}

		this.PlayerStats.ResetBuyables();
	}

	public void HandleBuySolarPanel()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			this.PlayerStats.Buy(Buyables.SolarPanel, 1);
		}
	}

}
