using System.Collections.Generic;

public class Player
{

	public int Budget { get; set; }

	private static int GetTotalPriceSolarPanel(int cantidad)
	{
		return cantidad * SolarPanel.PRICE_SOLAR_PANEL;
	}

	private static int GetTotalPriceOxygenExtractor(int cantidad)
	{
		return cantidad * OxygenExtractor.PRICE_OXYGEN_EXTRACTOR;
	}

	private static int GetTotalPriceMicrowaveAntenna(int cantidad)
	{
		return cantidad * MicrowaveAntenna.BUY_PRICE;
	}

	public bool HasBudgetToBuy(Buyables objectToBuy, int cantidad)
	{
		int totalPrice = 0;

		if (objectToBuy == Buyables.SolarPanel)
		{
			totalPrice = GetTotalPriceSolarPanel(cantidad);
		}
        
		if (objectToBuy == Buyables.OxygenExtractor)
        {
            totalPrice = GetTotalPriceOxygenExtractor(cantidad);
		}

		if (objectToBuy == Buyables.MicrowaveAntenna)
		{
			totalPrice = GetTotalPriceMicrowaveAntenna(cantidad);
		}

		if (objectToBuy == Buyables.SolarPanelFactory)
		{
			totalPrice = GetTotalPriceSolarPanelFactorry(cantidad);
		}

		if (objectToBuy == Buyables.MineralExtractor)
		{
			totalPrice = GetTotalPriceMineralExtractor(cantidad);
		}

		return totalPrice <= this.Budget;
	}

	private int GetTotalPriceMineralExtractor(int cantidad)
	{
		return cantidad * MineralExtractor.PRICE_MINERAL_EXTRACTOR;
	}

	private int GetTotalPriceSolarPanelFactorry(int cantidad)
	{
		return cantidad * SolarPanelFactory.PRICE_SOLAR_PANEL;
	}

	public void DiscountTotalPrice(Buyables objectToBuy, int cantidad)
	{
		int totalPrice = 0;

		if (objectToBuy == Buyables.SolarPanel)
		{
			totalPrice = GetTotalPriceSolarPanel(cantidad);
		}
        
		if (objectToBuy == Buyables.OxygenExtractor)
        {
			totalPrice = GetTotalPriceOxygenExtractor(cantidad);
		}

		if (objectToBuy == Buyables.MicrowaveAntenna)
		{
			totalPrice = GetTotalPriceMicrowaveAntenna(cantidad);
		}

		if (objectToBuy == Buyables.SolarPanelFactory)
		{
			totalPrice = GetTotalPriceSolarPanelFactorry(cantidad);
		}
		if (objectToBuy == Buyables.MineralExtractor)
		{
			totalPrice = GetTotalPriceMineralExtractor(cantidad);
		}

		this.Budget -= totalPrice;
	}

	public void RestoreTotalPrice(Buyables objectToBuy, int cantidad)
	{
		int totalPrice = 0;

		if (objectToBuy == Buyables.SolarPanel)
		{
			totalPrice = GetTotalPriceSolarPanel(cantidad);
		}

		if (objectToBuy == Buyables.OxygenExtractor)
		{
			totalPrice = GetTotalPriceOxygenExtractor(cantidad);
		}

		if (objectToBuy == Buyables.MicrowaveAntenna)
		{
			totalPrice = GetTotalPriceMicrowaveAntenna(cantidad);
		}

		this.Budget += totalPrice;
	}

	public void Restore(IDictionary<Buyables, int> buyInventory)
	{
		foreach (var item in buyInventory)
		{
			this.RestoreTotalPrice(item.Key, item.Value);
		}
	}


	public bool HasPrerequisites(Buyables objectToBuy, int cantidad)
	{
		//TODO: Implement!
		return true;
	}
}