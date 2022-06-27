using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

using Random = UnityEngine.Random;

public class Agent : MonoBehaviour 
{
	public GameObject see_target;

	public float move = 1.5f,rot = 30.0f;

	public bool fast_move;

	public float radius = 10.0f;
	public float angle = 60.0f;

	public int try_times = 1000;
	public int current_try_time  = 0;

	
	public float max_distance = 300.0f;
	public float comsume_distance;

	public Vector3 next_pos;

	UnityEngine.AI.NavMeshAgent agent;

	public bool isDone = false;

	List<Vector3> pathList;

	Vector3 oldPos, oldForward;

	public int current_OK_rate = 0;




	// Use this for initialization
	void Start () 
	{ 
		//current_try_time = 0;
		oldPos = gameObject.transform.position;
		oldForward = gameObject.transform.forward;
		
		current_OK_rate = 0;
		Init();

	}

	public void Init()
	{
		if (!isDone)
		{
			agent = GetComponent<UnityEngine.AI.NavMeshAgent>();
			pathList = new List<Vector3>();
			comsume_distance = 0;


			gameObject.transform.position = oldPos;
			gameObject.transform.forward = oldForward;
			Mark_Path();

			//isDone = false;
			Next_Pos();
		}
	}
	
	// Update is called once per frame
	void Update () 
	{
		if (pathList.Count >=4)
		{
			for(int i=0; i<pathList.Count-2; i+=2)
			{
				Debug.DrawLine(pathList[i], pathList[i+2], Color.red, 15.0f);
            }
            
        }

		Debug.DrawLine(pathList[pathList.Count-2], gameObject.transform.position, Color.red,15.0f);
	


	}

	void FixedUpdate()
	{
		if (current_try_time >= try_times)
		{
			isDone = true;

			Debug.Log( "OK rate: " + current_OK_rate +"/" + try_times);
		}

		if (!isDone)
		{
			//comsume_time += Time.fixedDeltaTime;

			// time out
			if (comsume_distance > max_distance)
			{
				// Debug.Log("Time out :"+ comsume_time +" / " + current_try_time);
				Mark_Path();
				Save(false);
				current_try_time ++;
                Init();
			}
			else
			{
				// get the target
				float dist = (see_target.transform.position - gameObject.transform.position).magnitude;
				float angle = Vector3.Angle (gameObject.transform.forward, 
				               (see_target.transform.position - gameObject.transform.position).normalized);
				if (dist <= radius && angle <= rot)
				{
					Mark_Path();
					Save(true);
					current_try_time ++;
					Init();
				}
				else
				{
					// find next pos
					float f = (agent.destination - gameObject.transform.position).magnitude;
                    if (f < 0.01f)
                    {
                        Mark_Path();
                        Next_Pos();
					}
				}
			}
		}

	}

	void LateUpdate()
	{

	}


	void Next_Pos()
	{
		//Debug.Log("!!!");

		Vector2 v2 = Random.insideUnitCircle * move;
		next_pos.x = v2.x ;
		next_pos.z = v2.y;
		next_pos.y = 0;
		
		next_pos += gameObject.transform.position;

		float f = Vector3.Angle(transform.forward, (next_pos - transform.position).normalized);


		while (f > (rot))
        {
			v2 = Random.insideUnitCircle * move;
			next_pos.x = v2.x ;
			next_pos.z = v2.y;
			next_pos.y = 0;

			next_pos += gameObject.transform.position;

			f = Vector3.Angle(transform.forward, (next_pos - transform.position).normalized);

		}

		next_pos.y += 0.0001f;

        agent.SetDestination(next_pos);
        
		float dist = (agent.destination - gameObject.transform.position).magnitude;
		if (dist < 0.01f)
		{
			comsume_distance += 0.01f;
		}
		else
		{
			comsume_distance += dist;
		}
        
        // fast move
		if (fast_move)
		{
			gameObject.transform.position = agent.destination;
		}
	}

	void Mark_Path()
	{
		if (!isDone)
		{
			pathList.Add(gameObject.transform.position);
			pathList.Add(gameObject.transform.forward);
		}

	}

	void Save(bool ok)
	{
		if (ok)
		{
			current_OK_rate ++;
		}

//		string filePath = Application.dataPath +"/DataResults/agent/KenmoreAgent-"+current_try_time+".txt";
//
//
//		StreamWriter sw = new StreamWriter(filePath, false);
//
//
//		// ok
//		if (ok)
//		{
//			Debug.Log("Good, Dist: " + comsume_distance + " Save To: " + filePath);
//			sw.WriteLine("Good");
//		}
//		else
//		{
//			Debug.Log("No,   Dist: " + comsume_distance + " Save To: " + filePath);
//			sw.WriteLine("NO");
//		}
//
//		// comsume_distance
//		sw.WriteLine(comsume_distance);
//		// lenght
//		sw.WriteLine(pathList.Count);
//
//		// postion and forwad
//		for(int i=0; i<pathList.Count; i+=2)
//		{
//			string s = pathList[i].x + "," + pathList[i].y + "," + pathList[i].z + ","+
//				pathList[i+1].x + "," + pathList[i+1].y + "," + pathList[i+1].z;
//			sw.WriteLine(s);
//		}
//
//		sw.Close();

	}

}
