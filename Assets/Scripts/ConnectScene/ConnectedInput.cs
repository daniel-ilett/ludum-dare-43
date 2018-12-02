using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedInput : MonoBehaviour
{
	private int playerID;
	private int joystickID;
	private ConnectionType connType;

	private Dictionary<InputName, string> inputMap;

	public InputRemap remapper { get; private set; }

	private bool playing = false;

	private void Awake()
	{
		DontDestroyOnLoad(gameObject);
	}

	// Set the kind of controller this input is.
	public void SetConnType(ConnectionType type, int joystickID, InputRemap remapper, int playerID)
	{
		Debug.Log("Connected - type is " + type.ToString() + ", joystick ID is " + joystickID + ", playerID is " + playerID);

		connType = type;
		this.playerID = playerID;
		this.remapper = remapper;
		this.joystickID = joystickID;

		if(type == ConnectionType.CONTROLLER)
		{
			inputMap = new Dictionary<InputName, string>
			{
				{ InputName.JUMP, "J" + joystickID + "_Jump" },
				{ InputName.THROW, "J" + joystickID + "_Throw" },
				{ InputName.SWING, "J" + joystickID + "_Swing" },
				{ InputName.MOVE_X, "J" + joystickID + "_MoveHorizontal" },
				{ InputName.MOVE_Y, "J" + joystickID + "_MoveVertical" },
			};
		}
		else
		{
			inputMap = new Dictionary<InputName, string>
			{
				{ InputName.JUMP, "K_Jump" },
				{ InputName.THROW, "K_Throw" },
				{ InputName.SWING, "K_Swing" },
				{ InputName.MOVE_X, "K_MoveHorizontal" },
				{ InputName.MOVE_Y, "K_MoveVertical" },
			};
		}
	}

	// Return the playerID.
	public int GetPlayerID()
	{
		return playerID;
	}

	// Return true when we pressed Jump.
	public bool PressedJump()
	{
		return Input.GetButtonDown(inputMap[InputName.JUMP]);
	}

	// Return true when we pressed Swing.
	public bool PressedSwing()
	{
		return Input.GetButtonDown(inputMap[InputName.SWING]);
	}

	// Return true when we pressed Throw.
	public bool PressedThrow()
	{
		return Input.GetButtonDown(inputMap[InputName.THROW]);
	}

	// Return the axis value for horizontal movement.
	public float GetHorizontal()
	{
		return Input.GetAxis(inputMap[InputName.MOVE_X]);
	}

	// Return the axis value for horizontal movement.
	public float GetVertical()
	{
		return Input.GetAxis(inputMap[InputName.MOVE_Y]);
	}

	/*
	private IEnumerator RemapInputs(InputName name)
	{
		// Loop through all buttons.
		for (int b = 0; b < 20; ++b)
		{
			if (Input.GetKeyDown("joystick " + joystickID + " button " + b))
			{
				Debug.Log("joystick " + js + " button " + b);
			}
		}
	}
	*/
}
