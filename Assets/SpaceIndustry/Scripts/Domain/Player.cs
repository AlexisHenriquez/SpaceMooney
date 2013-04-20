
public class Player
{

	public int Budget { get; set; }

	private static int GetTotalPrice(int cantidad)
	{
		return cantidad * SolarPanel.PRICE_SOLAR_PANEL;
	}

	public bool HasBudgetToBuy(Buyables objectToBuy, int cantidad)
	{
		if (objectToBuy == Buyables.SolarPanel)
		{
			int totalPrice = GetTotalPrice(cantidad);

			return totalPrice <= this.Budget;
		}

		return false;
	}

	public void DiscountTotalPrice(Buyables objectToBuy, int cantidad)
	{
		if (objectToBuy == Buyables.SolarPanel)
		{
			int totalPrice = GetTotalPrice(cantidad);

			this.Budget -= totalPrice;
		}
	}

}