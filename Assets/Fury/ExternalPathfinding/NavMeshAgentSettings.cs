using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

public class NavMeshAgentSettings : UnityEngine.MonoBehaviour
{
	void Start()
	{
		Fury.Behaviors.Manager.Instance.NavMeshAgentType = typeof(AStarNavAgent);
	}
}