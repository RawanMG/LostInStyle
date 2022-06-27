using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class Arrow : MonoBehaviour {
	Vector3 goal;
	public bool enabled = true;
    public bool activated = false;
	float elapsedTime = 0.0f;
	// main UI
	MainUI mainUI;

	// Mission system
	MissionSystem missionSys;
	// Use this for initialization
	void Start () {
		//set goal as mission goal
		goal = getDestinationPos ();
		//to display time until you can view path
		missionSys = FindObjectOfType<MissionSystem>();
	}
	
	// Update is called once per frame
	void Update () {
		elapsedTime += Time.deltaTime;
	//	Debug.Log ("elapsedTime = " + Mathf.Ceil(elapsedTime));
		if(Mathf.Ceil(elapsedTime) % 2==0)
			showPath ();

	}
	public void init()
	{

		//if(mainUI!=null)
	//	mainUI = FindObjectOfType<MainUI>();
	//	mainUI.SetTips("");
        GetComponentInChildren<MeshRenderer>().enabled = false;
        activated = false;
	}
    public bool isShown()
    {
        return activated;
    }

    public void ShowArrow(bool show)
    {
        activated = show;
        if(show==false)
            GetComponentInChildren<MeshRenderer>().enabled = false;
    }

    void showPath(){
        if (activated == false)
            return;

		NavMeshPath path = new NavMeshPath();
		Vector3 user_pos = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform.position;
		goal= getDestinationPos();
		Vector3 goal_pos = goal;
		user_pos.y += 1.5f;
		goal_pos.y += 1.5f;
		//		Debug.Log ("user " + user_pos);
		//		Debug.Log ("dest " + goal_pos);
		NavMesh.CalculatePath (user_pos, goal, -1, path);
		//		Debug.Log(" agent.CalculatePath = " + agent.CalculatePath (goal.position, path));
		//path = agent.path;
		if (path.corners.Length >= 2) {
			int numPoints = 2;
			var points = new Vector3[numPoints];

			for (int i = 0; i < numPoints; i++) {
				Vector3 currentCorner = path.corners [i];
				points [i] = new Vector3 (currentCorner.x, currentCorner.y + 1.0f, currentCorner.z);

			}
			Vector3 dir = points [numPoints - 1] - points [0]; 
			Vector3 newDir = Vector3.RotateTowards (transform.forward, dir, 2, 0.0f);
			Debug.DrawRay (transform.position, newDir, Color.red, 2);
			transform.rotation = Quaternion.LookRotation (newDir);
		}
        GetComponentInChildren<MeshRenderer>().enabled = true;

		for (int i = 0; i < path.corners.Length - 1; i++) {
			Debug.DrawLine (path.corners[i],path.corners[i+1],Color.yellow);
		}


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
				//	Debug.Log("Dest : " + dest.ToString());
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
