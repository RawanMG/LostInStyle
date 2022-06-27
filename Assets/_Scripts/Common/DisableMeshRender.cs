using UnityEngine;
using System.Collections;

public class DisableMeshRender : MonoBehaviour {

	// Use this for initialization
	//public bool show = true;
	void Awake () 
	{

		MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
		if (mr)
		{
			mr.enabled = false;
		}
	}

}
