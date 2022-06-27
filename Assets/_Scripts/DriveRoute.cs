using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DriveRoute : MonoBehaviour {
    public List<Vector3> lstOut = new List<Vector3>();
    int road_type=1;
//    public bool disableRight = false;
    public bool disableStraight = false;
    public void Add(Vector3 o)
    {
        lstOut.Add(o);
    }
    public bool isBranch()
    {
        if (lstOut.Count == 1 && road_type !=4)
            return false;
        return true;
    }
    // Use this for initialization
    void Start () {
        if (gameObject.name.Contains("_01"))
        {
            road_type = 1;
        }
        if (gameObject.name.Contains("_02"))
        {
            road_type = 2;
        }
        if (gameObject.name.Contains("_03"))
        {
            road_type = 3;
        }
        if (gameObject.name.Contains("_04"))
        {
            road_type = 4;
        }
        if (gameObject.name.Contains("_05"))
        {
            road_type = 5;
            int[] x = { 35, 81, 120, 85, 23, 45, 132, 94, 127, 93 };
            foreach(int xx in x)
            {
                if(gameObject.name.Contains("(" + xx + ")"))
                {
                    disableStraight = true;
                }
            }
        }

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void ShowPos()
    {
        
        foreach (Vector3 p in lstOut)
        {
            GameObject sphere = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            sphere.name = "SphereMarker";
            sphere.transform.localScale = Vector3.one;
            Material mat = new Material(Shader.Find("Specular"));
            mat.color = Color.red;
            sphere.GetComponent<Renderer>().material = mat;
            sphere.layer = LayerMask.NameToLayer("Ignore Raycast");
            sphere.transform.position = p;
            sphere.transform.parent = transform;
            sphere.GetComponent<Collider>().enabled = false;
        }
    }
    public void PrepDestroy()
    {
        Renderer[] rs = GetComponentsInChildren<Renderer>();
        foreach(Renderer r in rs)
        {
            GameObject go = r.gameObject;
            if (!go.name.Contains("SphereMarker"))
                continue;
            Destroy(go);
        }
    }
    /*
    public Vector3 GetDestinationRand(out bool forced)
    {
        int n = Random.Range(0, lstOut.Count);
        return lstOut[n];
    }
    public Vector3 GetDestinationDefault()
    {
        return lstOut[0];
    }
    */

    public bool isDestReady()
    {
        if (lstOut.Count == 0)
        {
            print("NO data");
            return false;
        }
        return true;
    }
    public void SetRoadType(int n)
    {
        road_type = n;
    }
    public int GetRoadType()
    {
        return road_type;
    }
}
