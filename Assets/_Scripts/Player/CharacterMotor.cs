// Quincy modified on May 19m 2017

using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityStandardAssets.CrossPlatformInput;
using UnityStandardAssets.Utility;
using UnityStandardAssets.Characters.FirstPerson;
using System.ComponentModel;
using UnityEngine.EventSystems;
using CognitiveVR;
//using NUnit.Framework;

public class CharacterMotor : MonoBehaviour {

	public float speed_walk = 2.0f;

	public float speed_run = 3.2f; // we donot use this in this project.

	public float spped_rot = 180f;

	//public Head vr_head;
	public Transform nor_head;

	public Transform vr_head;

	// Quincy added.
	// ref the actitived hear
	Transform head;

	//[SerializeField]
	//MouseLook m_MouseLook;

	public KeyCode runKey = KeyCode.LeftShift;

	CharacterController cc;
	CollisionFlags m_CollisionFlags;

	private Vector3 moveDir = Vector3.zero;

	//Rigidbody rig;

	// Use this for initialization

	public bool force3DMode = false;


	Transform cameraTransform;


	void Awake()
	{
		
		if (GData.VR_Mode)
		{
			//m_MouseLook.YSensitivity = 0.0f;
			vr_head.gameObject.SetActive(true);
			nor_head.gameObject.SetActive(false);
			// ref the acititive hear
			head = vr_head;

		}
		if (!GData.VR_Mode || force3DMode)
		{
			vr_head.gameObject.SetActive(false);
			nor_head.gameObject.SetActive(true);
			// ref the acititive hear
			head = nor_head;
		}

		//rig = GetComponent<Rigidbody> ();
	}

	void Start () 
	{
		cc = gameObject.GetComponent<CharacterController>();

		// find the eyes from children
		//head = gameObject.GetComponentInChildren<Head>().transform;

		//m_MouseLook.Init(transform , head);
		cameraTransform = Camera.main.transform;

	}
	
	// Update is called once per frame
	void Update () 
	{
		//m_MouseLook.LookRotation (transform, head);
		// adjust the player rotation

		//GetMove();
		GetMove2 ();
		//Gazing ();
	}





	// Quincy added
	// move controller
	// good to use
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
		if (!GData.VR_Mode)
		{
			head.Rotate (Vector3.right, -rJVertical * Time.deltaTime * spped_rot);
		}
		// otherwise, controlled by vr_head

		// move
		Vector3 forward  = head.forward;
		forward.y = 0f;
		forward.Normalize ();
		forward *= vertical * speed_walk * Time.deltaTime;


		Vector3 right = head.right;
		right.y = 0;
		right.Normalize ();
		right *= horizontal * speed_walk * Time.deltaTime;

		cc.Move (forward + right);

		cc.SimpleMove (Physics.gravity * Time.deltaTime);


	}

	void GetMove()
	{
		float horizontal = CrossPlatformInputManager.GetAxis("Horizontal");
		float vertical = CrossPlatformInputManager.GetAxis("Vertical");

		float RJHorizontal = CrossPlatformInputManager.GetAxis ("RJoystickH");
		float RJVertical = CrossPlatformInputManager.GetAxis ("RJoystickV");
		//Debug.Log ("horz " + horizontal + " vert " + vertical + " RJH " + RJHorizontal + " RJV " + RJVertical);

		float speed = Input.GetKey(runKey) ? speed_run : speed_walk;
		head.Rotate (0, RJHorizontal *speed*50, 0);
		//Vector3 oldforward  = head.forward;
		Vector3 forward  = head.forward;
		forward.y = 0f;
		forward.Normalize ();


		Vector3 right = head.right;
		right.y = 0;
		right.Normalize ();
		if(Mathf.Abs(horizontal) > 0.2f || Mathf.Abs(vertical) > 0.2f){
		Vector3 targetDir = (forward*vertical + right*horizontal);

		Vector3 desiredDir = targetDir;//Vector3.RotateTowards(targetDir, right, 200*Time.deltaTime*RJHorizontal, 0.0f );
		Vector3 movement = desiredDir * Time.deltaTime * 2;
		// apply gtavity
		movement += Physics.gravity * Time.deltaTime;
		cc.Move (movement);
		desiredDir.x  = -horizontal;
		desiredDir.y = 0f;
		desiredDir.z = vertical;

		desiredDir = desiredDir * Time.deltaTime * speed;
		//Vector3 desiredDir = (forward*vertical + right*horizontal)
		//	* Time.deltaTime * speed;

		// apply gtavity
		desiredDir += Physics.gravity * Time.deltaTime;

		//m_CollisionFlags = cc.Move(desiredDir);
		}
	}

	private void OnControllerColliderHit(ControllerColliderHit hit)
	{
		Rigidbody body = hit.collider.attachedRigidbody;
		//dont move the rigidbody if the character is on top of it
		if (m_CollisionFlags == CollisionFlags.Below)
		{
			return;
		}
		
		if (body == null || body.isKinematic)
		{
			return;
		}
		body.AddForceAtPosition(cc.velocity*0.1f, hit.point, ForceMode.Impulse);
	}
}
