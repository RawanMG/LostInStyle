using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class MainMenu : MonoBehaviour {

	public Text tips;


	// Use this for initialization
	void Start () 
	{
		tips.text = "*** Note: Please copy folder [DataResults] to " + Application.dataPath + " ***";
	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	// ***********************************************************************************************************
	// Function for button
	// ***********************************************************************************************************
	public void Btn_Data_Collect()
	{
		Application.LoadLevel("DataCollectMenu");
	}

	public void Btn_Data_Analyze()
	{
		Application.LoadLevel("DataAnalyzeMenu");
	}

	public void Btn_Free_Looking()
	{
		
	}
}
