using UnityEngine;
using System.Collections;
using UnityEngine.AI;

public class SafeArea : Sensor {
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

//	void OnTriggerEnter(Collider other){
//		Debug.Log ("onTriggerEnter");
//		NavMeshAgent agent = FindObjectOfType(typeof(NavMeshAgent) ) as NavMeshAgent;
//		if (agent.enabled) {
//			agent.enabled = false;
//			Debug.Log ("disabling Agent");
//		} else {
//			agent.enabled = true;
//			Debug.Log ("enabling Agent");
//		}
//	}
	void OnTriggerStay(Collider other) 
	{
			
		Escalators_Object es = other.gameObject.GetComponent<Escalators_Object>();
		if (es)
		{
			other.gameObject.transform.parent = null;
		}

		Elevator_Object el = other.gameObject.GetComponent<Elevator_Object>();
		if (el)
		{
			other.gameObject.transform.parent = null;
		}
	}
}
