using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Elevator")]
public class ElevatorPlatForm_Loading : FsmStateAction
{
	Elevator_Platform platform;

	public float delayTime = 3.0f; // until no one in the seneor area
	float currentTime = 0.0f;

	public FsmEvent close;

	// Code that runs on entering the state.
	public override void OnEnter()
	{
		platform = Owner.GetComponent<Elevator_Platform>();
		currentTime = delayTime;
	}

	// Code that runs every frame.
	public override void OnUpdate()
	{
		if (currentTime < 0.0f)
		{
			Fsm.Event(close);
		}
		else
		{
			currentTime -= Time.deltaTime;

			// if still have a object in the sensor area, reset the dalayTime;
			if (platform.SensorArea_Count() != 0)
			{
				currentTime = delayTime;
			}
		}
	}

	// Code that runs when exiting the state.
	public override void OnExit()
	{
		
	}
}
