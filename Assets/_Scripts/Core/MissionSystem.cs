using UnityEngine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using Random = UnityEngine.Random;
using System.IO;

/*******
 * choose a mission and place the player at the Awake seesion.
 * *******/


[Serializable]
public class Mission
{
    //public string start_portal;
    public bool random_start = false;
    public string starting_point = "Train #1 5";
    public GData.LocationType destination;
    public GData.LocationType startpoint;
    public bool bTaskGoAndReturn = false;
    public bool bGo = true;
    [Multiline(5)] public string comment;
    public int aid1st;
    public int aid2nd;
    public int trial_nb;
    public int[] question_ids;
    public int num_hit_by_car;
    public float distance;
}

public class MissionSystem : MonoBehaviour
{
    // static public GData.LocationType destination;
    public static bool randomly = false;

    //[SerializeField]
    //public Mission[] missions;

    List<string> startPoint;
    List<GData.LocationType> endPoint;
    GameObject flagObject;


    [SerializeField]
    public Mission current_mission;

    public Mission Get_Current_Mission()
    {
        if (current_mission == null)
        {
            Awake();
        }

        //current_mission.start_portal =  startPoint[3];
        //current_mission.start_portal =  startPoint[Random.Range(0,startPoint.Count)];
        Scene curr_scene = SceneManager.GetActiveScene();
        if (curr_scene.name == "Level4" || curr_scene.name == "Level5" || curr_scene.name == "Level5_displayGaze")
        {
            current_mission.comment = "";//"Look around. We will ask you which paintings you saw.";
            return current_mission;

        }
		if (current_mission.bTaskGoAndReturn)
			current_mission.comment = "";//"Get " + current_mission.destination.ToString() + " and return to the start point!";
        else
			current_mission.comment = "";//"Find a way to " + current_mission.destination.ToString();
        current_mission.comment = current_mission.comment.Replace("_", " ");


        return current_mission;
    }

    // mission done?
    bool isCompleted = false;
    public bool IsCompleted()
    {
        return isCompleted;
    }
    void Awake()
    {
        init();
    }
    public void init()
    {

        startPoint = new List<string>();
        endPoint = new List<GData.LocationType>();

        // find all the start point and endpoint
        Portal_Sensor[] pSensors = GameObject.FindObjectsOfType<Portal_Sensor>();
        foreach (Portal_Sensor p in pSensors)
        {
            // startpoint
            if (!startPoint.Contains(p.name))
            {
                startPoint.Add(p.name);
            }


            // endpoint
            foreach (GData.LocationType lo in p.locations)
            {
                if (!endPoint.Contains(lo))
                {
                    endPoint.Add(lo);
                }
            }
        }
		if (GData.playMode == GData.PlayMode.Replayer){
			replayMission ();
			InitMission();
			return;
		}
        //Randomly start by the stairs, elevator or escalator
        Scene curr_scene = SceneManager.GetActiveScene();
        Debug.Log("Scene " + curr_scene.name);
        if (curr_scene.name == "Level4" || curr_scene.name == "Level5" || curr_scene.name == "Level5_displayGaze")
        {
            if (current_mission.random_start)
            {
                do
                {
                    current_mission.starting_point = startPoint[Random.Range(0, startPoint.Count)]; // random starting point [Esc, Ele, Ele1 or Stairs]
                } while (current_mission.starting_point == "Bunny" || current_mission.starting_point == "Room");
            }
        }
		if (curr_scene.name == "VRCity" || curr_scene.name=="VRVillage")
        {
            Cycle_Destination_City();
        }
		else if (curr_scene.name == "VRCity_with_hint" && GData.playMode==GData.PlayMode.ValidationExp)
        {
            Cycle_Destination_City_Validation();
        }
		else if (curr_scene.name == "Kenmore")
        {
            Cycle_Destination();
        }


        InitMission();

    }
		
	public void replayMission(){

		current_mission.bGo = false;
		current_mission.bTaskGoAndReturn = true;
		PlaceFlagAtStartPoint(current_mission.starting_point);
		current_mission.startpoint = GetLocationType(current_mission.starting_point);
		Debug.Log("replayMission() startPoint: " + current_mission.starting_point);
	}
    public void Random_Destination()
    {
        // randomly choose a mission
        // current_mission = missions[Random.Range(0,missions.Length)]; 

        current_mission = new Mission();
        //current_mission.start_portal = startPoint[Random.Range(0,startPoint.Count)];
        current_mission.starting_point = startPoint[Random.Range(0, startPoint.Count)];//(GData.LocationType)Enum.Parse (typeof(GData.LocationType),startPoint[Random.Range(0,startPoint.Count)]);

        // the destination can not be the start point's location
        List<GData.LocationType> locs
            = new List<GData.LocationType>(GameObject.FindObjectOfType<PortalSystem>().
            //Get_Protals()[current_mission.start_portal.ToString()].locations);
            Get_Protals()[current_mission.starting_point.ToString()].locations);
        Debug.Log("Locs : " + locs.Count);
        current_mission.destination = endPoint[Random.Range(0, endPoint.Count)];
        while (locs.Contains(current_mission.destination))
        {
            current_mission.destination = endPoint[Random.Range(0, endPoint.Count)];
        }


        current_mission.comment = "Find a way to " + current_mission.destination.ToString();
        current_mission.comment = current_mission.comment.Replace("_", " ");

        // place the player
        PortalSystem sys = GameObject.FindObjectOfType<PortalSystem>();
        //sys.Place_Player(current_mission.start_portal.ToString());
        sys.Place_Player(current_mission.starting_point.ToString());

    }
    public void Cycle_Destination_City()
    {
        //       GData.LocationType[] endPoint = { GData.LocationType.HardwareShop, GData.LocationType.AutoParts, GData.LocationType.SuperMarket };
        GData.LocationType[] endPoint = { GData.LocationType.Apple, GData.LocationType.Banana, GData.LocationType.Strawberry, GData.LocationType.Kiwi };
        string dataFolder = Application.dataPath + "/" + GData.dataResultsPath;
        IniFile ifile = new IniFile(dataFolder + "/gdata/next_mission_city.txt");
        ifile.goTo("MData");
        int nextIndex = ifile.GetValue_Int("nextIndex", 0);

        //set mission destination
        current_mission = new Mission();
        current_mission.destination = endPoint[nextIndex];
        current_mission.starting_point = string.Format("Start_Point_{0:00}", Random.Range(1, 5 + 1));
        current_mission.startpoint = GetLocationType(current_mission.starting_point);
        current_mission.bGo = true;
        current_mission.bTaskGoAndReturn = true;
        PlaceFlagAtStartPoint(current_mission.starting_point);
        Debug.Log("startPoint: " + current_mission.starting_point);
        //        GameObject.FindObjectOfType<FindPath>().showPath();

        // for default setting
        Debug.Log("nextIndex " + (nextIndex + 1) % endPoint.Length);
        ifile.SetInt("nextIndex", (nextIndex + 1) % endPoint.Length);
        ifile.Save_To_File();
    }
    public void Cycle_Destination_City_Validation()
    {
        //       GData.LocationType[] endPoint = { GData.LocationType.HardwareShop, GData.LocationType.AutoParts, GData.LocationType.SuperMarket };
        GData.LocationType[] endPoint = { GData.LocationType.Apple, GData.LocationType.Banana, GData.LocationType.Strawberry, GData.LocationType.Kiwi };
        string dataFolder = Application.dataPath + "/" + GData.dataResultsPath;
        IniFile ifile = new IniFile(dataFolder + "/gdata/next_mission_city_validation.txt");
        ifile.goTo("MData");
        int nextIndex = ifile.GetValue_Int("nextIndex", 0);

        int aid1st;
        int aid2nd;
        int start_point_id;
        int end_point_id;
        int[] question_ids;
        int trials = ReadProtocol("protocol_validation_exp.csv", nextIndex, out aid1st, out aid2nd, out start_point_id, out end_point_id, out question_ids);
        if (trials == 0)
        {
            Debug.Log("Read error \" Resources/protocol_validation_exp.csv\"");
        }
        Debug.Log(("Index : " + nextIndex + "   Aid1st : " + aid1st + "    Aid2nd : " + aid2nd + "   StartPoint : " + start_point_id + "   EndPoint : " + end_point_id));
        //set mission destination
        current_mission = new Mission();
        //current_mission.destination = endPoint[nextIndex];
        //current_mission.starting_point = string.Format("Start_Point_{0:00}", Random.Range(1, 5 + 1));
        current_mission.destination = endPoint[end_point_id - 1];
        current_mission.starting_point = string.Format("Start_Point_{0:00}", start_point_id);
        current_mission.startpoint = GetLocationType(current_mission.starting_point);
        current_mission.bGo = true;
        current_mission.bTaskGoAndReturn = true;
        current_mission.aid1st = aid1st;
		current_mission.aid2nd = aid2nd;
        current_mission.trial_nb = nextIndex;
        current_mission.question_ids = question_ids;
        current_mission.num_hit_by_car = 0;
        PlaceFlagAtStartPoint(current_mission.starting_point);
        Debug.Log("startPoint: " + current_mission.starting_point);
        //        GameObject.FindObjectOfType<FindPath>().showPath();
        Vector3[] path_points = GameObject.FindObjectOfType<FindPath>().GetPathCorners();
        float distance = 0f;
        for (int i = 1; i < path_points.Length; i++)
        {
            distance += (path_points[i] - path_points[i - 1]).magnitude;
        }
        current_mission.distance = distance;

        // for default setting
        //       Debug.Log("nextIndex " + (nextIndex + 1) % trials);
        //       ifile.SetInt("nextIndex", (nextIndex + 1) % trials);
        //       ifile.Save_To_File();
    }
    public void Cycle_Destination()
    {
        GData.LocationType[] endPoint = {GData.LocationType.North_Station, GData.LocationType.Boston_College,
            GData.LocationType.Boston_University, GData.LocationType.Fenway_Park, GData.LocationType.Bus_Stop,
            GData.LocationType.Boston_University, GData.LocationType.Fenway_Park,  GData.LocationType.Bus_Stop};

        string dataFolder = Application.dataPath + "/" + GData.dataResultsPath;
        IniFile ifile = new IniFile(dataFolder + "/gdata/next_mission.txt");
        ifile.goTo("MData");
        int nextIndex = ifile.GetValue_Int("nextIndex", 0);

        //set mission destination
        current_mission.destination = endPoint[nextIndex];
        Debug.Log("end point: " + current_mission.destination.ToString());
        //NorthStation mission 
        switch (nextIndex)
        {
            case 2: //mission Boston Univ 1
            case 4: //mission Fenway 1
            case 6: //mission Bus stop 1
                    //mission Northstation
            case 0:
                current_mission.starting_point = string.Format("Train #{0} {1}", Random.Range(1, 3), Random.Range(1, 7));
                break;
            //mission Boston College
            case 3: //mission Boston Univ 2
            case 5: //mission Fenway 2
            case 7: //mission Bus stop 2
            case 1:
                current_mission.starting_point = string.Format("Train #{0} {1}", Random.Range(3, 5), Random.Range(1, 7));
                break;
        }

        Debug.Log("startPoint: " + current_mission.starting_point);

        // for default setting
        Debug.Log("nextIndex " + (nextIndex + 1) % 8);
        ifile.SetInt("nextIndex", (nextIndex + 1) % 8);
        ifile.Save_To_File();

    }
    int ReadProtocol(string filename, int id, out int aid1st, out int aid2nd, out int startPoint, out int endPoint, out int[] question_ids)
    {
        aid1st = 0; aid2nd = 0; startPoint = 0; endPoint = 0; question_ids = null;
        /*
        TextAsset csvFile = Resources.Load(filename) as TextAsset;
        if (csvFile == null)
        {
            return 0;
        }
        */
        string dataFolder = Application.dataPath + "/" + GData.dataResultsPath;
        string protocol_path = dataFolder + @"/gdata/" + filename;

        int line_nb = 0;
        //        StringReader sr = new StringReader(csvFile.text);
        StreamReader sr = new StreamReader(protocol_path);
        if (sr == null)
            return 0;
        //       while (sr.Peek() > -1)
        string line;
        while ((line = sr.ReadLine()) != null)
        {
            //            string line = sr.ReadLine();
            if (line.StartsWith("#"))
                continue;
            string[] nums = line.Split(',');
            if (line_nb == id)
            {
                aid1st = int.Parse(nums[0]);
                aid2nd = int.Parse(nums[1]);
                startPoint = int.Parse(nums[2]);
                endPoint = int.Parse(nums[3]);
                if (nums.Length > 4)
                {
                    question_ids = new int[nums.Length - 4];
                    for (int i = 4; i < nums.Length; i++)
                    {
                        question_ids[i - 4] = int.Parse(nums[i]);
                    }
                }
            }
            line_nb++;
        }
        return line_nb;
    }

    // **********************************************************************************
    // Called by sensor
    // **********************************************************************************
    public void Mission_Complete()
    {
        isCompleted = true;
        Debug.Log("Mission_Complete!");
    }

    public float durationDisplayAbondonedMessage = 5f;
    public void Mission_Abandoned()
    {
        StartCoroutine("ShowAbandonedMessage");
    }
    IEnumerator ShowAbandonedMessage()
    {
        string str = "<b>Mission Abandoned</b>\n"
            + "<size=14> Please wait...</size>\n";
        MainUI mainUI = FindObjectOfType<MainUI>();
        mainUI.SetTips(str);
        mainUI.SetTranlucent(true);
        yield return new WaitForSeconds(durationDisplayAbondonedMessage);
        mainUI.SetTranlucent(false);
        Mission_Complete();
    }


    public void InitMission()
    {
        isCompleted = false;
        Debug.Log("Mission_initialized!");
    }

    public GData.LocationType GetLocationType(string name)
    {
        // the destination can not be the start point's location
        List<GData.LocationType> locs
            = new List<GData.LocationType>(GameObject.FindObjectOfType<PortalSystem>().Get_Protals()[name.ToString()].locations);
        Debug.Log("Locs : " + locs.Count + " / Name : " + locs[0].ToString());
        return locs[0];
    }
    public Portal_Sensor GetPortal(string name)
    {
        return FindObjectOfType<PortalSystem>().Get_Protals()[name.ToString()];
    }
    public Portal_Sensor GetPortal(GData.LocationType lt)
    {
        Portal_Sensor[] ps = GameObject.FindObjectsOfType<Portal_Sensor>();
        foreach (Portal_Sensor s in ps)
        {
            if (s.locations.Contains(lt))
                return s;
        }
        return null;
    }
	void PlaceFlagAtStartPoint(string name)
    {
		Debug.Log ("placing flag in " + name);
        ResetFlag();
        Portal_Sensor ps = GetPortal(name);
        PortalOrigin po = ps.GetComponentInChildren<PortalOrigin>();
        GameObject prfb = (GameObject)Resources.Load("flag/flag_med");
        Behaviour halo = (Behaviour)prfb.GetComponent("Halo");
        halo.enabled = true;
        if (po != null)
        {
            flagObject = Instantiate(prfb, po.transform);
        }
        else
        {
            flagObject = Instantiate(prfb, ps.transform);
        }
        Rigidbody[] rb = flagObject.GetComponentsInChildren<Rigidbody>();
        foreach (Rigidbody r in rb)
        {
            r.useGravity = false;
        }
        Collider[] cd = flagObject.GetComponentsInChildren<Collider>();
        foreach (Collider c in cd)
        {
            c.isTrigger = true;
        }

    }
    void ResetFlag()
    {
        if (flagObject != null)
        {
            Destroy(flagObject);
            flagObject = null;
        }
    }
}
