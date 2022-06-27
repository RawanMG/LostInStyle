using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Elevator_InformationCase : MonoBehaviour {

	Text showCase; // show the information
	Elevator elevator; 

	// Use this for initialization
	void Start () 
	{
		// find the text UI in chindren
		showCase = gameObject.GetComponentInChildren<Text>();

		// find the elevator in parent
		elevator = gameObject.GetComponentInParent<Elevator>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// update the information from elevator
		if (elevator.showCaseInfo == "")
		{
			// in case the show case nothing.
			elevator.showCaseInfo = "To " + elevator.Get_Elevator_Platform().nextLevel.levelRef;
		}
		if (showCase != null)
			showCase.text = elevator.showCaseInfo;
	}
}
