using UnityEngine;
using UnityEngine.VR;
using System.Collections;
using System.IO;

// *****************************************************************************************************************
// This is the main system, manager the data collect and virtual path
// *****************************************************************************************************************
using UnityEngine.SceneManagement;


public class VRMainSystem : MonoBehaviour
{
    string dataFolder = "";

    // the data path for next record or replay
    public string dataPath;

    // database
    DataBase db;

    // Mission system
    MissionSystem missionSys;

    // done
    bool done = false;

    // playmode
    public GData.PlayMode playMode = GData.PlayMode.Read_From_GData;

    // arrow
    public GameObject arrow, userArrow;

    // main UI
    public MainUI mainUI;

    public float speedUp_For_Virtual_Path = 3.0f;

    float time_in_level = 3 * 60.0f;

    // public static bool isCalib = true; //is true while the headset is calibrating

    FindPath pathDisp;
    MiniMap aidMiniMap;
    Arrow aidArrow;

    public int replayer_start_index = 0;

    CommStressMeter cStressMeter;
    CtrlQuestionnaire ctrlQuestion;

    float startTime = 0f;
    public float abandonDuration = 300f;

    void Awake()
    {

    }

    // Use this for initialization
    private void Start()
    {
        init();
    }
    public void init()
    {
        done = false;
        dataFolder = Application.dataPath + "/" + GData.dataResultsPath;
        Debug.Log("dataFolder:" + dataFolder);

        // setup play mode
        if (playMode == GData.PlayMode.Read_From_GData)
        {
            playMode = GData.playMode;
        }
        else
        {
            GData.playMode = playMode;
        }

        missionSys = GameObject.FindObjectOfType<MissionSystem>();

        mainUI = FindObjectOfType<MainUI>();
        Debug.Log("[MAINui]: " + mainUI.name);

        pathDisp = GameObject.FindObjectOfType<FindPath>();
        aidArrow = GameObject.FindObjectOfType<Arrow>();
        aidMiniMap = GameObject.FindObjectOfType<MiniMap>();

        // according to the play mode, select a data collecter
        switch (playMode)
        {
            case GData.PlayMode.Free_Style:
                {
                    db = ScriptableObject.CreateInstance<DataBase>();
                    // read the Global data, and generate a file name for next user
                    IniFile ifile = new IniFile(dataFolder + "/gdata/common.txt");
                    ifile.goTo("GData");
                    int nextIndex = ifile.GetValue_Int("nextIndex", 0);

                    dataPath = dataFolder + "/results/" + Application.loadedLevelName + "-" + nextIndex + ".txt";

                    // for default setting
                    ifile.SetInt("nextIndex", nextIndex);
                    ifile.Save_To_File();


                    db.StartFunc(dataPath);
                    mainUI.mission_description.text = "<b>Time left: " + time_in_level + " minutes</b>\n" +
                            missionSys.Get_Current_Mission().comment;
                    //			mainUI.mission_description.text 
                    //				= "<size=24><b>Mission Description:</b></size>\n" +
                    //					"        Free Looking.";
                    //
                    //			string str = "<b>Free Looking</b>\n" 
                    //				+ "Please go back to Main Menu by Ctrl + U.";
                    mainUI.SetTips("");
                }
                break;

		case GData.PlayMode.Recoder:
			{
				// find a missio
				if (MissionSystem.randomly) {
					missionSys.Random_Destination ();
				}

				//db = ScriptableObject.CreateInstance<DataCollecter2>();

				// read the Global data, and generate a file name for next user
				IniFile ifile = new IniFile (dataFolder + "/gdata/common.txt");
				ifile.goTo ("GData");
				int nextIndex = ifile.GetValue_Int ("nextIndex", 0);

				dataPath = dataFolder + "/results/" + Application.loadedLevelName + "-" + nextIndex + (missionSys.Get_Current_Mission ().bTaskGoAndReturn ? "-1" : "") + ".txt";

				// for default setting
				ifile.SetInt ("nextIndex", nextIndex);

				ifile.Save_To_File ();
				//
				StartRecording (dataPath);

				//Randomly start by the stairs, elevator or escalator
				Scene curr_scene = SceneManager.GetActiveScene ();
				if (curr_scene.name == "Level4" || curr_scene.name == "Level5") {
					mainUI.mission_description.text = "<b>Time left: " + time_in_level + " minutes</b>\n" +
					missionSys.Get_Current_Mission ().comment;
				} else {
//					mainUI.mission_description.text
//                    = "<size=24><b>Mission Description:</b></size>\n" +
//					"        " + missionSys.Get_Current_Mission ().comment;
				}

				Mission curr_mission = missionSys.Get_Current_Mission ();
				Debug.Log ("start point " + curr_mission.starting_point.ToString ());
				// place the player
				PortalSystem sys = GameObject.FindObjectOfType<PortalSystem> ();
				//sys.Place_Player(current_mission.start_portal.ToString());
				//    if (sys)    
				sys.Place_Player (curr_mission.starting_point.ToString ().Trim ());

				if (missionSys.Get_Current_Mission ().bTaskGoAndReturn) {
					if (GameObject.FindObjectOfType<FindPath> () != null) {
						GameObject.FindObjectOfType<FindPath> ().showPath ();
						mainUI.SetTips ("");
					} else {
                            GameObject.FindObjectOfType<Arrow>().ShowArrow(true);
						mainUI.SetTips ("");

					}
				}
			}//end case
                break;

            case GData.PlayMode.Replayer:
                {
				cStressMeter = null; 

                    db = ScriptableObject.CreateInstance<DataReplayer2>();


                    // disable the charactor motor
                    GameObject.FindObjectOfType<CharacterMotor>().enabled = false;

                    //			dataPath = GData.replayer_filePath;
                    //                   dataPath = @"C:\Users\ysawa\Documents\unity_projects\VR_Lost_1006\VR Lost\Assets\DataResults\results\Kenmore-19.txt";
                    // Load last recording
//                    IniFile ifile = new IniFile(dataFolder + "/gdata/common.txt");
//                    ifile.goTo("GData");
//                    int nextIndex = ifile.GetValue_Int("nextIndex", 0) - 1; // last recording
//                    if(replayer_start_index!=0 && replayer_start_index<nextIndex)
//                    {
//                        nextIndex = replayer_start_index;
//                        replayer_start_index++;
//                    }

				string level_name = "VRVillage";
				dataPath = dataFolder + "/results/" + level_name + "-" + replayer_start_index + "-2.txt";

                db.StartFunc(dataPath);
				missionSys.init ();

				Arrow arrow = GameObject.FindObjectOfType<Arrow> ();
				if(arrow!=null)
					arrow.init();
				MiniMap map = GameObject.FindObjectOfType<MiniMap> ();
				if(map!=null)
					map.init();
				GameObject.FindObjectOfType<FindPath>().init();
				
				ShowAid(missionSys.Get_Current_Mission().aid1st, false);

//                    mainUI.mission_description.text
//                        = "<size=24><b>Mission Description:</b></size>\n";
                    //				+ db.tips;
                    mainUI.tips.tipsText.enabled = false;
                }

                break;

            case GData.PlayMode.Virtual_Path:
                {
                    // speed up.
                    CharacterMotor cm = GameObject.FindObjectOfType<CharacterMotor>();
                    cm.speed_walk = cm.speed_walk * speedUp_For_Virtual_Path;
                    cm.speed_run = cm.speed_run * speedUp_For_Virtual_Path;

                    db = ScriptableObject.CreateInstance<DataVirtualPath>();
                    dataPath = GData.replayer_filePath;

                    db.StartFunc(dataPath, arrow, userArrow);

                    mainUI.mission_description.text
                        = "<size=24><b>Mission Description:</b></size>\n"
                        + db.tips;

                    string str = "<b>Virtual Path</b>\n"
                        + "Please go back to Main Menu by Ctrl + U.";
                    mainUI.SetTips(str);

                }
                break;
            case GData.PlayMode.ValidationExp:
                {
                    // find a missio
                    if (MissionSystem.randomly)
                    {
                        missionSys.Random_Destination();
                    }

                    //db = ScriptableObject.CreateInstance<DataCollecter2>();

                    // read the Global data, and generate a file name for next user
                    IniFile ifile = new IniFile(dataFolder + "/gdata/common.txt");
                    ifile.goTo("GData");
                    int nextIndex = ifile.GetValue_Int("nextIndex", 0);

                    dataPath = dataFolder + "/validation_results/" + Application.loadedLevelName + "-" + nextIndex + (missionSys.Get_Current_Mission().bTaskGoAndReturn ? "-1" : "") + ".txt";

                    // for default setting
                    ifile.SetInt("nextIndex", nextIndex);

                    ifile.Save_To_File();
                    //
                    StartRecording(dataPath);

                    //Randomly start by the stairs, elevator or escalator
                    Scene curr_scene = SceneManager.GetActiveScene();
                    {
//                        mainUI.mission_description.text
//                        = "<size=24><b>Mission Description:</b></size>\n" +
//                        "        " + missionSys.Get_Current_Mission().comment;
                    }

                    Mission curr_mission = missionSys.Get_Current_Mission();
                    Debug.Log("start point " + curr_mission.starting_point.ToString());
                    // place the player
                    PortalSystem sys = GameObject.FindObjectOfType<PortalSystem>();
                    //sys.Place_Player(current_mission.start_portal.ToString());
                    //    if (sys)    
                    sys.Place_Player(curr_mission.starting_point.ToString().Trim());

                    if (missionSys.Get_Current_Mission().bTaskGoAndReturn)
                    {
                        GameObject.FindObjectOfType<Arrow>().init();
                        GameObject.FindObjectOfType<FindPath>().init();
                        GameObject.FindObjectOfType<MiniMap>().init();
                        ShowAid(missionSys.Get_Current_Mission().aid1st, true);
                        mainUI.SetTips("");
                    }
                    cStressMeter = GameObject.FindObjectOfType<CommStressMeter>();
                    ctrlQuestion= GameObject.FindObjectOfType<CtrlQuestionnaire>();
                    InitCars();
                }
                break;
        }

    }

    void InitCars()
    {
        DriveCar[] cars = GameObject.FindObjectsOfType<DriveCar>();
        foreach(DriveCar car in cars)
        {
            car.init();
        }
    }

    public void ShowAid(int aidId,bool bShow)
    {
        switch (aidId)
        {
            case 0:
                // do nothing
                break;
            case 1:
                if (bShow)
                    pathDisp.showPath();
                else
                    pathDisp.HidePath();
                break;
            case 2:
                aidArrow.ShowArrow(bShow);
                //mainUI.SetTips("");
                break;
            case 3:
                aidMiniMap.ShowMiniMap(bShow);
                //mainUI.SetTips("");
                break;
            case 4:
                aidArrow.ShowArrow(bShow);
                break;
            case 5:
                aidMiniMap.ShowMiniMap(bShow);
                break;
            default:
                Debug.Log("no corresponding id for the aid : " + aidId);
                break;
        }
    }



    public void StartRecording(string fname)
    {
        mainUI = FindObjectOfType<MainUI>();
        db = ScriptableObject.CreateInstance<DataCollecter2>();
        //       db.StartFunc(dataPath);
        db.StartFunc(fname);
        startTime = Time.time;
    }

    public DataBase GetDB()
    {
        return db;
    }

    void CheckAbandonce()
    {
        if (playMode == GData.PlayMode.ValidationExp && missionSys.Get_Current_Mission().bTaskGoAndReturn)
        {
            if(Time.time - startTime > abandonDuration)
            {
                missionSys.Mission_Abandoned();
            }
        }
    }
    
    void Update()
	{
		// back to main menu
		if ((Input.GetKey(KeyCode.LeftControl) || Input.GetKey(KeyCode.RightControl))
		    && Input.GetKey(KeyCode.U))
		{
			if (GData.playMode == GData.PlayMode.Virtual_Path 
			    || GData.playMode == GData.PlayMode.Replayer)
			{
				Application.LoadLevel("DataAnalyzeMenu");
			}
			else
			{
				Application.LoadLevel("MainMenu");
			}
		}

		// complete freestyle mission
		if (Input.GetKeyDown(KeyCode.Escape)){
			if (GData.playMode == GData.PlayMode.Free_Style){
				Debug.Log ("hold done escape");
				missionSys.Mission_Complete();
			}
		}

		//Level 4 & 5: should update time and check if the experiment duration passed 3 minutes
		Scene curr_scene = SceneManager.GetActiveScene ();
		if (curr_scene.name == "Level4" || curr_scene.name == "Level5" || curr_scene.name == "Level5_displayGaze") {
			time_in_level -= Time.deltaTime;
			if (time_in_level <= 0) {
				missionSys.Mission_Complete ();
				return;
			}
			string min_str = string.Format ("{0}:{1:00}", (int)time_in_level / 60, (int)time_in_level % 60);
			mainUI.mission_description.text = "<b>Time left: " + min_str + " minutes</b>\n" +
			missionSys.Get_Current_Mission ().comment;
		}

	
		if (Input.GetButtonDown ("A") && missionSys.Get_Current_Mission().bTaskGoAndReturn && missionSys.Get_Current_Mission().bGo==false && playMode!=GData.PlayMode.ValidationExp) {
			Debug.Log ("Button A pressed");
			bool done = pathDisp.getPath ();
			if (!done)
				Debug.Log ("Couldn't display path");
		}

        if (playMode == GData.PlayMode.Replayer)
            mainUI.mission_description.text = "";

        Mission m = missionSys.Get_Current_Mission();
        if (playMode == GData.PlayMode.ValidationExp && m.bTaskGoAndReturn == true && m.bGo == false && m.aid2nd > 3)
        {
            if (cStressMeter != null)
            {
                if (cStressMeter.shouldShowAid())
                {
                    ShowAid(m.aid2nd, true);
                }
                else
                {
                    ShowAid(m.aid2nd, false);
                }
            }
        }

        if(!done)
            CheckAbandonce();
	}
	
	// Update is called once per frame
	void FixedUpdate () 
	{
		

//		if (isCalib) {
//			if (Fove.FoveHeadset.GetHeadset ().IsHardwareConnected ()) {
//				isCalib = Fove.FoveHeadset.GetHeadset ().IsEyeTrackingCalibrating ();
//			} else
//				isCalib = false;
//			return;
//		}

		if (!missionSys.IsCompleted())
		{
			db.FixedUpdateFunc();
		}
		else
		{
			if (!done)
			{
				done = true;

                // show something here.
                switch (playMode)
                {
                    case GData.PlayMode.Free_Style:
                        {
                            db.Done(dataPath);
                        }
                        break;

                    case GData.PlayMode.Recoder:
                        {
                            db.Done(dataPath);

                            // updata the gdata
                            IniFile ifile = new IniFile(dataFolder + "/gdata/common.txt");
                            ifile.goTo("GData");
                            int nextIndex = ifile.GetValue_Int("nextIndex", 0) + 1;
                            ifile.SetInt("nextIndex", nextIndex);
                            ifile.Save_To_File();

                            string str = "<b>Mission Done!</b>\n"
                                + "<size=14>Data save to " + dataPath + "</size>\n"
                                + "Press <b>start</b> to play again.";
                            mainUI.SetTips(str);
//                            mainUI.mission_description.text = "Mission Done";

                            Mission curr_mission = missionSys.Get_Current_Mission();
                            //string startP = missionSys.Get_Current_Mission ().start_portal.ToString().Trim();
                            string startP = curr_mission.starting_point.ToString().Trim();
                            string endP = curr_mission.destination.ToString().Trim();



                        }
                        break;

                    case GData.PlayMode.Replayer:
                        {
                            mainUI.SetTips("");
                        }
                        break;

                    case GData.PlayMode.Virtual_Path:
                        {

                        }
                        break;

                    case GData.PlayMode.ValidationExp:
                        {
                            db.Done(dataPath);

                            // updata the gdata
                            IniFile ifile = new IniFile(dataFolder + "/gdata/common.txt");
                            ifile.goTo("GData");
                            int nextIndex = ifile.GetValue_Int("nextIndex", 0) + 1;
                            ifile.SetInt("nextIndex", nextIndex);
                            ifile.Save_To_File();

                            ifile = new IniFile(dataFolder + "/gdata/next_mission_city_validation.txt");
                            ifile.goTo("MData");
                            ifile.SetInt("nextIndex", (missionSys.Get_Current_Mission().trial_nb + 1));
                            ifile.Save_To_File();

                            string str = "<b>Mission Done!</b>\n"
                                + "<size=14>Data save to " + dataPath + "</size>\n"
                                + "Press <b>start</b> to play again.";
                            mainUI.SetTips(str);
                            print("Mission done update");
                            if (cStressMeter != null)
                                cStressMeter.saveData = true;

                            Mission curr_mission = missionSys.Get_Current_Mission();
                            //string startP = missionSys.Get_Current_Mission ().start_portal.ToString().Trim();
                            string startP = curr_mission.starting_point.ToString().Trim();
                            string endP = curr_mission.destination.ToString().Trim();

                            StartCoroutine("ProcQuestionnair");

                        }
                        break;
                }
            }
		}
	}

    IEnumerator ProcQuestionnair()
    {
        if (ctrlQuestion != null)
        {
            ctrlQuestion.StartQuestionnair();

            while (!ctrlQuestion.IsDone())
            {
                yield return null;
            }
            string resp = ctrlQuestion.GetResponseString();
            SaveReponses(resp);
        }
    }

    void SaveReponses(string s)
    {
        string datafile = Application.dataPath + @"\DataResults\validation_results\validationExpResponses.txt";
        StreamWriter sw = new StreamWriter(datafile, true);
        if(sw==null)
        {
            print("Failed to save file: " + datafile);
            return;
        }
        Mission curr_mission = missionSys.Get_Current_Mission();
        string line = curr_mission.trial_nb + "," + curr_mission.aid1st + "," + curr_mission.aid2nd + s;
        sw.WriteLine(line);
        sw.Close();
    }

    public void InitStressPredictionStandardization()
    {
        if(cStressMeter!=null)
        {
            Debug.Log("Initializing normaliation parameters for stress prediction");
            cStressMeter.InitStandization();
        }
    }
}


