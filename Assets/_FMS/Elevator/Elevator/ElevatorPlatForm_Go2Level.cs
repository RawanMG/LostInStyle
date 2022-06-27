using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Elevator")]
public class ElevatorPlatForm_Go2Level : FsmStateAction
{
	public FsmEvent findNextLevel;

	public FsmEvent reach;

	Elevator_Platform platform;

	// Code that runs on entering the state.
	public override void OnEnter()
	{
		platform = Owner.GetComponent<Elevator_Platform>();

		// need to find the next level 
		if (!platform.nextLevel)
		{
			Fsm.Event(findNextLevel);
		}

		// move to
		platform.MoveTo();
	}

	// Code that runs every frame.
	public override void OnUpdate()
	{
		// until reach the destination
		if (platform.ReachLevel())
		{
			Fsm.Event(reach);
		}
		else
		{
			//platform.MoveUpdate();
		}

	}

	// Code that runs when exiting the state.
	public override void OnExit()
	{
		
	}


}
