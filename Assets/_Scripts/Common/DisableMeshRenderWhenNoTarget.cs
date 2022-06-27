using UnityEngine;
using System.Collections;

public class DisableMeshRenderWhenNoTarget : MonoBehaviour {

    // Use this for initialization
    //public bool show = true;
    MissionSystem mSys;
	void Awake () 
	{

		MeshRenderer mr = gameObject.GetComponent<MeshRenderer>();
		if (mr)
		{
			mr.enabled = false;
		}
	}

    private void Start()
    {
  //      mSys = GameObject.FindObjectOfType<MissionSystem>();
      //  gameObject.GetComponent<Portal_Sensor>().locations[0];
    }
    void Update()
    {
        Mission m = GameObject.FindObjectOfType<MissionSystem>().Get_Current_Mission();
        if(m.bGo && m.bTaskGoAndReturn && gameObject.GetComponent<Portal_Sensor>().locations.Contains(m.destination))
        {
            ShowMesh(true);
        }
        else
        {
            ShowMesh(false);
        }
    }

    void ShowMesh(bool bShow)
    {
        MeshRenderer m = GetComponent<MeshRenderer>();
        if (m != null)
        {
            m.enabled = bShow;
        }
        LODGroup lodg = GetComponent<LODGroup>();
        if (lodg != null)
        {
            LOD[] lods = lodg.GetLODs();
            foreach(LOD l in lods)
            {
                Renderer[] r = l.renderers;
                foreach(Renderer rr in r)
                {
                    rr.enabled = bShow;
                }
            }
        }
    }

}
