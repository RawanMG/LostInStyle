//using System.Collections;
//using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class PortalOrigin : MonoBehaviour {

    void Awake()
    {
        DisableMeshRender dmr = gameObject.GetComponent<DisableMeshRender>();
        if(dmr==null)
        {
            gameObject.AddComponent<DisableMeshRender>();
        }
        RaycastSkipTag rst = gameObject.GetComponent<RaycastSkipTag>();
        if(rst==null)
        {
            gameObject.AddComponent<RaycastSkipTag>();
        }
        Collider cdr = gameObject.GetComponent<Collider>();
        if(cdr!=null)
        {
            cdr.enabled = false;
        }
    }

}
