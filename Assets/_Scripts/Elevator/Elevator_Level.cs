using UnityEngine;
using System.Collections;

[RequireComponent(typeof(Animator))]
public class Elevator_Level : MonoBehaviour 
{
	// specify which level, make sure this value be unique
	public string levelRef = "0";

	public string openAnim = "open";
	public string closeAnim = "close";

	[Multiline (10)]
	public string description;

	// Use this for initialization
	void Start () 
	{
		
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}
}
