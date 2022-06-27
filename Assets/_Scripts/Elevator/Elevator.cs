using UnityEngine;
using System.Collections;
using System.Collections.Generic;

// this is a Elevator, managers the platform and all the levels


public class Elevator : MonoBehaviour 
{


	public Elevator_Level[] levels;

	public Dictionary<string,Elevator_Level> levelDict;

	public int startAtLevelIndex = 0;

	Elevator_Platform platform;
	public Elevator_Platform Get_Elevator_Platform() {return platform;}

	Elevator_Level nextLevel;

	public string showCaseInfo = ""; // show case information

	// Use this for initialization
	void Start () 
	{
		// make dictionary
		MakeDictionary();

		// find platform in children
		if (!platform)
		{
			platform = gameObject.GetComponentInChildren<Elevator_Platform>();
		}

		// initial the next level
		nextLevel = levels[startAtLevelIndex];
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	// *************************************************************************************************************
	// find the next destination, and then, recalculate for the next call.
	// this be called by platform
	// *************************************************************************************************************
	public Elevator_Level Next()
	{
		Elevator_Level ret = nextLevel;
		RecalculateNext();
		return ret;
	}

	void RecalculateNext()
	{
		// in this case just loop the levels, use the start at level index as the current index
		startAtLevelIndex = (startAtLevelIndex + 1) % levels.Length;
		nextLevel = levels[startAtLevelIndex];
	}

	// *************************************************************************************************************
	// make the dictionary, use the level ref to ref the level infomations.
	// *************************************************************************************************************
	void MakeDictionary()
	{
		levelDict = new Dictionary<string, Elevator_Level>();

		foreach(Elevator_Level el in levels)
		{
			// make sure each level's levelref be unique
			if (levelDict.ContainsKey(el.levelRef))
			{
				Debug.LogError("duplication level ref: " + el.levelRef + " in " + gameObject.name);
			}
			else
			{
				levelDict.Add(el.levelRef, el);
			}
		}
	}
}







