﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Store a static representation of all controllers connected to the game.
public static class Connections
{
	public static List<ConnectedInput> connectedInputs;

	// Initialise the connection list.
	static Connections()
	{
		connectedInputs = new List<ConnectedInput>();
	}

	public static void AddController()
	{

	}

	// Add a controller connection.
	public static void AddInput(ConnectedInput input)
	{
		connectedInputs.Add(input);
	}

	// Remove a controller connection.
	public static void RemoveInput(ConnectedInput input)
	{
		connectedInputs.Remove(input);
	}

	// When the game returns to the connection scene, remove existing connections.
	public static void RemoveAllInputs()
	{
		connectedInputs = new List<ConnectedInput>();
	}
}