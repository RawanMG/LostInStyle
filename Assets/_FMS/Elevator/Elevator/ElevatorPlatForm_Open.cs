using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Elevator")]
public class ElevatorPlatForm_Open : FsmStateAction
{
	Elevator_Platform platform;
	
	public FsmEvent loading;
	
	public float delayTime = 2.0f;
	public float currentTime = 0.0f;
	
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		platform = Owner.GetComponent<Elevator_Platform>();

		// open the outter door
		OpenOutter();
		// open the inter door
		OpenInner();

		currentTime = delayTime;
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
			Fsm.Event(loading);
		}
	}

	// Code that runs when exiting the state.
	public override void OnExit()
	{
		
	}

	// **********************************************************************************************************
	// Open the outter door
	// **********************************************************************************************************
	void OpenOutter()
	{
		Animator anim = platform.nextLevel.GetComponent<Animator>();
		anim.Play(platform.nextLevel.openAnim);
	}

	// **********************************************************************************************************
	// Open the inner door
	// **********************************************************************************************************

	void OpenInner()
	{
		Animator anim = platform.GetComponent<Animator>();
		anim.Play(platform.openAnim);
	}
}
