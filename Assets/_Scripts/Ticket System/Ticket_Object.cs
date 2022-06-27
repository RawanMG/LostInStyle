using UnityEngine;
using System.Collections;

// this is a simple ticket system

public class Ticket_Object : MonoBehaviour 
{

	public bool ticket = false;

	public bool all_ways_ticket = false;

	void Update()
	{
		if (all_ways_ticket)
			ticket = true;
	}
	
}
