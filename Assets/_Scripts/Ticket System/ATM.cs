using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class ATM : MonoBehaviour {

	//[Multiline (5)]
	public string screenInfo {set;get;}
	[Multiline (5)]
	public string[] ADInfos; // this is for showing the ADs

	Text screen;

	// Use this for initialization
	void Start () 
	{

		// find screen in children
		if (!screen)
		{
			screen = gameObject.GetComponentInChildren<Text>();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	// *********************************************************************************************************
	// Find a Next AD
	// *********************************************************************************************************
	public void NextAD()
	{
		screenInfo = ADInfos[Random.Range(0, ADInfos.Length)];
	}

	// *********************************************************************************************************
	// Display to screen
	// *********************************************************************************************************
	public void Display()
	{
		if (screen != null)
			screen.text = screenInfo;
	}

}
