using UnityEngine;
using System.Collections;
using System.Collections.Generic;


// ref name is gameobject's name
[RequireComponent(typeof(Collider))]
public class Portal_Sensor : Sensor 
{
	public Transform startingPoint;

	[SerializePrivateVariables]
	public List<GData.LocationType> locations;



	// Use this for initialization
	void Start () 
	{

	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	void OnTriggerEnter(Collider other) 
	{
		// if recoder session
		if (GData.playMode == GData.PlayMode.Recoder || GData.playMode == GData.PlayMode.ValidationExp)
		{
			User_Object uo = other.gameObject.GetComponent<User_Object>();
            // if the destination match the mission
            MissionSystem mSys = GameObject.FindObjectOfType<MissionSystem>();
            Mission m = mSys.Get_Current_Mission();
            if (uo && m.bTaskGoAndReturn == false)
            {


                if (locations.Contains(mSys.Get_Current_Mission().destination))
                {
                    Debug.Log("Location: " + name + ", dest: " + mSys.Get_Current_Mission().destination.ToString());
                    mSys.Mission_Complete();
                }
            }
            else if (uo && m.bTaskGoAndReturn == true)
            {

                if(m.bGo==true && locations.Contains(mSys.Get_Current_Mission().destination))
                {
                    Debug.Log("Location: " + name + ", dest: " + mSys.Get_Current_Mission().destination.ToString() + "   Origin: " + mSys.Get_Current_Mission().starting_point);
                    m.bGo = false;
//                    m.comment = "Go back to the start point";
 //                   m.comment = "Find a way to " + m.startpoint.ToString();
//                    m.comment = m.comment.Replace("_", " ");
                    mSys.current_mission = m;
                    MainUI mainUI = FindObjectOfType<MainUI>();
//                    mainUI.mission_description.text
//                        = "<size=24><b>Mission Description:</b></size>\n" +
//                            "        " + m.comment;
                    mainUI.SetTips("");
                    /*
					if(GameObject.FindObjectOfType<FindPath>() != null)
						GameObject.FindObjectOfType<FindPath>().init();
					else
						GameObject.FindObjectOfType<Arrow>().init();
                        */
                    if (GameObject.FindObjectOfType<FindPath>() != null)
                        GameObject.FindObjectOfType<FindPath>().init();
                    if (GameObject.FindObjectOfType<Arrow>() != null)
                        GameObject.FindObjectOfType<Arrow>().init();
                    if (GameObject.FindObjectOfType<MiniMap>() != null)
                        GameObject.FindObjectOfType<MiniMap>().init();


                    // Activate Barrirer(s)
                    CtrlObstacleCollider coc = GameObject.FindObjectOfType<CtrlObstacleCollider>();
                    if(coc!=null)
                    {
                        coc.ActivatePassedObstacles(1);
                    }

                    //
                    VRMainSystem vrms = GameObject.FindObjectOfType<VRMainSystem>();
                    
                    if(vrms.playMode==GData.PlayMode.ValidationExp)
                    {
                        switch(mSys.current_mission.aid2nd)
                        {
                            case 1:
                                GameObject.FindObjectOfType<FindPath>().showPath();
                                break;
                            case 2:
                                GameObject.FindObjectOfType<Arrow>().ShowArrow(true);
                                break;
                            case 3:
                                GameObject.FindObjectOfType<MiniMap>().ShowMiniMap(true);
                                break;
                            default:
                                Debug.Log("No aid shown");
                                break;
                        }
                    }
                    Vector3[] path_points = GameObject.FindObjectOfType<FindPath>().GetPathCorners();
                    float distance = 0f;
                    for (int i = 1; i < path_points.Length; i++)
                    {
                        distance = +(path_points[i] - path_points[i - 1]).magnitude;
                    }
                    m.distance = distance;

                    // Store data into a file
                    vrms.GetDB().Done(vrms.dataPath);

                    // Prep a new file 
                    string fname = vrms.dataPath;
                    fname = fname.Remove(fname.Length - 5);
                    fname += "2.txt";
                    vrms.dataPath = fname;
                    vrms.InitStressPredictionStandardization();
                    vrms.StartRecording(vrms.dataPath);
                }
                else if( m.bGo == false && locations.Contains(mSys.Get_Current_Mission().startpoint))
                {
                    Debug.Log("Location: " + name + ", dest: " + mSys.Get_Current_Mission().destination.ToString());
                    mSys.Mission_Complete();
                }

            }
        }
	}
}
