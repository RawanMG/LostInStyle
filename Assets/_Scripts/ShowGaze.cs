using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Fove.Managed;
public class ShowGaze : MonoBehaviour {
    Fove.Managed.SFVR_GazeConvergenceData gazeConv;
    Fove.Managed.FVRHeadset headset;
    public bool show = false;
    public Transform tr;
    Transform foveTr;
    public void init()
    {
        gazeConv = FoveInterface2.GetFVRHeadset().GetGazeConvergence();
        headset = FoveInterface2.GetFVRHeadset();
    }
    Vector3 Conv(Fove.Managed.SFVR_Vec3 v)
    {
        return new Vector3(v.x, v.y, v.z);
    }
    // Use this for initialization
    void Start() {
        //   init();
        foveTr = FindObjectOfType<FoveInterface2>().transform;
    }

    // Update is called once per frame
    void Update() {
        ShowGazeSphere();
    }

    void ShowGazeSphere()
    {
        if (show )
        {
            gazeConv = FoveInterface2.GetFVRHeadset().GetGazeConvergence();
            RaycastHit hit;

           Physics.Raycast(foveTr.position, foveTr.rotation * Conv(gazeConv.ray.direction), out hit, Mathf.Infinity);
            if (hit.point != Vector3.zero)
            {
                if (tr != null)
                    tr.position = hit.point;
            }
            else
            {
              //  print("nogaze");
            }
        }
    }
}
