using UnityEngine;
using System.Collections;

public class GUIPoc : MonoBehaviour {

	public GameObject Buildable;

	public void OnGUI()
	{
		GUI.Button(new Rect(10, 10, 50, 25), "(P)anel Solar");

		//if (GUI.Button(new Rect(10, 10, 100, 50), "Panel Solar"))
		//{
		//    Debug.Log("Button clicked!");
		//}
	}

	//void Update()
	//{
	//    GameObject clone;

	//    if (Input.GetKeyDown(KeyCode.P))
	//    {
	//        clone = Instantiate(this.Buildable, 
	//    }
	//}
	
}
