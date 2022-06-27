using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Elevator")]
public class ElevatorPlatForm_Close : FsmStateAction
{
	Elevator_Platform platform;
	
	public FsmEvent readyForNext;

	// public FsmEvent open;
	
	public float delayTime = 2.0f;
	public float currentTime = 0.0f;
	
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		platform = Owner.GetComponent<Elevator_Platform>();
		
		// open the outter door
		CloseOutter();
		// open the inter door
		CloseInner();

		currentTime = delayTime;
	}
	
	// Code that runs every frame.
	public override void OnUpdate()
	{
		if (currentTime > 0.0f)
		{
			currentTime -= Time.deltaTime;

			// if some one in the sensor area, open the door
//			if (platform.SensorArea_Count() != 0)
//			{
//				Fsm.Event(open);
//			}
		}
		else
		{
			Fsm.Event(readyForNext);
		}
	}
	
	// Code that runs when exiting the state.
	public override void OnExit()
	{
		
	}
	
	// **********************************************************************************************************
	// Close the outter door
	// **********************************************************************************************************
	void CloseOutter()
	{
		Animator anim = platform.nextLevel.GetComponent<Animator>();
		anim.Play(platform.nextLevel.closeAnim);
	}
	
	// **********************************************************************************************************
	// Close the inner door
	// **********************************************************************************************************
	
	void CloseInner()
	{
		Animator anim = platform.GetComponent<Animator>();
		anim.Play(platform.closeAnim);
	}


}
