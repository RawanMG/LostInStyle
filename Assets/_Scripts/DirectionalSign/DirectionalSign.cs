using UnityEngine;
using System.Collections;

public class DirectionalSign : MonoBehaviour 
{

	DirectionalSign_Entity[] entities;
	DirectionalSign_Sensor sensor;

	public bool visualEntity = true;

	public float delayTime = 0.5f;  // if in invisual, how long the entity will be show up when the user seeing at this sign
	public float currentTime = 0.0f;

	public float resetTime = 0.2f; // if user without seeing this sign for a while, the current time will be reseted.
	public float currentResetTime = float.PositiveInfinity;

	bool seeingFlag = false;


	void Awake()
	{
		// load the global setting here, for the visual entity value
	}

	// Use this for initialization
	void Start () 
	{
		// find entity from children
		entities = gameObject.GetComponentsInChildren<DirectionalSign_Entity>();
		sensor = gameObject.GetComponentInChildren<DirectionalSign_Sensor>();


		// initial entity 
		Visual_Entity(visualEntity);

	}
	
	// Update is called once per frame
	void Update () 
	{
		// if it is visual, returns 
		if (visualEntity)
			return;


	}

	void LateUpdate()
	{
		// if it is visual, returns 
		if (visualEntity)
			return;

		if (currentResetTime >= resetTime)
		{
			currentTime = 0.0f;
		}

		if (currentTime >= delayTime)
		{
			// visual the entity
			Visual_Entity(true);
		}

	}

	void FixedUpdate()
	{
		// if it is visual, returns 
		if (visualEntity)
			return;

		if (seeingFlag)
		{
			currentTime += Time.fixedDeltaTime;
			seeingFlag = false;

			currentResetTime = 0.0f;
		}
		else
		{
			currentResetTime += Time.fixedDeltaTime ;
		}
	}

	// *************************************************************************************************************
	// Visual entity
	// *************************************************************************************************************
	public void Visual_Entity(bool b)
	{
		visualEntity = b;
		foreach (DirectionalSign_Entity e in entities)
		{
			e.gameObject.SetActive(visualEntity);
		}
	}

	// ***********************************************************************************************************
	// Seeing, when the user seeing this sign, this function will be called by sensor
	// ***********************************************************************************************************
	public void Seeing()
	{
		// if it is visual, returns 
		if (visualEntity)
			return;

		// set flag
		seeingFlag = true;
	}

}
