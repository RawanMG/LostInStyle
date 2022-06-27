using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CalbiScreen : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		if (Input.GetButtonDown("start")){
            //   UnityEngine.SceneManagement.Scene sc = UnityEngine.SceneManagement.SceneManager.GetActiveScene();
            //   UnityEngine.SceneManagement.SceneManager.LoadScene (sc.name);
            init();
		}
//		if (Input.GetKeyDown (KeyCode.Alpha1)) {
//			UnityEngine.SceneManagement.SceneManager.LoadScene ("Kenmore");
//		}
//
	}

    public void init()
    {
        MissionSystem mSys = GameObject.FindObjectOfType<MissionSystem>();
        mSys.init();
        VRMainSystem mainSys = GameObject.FindObjectOfType<VRMainSystem>();
        mainSys.init();
        CtrlObstacleCollider coc = GameObject.FindObjectOfType<CtrlObstacleCollider>();
        coc.init();
		if (GData.playMode == GData.PlayMode.ValidationExp) {
			CommStressMeter csm = GameObject.FindObjectOfType<CommStressMeter> ();
			if (csm != null)
				csm.init ();
		}
    }
}
