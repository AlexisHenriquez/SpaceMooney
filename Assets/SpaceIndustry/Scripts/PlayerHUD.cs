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

		GUI.Button(new Rect(195, 10, 80, 30), "(I)nventario");

		GUI.Button(new Rect(1050, 10, 120, 30), "Estadísticas");

		this.HandleBuy();

		this.HandleInventory();

		this.HandleShipping();

		this.HandleResources();

		if (PlayerStats.TotalEnergy <= 0 ||
			this.PlayerStats.Player.Budget <= 0)
		{
			GUIStyle style = new GUIStyle();
			style.fontSize = 100;

			GUILayout.BeginArea(new Rect(160, Screen.height / 2, 1100, Screen.height - 20));
			GUILayout.Label("Impossible to progress!",style);
			GUILayout.EndArea();
		}
	}

	public void HandleInventory()
	{
		if (ShowInventory)
		{
			GUI.Button(new Rect(195, 50, 180, 260), string.Empty);

			GUI.Button(new Rect(200, 60, 110, 30), string.Format("Panel Solar: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.SolarPanel).ToString()));

			GUI.Button(new Rect(200, 95, 110, 30), string.Format("Antena: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.MicrowaveAntenna).ToString()));

			GUI.Button(new Rect(200, 130, 150, 30), string.Format("Extrac. o2: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.OxygenExtractor).ToString()));
			GUI.Button(new Rect(200, 165, 150, 30), string.Format("Refinería de o2: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.OxygenRefinery).ToString()));

			GUI.Button(new Rect(200, 200, 150, 30), string.Format("Extrac. Mineral: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.MineralExtractor).ToString()));
			GUI.Button(new Rect(200, 235, 150, 30), string.Format("Fáb. Paneles Sol.: {0}", this.PlayerStats.GetInventoryAmountOf(Buyables.SolarPanelFactory).ToString()));
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
			//this.HandleBuySolarPanel();

			//this.HandleBuyMicrowaveAntenna();

			//this.HandleBuyOxygenExtractor();
			this.HandleBuyGUI();
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

			this.PlayerStats.ConsumeMinerals();

			this.PlayerStats.ManageIncomes();

			HandlingResourceTime = HandlingResourceInterval;
		}

		GUI.Button(new Rect(1050, 50, 150, 230), string.Empty);

		GUI.Button(new Rect(1055, 60, 140, 30), string.Format("Presupuesto: ${0}", this.PlayerStats.Player.Budget));

		GUI.Button(new Rect(1055, 95, 140, 30), string.Format("Energía: {0}", this.PlayerStats.TotalEnergy));

		GUI.Button(new Rect(1055, 130, 140, 30), string.Format("Oxígeno: {0}", this.PlayerStats.TotalOxygen));
		GUI.Button(new Rect(1055, 165, 140, 30), string.Format("Aluminio: {0}", this.PlayerStats.TotalAlluminum));
		GUI.Button(new Rect(1055, 200, 140, 30), string.Format("Silicio: {0}", this.PlayerStats.TotalSilicon));
		GUI.Button(new Rect(1055, 235, 140, 30), string.Format("Hierro: {0}", this.PlayerStats.TotalIron));
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

	public void HandleBuy()
	{
		if (ShowBuyMenu &&
			!ShippingInProgress)
		{
			GUI.Button(new Rect(10, 50, 180, 230), string.Empty);

			GUI.Button(new Rect(15, 60, 140, 30), string.Format("(P)anel Solar: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.SolarPanel).ToString()));

			GUI.Button(new Rect(15, 95, 140, 30), string.Format("A(n)tena: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.MicrowaveAntenna).ToString()));

			GUI.Button(new Rect(15, 130, 140, 30), string.Format("(E)xtrac. O2: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.OxygenExtractor).ToString()));
			GUI.Button(new Rect(15, 165, 140, 30), string.Format("(R)efin. O2: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.OxygenRefinery).ToString()));

			GUI.Button(new Rect(15, 200, 140, 30), string.Format("Extrac. (M)ineral: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.MineralExtractor).ToString()));
			GUI.Button(new Rect(15, 235, 140, 30), string.Format("(F)áb. Panel Sol: {0}", this.PlayerStats.GetBuyingAmountOf(Buyables.SolarPanelFactory).ToString()));
		}
	}

	private void HandleBuyGUI()
	{
		if (Input.GetKeyDown(KeyCode.P))
		{
			this.PlayerStats.Buy(Buyables.SolarPanel, 1);
		}
		if (Input.GetKeyDown(KeyCode.E))
		{
			this.PlayerStats.Buy(Buyables.OxygenExtractor, 1);
		}
		if (Input.GetKeyDown(KeyCode.N))
		{
			this.PlayerStats.Buy(Buyables.MicrowaveAntenna, 1);
		}
		if (Input.GetKeyDown(KeyCode.R))
		{
			this.PlayerStats.Buy(Buyables.OxygenRefinery, 1);
		}
		if (Input.GetKeyDown(KeyCode.M))
		{
			this.PlayerStats.Buy(Buyables.MineralExtractor, 1);
		}
		if (Input.GetKeyDown(KeyCode.F))
		{
			this.PlayerStats.Buy(Buyables.SolarPanelFactory, 1);
		}
	}
}
