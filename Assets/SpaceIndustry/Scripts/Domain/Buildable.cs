
public class Buildable
{

	public Buyable Buyable;

	public Buildable()
	{
		Buyable = new Buyable();
	}

	public Parts Parts { get; set; }

	public virtual int GetConsumeEnergyFactor()
	{
		return 0;
	}

}