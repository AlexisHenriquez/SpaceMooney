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

	internal const float HandlingResourceInterval = 10F;

	internal float HandlingResourceTime;

	internal float ArrivalTime;

	void Start()
	{
		HandlingResourceTime = HandlingResourceInterval;
	}

	public void OnGUI()
	{
		GUI.Button(new Rect(10, 10, 80, 30), "(C)omprar");

		GUI.Button(new Rect(155, 10, 80, 30), "(I)nventario");

		GUI.Button(new Rect(350, 10, 120, 30), "Estadísticas");

		this.HandleBuy();

		this.HandleInventory();

		this.HandleShipping();

		this.HandleResources();
	}

	public void HandleInventory()
	{
		if (ShowInventory)
		{
			GUI.Button(new Rect(155, 50, 180, 170), string.Empty);

			GUI.Button(new Rect(160, 60, 110, 30), string.Format("Panel Solar: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.SolarPanel).ToString()));

			GUI.Button(new Rect(180, 95, 110, 30), string.Format("Antena: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.MicrowaveAntenna).ToString()));

			GUI.Button(new Rect(160, 130, 150, 30), string.Format("Extrac. o2: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.OxygenExtractor).ToString()));
			GUI.Button(new Rect(180, 165, 150, 30), string.Format("Refinería de o2: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.OxygenRefinery).ToString()));
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
				this.PlayerStats.Player.Restore(this.PlayerStats.BuyingInventory);

				this.PlayerStats.ResetBuyables();
			}
		}

		if (ShowBuyMenu)
		{
			this.HandleBuySolarPanel();

			this.HandleBuyMicrowaveAntenna();

            this.HandleBuyOxygenExtractor();
		}

		if (Input.GetKeyDown(KeyCode.I))
		{
			ShowInventory = !ShowInventory;
		}

		this.HandleBuyConfirm();
	}

	public void HandleResources()
	{
		HandlingResourceTime -= Time.deltaTime;

		if (HandlingResourceTime <= 0)
		{
			this.PlayerStats.DegradateObjects();

			this.PlayerStats.ConsumeEnergy();

			this.PlayerStats.ExtractResources();

			this.PlayerStats.ManageIncomes();

			HandlingResourceTime = HandlingResourceInterval;
		}

		GUI.Button(new Rect(350, 50, 150, 120), string.Empty);

		GUI.Button(new Rect(355, 60, 140, 30), string.Format("Presupuesto: {0}", this.PlayerStats.Player.Budget));

		GUI.Button(new Rect(355, 95, 140, 30), string.Format("Energía: {0}", this.PlayerStats.TotalEnergy));

        GUI.Button(new Rect(355, 130, 140, 30), string.Format("Oxigeno: {0}", this.PlayerStats.TotalOxygen));
	}

	public void HandleBuyConfirm()
	{
		if (Input.GetKeyDown(KeyCode.Return) && !ShippingInProgress)
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
			GUI.Button(new Rect(10, 50, 140, 170), string.Empty);

			GUI.Button(new Rect(15, 60, 110, 30), string.Format("(P)anel Solar: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.SolarPanel).ToString()));

			GUI.Button(new Rect(35, 95, 110, 30), string.Format("A(n)tena: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.MicrowaveAntenna).ToString()));

			GUI.Button(new Rect(15, 130, 110, 30), string.Format("(E)xtrac. O2: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.OxygenExtractor).ToString()));
			GUI.Button(new Rect(35, 165, 110, 30), string.Format("(R)efin. O2: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.OxygenRefinery).ToString()));
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

	public void HandleBuyMicrowaveAntenna()
	{
		if (Input.GetKeyDown(KeyCode.N))
		{
			this.PlayerStats.Buy(Buyables.MicrowaveAntenna, 1);
		}
	}

    public void HandleBuyOxygenExtractor()
    {
        if (Input.GetKeyDown(KeyCode.E))
        {
            this.PlayerStats.Buy(Buyables.OxygenExtractor, 1);
        }
    }

}
