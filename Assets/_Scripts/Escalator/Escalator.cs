// Haikun Huang
using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Escalator : MonoBehaviour 
{
	public enum Direction
	{
		Up,
		Down
	}
	// going where? up or down
	public Direction dir = Direction.Up;

	public Escalator_Level[] levels;

	public float interval_time = 1.0f;

	public float tolerance = 0.0001f;
	List<Vector3> positions = new List<Vector3>();

	// Use this for initialization
	void Awake () 
	{
		for(int i=0;i<levels.Length;i++)
        {
			if (levels[i])
				positions.Add(levels[i].transform.position);
		}

		// init levels
		Init_Level();

	}
	
	// Update is called once per frame
	void Update () 
	{
	
	}

	void Init_Level()
	{
		for(int i=0;i<levels.Length;i++)
		{
			levels[i].index = i;
			levels[i].dir = dir;
			levels[i].positions = positions;
			levels[i].tolerance = tolerance;
			levels[i].interval_time = interval_time;

			levels[i].Next();
		}

	}
}
