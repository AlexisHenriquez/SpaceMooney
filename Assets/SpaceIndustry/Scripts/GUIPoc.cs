using UnityEngine;
using System.Collections;
using System;

public class GUIPoc : MonoBehaviour {

	public GameObject Buildable;

	public PlayerStorage Storage;

	public void OnGUI()
	{
		int cantidadPaneles = 0;

		if (this.Storage != null)
		{
			cantidadPaneles = this.Storage.CantidadPanelesSolares;
		}

		GUI.Button(new Rect(10, 10, 200, 30), String.Format("(P)aneles Solares: {0}", cantidadPaneles.ToString()));
	}

	void Update()
	{
		this.ComprarPanelSolar();
	}

	public void ComprarPanelSolar()
	{
		if (this.Storage != null &&
			Input.GetKeyDown(KeyCode.P))
		{
			this.Storage.AumentarUnidadPanelSolar();
		}
	}
	
}
