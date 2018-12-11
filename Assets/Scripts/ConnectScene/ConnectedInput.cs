using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConnectedInput : MonoBehaviour
{
	private int playerID;
	private int joystickID;

	private Dictionary<InputName, string> inputMap;

	public InputRemap remapper { get; private set; }

	private bool playing = false;

	private void Awake()
	{
		// Subscribe to the main menu start event.
		Connections.ReturnToMainMenuEvent += OnReturnToMainMenu;

		DontDestroyOnLoad(gameObject);
	}

	// Set the kind of controller this input is.
	public void SetConnType(int joystickID, InputRemap remapper, int playerID)
	{
		Debug.Log("Connected - joystick ID is " + joystickID + ", playerID is " + playerID);

		this.playerID = playerID;
		this.remapper = remapper;
		this.joystickID = joystickID;

		if(joystickID > 0)
		{
			inputMap = new Dictionary<InputName, string>
			{
				{ InputName.JUMP, "J" + joystickID + "_Jump" },
				{ InputName.THROW, "J" + joystickID + "_Throw" },
				{ InputName.SWING, "J" + joystickID + "_Swing" },
				{ InputName.BACK, "J" + joystickID + "_Back" },
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
				{ InputName.BACK, "K_Back" },
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

	// Return the joystickID.
	public int GetJoystickID()
	{
		return joystickID;
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

	// Return true when we pressed Back.
	public bool PressedBack()
	{
		return Input.GetButtonDown(inputMap[InputName.BACK]);
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

	// Return a 2D vector of the movement direction.
	public Vector2 GetMoveDir()
	{
		return new Vector2(GetHorizontal(), GetVertical());
	}

	// Called when the game returns to the main menu.
	public void OnReturnToMainMenu(object sender, EventArgs e)
	{
		Destroy(gameObject);
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

	// Unsubscribe from the main menu event.
	private void OnDestroy()
	{
		Connections.ReturnToMainMenuEvent -= OnReturnToMainMenu;
	}
}
