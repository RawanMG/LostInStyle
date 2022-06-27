using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Ticket System")]
public class ATM_NextAD : FsmStateAction
{
	ATM atm;

	// Code that runs on entering the state.
	public override void OnEnter()
	{
		atm = Owner.GetComponent<ATM>();

		// Next AD to show
		atm.NextAD();
		atm.Display();

		Finish();
	}


}
