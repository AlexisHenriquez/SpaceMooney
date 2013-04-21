using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

class Water : MonoBehaviour
{
	public Vector2 Velocity = Vector2.zero;

	private Material WaterMaterial;

	private Vector2 Offset;

	void Update()
	{
		if (WaterMaterial == null)
			WaterMaterial = renderer.material;

		Offset += Velocity * Time.deltaTime;
		WaterMaterial.mainTextureOffset = new Vector2(Offset.x, -Offset.y) * 0.5f;
		WaterMaterial.SetTextureOffset("_BumpMap", Offset);
	}
}