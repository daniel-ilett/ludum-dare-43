using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameTimer : MonoBehaviour
{
	[SerializeField]
	private Text timerText;

	[SerializeField]
	private Text startTimeText;

	private int _seconds = 3;
	private int Seconds
	{
		get
		{
			return _seconds;
		}
		set
		{
			_seconds = value;
			// Set the state of the world timer.
			timerText.text = (Seconds < 10 ? "0" : "") + Seconds.ToString();
		}
	}

	public event EventHandler<EventArgs> TimerExpired;

	private void Awake()
	{
		StartCoroutine(TimerLoop());
	}

	// Maintain a timer loop that acts as the timespan of the game.
	private IEnumerator TimerLoop()
	{
		var wait = new WaitForSeconds(1.0f);

		while(Seconds > 0)
		{
			yield return wait;
			--Seconds;
		}

		// End game.
		OnTimerExpired(new EventArgs());
	}

	// Fire an event when the game timer expires.
	private void OnTimerExpired(EventArgs e)
	{
		EventHandler<EventArgs> handler = TimerExpired;

		if (handler != null)
		{
			handler(this, e);
		}
	}
}
