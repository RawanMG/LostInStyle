using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveMarker : MonoBehaviour {
	Transform gaze_marker;
	FoveInterface.EyeRays rays;
	int count;
	// Use this for initialization
	void Start () {
		rays = FoveInterface.GetEyeRays();
		count = 0;
		
	}
	
	// Update is called once per frame
	void Update () {
		if (count % 5 == 0) {

			rays = FoveInterface.GetEyeRays ();
			Debug.DrawRay (rays.left.origin, 10 * rays.left.direction, Color.blue, 10);
			//TODO: try GazeVector
			RaycastHit hit;
			if (Physics.Raycast (rays.left.origin, rays.left.direction, out hit, Mathf.Infinity)) {
				transform.position = hit.point;
			}
		}
		count += 1;
		
	}
}
