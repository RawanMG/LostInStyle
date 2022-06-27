using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class FindPath : MonoBehaviour {
    //Transform goal;
    Vector3 goal;
	public Color c1 = Color.green;
	public Color c2 = Color.blue;
	LineRenderer l;

	float elapsedTimeShowing = 0;
	public float maxTimeShowing = 2; // number of seconds to display path
	public float minTimeBetween = 2; // minimum time allowed in between displaying the path
	float elapsedTimeBetween;

	// main UI
	MainUI mainUI;

	// Mission system
	MissionSystem missionSys;
    VRMainSystem vrms;

	void Start () {
		l = new GameObject ().AddComponent<LineRenderer> ();

        //set goal as mission goal
 //       goal.position = getDestinationPos();
        goal = getDestinationPos();
        //to display time until you can view path
        mainUI = FindObjectOfType<MainUI>();
		elapsedTimeBetween = minTimeBetween;
		missionSys = FindObjectOfType<MissionSystem>();
        vrms = FindObjectOfType<VRMainSystem>();
        //l.enabled = false;
        //elapsedTimeBetween = 0.0f;
        //string min_str = string.Format ("{0}:{1:00}", (int)minTimeBetween / 60, (int)minTimeBetween % 60);
        //mainUI.SetTips (min_str);
        init();
	}
    public void init()
    {
        if (l != null)
            l.enabled = false;
        elapsedTimeBetween = 0.0f;
        string min_str = string.Format("{0}:{1:00}", (int)minTimeBetween / 60, (int)minTimeBetween % 60);
        //if(mainUI!=null)
        mainUI = FindObjectOfType<MainUI>();
        if (vrms != null && vrms.playMode == GData.PlayMode.ValidationExp)
            mainUI.SetTips("");
        else
            mainUI.SetTips(min_str);
    }
    public void HidePath()
    {
        if (l != null)
            l.enabled = false;
    }
    void Update(){
		if (Time.deltaTime % 1000 == 0) {
			
			//showing the path
			missionSys = FindObjectOfType<MissionSystem> ();
			if (missionSys == null)
				return;
			if (missionSys.IsCompleted ())
				return;
			if (vrms != null && vrms.playMode == GData.PlayMode.ValidationExp)
				return;
			if (l.enabled) {
				if (missionSys.Get_Current_Mission ().bTaskGoAndReturn && missionSys.Get_Current_Mission ().bGo && GData.playMode == GData.PlayMode.ValidationExp)
					return;

				//elapsed time showing the path above max time allowed to show the path
				if (elapsedTimeShowing >= maxTimeShowing) {
					l.enabled = false;
					elapsedTimeBetween = 0.0f;
					return;
				}
				elapsedTimeShowing += Time.deltaTime;
			} else {
				elapsedTimeBetween += Time.deltaTime;
				float time = Mathf.Max (minTimeBetween - elapsedTimeBetween, 0.0f); 
				string min_str = string.Format ("{0}:{1:00}", (int)time / 60, (int)time % 60);
				//mainUI.SetTips (min_str);
			}
		}
	}
	// shows the path if allowed o.w. returns false
	public bool getPath () {
		bool done = false;
		if (!l.enabled) {
			if (elapsedTimeBetween >= minTimeBetween) {
				showPath ();
				elapsedTimeShowing = 0.0f;
				done = true;
			}
		} else {
			//path already been shown
			done = true;
		}

		return done;
	}

	//this function displays the path 
	public void showPath(){
        /*		 
                NavMeshPath path = new NavMeshPath();
                Vector3 user_pos = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform.position;
                goal.position = getDestinationPos();
                Vector3 goal_pos = goal.position;
                user_pos.y += 1.5f;
                goal_pos.y += 1.5f;
        //		Debug.Log ("user " + user_pos);
        //		Debug.Log ("dest " + goal_pos);
                NavMesh.CalculatePath (user_pos, goal.position, -1, path);
        //		Debug.Log(" agent.CalculatePath = " + agent.CalculatePath (goal.position, path));
                //path = agent.path;

                int numPoints = path.corners.Length;
                */
        Vector3[] corners = GetPathCorners();
        int numPoints = corners.Length;
        var points = new Vector3[numPoints];

        for (int i = 0; i < numPoints; i++) {
//            Vector3 currentCorner = path.corners[i];
            Vector3 currentCorner = corners[i];
            points[i] = new Vector3 (currentCorner.x, currentCorner.y+1.0f, currentCorner.z);

		}
		//		LineRenderer l = new GameObject().AddComponent<LineRenderer> ();
		l.material = new Material (Shader.Find ("Particles/Additive"));
		l.material = new Material (Shader.Find ("Sprites/Default"));
		l.widthMultiplier = 0.5f;
		//lineRenderer.positionCount = lengthOfLineRenderer;

		// A simple 2 color gradient with a fixed alpha of 1.0f.
		float alpha = 1.0f;
		Gradient gradient = new Gradient ();
		gradient.SetKeys (
			new GradientColorKey[] { new GradientColorKey (c1, 0.0f), new GradientColorKey (c2, 1.0f) },
			new GradientAlphaKey[] { new GradientAlphaKey (alpha, 0.0f), new GradientAlphaKey (alpha, 1.0f) }
		);
		l.colorGradient = gradient;
		l.SetVertexCount (numPoints);
		l.SetPositions (points);
		l.enabled = true;
	}
    public Vector3[] GetPathCorners()
    {
        NavMeshPath path = new NavMeshPath();
        Vector3 user_pos = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform.position;
 //       goal.position = getDestinationPos();
        goal = getDestinationPos();
        Vector3 goal_pos = goal;
        user_pos.y += 1.5f;
        goal_pos.y += 1.5f;
        //		Debug.Log ("user " + user_pos);
        //		Debug.Log ("dest " + goal_pos);
        NavMesh.CalculatePath(user_pos, goal, -1, path);
        return path.corners;
    }

	//returns the location of the Portal_sensor that's been set as the mission goal
	public Vector3 getDestinationPos(){
		Vector3 port_pos = Vector3.zero;
		MissionSystem missionSys = GameObject.FindObjectOfType<MissionSystem>();
		Mission curr_mission = missionSys.Get_Current_Mission ();

		PortalSystem sys = GameObject.FindObjectOfType<PortalSystem>();
		Dictionary<string, Portal_Sensor> portals = sys.Get_Protals();
		//search for the sensor postion
		foreach (KeyValuePair<string, Portal_Sensor> portal in portals) {
			Portal_Sensor sensor = portal.Value;
            //found the correct sensor, get its location
            if (curr_mission.bTaskGoAndReturn == false)
            {
                if (sensor.locations.Contains(curr_mission.destination))
                {
                    port_pos = sensor.transform.position;
                    PortalOrigin po = sensor.GetComponentInChildren<PortalOrigin>();
                    if (po != null)
                    {
                        port_pos = po.transform.position;
                    }
                    break;
                }
            }
            else
            {
                GData.LocationType dest;
                if(curr_mission.bGo)
                {
                    dest = curr_mission.destination;
                }
                else
                {
                    dest = curr_mission.startpoint;
                }
                if (sensor.locations.Contains(dest))
                {
                    Debug.Log("Dest : " + dest.ToString());
                    port_pos = sensor.transform.position;
                    PortalOrigin po = sensor.GetComponentInChildren<PortalOrigin>();
                    if (po != null)
                    {
                        port_pos = po.transform.position;
                    }
                    break;
                }
            }

				
		}

		return port_pos;
	}
}