using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Ticket_Door : MonoBehaviour {

	public string openAnim = "open";
	public string closeAnim = "close";

	public Ticket_Sensor sensorIn, sensorOut, sensorPassage;

	bool isReadyForEnter = true;
	public bool Is_Ready_For_Enter() {return isReadyForEnter;}

	public Material lightNull, lightRed, lightGreen;
	public MeshRenderer lightIn0,lightIn1,lightOut0,lightOut1;

	Animator anim;

	// Use this for initialization
	void Start () 
	{
		anim = gameObject.GetComponent<Animator>();

		sensorIn.gameObject.SetActive(true);
		sensorOut.gameObject.SetActive(true);
		sensorPassage.gameObject.SetActive(false);

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	// ************************************************************************************************************
	// Mode change
	// ************************************************************************************************************
	public void Mode_Listen()
	{
		sensorIn.gameObject.SetActive(true);
		sensorOut.gameObject.SetActive(true);
		sensorPassage.gameObject.SetActive(false);
		isReadyForEnter = true;

		CloseDoor();
	}

	public void Mode_Passage()
	{
		sensorIn.gameObject.SetActive(false);
		sensorOut.gameObject.SetActive(false);
		sensorPassage.gameObject.SetActive(true);
		isReadyForEnter = false;

		OpenDoor();
	}

	// ************************************************************************************************************
	// Play Animation
	// ************************************************************************************************************
	public void OpenDoor()
	{
		anim.CrossFade(openAnim, 0.1f);
	}

	public void CloseDoor()
	{
		anim.CrossFade(closeAnim, 0.1f);
	}

	// ************************************************************************************************************
	// Change the lights
	// ************************************************************************************************************
	public void Light_In()
	{
		lightIn0.material = lightGreen;
		lightOut0.material = lightRed;

		lightIn1.material = lightGreen;
		lightOut1.material = lightRed;
	}

	public void Light_Out()
	{
		lightIn0.material = lightRed;
		lightOut0.material = lightGreen;

		lightIn1.material = lightRed;
		lightOut1.material = lightGreen;
	}

	public void light_Null()
	{
		lightIn0.material = lightNull;
		lightOut0.material = lightNull;

		lightIn1.material = lightNull;
		lightOut1.material = lightNull;
	}

	public void light_Error()
	{
		lightIn0.material = lightRed;
		lightOut0.material = lightRed;

		lightIn1.material = lightRed;
		lightOut1.material = lightRed;
	}
}
