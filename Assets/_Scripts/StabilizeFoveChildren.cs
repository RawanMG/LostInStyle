using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VR;

public class StabilizeFoveChildren : MonoBehaviour {

    public Transform trParent = null;
    Vector3 orgPos;
    Quaternion orgRot;
    Vector3 glScale;
    public bool preserveInitialRotation = true;

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    void Awake()
    {
        if (UnityEngine.VR.VRSettings.enabled)
        {
            //            trParent = transform.parent;
            trParent = GetHead();
            orgPos = trParent.worldToLocalMatrix.MultiplyPoint3x4(transform.position);
            orgRot = transform.localRotation;
            glScale = transform.lossyScale;
            transform.parent = null;
        }
    }

    Transform GetHead()
    {
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
        return head.transform;
    }
    private void FixedUpdate()
    {
        if(trParent!=null)
        {
            Vector3 newPos = trParent.localToWorldMatrix.MultiplyPoint3x4(orgPos);
            transform.position = newPos;
            if (preserveInitialRotation)
                transform.rotation = trParent.rotation;
            transform.localScale = glScale;
//            trMap.Rotate(new Vector3(90, 180f, 0f));

        }
    }
}
