using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;

public class PlayerStats : MonoBehaviour
{

	public IDictionary<Buyables, int> BuyingInventory;

	internal Array BuyablesArray = Enum.GetValues(typeof(Buyables));

	public IDictionary<Buyables, int> Inventory;

	public IList<SolarPanel> SolarPanels;

	public Player Player;

	void Start()
	{
		this.BuyingInventory = new Dictionary<Buyables, int>(BuyablesArray.Length);

		this.Inventory = new Dictionary<Buyables, int>(BuyablesArray.Length);

		this.SolarPanels = new List<SolarPanel>();

		this.Player = new Player() { Budget = 100 };

		foreach (var item in BuyablesArray)
		{
			this.BuyingInventory.Add((Buyables)item, 0);

			this.Inventory.Add((Buyables)item, 0);
		}
	}
	
	public bool Buy(Buyables objectToBuy, int cantidad)
	{
		bool canBuy = this.Player.HasBudgetToBuy(objectToBuy, cantidad);

		if (canBuy)
		{
			this.BuyingInventory[objectToBuy] += cantidad;

			this.Player.DiscountTotalPrice(objectToBuy, cantidad);
		}

		//if (GetBuyingAmountOf(objectToBuy) < 0)
		//    this.BuyingInventory[objectToBuy] = 0;

		return canBuy;
	}

	public int GetBuyingAmountOf(Buyables objectToBuy)
	{
		return this.BuyingInventory[objectToBuy];
	}

	public int GetInventoryAmountOf(Buyables objectToBuy)
	{
		return this.Inventory[objectToBuy];
	}

	public void ResetBuyables()
	{
		for (int i = 0; i < BuyablesArray.Length; i++)
			this.BuyingInventory[(Buyables)BuyablesArray.GetValue(i)] = 0;
	}

	public bool HasInInventory(Buyables objectToVerify)
	{
		return this.Inventory[objectToVerify] > 0;
	}

	public void UpdateAmountInInventory(Buyables objectToUpdate, int amount)
	{
		this.Inventory[objectToUpdate] += amount;

		if (this.Inventory[objectToUpdate] < 0)
		{
			this.Inventory[objectToUpdate] = 0;
		}
	}

	public void AddSolarPanel()
	{
		this.SolarPanels.Add(new SolarPanel());
	}

}