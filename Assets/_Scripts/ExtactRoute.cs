using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ExtactRoute : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        MeshFilter[] mfs = GetComponentsInChildren<MeshFilter>();
        foreach (MeshFilter mf in mfs)
        {
            if (!mf.sharedMesh.name.Contains("road"))
                continue;
            Vector3[] verts=mf.sharedMesh.vertices;
            if (mf.gameObject.GetComponent<DriveRoute>() != null)
                continue;
            DriveRoute dr = mf.gameObject.AddComponent<DriveRoute>();
            if(mf.sharedMesh.name.Contains("01"))
            {
                Vector3 o = 0.5f*verts[7] + 0.5f*verts[8];
                o = mf.transform.localToWorldMatrix.MultiplyPoint3x4(o);
                dr.Add(o);
                dr.SetRoadType(1);
            }
            if ( mf.sharedMesh.name.Contains("02"))
            {
                Vector3 o;
                if (mf.transform.localScale.x < 0)
                    o = 0.5f * verts[0] + 0.5f * verts[3];
                else
                {
                    o = 0.5f * verts[1] + 0.5f * verts[5];
               //     o = verts[1];
                }
                o = mf.transform.localToWorldMatrix.MultiplyPoint3x4(o);
                dr.Add(o);
                dr.SetRoadType(2);
            }
            if (mf.sharedMesh.name.Contains("03"))
            {
                Vector3 o = 0.5f * verts[0] + 0.5f * verts[2];
                o = mf.transform.localToWorldMatrix.MultiplyPoint3x4(o);
                dr.Add(o);
                o = 0.25f * verts[1] + 0.75f * verts[2];
                o = mf.transform.localToWorldMatrix.MultiplyPoint3x4(o);
                dr.Add(o);
                dr.SetRoadType(3);
            }
            if (mf.sharedMesh.name.Contains("04"))
            {
                Vector3 o = 0.5f * verts[0] + 0.5f * verts[2];
                o = mf.transform.localToWorldMatrix.MultiplyPoint3x4(o);
                dr.Add(o);
                dr.SetRoadType(4);
            }
            if (mf.sharedMesh.name.Contains("05"))
            {
                Vector3 o = 0.5f * verts[4] + 0.5f * verts[11];
                o = mf.transform.localToWorldMatrix.MultiplyPoint3x4(o);
                dr.Add(o);
                o = 0.5f * verts[16] + 0.5f * verts[18];
                o.z = verts[4].z;
                o = mf.transform.localToWorldMatrix.MultiplyPoint3x4(o);
                dr.Add(o);
                dr.SetRoadType(5);
            }
            //  dr.ShowPos();
        }

    }
    private void OnDisable()
    {
        DriveRoute[] drs = GetComponentsInChildren<DriveRoute>();
        foreach (DriveRoute dr in drs)
        {
            dr.PrepDestroy();
            DestroyImmediate(dr);
        }
    }
    private void OnEnable()
    {
        Awake();
    }
}
