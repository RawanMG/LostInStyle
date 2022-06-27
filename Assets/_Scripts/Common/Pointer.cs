using UnityEngine;
using System.Collections;

public class Pointer : MonoBehaviour {

	// find all the render
	MeshRenderer[] renderers;

	// user object
	User_Object user;

	float hide_dist = 15.0f;

	// Use this for initialization
	void Start () 
	{
		renderers = gameObject.GetComponentsInChildren<MeshRenderer>();
		user = GameObject.FindObjectOfType<User_Object>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		float dist = (user.transform.position - gameObject.transform.position).magnitude;
		if (dist >= hide_dist)
		{
			foreach(MeshRenderer mr in renderers)
			{
				mr.enabled = false;
			}
		}
		else
		{
			foreach(MeshRenderer mr in renderers)
			{
				mr.enabled = true;
			}
		}
	}
}
