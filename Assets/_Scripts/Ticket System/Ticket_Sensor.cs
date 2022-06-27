using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Ticket_Sensor : Sensor 
{
	public enum Type
	{
		In,
		Out,
		Passage
	}

	public Type sensorType = Type.In;

	Ticket_Door door;

	void Start()
	{
		// find the door in parent
		if (!door)
		{
			door = gameObject.GetComponentInParent<Ticket_Door>();
		}
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
			switch (sensorType)
			{
			case Type.In:
			{
				if (ticket.ticket)
				{
					// use a ticket
					ticket.ticket = false;
					door.Light_In();
					door.GetComponent<PlayMakerFSM>().SendEvent("Passage Mode");
				}
				else
				{
					door.light_Error();
				}

			}
				break;

			case Type.Out:
			{
				door.Light_Out();
				door.GetComponent<PlayMakerFSM>().SendEvent("Passage Mode");
			}
				break;

			case Type.Passage:
			{

			}
				break;
			}
		}

	}
	
	void OnTriggerExit(Collider other) 
	{
		Ticket_Object ticket = other.gameObject.GetComponent<Ticket_Object>();

		// each case will send a event to Fsm

		if (ticket)
		{
			switch (sensorType)
			{
			case Type.In:
			{
				door.light_Null();
			}
				break;

			case Type.Out:
			{

			}
				break;

			case Type.Passage:
			{
				door.light_Null();
				door.GetComponent<PlayMakerFSM>().SendEvent("Listen Mode");
			}
				break;
			}
		}
	}
}
