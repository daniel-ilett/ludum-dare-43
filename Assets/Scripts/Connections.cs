using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Store a static representation of all controllers connected to the game.
public static class Connections
{
	public static List<ConnectedInput> connectedInputs;

	public static event EventHandler<EventArgs> ReturnToMainMenuEvent;

	// Initialise the connection list.
	static Connections()
	{
		connectedInputs = new List<ConnectedInput>();
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
		EventHandler<EventArgs> handler = ReturnToMainMenuEvent;

		if(handler != null)
		{
			handler(null, new EventArgs());
		}

		connectedInputs = new List<ConnectedInput>();
	}

	// Return whether the requested joystick ID is in the connected list.
	public static bool IsControllerConnected(int joystickID)
	{
		foreach (var connectedInput in connectedInputs)
		{
			if (connectedInput.GetJoystickID() == joystickID)
			{
				return true;
			}
		}

		return false;
	}

	// Return the input with the given joystickID, if it exists.
	public static ConnectedInput GetInputWithJoystickID(int joystickID)
	{
		foreach(var connectedInput in connectedInputs)
		{
			if(connectedInput.GetJoystickID() == joystickID)
			{
				return connectedInput;
			}
		}

		return null;
	}

	// Return the input with the given playerID, if it exists.
	public static ConnectedInput GetInputWithPlayerID(int playerID)
	{
		foreach (var connectedInput in connectedInputs)
		{
			if (connectedInput.GetPlayerID() == playerID)
			{
				return connectedInput;
			}
		}

		return null;
	}

	// Return the ID of the earliest unassigned player.
	public static int GetNextPlayerID()
	{
		for(int i = 1; i <= ConnectionController.maxPlayerCount; ++i)
		{
			if(GetInputWithPlayerID(i) == null)
			{
				return i;
			}
		}

		return -1;
	}
}
