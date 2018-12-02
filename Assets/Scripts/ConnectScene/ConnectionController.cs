using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ConnectionController : MonoBehaviour
{
	[SerializeField]
	private ConnectedInput inputPrefab;

	[SerializeField]
	private List<InputRemap> remappers;

	private int playersConnected = 0;

	private const int maxPlayerCount = 2;

	private List<int> connectedControllerIDs;
	private bool keyboardConnected;

	private Dictionary<int, bool> accepted;

	public static List<ConnectedInput> connectedInputs;
	public static ConnectionController instance;

	private void Awake()
	{
		// Set self as singleton ConnectionController and reset connection list.
		if(instance != null)
		{
			Destroy(instance.gameObject);
		}

		instance = this;
		connectedInputs = new List<ConnectedInput>();
		connectedControllerIDs = new List<int>();
		accepted = new Dictionary<int, bool>();
	}

	private void Update()
	{
		/*
		// Loop through all joysticks.
		for(int js = 1; js <= 16; ++js)
		{
			// Loop through all buttons.
			for(int b = 0; b < 20; ++b)
			{
				if(Input.GetKeyDown("joystick " + js + " button " + b))
				{
					Debug.Log("joystick " + js + " button " + b);
				}
			}

			for(int a = 0; a < 20; ++a)
			{
				if (Mathf.Abs(Input.GetAxis("joystick " + js + " axis " + a)) > 0.5f)
				{
					Debug.Log("joystick " + js + " axis " + a);
				}
			}
		}
		*/

		// If a connected player presses Jump, they are ready to play.
		foreach (var connectedInput in connectedInputs)
		{
			if (accepted[connectedInput.GetPlayerID()] == false && 
				connectedInput.PressedJump())
			{
				accepted[connectedInput.GetPlayerID()] = true;
				connectedInput.remapper.SetPlayerReady();
			}
		}

		// Attempt to connect new controllers.
		if (playersConnected < maxPlayerCount)
		{
			if (Input.GetButtonDown("K_Jump"))
			{
				AddKeyboard();
			}

			if (Input.GetButtonDown("J1_Jump"))
			{
				AddController(1);
			}

			if (Input.GetButtonDown("J2_Jump"))
			{
				AddController(2);
			}
		}

		if(accepted.Count > 0)
		{
			foreach(var value in accepted.Values)
			{
				if(value == false)
				{
					return;
				}
			}

			SceneManager.LoadScene("sc_GameScene");
		}
	}

	// Add the keyboard if not already added.
	private void AddKeyboard()
	{
		if(!keyboardConnected)
		{
			var input = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
			input.SetConnType(ConnectionType.KEYBOARD, 0, remappers[playersConnected], ++playersConnected);

			accepted.Add(playersConnected, false);
			connectedInputs.Add(input);

			keyboardConnected = true;
		}
	}

	// Add a controller if not already added.
	private void AddController(int joystickID)
	{
		if (!connectedControllerIDs.Contains(joystickID))
		{
			var input = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
			input.SetConnType(ConnectionType.CONTROLLER, joystickID, remappers[playersConnected], ++playersConnected);

			accepted.Add(playersConnected, false);
			connectedInputs.Add(input);

			connectedControllerIDs.Add(joystickID);
		}
	}
}
