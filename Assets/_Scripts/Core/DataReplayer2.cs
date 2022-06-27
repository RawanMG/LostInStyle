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

public class DataReplayer2 : DataBase 
{
	Transform user;
    Transform trFove;
    GameObject sphere_gaze;
    GameObject sphere_center;
    GameObject line;
    //Transform eye;

    //	FoveInterface.GazeConvergenceData gazeConv;
    //	Fove.Managed.FVRHeadset headset;


    Queue<Vector3> quene;
    GameObject barrier;
    Portal_Sensor startPS;
    Portal_Sensor destPS;

    List<Vector3> listPos;
    List<List<Vector3>> listPressAlist;
    List<Vector3> listPressA;
    List<LineRenderer> listTracesPressA;

    LineRenderer traces;
    GameObject stMarker;
    GameObject edMarker;
    GameObject barrMarker;
	
	Vector3 nextPos, nextDir, nextEyeDir, nextFovePos, nextFoveDir, nextGazePos, nextGazeDir, nextGazeFlag, nextShowGuide;
	float speed_pos, speed_dir, speed_eyeDir;

	// Use this for initialization
	override public void StartFunc (string filePath) 
	{
		// find User Object;
		user = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform;
        //        trFove = FindObjectOfType<FoveInterface2>().transform;
        trFove = user;
		quene = new Queue<Vector3>();
        nextShowGuide = Vector3.zero ;

        // traces
        listPos = new List<Vector3>();
        listPressAlist = new List<List<Vector3>>();
        listPressA = null;
        listTracesPressA = new List<LineRenderer>();
 //       listPressA = new List<Vector3>();

        //
        RemoveTraceMarker();
        GameObject l=new GameObject("Trace");
        l.AddComponent<TraceMarker>();
        traces = l.AddComponent<LineRenderer>();
        traces.material = new Material(Shader.Find("Standard"));
        Color c = Color.magenta;
        traces.material.color = c;
        traces.startWidth = 10f;
        traces.endWidth = 10f;
        traces.gameObject.layer = 4;

        Debug.Log("Loading: " + filePath);
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
            // FOVE position in World coordinates
            quene.Enqueue(
                new Vector3(float.Parse(info[12]),
                            float.Parse(info[13]),
                             float.Parse(info[14])));
            // flag: gaze data available
            quene.Enqueue(
                new Vector3(float.Parse(info[15]),
                            float.Parse(info[15]),
                             float.Parse(info[15])));
            // Gaze Pisition in World coordinates
            quene.Enqueue(
                new Vector3(float.Parse(info[16]),
                            float.Parse(info[17]),
                             float.Parse(info[18])));
            // Gaze Direction Vector (Nomal)
            quene.Enqueue(
                new Vector3(float.Parse(info[19]),
                            float.Parse(info[20]),
                             float.Parse(info[21])));
            // Head Direction Vector (Normal)
            quene.Enqueue(
                new Vector3(float.Parse(info[22]),
                            float.Parse(info[23]),
                             float.Parse(info[24])));
            // 25-20 ; up/right direction vector
            // Button press?
            float tmp = (info[31] == "False" ? 0f : 1f);
            quene.Enqueue(
                new Vector3(tmp,tmp,tmp));
        }
        Debug.Log("dbg: " + line);
        if (line.Length >0) {
			// consume time.
			tips += ("        Comsume time: " + line + "\n");

			// lenght
			if (!sr.EndOfStream) {
				sr.ReadLine ();
				// tips += ("length: " +sr.ReadLine());
			}

            // start portal
            Vector3 stPos;
			if (!sr.EndOfStream) {
//				tips += ("        Start portal: " + sr.ReadLine () + "\n");
                line = sr.ReadLine().Trim();
                info = line.Split(',');
                tips += ("        Start portal: " + info[0] + "\n");
                startPS = GameObject.Find(info[0]).GetComponent<Portal_Sensor>();
                stPos = new Vector3(float.Parse(info[1]),float.Parse(info[2]),float.Parse(info[3]));
            //    RemoveTraceMarker("StartMarker");
                stMarker = PutMarker("StartMarker", stPos, Color.red);
				MissionSystem mSys = GameObject.FindObjectOfType<MissionSystem>();
				Mission m = mSys.Get_Current_Mission();
				m.starting_point = info [0];
				m.bGo = false; 
				m.bTaskGoAndReturn = true;
	
            }


            // destination
            Vector3 dstPos;
            if (!sr.EndOfStream) {
//				string dest = sr.ReadLine ();
//				dest = dest.Replace ("_", " ");
//				tips += ("        Destination: " + dest + "\n");
                line = sr.ReadLine().Trim();
                info = line.Split(',');
                tips += ("        Destination: " + info[0] + "\n");
                destPS = GameObject.Find(info[0]).GetComponent<Portal_Sensor>();
//				MissionSystem mSys = GameObject.FindObjectOfType<MissionSystem>();
//				Mission m = mSys.Get_Current_Mission();
//				m.destination = destPS;
                dstPos = new Vector3(float.Parse(info[1]), float.Parse(info[2]), float.Parse(info[3]));
           //     RemoveTraceMarker("DestinationMarker");
                edMarker = PutMarker("DestinationMarker", dstPos, Color.blue);
            }

            //barrier
            string barrierName;
            Vector3 barPos;
            if (!sr.EndOfStream)
            {
                line = sr.ReadLine().Trim();
                info = line.Split(',');
                tips += ("        Barrier: " + info[0] + "\n");
                barrierName = info[0];
				Debug.Log ("Barrier name " + info [0]);
                barrier = GameObject.Find(info[0]);
				if (barrier != null) {
					
					MeshRenderer[] mr = barrier.GetComponentsInChildren<MeshRenderer> ();
					foreach (MeshRenderer m in mr) {
						if (m.name == "Cube")
							continue;
						m.enabled = true;
					}
					barPos = new Vector3 (float.Parse (info [1]), float.Parse (info [2]), float.Parse (info [3]));
					//      RemoveTraceMarker("BarrierMarker");
					barrMarker = PutMarker ("BarrierMarker", barPos, Color.green);
				}
	
            }
			// 1st aid 
			if (!sr.EndOfStream)
			{
				line = sr.ReadLine().Trim();

			}
			//2nd aid 
			string aid2; 
			if (!sr.EndOfStream)
			{
				line = sr.ReadLine().Trim();
				info = line.Split(':');
				tips += ("        Aid 2: " + info[1] + "\n");
				aid2 = info[1];
				Debug.Log ("Aid2 name " + info [1]);
				MissionSystem mSys = GameObject.FindObjectOfType<MissionSystem>();
				Mission m = mSys.Get_Current_Mission();
				m.aid2nd = int.Parse(info [1]);

			}
            Debug.Log(tips);

     
	

	}
		Debug.Log(tips);
        sr.Close();

        RemoveTraceMarker("GazeSphere");
        sphere_gaze = null;
        RemoveTraceMarker("GazeSphereCenter");
        sphere_center = null;
        RemoveTraceMarker("GazeDirection");
        line = null;

//        ScreensRecoder srec = (ScreensRecoder)GameObject.FindObjectOfType<ScreensRecoder>();
//        srec.UpdateBaseFilename();

        Camera[] cams = GameObject.FindObjectsOfType<Camera>();
        foreach(Camera cam in cams)
        {
            if(cam.GetComponent<ScreensRecoder>()==null)
            {
                Debug.Log("Set Cull Mask :" + cam.name);
                cam.cullingMask &= ~(1 << 4);
            }
        }

        FixedUpdateFunc();
	}

    void RemoveTraceMarker()
    {
        TraceMarker[] tms = GameObject.FindObjectsOfType<TraceMarker>();
        foreach(TraceMarker tm in tms)
        {
            GameObject go = tm.gameObject;
            LineRenderer lr = go.GetComponent<LineRenderer>();
            if (lr != null)
                Destroy(lr);
            Destroy(tm);
            Destroy(go);
        }
    }

    void RemoveTraceMarker(string name)
    {
        GameObject go = GameObject.Find(name);
        if (go != null)
        {
            LineRenderer lr = go.GetComponent<LineRenderer>();
            if (lr != null)
                Destroy(lr);
            Destroy(go);
            Debug.Log("Destroyed " + name);
        }
    }

    GameObject PutMarker(string name,Vector3 p,Color col)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        p.y += 50f;
        go.transform.position = p;
        go.transform.localScale = Vector3.one * 25f;
        Material mat = new Material(Shader.Find("Specular"));
        mat.color = col;
        go.GetComponent<Renderer>().material = mat;
        go.layer = 4;
        go.AddComponent<TraceMarker>();
        return go;
    }

    // Update is called once per frame
    override public void FixedUpdateFunc () 
	{
        bool bPrevPressA = (nextShowGuide.x==0f ? false : true);
		GetNext();
        bool bCurrPressA = (nextShowGuide.x == 0f ? false : true);
		MissionSystem mSys = GameObject.FindObjectOfType<MissionSystem>();
		Mission m = mSys.Get_Current_Mission();
		if (m.aid2nd == 4) {
			Arrow arrow = GameObject.FindObjectOfType<Arrow> ();
			arrow.ShowArrow (bCurrPressA);
		} else if (m.aid2nd == 5) {
			MiniMap map = GameObject.FindObjectOfType<MiniMap> ();
			map.ShowMiniMap (bCurrPressA);
		} else if (m.aid2nd == 1) {
			FindPath path = GameObject.FindObjectOfType<FindPath> ();
			if (bCurrPressA)
				path.showPath ();
			else
				path.HidePath ();
		}

			
        Vector3 tmp = nextFovePos;
        tmp.y = 50f;
        listPos.Add(tmp);
//        if (bPrevPressA == false && bCurrPressA == true)
//        {
//            listPressA = new List<Vector3>();
//            GameObject l = new GameObject("Trace");
//            l.AddComponent<TraceMarker>();
//            LineRenderer lr = l.AddComponent<LineRenderer>();
//            lr.material = new Material(Shader.Find("Standard"));
//            Color c = Color.cyan;
//            lr.material.color = c;
//            lr.startWidth = 10f;
//            lr.endWidth = 10f;
//            lr.gameObject.layer = 4;
//            listTracesPressA.Add(lr);
//        }
//        if (bCurrPressA)
//        {
////            tmp.y = 55f;
////            listPressA.Add(tmp);
//
//        }
//
//        if (bCurrPressA == true && bCurrPressA == false)
//        {
//            listPressAlist.Add(listPressA);
//            listPressA = null;
//        }

//        DrawTraces(listPos.ToArray());
//        DrawTracesPressA(listPressAlist);
//        if (listPressA != null)
//            DrawTracesPressA(listPressA.ToArray(),listTracesPressA[listTracesPressA.Count-1]);

 //       Vector3 p = user.localToWorldMatrix * new Vector3(0f, 1.8f,0f);
//        nextFovePos = nextPos + p;
//        user.position = nextFovePos - p;
        user.position = nextPos;
        user.forward = nextFoveDir;

		Debug.DrawRay (user.position, user.forward, Color.red, 10);
		RaycastHit hit; 

//		if (Physics.Raycast (nextFovePos, nextFoveDir, out hit)) {
//            ShowSphereCenter(hit.point, Color.magenta);
//		//	gaze_marker.transform.position = hit.point;
//		}
        if (nextGazeFlag.x != 0)
        {
            if (nextShowGuide.x >0f)
            {
				//ShowSphere(nextGazePos ,Color.red);
//                ShowSphere(nextGazePos, Color.cyan);
            }
            else
            {
                //ShowSphere(nextGazePos ,Color.red);
            }
        }
       

	}
	
	override public void Done(string filePath)
	{

	}
    void DrawTraces(Vector3[] positions)
    {
        traces.positionCount = positions.Length;
        traces.SetPositions(positions);
    }
    void DrawTracesPressA(List<List<Vector3>> lst)
    {
        if (listTracesPressA == null)
            return;

        List<Vector3>[] array = lst.ToArray();
        LineRenderer[] lr_arrray = listTracesPressA.ToArray();
        for(int i=0;i<array.Length;i++)
        {
            DrawTracesPressA(array[i].ToArray(),lr_arrray[i]);
        }
    }
    void DrawTracesPressA(Vector3[] positions,LineRenderer lr)
    {
        lr.positionCount = positions.Length;
        lr.SetPositions(positions);
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
//			Debug.Log(head.position);
			nextEyeDir = quene.Dequeue(); //gaze forward

            //
            nextFovePos = quene.Dequeue();
            nextGazeFlag = quene.Dequeue();
            nextGazePos = quene.Dequeue();
            nextGazeDir = quene.Dequeue();
            nextFoveDir = quene.Dequeue();
            nextShowGuide = quene.Dequeue();

            // and calculate the speed, for smooth moving
            //speed_pos = Math.Abs((nextPos - user.position).magnitude) / Time.fixedDeltaTime;
            //speed_dir = Vector3.Angle(user.forward, nextDir) / Time.fixedDeltaTime;
            //speed_eyeDir = Vector3.Angle(gaze.forward, nextEyeDir) / Time.fixedDeltaTime;
//            speed_pos = Math.Abs((nextFovePos - trFove.position).magnitude) / Time.fixedDeltaTime;
            speed_pos = Math.Abs((nextPos - trFove.position).magnitude) / Time.fixedDeltaTime;
            speed_dir = Vector3.Angle(trFove.forward, nextFoveDir) / Time.fixedDeltaTime;
            //           speed_eyeDir = Vector3.Angle(gaze.forward, nextEyeDir) / Time.fixedDeltaTime;


        }else
        {
            VRMainSystem vrms = GameObject.FindObjectOfType<VRMainSystem>();
            string dataFolder = Application.dataPath + "/" + GData.dataResultsPath;
            IniFile ifile = new IniFile(dataFolder + "/gdata/common.txt");
            ifile.goTo("GData");
            int nextIndex = ifile.GetValue_Int("nextIndex", 0) - 1; // last recording

            if (vrms.replayer_start_index < nextIndex)
            {
                Debug.Log("Next exp");
                CalbiScreen cs = (CalbiScreen)GameObject.FindObjectOfType<CalbiScreen>();
                cs.init();
            }
            else
            {
                Debug.Log("Finished");
            }
        }
    }
    void ShowSphere(Vector3 p,Color col)
    {
        if (sphere_gaze == null)
        {
            sphere_gaze = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere_gaze.name = "GazeSphere";
   //         sphere.transform.parent = trFove;
            sphere_gaze.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Material mat = new Material(Shader.Find("Specular"));
            mat.color = Color.red;
            sphere_gaze.GetComponent<Renderer>().material = mat;
            sphere_gaze.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        sphere_gaze.transform.position = p;
        sphere_gaze.GetComponent<Renderer>().material.color = col;

		if (false) {
			LineRenderer lr = null;
			if (line == null) {
				line = new GameObject ("GazeDirection");
				lr = line.AddComponent<LineRenderer> ();
				Material m = new Material (Shader.Find ("Standard"));
				m.SetFloat ("_Mode", 2f);
				Color c = col;
				c.a = 0.3f;
				m.color = c;
				lr.material = m;
			}

			if (lr == null)
				lr = line.GetComponent<LineRenderer> ();
			lr.material.color = col;
			lr.widthMultiplier = 0.1f;
        
			//lr.positionCount = 2;
			// A simple 2 color gradient with a fixed alpha of 1.0f
			/*
        float alpha = 0.3f;
        Gradient gradient = new Gradient();
        gradient.SetKeys(
            new GradientColorKey[] { new GradientColorKey(col, 0.0f), new GradientColorKey(col, 1.0f) },
            new GradientAlphaKey[] { new GradientAlphaKey(alpha, 0.0f), new GradientAlphaKey(alpha, 1.0f) }
        );
        lr.colorGradient = gradient;*/

			lr.positionCount = 2;
			Vector3 q = FindObjectOfType<FoveInterface2> ().transform.position;
			q.y -= 0.3f;
			lr.SetPosition (0, p);
			lr.SetPosition (1, q);
 
			lr.enabled = true;
		}


    }
    void ShowSphereCenter(Vector3 p, Color col)
    {
        if (sphere_center == null)
        {
            sphere_center = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere_center.name = "GazeSphereCenter";
            //         sphere.transform.parent = trFove;
            sphere_center.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
            Material mat = new Material(Shader.Find("Specular"));
            mat.color = Color.red;
            sphere_center.GetComponent<Renderer>().material = mat;
            sphere_center.layer = LayerMask.NameToLayer("Ignore Raycast");
        }
        sphere_center.transform.position = p;
        sphere_center.GetComponent<Renderer>().material.color = col;
    }
}
