using UnityEngine;
using System.Collections;
using System.Collections.Generic;

[RequireComponent(typeof(Animator))]
public class Elevator_Platform : MonoBehaviour 
{

	Elevator elevator;
	public Elevator Get_Elevator(){return elevator;}

	public Elevator_Level nextLevel {get;set;} // get the next level from the elevator

	public float speed = 1.0f;

	Vector3 destPos;  // the position of the destination

	public float posTolerance = 0.00001f; // the tolerance between the position and the destination

	public string openAnim = "open";
	public string closeAnim = "close";


	// for sensor area, recored how many object stay in this area.
	List<Elevator_Object> sensorArea;

	// Use this for initialization
	void Start () 
	{
		// initial sensor area
		sensorArea = new List<Elevator_Object>();

		// find elevator in parent
		if (!elevator)
		{
			elevator = gameObject.GetComponentInParent<Elevator>();
		}

		// get the next level
		Next();
	}
	

	// *************************************************************************************************************
	// find the next level, and calculate the position of the destination
	// *************************************************************************************************************
	public void Next()
	{
		nextLevel = elevator.Next();

		destPos.x = transform.position.x;
		destPos.y = nextLevel.gameObject.transform.position.y;
		destPos.z = transform.position.z;
	}

	// *************************************************************************************************************
	// reach the level?
	// *************************************************************************************************************
	public bool ReachLevel()
	{
		float remain = (destPos - transform.position).magnitude;

		if (remain <= posTolerance)
			return true;
		return false;
	}

	// *************************************************************************************************************
	// move in update, should be called in update function.
	// *************************************************************************************************************
//	public void MoveUpdate()
//	{
//		transform.position= Vector3.MoveTowards(transform.position, destPos, Time.deltaTime * speed);
//	}

	// by iTween
	public void MoveTo()
	{
		iTween.MoveTo(gameObject, iTween.Hash(
			"position", destPos,
			"speed", speed,
			"easetype", "easeInOutQuad"
						));
	}

	// *************************************************************************************************************
	// senseor area
	// *************************************************************************************************************
	public void SensorArea_Join(Elevator_Object target)
	{
		if (!sensorArea.Contains(target))
		{
			sensorArea.Add(target);
		}
	}

	public void SensorArea_Leave(Elevator_Object target)
	{
		sensorArea.Remove(target);
	}

	public int SensorArea_Count()
	{
		return sensorArea.Count;
	}

}	
