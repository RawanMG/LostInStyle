using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

// ****************************************************************************************************************
// read the user path and replay it
// ****************************************************************************************************************

/*
 * File format
 * consume time.
 * lenght
 * start portal
 * destination
 * position, direction, headset's position, gaze ray direction, hit(0 or 1) {<, hitpoint>}  // each line
 * */

public class DataReplayer : DataBase 
{
	Transform user;
	//Transform eye;

	FoveInterface.GazeConvergenceData gazeConv;
	Fove.Managed.FVRHeadset headset;

	Transform gaze; 
	Transform head;

	Queue<Vector3> quene;
	
	Vector3 nextPos, nextDir, nextEyeDir;
	float speed_pos, speed_dir, speed_eyeDir;

	Transform gaze_marker; 

	// Use this for initialization
	override public void StartFunc (string filePath) 
	{
		// find User Object;
		user = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform;
		//eye = user.gameObject.GetComponentInChildren<Eyes>().transform;

//		gazeConv = FoveInterface.GetGazeConvergence ();
//		headset = Fove.FoveHeadset.GetHeadset ();
		gaze = (GameObject.FindObjectOfType<FoveEyeCamera>() as FoveEyeCamera).transform;
		head = (GameObject.FindObjectOfType<Head> () as Head).transform;
		gaze_marker = GameObject.FindGameObjectWithTag ("gazeMarker").transform;
		quene = new Queue<Vector3>();

		// load the data from file
		StreamReader sr = new StreamReader(filePath);



		string line = "";
		string[] info;
		// position, direction, headset position, gaze direction, hit(0,1) <, hitpoint>} // each line, the hitpoint’s normal =-direction
		while(!sr.EndOfStream)
		{
			// read line by line
			line = sr.ReadLine().Trim();
			info = line.Split(',');
			//if we are reading file information (consume time, length, start portal, end portal) 
			if (info.Length <= 1) {
				break;
			}
			// the first 3 data are elements of user position
			quene.Enqueue(
				new Vector3(float.Parse(info[0]),
			            float.Parse(info[1]),
			            float.Parse(info[2])));
			// the next 3 data are elements of user direction
			quene.Enqueue(
				new Vector3(float.Parse(info[3]),
			            float.Parse(info[4]),
			            float.Parse(info[5])));

			// the next 3 data are elements of headset position
			quene.Enqueue(
				new Vector3(float.Parse(info[6]),
			            float.Parse(info[7]),
                        float.Parse(info[8])));

			// the next 3 data are elements of gaze direction
			quene.Enqueue(
				new Vector3(float.Parse(info[9]),
			            float.Parse(info[10]),
			            float.Parse(info[11])));
		}

		if (line.Length >0) {
			// consume time.
			tips += ("        Comsume time: " + line + "\n");

			// lenght
			if (!sr.EndOfStream) {
				sr.ReadLine ();
				// tips += ("Comsume time: " +sr.ReadLine());
			}

			// start portal
			if (!sr.EndOfStream) {
				tips += ("        Start portal: " + sr.ReadLine () + "\n");
			} 


			// destination
			if (!sr.EndOfStream) {
				string dest = sr.ReadLine ();
				dest = dest.Replace ("_", " ");
				tips += ("        Destination: " + dest + "\n");
			}
		}
		sr.Close();

		// get the next set
		GetNext();
		user.position = nextPos;
		user.forward = nextDir;
		gaze.forward = nextEyeDir;
	}
	
	// Update is called once per frame
	override public void FixedUpdateFunc () 
	{
		GetNext();
		user.position = Vector3.Lerp(user.position,nextPos, speed_pos);
		user.forward = Vector3.Lerp(user.forward,nextDir, speed_dir);
		gaze.forward = Vector3.Lerp(gaze.forward,nextEyeDir, speed_eyeDir);
		Debug.Log (gaze.forward);
		Debug.DrawRay (user.position, user.forward, Color.red, 10);
		RaycastHit hit; 
		if (Physics.Raycast (user.position, user.forward, out hit)) {
			gaze_marker.transform.position = hit.point;
		}
	}
	
	override public void Done(string filePath)
	{

	}

	void GetNext()
	{	
		if (quene.Count > 0)
		{
			// both position and direction
			nextPos = quene.Dequeue(); //user pos
			nextDir = quene.Dequeue(); //user forward
			quene.Dequeue();
//			head.position = quene.Dequeue(); //headset pos
			Debug.Log(head.position);
			nextEyeDir = quene.Dequeue(); //gaze forward
			
			// and calculate the speed, for smooth moving
			speed_pos = Math.Abs((nextPos - user.position).magnitude) / Time.fixedDeltaTime;
			speed_dir = Vector3.Angle(user.forward, nextDir) / Time.fixedDeltaTime;
			speed_eyeDir = Vector3.Angle(gaze.forward, nextEyeDir) / Time.fixedDeltaTime;
				
		}
	}
}
