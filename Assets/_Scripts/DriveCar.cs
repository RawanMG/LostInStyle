using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveCar : MonoBehaviour {

    public float speed = 0.80f;
    public float th_corner = 0.1f;
    public int interpolation = 50;
    Vector3 moveDirLast;
    public bool bCollision = false;
    int lastRoadType = 1;
    public List<Vector3> listTraces;

    Vector3 initPos;
    Quaternion initRot;
	// Use this for initialization
	void Start () {
        initPos = transform.position;
        initRot = transform.rotation;
	}
    public void init()
    {
        transform.position = initPos;
        transform.rotation = initRot;
    }

    // Update is called once per frame
    void Update () {
        if (bCollision)
            return;

        if (listTraces.Count> 0)
        {
            Vector3 movePos;
            Vector3 moveDir;
            GetPosMoveForward(out movePos, out moveDir);
            moveDirLast = moveDir;
            Vector3 p = transform.position;
            movePos.y = p.y;
            transform.position = movePos;
            transform.forward = moveDir;
        }
        else
        {
            Vector3 p = -transform.forward * speed / 10f + transform.position;
            transform.position = p;
        }
	}

    void GetPosMoveForward(out Vector3 destPos, out Vector3 direction)
    {
        Vector3 curPos = transform.position;
        float y = listTraces[0].y;
        curPos.y = y;

        float length_trace = 0f;
        Vector3 prevPos = curPos;
        Vector3 prevprevPos = curPos;
        while(listTraces.Count>0)
        {
            Vector3 p = listTraces[0];
            if(length_trace + (p - prevPos).magnitude > speed)
            {
                destPos = (p - prevPos).normalized * (speed - length_trace) + prevPos;
                direction = (destPos - curPos).normalized;
                return;
            }

            length_trace += (p - prevPos).magnitude;
            prevprevPos = prevPos;
            prevPos = p;
            listTraces.RemoveAt(0);
        }
        destPos = (prevPos - prevprevPos).normalized * (speed - length_trace) + prevPos;
        direction = (destPos - curPos).normalized;
        return;
    }

    public void OnNewRoad(GameObject road)
    {
        DriveRoute dr = road.GetComponent<DriveRoute>();
        if (dr == null)
            return;
    //    dr.ShowPos();
        if (dr.isDestReady() == false)
            return;

        int length = interpolation;
        listTraces = new List<Vector3>(GetTraces(length, dr));
    //    DestroyPos(dr.transform);
    //    ShowPos(listTraces.ToArray(), dr.transform);

    }
    public void OnExitRoad(GameObject road)
    {
        DriveRoute dr = road.GetComponent<DriveRoute>();
        if (dr == null)
            return;
        dr.PrepDestroy();
    //    DestroyPos(dr.transform);
    }

    Vector3[] GetTraces(int n,DriveRoute dr)
    {
        Vector3 st = transform.position;
        int rnd = Random.Range(0, dr.lstOut.Count);
        Vector3 ed = dr.lstOut[rnd];
  //      print("Type : " + lastRoadType + "  ->>  " + dr.GetRoadType());
        if (lastRoadType==5)
        {
            if(dr.GetRoadType()==5)
            {
                ed = dr.lstOut[0];
            }else if(dr.GetRoadType() == 3)
            {
                ed = dr.lstOut[0];
            }
        }else if(lastRoadType==3)
        {
            if (dr.GetRoadType() == 5)
            {
                ed = dr.lstOut[0];
            }
        }
        if (dr.disableStraight)
            ed = dr.lstOut[0];

        lastRoadType = dr.GetRoadType();
        st.y = ed.y;

        Vector3 vec = transform.forward;

        Vector3 med_v = Vector3.Dot(vec.normalized, ed - st) * vec.normalized;
        med_v.y = 0f;
        Vector3 med = med_v + st;
  //      print((ed - med).magnitude);
        float len = (med - st).magnitude + (ed - med).magnitude;
        if((ed - med).magnitude < th_corner|| (med - st).magnitude < th_corner || !dr.isBranch()  )
        {
            med = ed;
            len = (med - st).magnitude;
        }
        float delta = len / (float)(n-1);
        Vector3[] traces = new Vector3[n];

        traces[0] = st;
 //       traces[n - 1] = ed;
        float cur_len = 0f;
        float len2med = (med - st).magnitude - delta;
        for (int i=1;i<n;i++)
        {
            Vector3 dir = (cur_len < len2med ? (med - st).normalized : (ed - med).normalized);
            traces[i] = traces[i-1] + dir * delta;
            cur_len += delta;
        }
        return traces;
    }
    public void ShowPos(Vector3[] traces,Transform tr)
    {

        foreach (Vector3 p in traces)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "traces";
            sphere.transform.localScale = Vector3.one;
            Material mat = new Material(Shader.Find("Specular"));
            mat.color = Color.blue;
            sphere.GetComponent<Renderer>().material = mat;
            sphere.layer = LayerMask.NameToLayer("Ignore Raycast");
            sphere.transform.position = p;
            sphere.transform.parent = tr;
            sphere.GetComponent<Collider>().enabled = false;
        }
    }
    void DestroyPos(Transform tr)
    {
        MeshFilter[] ms = tr.GetComponentsInChildren<MeshFilter>();
        foreach(MeshFilter m in ms)
        {
            if (m.gameObject.name.Contains("traces"))
                Destroy(m.gameObject);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.name.Contains("scp_") || collision.gameObject.name.Contains("jeep"))
            return;
        if(collision.gameObject.GetComponent<User_Object>()!=null)
        {
            MissionSystem missionSys = GameObject.FindObjectOfType<MissionSystem>();
            missionSys.Get_Current_Mission().num_hit_by_car += 1;
        }
        bCollision = true;
    }
    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.name.Contains("scp_") || collision.gameObject.name.Contains("jeep"))
            return;
        bCollision = false;
    }

}
