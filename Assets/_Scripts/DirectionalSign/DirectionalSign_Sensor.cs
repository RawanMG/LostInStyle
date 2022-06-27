using UnityEngine;
using System.Collections;

public class DirectionalSign_Sensor : Sensor 
{
	public DirectionalSign sign{get;set;}

	// Use this for initialization
	void Awake () 
	{
		// find the sign from parent
		if (!sign)
		{
			sign = gameObject.GetComponentInParent<DirectionalSign>();
		}
	}

	// ***********************************************************************************************************
	// Seeing, when the user seeing this sign, this function will be called in the ray-casting setp;
	// ***********************************************************************************************************
	public void Seeing()
	{
		sign.Seeing();
	}

}
