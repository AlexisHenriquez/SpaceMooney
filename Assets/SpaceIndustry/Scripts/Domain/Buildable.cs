
using UnityEngine;

public abstract class Buildable
{

	public Buyable Buyable;
	public int Id { get; set; }

	public Buildable()
	{
		Buyable = new Buyable();
	}

    public GameObject gameObject;

	public Parts Parts { get; set; }

	public int LifeTime { get; set; }

	public bool IsDead { get { return this.LifeTime <= 0; } }

	public virtual int GetConsumeEnergyFactor()
	{
		return 0;
	}

	public abstract int GetDegradationFactor();

	public virtual void Degradate()
	{
		this.LifeTime -= this.GetDegradationFactor();

		if (this.LifeTime < 0)
		{
			this.LifeTime = 0;
		}
	}

	public virtual void ShowLifeTimeGUI()
	{
		//var pos = this.GameObject.transform.position;
		Vector3 screenPos = UnityEngine.GameObject.Find("Main Camera").camera.WorldToScreenPoint(this.gameObject.transform.position);
		GUI.Label(new Rect(screenPos.x, screenPos.y, 100, 30), string.Format("HP: {0}", this.LifeTime));
		//var text = (this.GameObject.GetComponent(typeof(GUIText)) as GUIText);
		//text.text = string.Format("HP: {0}", this.LifeTime);
		//Debug.Log("pico " + (this.GameObject.GetComponent(typeof(GUIText)) as GUIText).text);
	}

	

}