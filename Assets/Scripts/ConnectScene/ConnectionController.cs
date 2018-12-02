using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class ConnectionController : MonoBehaviour
{
	[SerializeField]
	private Text headerText;

	[SerializeField]
	private ConnectedInput inputPrefab;
	
	[SerializeField]
	private InputRemap remapperPrefab;

	[SerializeField]
	private Transform remapperRoot;

	private List<InputRemap> remappers;

	private int playersConnected = 0;

	private const int maxPlayerCount = 8;

	private List<int> connectedControllerIDs;
	private bool keyboardConnected;

	private Dictionary<int, bool> accepted;

	private bool isLoadingGame = false;

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
		remappers = new List<InputRemap>();

		for(int i = 0; i < maxPlayerCount; ++i)
		{
			var newRemapper = Instantiate(remapperPrefab, remapperRoot);
			newRemapper.SetPlayerNumber(i + 1);

			remappers.Add(newRemapper);
		}
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

		if(!isLoadingGame)
		{
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

				if (Input.GetButtonDown("J3_Jump"))
				{
					AddController(3);
				}

				if (Input.GetButtonDown("J4_Jump"))
				{
					AddController(4);
				}

				if (Input.GetButtonDown("J5_Jump"))
				{
					AddController(5);
				}

				if (Input.GetButtonDown("J6_Jump"))
				{
					AddController(6);
				}

				if (Input.GetButtonDown("J7_Jump"))
				{
					AddController(7);
				}

				if (Input.GetButtonDown("J8_Jump"))
				{
					AddController(8);
				}
			}

			if (accepted.Count > 0)
			{
				foreach (var value in accepted.Values)
				{
					if (value == false)
					{
						return;
					}
				}

				StartCoroutine(LoadGame());
			}
		}
	}

	// Load the game with a nice countdown.
	private IEnumerator LoadGame()
	{
		isLoadingGame = true;

		int count = 3;
		var wait = new WaitForSeconds(1.0f);

		while(count > 0)
		{
			headerText.text = "Everyone is ready - starting in " + count.ToString() + "...";
			--count;
			yield return wait;
		}

		SceneManager.LoadScene("sc_GameScene");
	}

	// Add the keyboard if not already added.
	private void AddKeyboard()
	{
		if(!keyboardConnected)
		{
			var input = Instantiate(inputPrefab, Vector3.zero, Quaternion.identity);
			input.SetConnType(ConnectionType.KEYBOARD, 0, remappers[playersConnected], ++playersConnected);
			remappers[playersConnected - 1].SetPlayerConnected();

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
			remappers[playersConnected - 1].SetPlayerConnected();

			accepted.Add(playersConnected, false);
			connectedInputs.Add(input);

			connectedControllerIDs.Add(joystickID);
		}
	}
}
