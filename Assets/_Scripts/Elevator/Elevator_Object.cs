using UnityEngine;
using System.Collections;

public class Elevator_Object : MonoBehaviour
{

	CharacterController charController;
	Vector3 oldPosition; // last position of this object
	
	void Awake ()
	{
		charController = GetComponent<CharacterController> ();
	}
	
	void Start ()
	{
		oldPosition = transform.localPosition;
	}
	
	// Update is called once per frame
	void LateUpdate ()
	{
		// if this game object is a character controller
		if (charController) {
			if (!charController.isGrounded) 
			{
				transform.parent = null;
			}
//			// does this object move?
//			float dis = (transform.localPosition - oldPosition).magnitude;
//			if (dis > 0.0001f) 
//			{
//				transform.parent = null;
//			}
//			oldPosition = transform.localPosition;
		}
	}
	
	void OnCollisionEnter (Collision collision)
	{
		Elevator_Platform obj = collision.gameObject.GetComponent<Elevator_Platform> ();
		if (obj) 
		{
			if (transform.parent == null) 
			{
				transform.parent = obj.transform;
			}
		}
	}
	
	void OnCollisionExit (Collision collision)
	{
		Elevator_Platform obj = collision.gameObject.GetComponent<Elevator_Platform> ();
		if (obj) 
		{
			//if (transform.parent == obj.transform)
			{
				transform.parent = null;
			}
		}
	}
	
//	// this function will be called every frame if the character controller collide any colliders
	void OnControllerColliderHit (ControllerColliderHit collision)
	{
		Elevator_Platform obj = collision.gameObject.GetComponent<Elevator_Platform> ();
		if (obj) 
		{
			if (transform.parent == null) 
			{
				transform.parent = obj.transform;
			}
		}
		
	}
}
