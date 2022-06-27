using UnityEngine;
using System.Collections;

public class DirectionalSign_Obejct : MonoBehaviour {

	Eyes eye;

	// Use this for initialization
	void Start () 
	{
		// find the sensor in children
		if (!eye)
		{
			eye = gameObject.GetComponentInChildren<Eyes>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void FixedUpdate()
	{
		if (eye == null)
		{
			eye = gameObject.GetComponentInChildren<Eyes>();
		}

		if (eye == null) {
			return;
		}

		// create a ray
		Ray ray = new Ray(eye.transform.position,eye.transform.forward);

		RaycastHit hit;

		if (Physics.Raycast(ray,out hit))
		{

			// if the object is a directional sensor, call the seeing
			DirectionalSign_Sensor sensor 
				= hit.collider.gameObject.GetComponent<DirectionalSign_Sensor>();

			if (sensor)
			{
				sensor.Seeing();
			}
		}
	}
}
