using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Collider))]
public class Elevator_Sensor : Sensor {

	Elevator_Platform platform;

	// Use this for initialization
	void Start () 
	{
		// find platform from parent
		if (!platform)
		{
			platform = gameObject.GetComponentInParent<Elevator_Platform>();
		}
	}

	void OnTriggerEnter(Collider other) 
	{
		Elevator_Object eo = other.gameObject.GetComponent<Elevator_Object>();
		if (eo)
		{
			platform.SensorArea_Join(eo);
		}
	}

	void OnTriggerExit(Collider other) 
	{
		Elevator_Object eo = other.gameObject.GetComponent<Elevator_Object>();
		if (eo)
		{
			platform.SensorArea_Leave(eo);
			// also force the object's parent to null
			eo.gameObject.transform.parent = null;
		}
	}
}
