// Haikun Huang
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
public class Escalator_Level : MonoBehaviour 
{
	// public Escalator_Level next_level {set;get;}
	Vector3 next_position;

	public float interval_time {set;get;}
	public float tolerance {set;get;}



	public Escalator.Direction dir {set;get;}
	public List<Vector3> positions{set;get;}
	public int index{set;get;}

	float speed;

	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		// move to next
		transform.position = Vector3.MoveTowards(transform.position,
		                                         next_position,
		                                         speed * Time.fixedDeltaTime);

		// meet the next position
		if (((transform.position - next_position).magnitude) <= tolerance)
		{
			// transform.position = next_position;
			Next ();
		}
	}

	void LateUpdate()
	{
		// meet the next position
//		if (((transform.position - next_position).magnitude) <= tolerance)
//		{
//
//		}
	}

	// find the next position
	public void Next()
	{
		if (dir == Escalator.Direction.Up)
		{
			index = (index+1) % positions.Count;
		}
		else
		{
			index = (index + positions.Count -1) % positions.Count;
        }

		next_position = positions[index];
		float distance = (transform.position - next_position).magnitude;
		speed = distance / interval_time;
	}
}
