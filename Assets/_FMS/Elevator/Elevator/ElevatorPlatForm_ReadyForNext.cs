using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Elevator")]
public class ElevatorPlatForm_ReadyForNext : FsmStateAction
{

	Elevator_Platform platform;
	
	public float waitingTime = 2.0f; // waiting 
	public float currentTime = 0.0f;
	
	public FsmEvent findNextLevel;
	
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		platform = Owner.GetComponent<Elevator_Platform>();

		currentTime = waitingTime;
	}
	
	// Code that runs every frame.
	public override void OnUpdate()
	{
		if (currentTime > 0.0f)
		{
			currentTime -= Time.deltaTime;
		}
		else
		{
			// go to loading
			Fsm.Event(findNextLevel);
		}
	}
	
	// Code that runs when exiting the state.
	public override void OnExit()
	{
		
	}
}
