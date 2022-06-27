using UnityEngine;
using System.Collections;

public class ATM_Sensor : Sensor {

	ATM atm;

	// Use this for initialization
	void Start () 
	{
		// find ATM in parent
		if (!atm)
		{
			atm = gameObject.GetComponentInParent<ATM>();
		}
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	// *********************************************************************************************************
	// require a ticket for enter.
	// *******************************************************************************************************
	void OnTriggerEnter(Collider other) 
	{
		Ticket_Object ticket = other.gameObject.GetComponent<Ticket_Object>();
		
		// each case will send a event to Fsm
		// *** gameObject.GetComponent<PlayMakerFSM>().SendEvent("Die"); *** //
		if (ticket)
		{
			ticket.ticket = true;
			// send a screen event to Fsm
			atm.GetComponent<PlayMakerFSM>().SendEvent("Purchase Mode");
			
		}
		
	}
	
	void OnTriggerExit(Collider other) 
	{

	}
}
