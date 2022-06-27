using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

// ****************************************************************************************************************
// collect the user path and save to a file
// ****************************************************************************************************************

/*
 * File format
 * consume time.
 * lenght
 * start portal
 * destination
 * position, direction, headset's position, gaze ray direction, hit(0 or 1) {<, hitpoint>} // each line
 * */

public class DataCollecter : DataBase 
{

	// player Object
	Transform user;
	//Transform eye;

	//FoveInterface.EyeRays rays;
	FoveInterface.GazeConvergenceData gazeConv;
	Fove.Managed.FVRHeadset headset;
    
	List<string> list;

	float consumeTime = 0.0f;

	// Use this for initialization
	override public void StartFunc (string filePath) 
	{
	
		// find User Object;
		user = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform;
		//eye = user.gameObject.GetComponentInChildren<Eyes>().transform;

		//rays = FoveInterface.GetEyeRays ();
		gazeConv = FoveInterface.GetGazeConvergence ();
		headset = FoveInterface.GetFVRHeadset ();
		list = new List<string>();

	}
	
	// Update is called once per frame
	override public void FixedUpdateFunc () 
	{
		//info format(all in one row): 
		//userpos.x,userpos.y,userpos.z,
		//userforward.x,userforward.y,userforward.z,
		//headsetpos.x,headsetpos.y,headsetpos,z
		//hitpoint.x,hitpoint.y,hitpoint.z
		//gazeRay.x,gazeRay.y,gazeRay.z

		consumeTime += Time.fixedDeltaTime;
		//headset position 
		 Fove.Managed.SFVR_Pose pose = headset.GetHMDPose(); 

		// record the position and direction
		string info = user.position.x + "," + user.position.y + "," + user.position.z + ","
		              + user.forward.x + "," + user.forward.y + "," + user.forward.z + ","
		              + pose.position.x + "," + pose.position.y + "," + pose.position.z + ","
			+ gazeConv.ray.direction.x + "," + gazeConv.ray.direction.y + "," + gazeConv.ray.direction.z;

		//TODO: try GazeVector
		RaycastHit hit;
		if (Physics.Raycast (gazeConv.ray, out hit, Mathf.Infinity)) {
			info += ",1"; //there's a hit
			info += "," + hit.point.x + "," + hit.point.y + "," + hit.point.z;

		}
		/**
		// record the hitpoint right eye
		RaycastHit hit;
		if (Physics.Raycast(rays.right, out hit, Mathf.Infinity))
		{
			info += ",1"; //look at target
			info += "," + hit.point.x + "," + hit.point.y + "," + hit.point.z;
			Vector3 normal = -eye.forward.normalized;
			info += "," + normal.x + "," + normal.y + "," + normal.z;
			
		}
		else
		{
			info += ",0,0,0,0";
		}

		// left eye
		if (Physics.Raycast (rays.left, out hit, Mathf.Infinity)) {
			info += ",1";
			info += "," + hit.point.x + "," + hit.point.y + "," + hit.point.z;
			Vector3 normal = -eye.forward.normalized;
			info += "," + normal.x + "," + normal.y + "," + normal.z;
		} else {
			info += ",0,0,0,0";
		}
		**/
		
		list.Add(info);
	}


	// ***************
	// save the data to file
	// ***************
	override public void Done(string filePath)
	{
		/*
		 * File format
		 * consume time.
		 * lenght
		 * start portal
		 * destination
		 * position, direction, headset's position, gaze ray direction, hit(0 or 1) {<, hitpoint>}  // each line
		 * */
		StreamWriter sw = new StreamWriter(filePath, false);



				
		// position, direction, headset's position, gaze ray direction, hit(0 or 1) {<, hitpoint>}  // each line
		foreach(string info in list)
		{
			sw.WriteLine(info);
		}

		// consume time.
		// lenght
		sw.WriteLine(consumeTime);
		sw.WriteLine(list.Count);

		// start portal
		// destination
		MissionSystem mSys = GameObject.FindObjectOfType<MissionSystem>();
		//string startP = mSys.Get_Current_Mission().start_portal.ToString();
		string startP = mSys.Get_Current_Mission().starting_point.ToString();
		if (startP.StartsWith("Train #1"))
		{
			startP = "Train #1";
		}
		if (startP.StartsWith("Train #2"))
		{
			startP = "Train #2";
		}
		if (startP.StartsWith("Train #3"))
		{
			startP = "Train #3";
		}
		if (startP.StartsWith("Train #4"))
		{
			startP = "Train #4";
		}
		sw.WriteLine(startP);
		sw.WriteLine(mSys.Get_Current_Mission().destination.ToString());

		sw.Close();

		Debug.Log("File Saved : " + filePath);

	}


}
