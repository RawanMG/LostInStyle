using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityStandardAssets.CrossPlatformInput;

public class FreeCamera : MonoBehaviour {

	public float speed_walk = 5f;

	public float spped_rot = 180f;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		GetMove2 ();
	}

	void GetMove2()
	{
		// movement
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");
		horizontal = Mathf.Abs (horizontal) < 0.2f ? 0 : horizontal;
		vertical = Mathf.Abs (vertical) < 0.2f ? 0 : vertical;

		// roatation
		float rJHorizontal = CrossPlatformInputManager.GetAxis ("RJoystickH");
		float rJVertical = CrossPlatformInputManager.GetAxis ("RJoystickV");
		rJHorizontal = Mathf.Abs (rJHorizontal) < 0.2f ? 0 : rJHorizontal;
		rJVertical = Mathf.Abs (rJVertical) < 0.2f ? 0 : rJVertical;

		// rotate 
		transform.Rotate (Vector3.up, rJHorizontal * Time.deltaTime * spped_rot);
		//transform.Rotate (Vector3.right, -rJVertical * Time.deltaTime * spped_rot);

		// fixed rolling
		Vector3 fixedroll = transform.rotation.eulerAngles;
		fixedroll.z = 0;
		transform.rotation = Quaternion.Euler (fixedroll);

		// otherwise, controlled by vr_head

		// move
		Vector3 forward  = transform.forward;
		forward.Normalize ();
		forward *= vertical * speed_walk * Time.deltaTime;


		Vector3 right = transform.right;
		right.Normalize ();
		right *= horizontal * speed_walk * Time.deltaTime;

		Vector3 up = transform.up;
		up.Normalize ();
		up *= rJVertical * speed_walk * Time.deltaTime;

		transform.position += (forward + right + up);

	}
}
