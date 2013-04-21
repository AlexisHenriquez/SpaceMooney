using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

using UnityEngine;

public class Lobby : MonoBehaviour
{
	public Fury.Database.Map TutorialMap;

	public GUIStyle ButtonStyle, TextfieldStyle, WindowStyle;

	private String SelectedServer = "localhost";

	void OnGUI()
	{
		var width = GUILayout.Width(80f);

		if (Fury.Behaviors.Manager.Instance.GameState == Fury.GameStates.Menu ||
			Fury.Behaviors.Manager.Instance.GameState == Fury.GameStates.Lobby)
		{
			GUILayout.BeginArea(new Rect(Screen.width / 2 - 150, 100, 300, 200), WindowStyle);
			if (Fury.Behaviors.Manager.Instance.GameState == Fury.GameStates.Menu)
			{
				if (GUILayout.Button("Quickstart", ButtonStyle))
				{
					GameObject.Destroy(gameObject);
					Application.LoadLevel("Tutorial");
				}

				GUILayout.FlexibleSpace();

				GUILayout.Label("Multiplayer");

				GUILayout.BeginHorizontal();
				SelectedServer = GUILayout.TextField(SelectedServer, TextfieldStyle);
				if (GUILayout.Button("Join", ButtonStyle, width))
					Fury.Behaviors.Manager.Instance.Join(SelectedServer, 7000, null);
				GUILayout.EndHorizontal();

				GUILayout.BeginHorizontal();
				if (GUILayout.Button("Host Tutorial", ButtonStyle))
				{
					Fury.Behaviors.Manager.Instance.Host(7000, TutorialMap, "FuryFramework.Demo.Tutorial", "", false, null);
					Fury.Behaviors.Manager.Instance.CreateAICommander(Fury.CommanderIndices.Two, null);
				}
				GUILayout.EndHorizontal();
			}
			else if (Fury.Behaviors.Manager.Instance.GameState == Fury.GameStates.Lobby) 
			{
				GUILayout.Label("Commanders");

				foreach (var cmdr in Fury.Behaviors.Manager.Instance.Commanders)
				{
					String cmdrType = "AI";
					if (cmdr.Index == Fury.CommanderIndices.One)
						cmdrType = "Mage";
					else if (cmdr.Index == Fury.CommanderIndices.Three)
						cmdrType = "Warrior";
					GUILayout.Label(cmdr.Index + " (" + cmdrType + ")"); // (cmdr.IsAI ? "(AI)" : ""));
				}

				GUILayout.FlexibleSpace();

				/*
				if (GUILayout.Button("Switch Character", ButtonStyle))
				{
					var localCmdr = Fury.Behaviors.Manager.Instance.Commanders.FirstOrDefault(c => c.IsLocal);
					localCmdr.SetIndex(localCmdr.Index == Fury.CommanderIndices.One ? Fury.CommanderIndices.Three : Fury.CommanderIndices.One);
				}
				*/

				if (Network.isServer)
				{
					if (GUILayout.Button("Start", ButtonStyle))
					{
						Fury.Behaviors.Manager.Instance.StartGame();
					}
				}
				else
				{
					GUILayout.Label("Waiting for host to start...");
				}
			}
			GUILayout.EndArea();
		}
	}
}