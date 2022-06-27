using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using NUnit.Framework;
using System.Security.Policy;
using System;
using System.IO;

[ExecuteInEditMode]
public class WaypointSystem : MonoBehaviour {

	LineRenderer line;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		Transform[] wps = GetWaypoints ();
		for (int i = 0; i < wps.Length-1; i++) {
			Debug.DrawLine (wps[i].position, wps[i+1].position, Color.red);
		}

		line = GetComponent<LineRenderer> ();
		if (line != null) {
			line.positionCount = wps.Length;
			for (int i = 0; i < wps.Length; i++) {
				line.SetPosition (i, wps[i].position);
			}
		}
	}

	// get all waypoints trans
	public Transform[] GetWaypoints(){
		List<Transform> ret = new List<Transform> ();
		for (int i = 0; i < transform.childCount; i++) {
			ret.Add (transform.GetChild (i));
		}
		return ret.ToArray ();
	}

	// save file
	public void SaveToFile(string file){
		Transform[] wps = GetWaypoints ();
		StreamWriter sw = new StreamWriter (file);

		// override your format here
		for (int i = 0; i < transform.childCount; i++) {
			sw.WriteLine (wps[i].position.x + ", "+wps[i].position.y + ", " + wps[i].position.z);
		}
		sw.Close ();

		Debug.Log ("Save to :" + file);
	}
}
