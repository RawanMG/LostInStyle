using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MiniMap : MonoBehaviour {

    Camera miniMapcam;
    Camera parentCam;
    //    public GameObject originObj;
    public float orthSize = 300f;
    public float arrowSize = 50f;
    public float sizeMarker = 40f;
    public Vector3 posMinimap = new Vector3(0, 1.8f, 5f);
    public Vector3 scaleMinimap = new Vector3(0.2f, 0.2f, 0.2f);

    Transform trMap;
    Transform trHead;
    void Awake()
    {
        Camera[] cams = GameObject.FindObjectsOfType<Camera>();
        foreach (Camera cam in cams)
        {
            if (cam.GetComponent<ScreensRecoder>() == null)
            {
                Debug.Log("Set Cull Mask :" + cam.name);
                cam.cullingMask &= ~(1 << 4);
            }
        }

        GameObject go = new GameObject("MinimapCamera");
        miniMapcam = go.AddComponent<Camera>();
        Vector3 p = new Vector3(-30f, 100, -30f);
        miniMapcam.transform.position = p;
        miniMapcam.transform.Rotate(new Vector3(90f, 90f, 0));
        miniMapcam.orthographic = true;
        miniMapcam.allowHDR = false;
        miniMapcam.orthographicSize = orthSize;
        RenderTexture rt = new RenderTexture(1000, 1000, 24);
        miniMapcam.targetTexture = rt;

        Head[] heads = (GameObject.FindObjectOfType<User_Object>() as User_Object).GetComponentsInChildren<Head>();
        Head head = null;
        foreach (Head h in heads)
        {
            if (h.enabled)
            {
                head = h;
                break;
            }
        }
        trHead = head.transform;
        GameObject mapScreen = GameObject.CreatePrimitive(PrimitiveType.Plane);
        mapScreen.name = "MiniMapScreen";
        Vector3 mapPos = head.transform.localToWorldMatrix.MultiplyPoint3x4(posMinimap);
        mapScreen.transform.position = mapPos;
        mapScreen.transform.Rotate(new Vector3(0, 90f, 90f));
        mapScreen.transform.localScale = scaleMinimap;
        //     mapScreen.transform.parent = head.transform;

        Material mat = new Material(Shader.Find("Unlit/Texture"));
        mat.SetTexture("_MainTex", rt);

        mapScreen.GetComponent<Renderer>().material = mat;
        //      mr.material = mat;

        trMap = mapScreen.transform;


    }
    // Use this for initialization
    void Start() {
        init();
    }

    public void ShowMiniMap(bool bShow)
    {
        GameObject map = trMap.gameObject;
        map.GetComponent<MeshRenderer>().enabled = bShow;
    }
    public bool isShown()
    {
        GameObject map = trMap.gameObject;
        return map.GetComponent<MeshRenderer>().enabled;
    }

    public void init()
    {
        RemoveTraceMarker();

        User_Object user = GameObject.FindObjectOfType<User_Object>();
        Vector3 user_pos = user.transform.position;
        GameObject user_obj = PutUserMarker("UserMarker", user_pos, Color.red);
        user_obj.transform.parent = user.gameObject.transform;
        user_obj.transform.localRotation = Quaternion.Euler(90f, 0, 0);

        Vector3 dest_pos = getDestinationPos();
        PutMarker("Destination", dest_pos, Color.blue);
        ShowMiniMap(false);

    }
    void RemoveTraceMarker()
    {
        TraceMarker[] tms = GameObject.FindObjectsOfType<TraceMarker>();
        foreach (TraceMarker tm in tms)
        {
            GameObject go = tm.gameObject;
            LineRenderer lr = go.GetComponent<LineRenderer>();
            if (lr != null)
                Destroy(lr);
            Destroy(tm);
            Destroy(go);
        }
    }


    // Update is called once per frame
    void Update () {
//        trMap.position = trHead.position;
        
	}

    private void FixedUpdate()
    {
        Vector3 mapPos = trHead.localToWorldMatrix.MultiplyPoint3x4(posMinimap);
        trMap.position = mapPos;
        trMap.localRotation = trHead.rotation;
        trMap.localScale = scaleMinimap;

        trMap.Rotate(new Vector3(90, 180f, 0f));

    }

    GameObject PutMarker(string name, Vector3 p, Color col)
    {
        GameObject go = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        go.name = name;
        p.y += 50f;
        go.transform.position = p;
        go.transform.localScale = Vector3.one * sizeMarker;
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = col;
        go.GetComponent<Renderer>().material = mat;
        go.layer = 4;
        go.AddComponent<TraceMarker>();
        return go;
    }
    GameObject PutUserMarker(string name, Vector3 p, Color col)
    {
        //    GameObject go = GameObject.CreatePrimitive(PrimitiveType.Plane);
        GameObject go = new GameObject("Arrow");
        go.name = name;
        go.transform.rotation = new Quaternion(0, 0, 0, 0);
        MeshFilter mf = go.AddComponent<MeshFilter>();
        Mesh mesh = new Mesh();
        mesh.Clear();
        mesh.vertices = new Vector3[] { new Vector3(0, -0.5f, 0), new Vector3(0, 0.75f, 0), new Vector3(-0.5f, -0.75f, 0), new Vector3(0.5f,-0.75f,0)};
        mesh.uv = new Vector2[] { new Vector2(0, 0), new Vector2(0, 1), new Vector2(1, 1), new Vector2(1, 0)};
        mesh.triangles = new int[] { 2, 1, 0, 1,3,0 };
        mesh.RecalculateBounds();
        mesh.RecalculateNormals();
        mesh.RecalculateTangents();
        mf.mesh = mesh;

        MeshRenderer mr = go.AddComponent<MeshRenderer>();
        
        p.y += 50f;
        go.transform.position = p;
        go.transform.localScale = Vector3.one * arrowSize;
        Material mat = new Material(Shader.Find("Unlit/Color"));
        mat.color = col;
        go.GetComponent<Renderer>().material = mat;
        go.layer = 4;
        go.AddComponent<TraceMarker>();
        return go;
    }
    //returns the location of the Portal_sensor that's been set as the mission goal
    public Vector3 getDestinationPos()
    {
        Vector3 port_pos = Vector3.zero;
        MissionSystem missionSys = GameObject.FindObjectOfType<MissionSystem>();
        Mission curr_mission = missionSys.Get_Current_Mission();

        PortalSystem sys = GameObject.FindObjectOfType<PortalSystem>();
        Dictionary<string, Portal_Sensor> portals = sys.Get_Protals();
        //search for the sensor postion
        foreach (KeyValuePair<string, Portal_Sensor> portal in portals)
        {
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
                if (curr_mission.bGo)
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
