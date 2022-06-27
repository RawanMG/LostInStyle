using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using System.IO;
using UnityEngine.AI;

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

public class DataCollecter2 : DataBase 
{

	// player Object
	Transform user;
    //Transform eye;

    //FoveInterface.EyeRays rays;
    //FoveInterface.GazeConvergenceData gazeConv;
    Fove.Managed.SFVR_GazeConvergenceData gazeConv;
	Fove.Managed.FVRHeadset headset;
    
	List<string> list;

	float consumeTime = 0.0f;
    Transform trFove;

	LineRenderer l;

    // for debug
    GameObject sphere;

	Arrow arrow;
    MiniMap miniMap;

	// Use this for initialization
	override public void StartFunc (string filePath) 
	{
	
		// find User Object;
		user = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform;
        //eye = user.gameObject.GetComponentInChildren<Eyes>().transform;

        //rays = FoveInterface.GetEyeRays ();
        //gazeConv = FoveInterface.GetGazeConvergence ();
		list = new List<string>();

		l = FindObjectOfType<LineRenderer> () as LineRenderer;

		arrow = GameObject.FindObjectOfType<Arrow> ();
        miniMap = GameObject.FindObjectOfType<MiniMap>();

	}



    Vector3 Conv(Fove.Managed.SFVR_Vec3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

	public string getCameraData(){
		string info = "";
		Transform cameraTransform = Camera.main.transform;

		// record the user position and direction
		info += user.position.x + "," + user.position.y + "," + user.position.z + ","
			+ user.forward.x + "," + user.forward.y + "," + user.forward.z + ",";

		// camera postion
		info += cameraTransform.position.x + "," + cameraTransform.position.y + "," + cameraTransform.position.z + ",";
		RaycastHit hit;
		if (Physics.Raycast (cameraTransform.position, cameraTransform.forward, out hit, Mathf.Infinity)) {
			info += "1,";
			//record hit postion 
			info += hit.point.x + "," + hit.point.y + "," + hit.point.z + ",";
			// record gaze direction + postion
			info += cameraTransform.forward.x + "," + cameraTransform.forward.y + "," + cameraTransform.forward.z + ",";
			info += cameraTransform.up.x + "," + cameraTransform.up.y + "," + cameraTransform.up.z + ",";
			info += cameraTransform.right.x + "," + cameraTransform.right.y + "," + cameraTransform.right.z;
			Debug.DrawRay (cameraTransform.position, -1 * cameraTransform.forward);

		} else {
			info += "0,0,0,0,"; //hitpoint info
			// record gaze direction + postion
			info += cameraTransform.forward.x + "," + cameraTransform.forward.y + "," + cameraTransform.forward.z + ",";
			info += cameraTransform.up.x + "," + cameraTransform.up.y + "," + cameraTransform.up.z + ",";
			info += cameraTransform.right.x + "," + cameraTransform.right.y + "," + cameraTransform.right.z;
			//info += "0,0,0,0,0,0,0";
		}
		return info;	


	}
	// Update is called once per frame
	override public void FixedUpdateFunc () 
	{
		bool isAidShown =  (l!=null && l.enabled) || (arrow!=null && arrow.isShown()) || (miniMap != null && miniMap.isShown());
		string info = "";
		//if not using the Fove
		if (trFove==null && FindObjectOfType<FoveInterface2>()==null) {
			info = getCameraData ();
		} else {
		
			if (trFove == null) {
				trFove = FindObjectOfType<FoveInterface2> ().transform;
			}

			consumeTime += Time.fixedDeltaTime;
		
			//headset position 
		    headset = FoveInterface2.GetFVRHeadset ();
			Fove.Managed.SFVR_Pose pose = headset.GetHMDPose (); 
        
			//update gaze data
			gazeConv = headset.GetGazeConvergence ();
//			Debug.Log("Camera pixel height = " + Camera.main.pixelHeight + "\t width = " + Camera.main.pixelWidth);
//			Debug.Log("Screen height = " + Screen.height + "\t width = " + Screen.width);
			// record the position and direction
			info = user.position.x + "," + user.position.y + "," + user.position.z + ","
			             + user.forward.x + "," + user.forward.y + "," + user.forward.z + ","
			             + pose.position.x + "," + pose.position.y + "," + pose.position.z + ","
			             + gazeConv.ray.direction.x + "," + gazeConv.ray.direction.y + "," + gazeConv.ray.direction.z;

			//TODO: try GazeVector
			RaycastHit hit;
			if (trFove != null) {
                // Record convergence
                Vector3 gazeDirection = Conv(gazeConv.ray.direction);

                Physics.Raycast (trFove.position, trFove.rotation * gazeDirection, out hit, Mathf.Infinity);
				info += "," + trFove.position.x + "," + trFove.position.y + "," + trFove.position.z; // gaze origin
				if (hit.point != Vector3.zero) {
					info += ",1"; //look at target
                    info += "," + hit.point.x + "," + hit.point.y + "," + hit.point.z;
					Vector3 normal = (trFove.rotation * gazeDirection).normalized;
                    Vector3 head_normal = trFove.forward.normalized;
					//Debug.DrawRay (trFove.position, -1 * trFove.right.normalized, Color.blue); 
					info += "," + normal.x + "," + normal.y + "," + normal.z;
                    info += "," + head_normal.x + "," + head_normal.y + "," + head_normal.z;
					Vector3 up_normal = (trFove.up).normalized;
					info += "," + up_normal.x + "," + up_normal.y + "," + up_normal.z; 
					Vector3 right_normal = (trFove.right).normalized; 
					info += "," + right_normal.x + "," + right_normal.y + "," + right_normal.z; 
				} else {
					info += ",0,0,0,0";
					Vector3 normal = (trFove.rotation * gazeDirection).normalized;
					Vector3 head_normal = trFove.forward.normalized;
					//Debug.DrawRay (trFove.position, -1 * trFove.right.normalized, Color.blue); 
					info += "," + normal.x + "," + normal.y + "," + normal.z;
					info += "," + head_normal.x + "," + head_normal.y + "," + head_normal.z;
					Vector3 up_normal = (trFove.up).normalized;
					info += "," + up_normal.x + "," + up_normal.y + "," + up_normal.z; 
					Vector3 right_normal = (trFove.right).normalized; 
					info += "," + right_normal.x + "," + right_normal.y + "," + right_normal.z; 
				}
                //debug
 //               ShowSphere(hit.point);
 //               Debug.DrawRay(trFove.position, hit.point - trFove.position, Color.green);
//                Debug.DrawRay(trFove.position, (trFove.forward).normalized, Color.red);
                // Record left eye
                /*
                if (false) {
					Fove.Managed.SFVR_GazeVector gv = FoveInterface2.GetFVRHeadset ().GetGazeVector (Fove.Managed.EFVR_Eye.Left);
					Physics.Raycast (trFove.position, trFove.rotation * Conv (gv.vector), out hit, Mathf.Infinity);
					if (hit.point != Vector3.zero) {
						info += ",1"; //look at target
						info += "," + hit.point.x + "," + hit.point.y + "," + hit.point.z;
						Vector3 normal = (trFove.rotation * Conv (gv.vector)).normalized;
						info += "," + normal.x + "," + normal.y + "," + normal.z;
					} else {
						info += ",0,0,0,0,0,0,0";
					}
				}
				// Record right eye
				if (false) {
					Fove.Managed.SFVR_GazeVector gv = FoveInterface2.GetFVRHeadset ().GetGazeVector (Fove.Managed.EFVR_Eye.Right);
					Physics.Raycast (trFove.position, trFove.rotation * Conv (gv.vector), out hit, Mathf.Infinity);
					if (hit.point != Vector3.zero) {
						info += ",1"; //look at target
						info += "," + hit.point.x + "," + hit.point.y + "," + hit.point.z;
						Vector3 normal = (trFove.rotation * Conv (gv.vector)).normalized;
						info += "," + normal.x + "," + normal.y + "," + normal.z;
					} else {
						info += ",0,0,0,0,0,0,0";
					}
                    
				}
                */
			}
		}//end else ( headset is connected)

		info += "," + isAidShown;

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
        if (mSys.Get_Current_Mission().bTaskGoAndReturn)
        {
            Portal_Sensor ps = mSys.GetPortal(startP);
            Vector3 p = GetPortalPosition(ps);
            startP += "," + p.x + "," + p.y + "," + p.z;
            sw.WriteLine(startP);

            ps = mSys.GetPortal(mSys.Get_Current_Mission().destination);
            p = GetPortalPosition(ps);
            string endP = mSys.Get_Current_Mission().destination.ToString() + "," + p.x + "," + p.y + "," + p.z;
            sw.WriteLine(endP);

            NavMeshObstacle[] nmos = GameObject.FindObjectsOfType<NavMeshObstacle>();
            foreach(NavMeshObstacle nmo in nmos)
            {
                if (nmo.carving)
                {
                    string line = nmo.transform.parent.name + "," + nmo.transform.position.x + "," + nmo.transform.position.y + "," + nmo.transform.position.z;
                    sw.WriteLine(line);
                }
            }
            sw.WriteLine("1st Aid: " + mSys.Get_Current_Mission().aid1st);
            sw.WriteLine("2nd Aid: " + mSys.Get_Current_Mission().aid2nd);
            sw.WriteLine("TrialNumber: " + mSys.Get_Current_Mission().trial_nb);
            sw.WriteLine("Times of hit-by-car: " + mSys.Get_Current_Mission().num_hit_by_car);
            sw.WriteLine("Lentgh the optimal path: " + mSys.Get_Current_Mission().distance);
            sw.WriteLine(DateTime.Now.ToLongDateString() + " " + DateTime.Now.ToLongTimeString());

        }
        else
        {
            sw.WriteLine(startP);
            sw.WriteLine(mSys.Get_Current_Mission().destination.ToString());
        }
        sw.Close();

		Debug.Log("File Saved : " + filePath);

	}
    Vector3 GetPortalPosition(Portal_Sensor ps)
    {
        PortalOrigin po = ps.GetComponentInChildren<PortalOrigin>();
        Vector3 p;
        if (po != null)
            p = po.transform.position;
        else
            p = ps.transform.position;
        return p;
    }

    void ShowSphere(Vector3 p)
    {
        if(sphere==null)
        {
            sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.transform.parent = trFove.parent.parent;
            sphere.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Material mat = new Material(Shader.Find("Specular"));
            mat.color = Color.red;
            sphere.GetComponent<Renderer>().material = mat;
            sphere.layer = LayerMask.NameToLayer("Ignore Raycast");
        }

        sphere.transform.position = p;
    }
}
