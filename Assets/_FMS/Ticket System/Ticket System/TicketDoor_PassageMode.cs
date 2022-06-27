using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Ticket Door")]
public class TicketDoor_PassageMode : FsmStateAction
{
	Ticket_Door door;


	// Code that runs on entering the state.
	public override void OnEnter()
	{
		door = Owner.GetComponent<Ticket_Door>();

		// change the mode
		if (door.Is_Ready_For_Enter())
			door.Mode_Passage();

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
