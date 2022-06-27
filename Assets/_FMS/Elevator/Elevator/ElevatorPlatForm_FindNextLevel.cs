using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Elevator")]
public class ElevatorPlatForm_FindNextLevel : FsmStateAction
{
	Elevator_Platform platform;

	public FsmEvent go2TheLevel;

	// Code that runs on entering the state.
	public override void OnEnter()
	{
		platform = Owner.GetComponent<Elevator_Platform>();
		platform.Next();

		// show case information
		platform.Get_Elevator().showCaseInfo = "To " +platform.nextLevel.levelRef;

		Fsm.Event(go2TheLevel);
	}

	// Code that runs every frame.
	public override void OnUpdate()
	{
		
	}

	// Code that runs when exiting the state.
	public override void OnExit()
	{
		
	}

}
