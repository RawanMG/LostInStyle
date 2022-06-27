using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;

// ****************************************************************************************************************
// base class
// ****************************************************************************************************************
public class DataBase : ScriptableObject {

	// tips message;
	public string tips = "";

	// player Object
	Transform user;
	Transform eye;
	Transform gaze_marker;

	FoveInterface.EyeRays rays;
	FoveInterface.GazeConvergenceData gazeConv;
	Matrix4x4 mat;

	List<string> list;
	StreamWriter sw;


	float consumeTime = 0.0f;


	// Use this for initialization
	virtual public void StartFunc (string filePath) 
	{
		list = new List<string>();	
		sw = new StreamWriter(filePath, false);
		// find User Object;
		user = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform;
//		gaze_marker = GameObject.FindGameObjectWithTag ("gazeMarker").transform;



		// Fove stuff
//		headset = Fove.FoveHeadset.GetHeadset ();
//		mat = (headset.GetProjectionMatrixRH(Fove.EFVR_Eye.Both, 0, 100));
//		bool connected = headset.IsHardwareConnected();
//		Debug.Log ("Headset connected= " + connected);
//		if (headset == null || !connected)
//			return;

		//Fove 2.0
//		FoveInterface2 fint = FindObjectOfType<FoveInterface2>();
//		rays = fint.GetGazeRays ();

//		rays = FoveInterface.GetEyeRays();


	}

	virtual public void StartFunc (string filePath, GameObject goArrow, GameObject goUser)
	{
		list = new List<string>();	
		sw = new StreamWriter(filePath, false);
		//Debug.Log("Need to override.");
	}
	
	// Update is called once per frame
	virtual public void FixedUpdateFunc () 
	{
//		rays = FoveInterface.GetEyeRays ();
//		gazeConv = FoveInterface.GetGazeConvergence ();
		//info format(all in one row): userpos.x,userpos.y,userpos.z,
		//userforward.x,userforward.y,userforward.z,
		//headsetpos.x,headsetpos.y,headsetpos,z
		//hitpoint.x,hitpoint.y,hitpoint.z
		//gazeRay.x,gazeRay.y,gazeRay.z

		consumeTime += Time.fixedDeltaTime;
		//headset position 

//		if (headset == null || !headset.IsHardwareConnected())
//			return;
		
//		Fove.SFVR_Pose pose = headset.GetHMDPose(); 

		// record the position and direction
		string info = user.position.x + "," + user.position.y + "," + user.position.z + ","
			+ user.forward.x + "," + user.forward.y + "," + user.forward.z + ","
			+ gazeConv.ray.direction.x + "," + gazeConv.ray.direction.y + "," + gazeConv.ray.direction.z;
		
//		Debug.DrawRay (gazeConv.ray.origin, 10*gazeConv.ray.direction, Color.red, 10);
//		Debug.DrawRay (rays.left.origin, 10*rays.left.direction, Color.blue, 10);
//		//TODO: try GazeVector
//		RaycastHit hit;
//		if (Physics.Raycast (rays.left.origin, rays.left.direction, out hit, Mathf.Infinity)) {
//			info += ",1"; //there's a hit
//			info += "," + hit.point.x + "," + hit.point.y + "," + hit.point.z;
//
//			gaze_marker.position = hit.point;
//
//		}
		sw.WriteLine (info);
	}

	virtual public void Done(string filePath)
	{
		// consume time.
		// lenght
		sw.WriteLine(consumeTime);
		sw.WriteLine(list.Count);

		// start portal
		// destination
		MissionSystem mSys = GameObject.FindObjectOfType<MissionSystem>();
		//string startP = mSys.Get_Current_Mission().start_portal.ToString();
		string startP = mSys.Get_Current_Mission().starting_point.ToString();
		if (startP.Equals (""))
			startP = "Freestyle";
		sw.WriteLine(startP);
		sw.WriteLine(mSys.Get_Current_Mission().destination.ToString());

		sw.Close();

		Debug.Log("File Saved : " + filePath);	
	}
}
