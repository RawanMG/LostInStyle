using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System;
using System.IO;
using UnityEngine;
using System.Net;
using System.Net.Sockets;
using System.Text;
public class CommStressMeter : MonoBehaviour {
    // player Object
    Transform user;
    Fove.Managed.SFVR_GazeConvergenceData gazeConv;
    Fove.Managed.FVRHeadset headset;
    List<float> gazeBuffer;
    List<float> filterBuffer;
    public int filterSize = 5;

    float consumeTime = 0.0f;
    Transform trFove;
    public int gazeWindowSize = 350;
    float lastTimeStressed = -99999f;
    public float showAidDur = 3f;
    float lastTimeDestressed = -9999;
    public float supressAidDur = 3f;
    bool bActiveAid = false;

    MissionSystem missionSys;
    VRMainSystem vrMain;

    Socket sender;
    Process pythohProcess;
    public const int iBufferSize = 1024;
    public byte[] Buffer  = new byte[iBufferSize];
    bool bStressUpdated = false;
    bool bStressProcessing = false;
    float fStressLevel = 0f;
    public string fileNameStd = "std_350.txt";

    public bool saveData = false;
    public bool startRec = false;
    public bool isDebug = true;
    public bool useServerInternal = true;
    public bool standardization = true;
    List<float> allData = new List<float>();
    ReadStdFile stdFile;
    ReadStdFile stdFileOrig;


    private void Awake()
    {
		if (GData.playMode == GData.PlayMode.Replayer)
			return;
        init();
        if(useServerInternal)
            SetupConnectionWithPython();
    }
    public void init()
    {
		if (GData.playMode == GData.PlayMode.Replayer)
			return;
        gazeBuffer = new List<float>();
        filterBuffer = new List<float>();
        if (isDebug)
            return;

        // find User Object;
        user = (GameObject.FindObjectOfType<User_Object>() as User_Object).transform;
        if (trFove == null)
        {
            trFove = FindObjectOfType<FoveInterface2>().transform;
        }
        missionSys = GameObject.FindObjectOfType<MissionSystem>();
        vrMain = GameObject.FindObjectOfType<VRMainSystem>();

        bStressProcessing = false;
        bStressUpdated = false;

        stdFileOrig = new ReadStdFile();
        stdFileOrig.std_file = fileNameStd;
        stdFileOrig.ReadFile();
        InitStandization();
    }

    public void InitStandization()
    {
        stdFile = stdFileOrig.GetCopy();
    }

    void SetupConnectionWithPython()
    {
        string path_to_python = Environment.GetEnvironmentVariable("TENSOR_PATH") + @"\python";
        print(path_to_python);
        try
        {
            Process p = new Process();
            p.StartInfo.WindowStyle = ProcessWindowStyle.Hidden;
            p.StartInfo.CreateNoWindow = true;
            p.StartInfo.UseShellExecute = false;
            p.StartInfo.FileName = path_to_python;
            string working_dir = Application.dataPath + "/_Scripts/Core/";
            p.StartInfo.WorkingDirectory = working_dir;
//            string seq_str = "";


            p.StartInfo.Arguments = working_dir + "SocketComm.py";
//            print("Running : " + p.StartInfo.Arguments);
            p.StartInfo.RedirectStandardError = false;
            p.StartInfo.RedirectStandardOutput = false;
            p.EnableRaisingEvents = true;
            p.Start();
            print("Running : " + p.StartInfo.Arguments);
            pythohProcess = p;

        }
        catch (Exception e)
        {
            print(e);
        }

    }

    void processExited(object sender, System.EventArgs e)
    {
        UnityEngine.Debug.Log("Process exited");
        print(e);
    }

    void ConnectPython()
    {
        byte[] bytes = new byte[1024];
        try
        {
            IPHostEntry ipHostInfo = Dns.GetHostEntry("localhost");
            IPAddress ipAddress = ipHostInfo.AddressList[0];
            IPEndPoint remoteEP = new IPEndPoint(ipAddress, 9998);

            sender = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            while (sender.Connected == false)
            {
                try
                {
                    UnityEngine.Debug.Log("Connect..." + ipAddress);
                    sender.Connect(remoteEP);
                    UnityEngine.Debug.Log("Connected to " + sender.RemoteEndPoint.ToString());
                    sender.BeginReceive(Buffer, 0, iBufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), sender);
                }
                catch (Exception e)
                {
                    print(e);
                }
            }
        }
        catch (Exception e)
        {
            print(e); 
        }
    }

    private void ReceiveCallback(IAsyncResult asyncResult)
    {
        var socket = asyncResult.AsyncState as Socket;
        var byteSize = -1;
        try
        {
            byteSize = socket.EndReceive(asyncResult);
        }catch(Exception ex)
        {
            print(ex);
        }

        if(byteSize>0)
        {
            string recv = Encoding.ASCII.GetString(Buffer, 0, byteSize);
            fStressLevel = float.Parse(recv);
        //    print(recv);
            bStressUpdated = true;
            bStressProcessing = false;

            socket.BeginReceive(Buffer, 0, iBufferSize, SocketFlags.None, new AsyncCallback(ReceiveCallback), socket);
        }
    }

    private void OnDestroy()
    {
        if(pythohProcess!=null)
        {
            pythohProcess.Kill();
        }

    }
    void DisconnectPython()
    {
        if (sender != null && sender.Connected)
        {
            sender.Shutdown(SocketShutdown.Both);
            sender.Close();
            sender = null;
        }
    }

    
    void SendMsgPython(string msg)
    {
        if (sender == null || sender.Connected == false)
            return ;
        
        byte[] bmsg = Encoding.ASCII.GetBytes(msg+"$");
        int byteSent = sender.Send(bmsg);
        return ;
    }
    // Use this for initialization
    void Start () {
        if (sender == null)
        {
            UnityEngine.Debug.Log("attempt to connect");
            ConnectPython();
        }
    }
	
	// Update is called once per frame
	void Update ()
    {
        if (isDebug)
        {
            UnityEngine.Debug.Log("Stress Level : " + fStressLevel);

            return;
        }
        string msg = "TrN: (" + missionSys.Get_Current_Mission().trial_nb +" <" + (missionSys.Get_Current_Mission().trial_nb % 5 + 1) + ">) 1st: (" + missionSys.Get_Current_Mission().aid1st + ") 2nd: (" + missionSys.Get_Current_Mission().aid2nd + ") SL: " + fStressLevel;
        if (stdFile != null)
        {
            msg += " STD: " + stdFile.GetListSize();
        }
        vrMain.mainUI.ShowLogMessage(msg);

        if(saveData && startRec)
        {
            SaveDataArray(allData.ToArray());
            saveData = false;
            startRec = false;
        }
	}

    void SaveDataArray(float[] dat)
    {
        string datafile = Application.dataPath + @"\DataResults\gaze_log.csv";

        StreamWriter sw = new StreamWriter(datafile, false);
        string line = "";
        foreach(float d in dat)
        {
            line = line + d.ToString() + ",";
        }
        sw.WriteLine(line);
        sw.Close();
    }

    public bool shouldShowAid()
    {
        if (Time.time - lastTimeStressed < Time.time - lastTimeDestressed)
        {
            // estimation : stressed
            if(!bActiveAid && Time.time - lastTimeDestressed > supressAidDur)
            {
                bActiveAid = true;
            }
        }
        else {
            // estimation : destressed
            if (bActiveAid && Time.time - lastTimeStressed > showAidDur)
            {
                bActiveAid = false;
            }
        }
        return bActiveAid;
    }

    float addFilterBuffer(float dat)
    {
        filterBuffer.Add(dat);
        float ret = 0f;
        if (filterBuffer.Count >= filterSize)
        {
            int count = filterBuffer.Count;
            for (int i = count - 1; i > count - 1 - filterSize; i--)
                ret += filterBuffer[i];
            ret /= (float)filterSize;
            for (int i=0;i<= count - 1 - filterSize;i++)
              filterBuffer.RemoveAt(0);
        }
        return ret;
    }

    private void FixedUpdate()
    {
		if (GData.playMode == GData.PlayMode.Replayer)
			return;
        float newData;
        if (isDebug)
        {   //debug
            System.Random random = new System.Random();
            newData = (float)random.NextDouble();
        }
        else
        {
            newData = GetAngle();
        }
        //print(newData);
        // standardization
        float gaze_mean, gaze_var;
        stdFile.GetValues(out gaze_mean, out gaze_var);
        if(standardization)
            newData = (newData - gaze_mean) / Mathf.Sqrt(gaze_var);   //converting into z-score
 
        // Denoising (boxfilter)
        float filtered = addFilterBuffer(newData);
        //       float filtered = newData;

        //debug
        if(startRec)
            allData.Add(filtered);

        // Windowing
        gazeBuffer.Add(filtered);
        if (gazeBuffer.Count > gazeWindowSize)
            gazeBuffer.RemoveAt(0);
        if(gazeBuffer.Count == gazeWindowSize && bStressProcessing==false)
        {
          //  Prediction
            StartPrediction(gazeBuffer.ToArray());
        }

        if (bStressUpdated)
        {
            if (fStressLevel > 0.5f)
            {
 //               if (!bActiveAid)
                    lastTimeStressed = Time.time;
            }
            else
            {
 //               if(bActiveAid)
                    lastTimeDestressed = Time.time;
            }
           // print("Detect Stress");
            bStressUpdated = false;
        }
    }

    void StartPrediction(float[] data)
    {
        bStressProcessing = true;
        string s = "";
        foreach(float v in data)
        {
            s = s + v.ToString() + ",";
        }
        s = s.Substring(0, s.Length - 1);
        SendMsgPython(s);
    //    print("Sent a series of data to python! Length : " + data.Length);
    }

    float GetAngle()
    {
        headset = FoveInterface2.GetFVRHeadset();
        gazeConv = headset.GetGazeConvergence();
        Vector3 normal = (trFove.rotation * Conv(gazeConv.ray.direction)).normalized;
        Vector3 head_normal = trFove.forward.normalized;
        float angle = Vector3.Dot(normal, head_normal);
        if( angle < -1)
            angle = -1;
        if (angle > 1)
            angle = 1;
        angle = Mathf.Acos(angle);
        return angle;
    }
    Vector3 Conv(Fove.Managed.SFVR_Vec3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }

	float[] getRandomArray(int size){
		System.Random random = new System.Random ();
		float[] arr = new float[size];
		for (int i = 0; i < size; i++) {
			arr [i] = (float)random.NextDouble ();
			print (arr [i]);
		}
		return arr;
	}
    string getDummyData(int n)
    {
        string s = "";
        for (int i = 0; i < n; i++)
        {
            s = s + UnityEngine.Random.Range(0, 100) / 100f + ",";
        }
        return s.Substring(0, s.Length - 1);

    }
}
