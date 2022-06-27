using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DataCollectMenu : MonoBehaviour {

	public Toggle tog_vrMode;

	// Use this for initialization
	void Start () 
	{
		tog_vrMode.isOn = GData.VR_Mode;
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}


	// ***********************************************************************************************************
	// Function for button
	// ***********************************************************************************************************
	public void Btn_Back()
	{
		Application.LoadLevel("MainMenu");
	}

	public void Btn_Kenmore_Station()
	{
		GData.playMode = GData.PlayMode.Recoder;
		GData.VR_Mode = tog_vrMode.isOn;

		GData.next_scene = "Kenmore";

		Application.LoadLevel("ReadyMenu");
	}

	public void Btn_Kenmore_Station_Free_Looking()
	{
		GData.playMode = GData.PlayMode.Free_Style;
		GData.VR_Mode = tog_vrMode.isOn;
		
		GData.next_scene = "Kenmore";
		
		Application.LoadLevel("ReadyMenu");
	}
}
