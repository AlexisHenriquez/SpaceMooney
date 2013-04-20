using UnityEngine;
using System.Collections;

public class PlayerStorage : MonoBehaviour
{

	public int CantidadPanelesSolares;

	public bool HasPanelesSolares()
	{
		return this.CantidadPanelesSolares > 0;
	}
	
	public void DisminuirUnidadPanelSolar()
	{
		this.CantidadPanelesSolares--;
	}

	public void AumentarUnidadPanelSolar()
	{
		this.CantidadPanelesSolares++;
	}

}