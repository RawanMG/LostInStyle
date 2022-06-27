using UnityEngine;
using HutongGames.PlayMaker;

[ActionCategory("Ticket System")]
public class ATM_PurchaseMode : FsmStateAction
{
	ATM atm;
	
	public float delayTime = 5.0f;
	float currentTime = 0.0f;
	
	public FsmEvent nextAD;
	
	// Code that runs on entering the state.
	public override void OnEnter()
	{
		atm = Owner.GetComponent<ATM>();

		atm.screenInfo = "You Got a Ticket!\nThank you.";
		atm.Display();
		
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
			Fsm.Event(nextAD);
		}
	}
	
	// Code that runs when exiting the state.
	public override void OnExit()
	{
		
	}


}
