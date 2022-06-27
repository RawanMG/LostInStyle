using UnityEngine;
using System.Collections;

public class ManualDriftCorrection : MonoBehaviour
{
	Fove.Managed.SFVR_Vec3 pos; 
    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.M))
        {
            this.GetComponent<Renderer>().enabled = true;
        }

        if (this.GetComponent<Renderer>().enabled)
        {
            RaycastHit hit;
            FoveInterface foveInterface = transform.parent.GetComponent<FoveInterface>();
            Ray ray = new Ray(foveInterface.transform.position, foveInterface.transform.forward);
            if (Physics.Raycast(ray, out hit, 1.0f))
            {
                transform.position = hit.point;
            }
            else
            {
                transform.position = foveInterface.transform.position + foveInterface.transform.forward * 1.5f;
            }
        }

        if (Input.GetKeyUp(KeyCode.M))
        {
            this.GetComponent<Renderer>().enabled = false;
			pos = new Fove.Managed.SFVR_Vec3 ();
			pos.x = transform.localPosition.x;
			pos.y = transform.localPosition.y;
			pos.z = transform.localPosition.z;
			FoveInterface.GetFVRHeadset ().ManualDriftCorrection3D (
				pos);
//			Fove.FoveHeadset.GetHeadset().ManualDriftCorrection3D(transform.localPosition);
        }
    }
}
