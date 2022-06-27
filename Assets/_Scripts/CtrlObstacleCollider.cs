using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class CtrlObstacleCollider : MonoBehaviour {
    List<Collider> passedCollider;

	// Use this for initialization
	void Start () {
        init();
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    void Awake()
    {
        BoxCollider[] bc = GetComponentsInChildren<BoxCollider>();
        foreach (BoxCollider b in bc)
        {
            if (b.gameObject.GetComponent<CtrlObstacle>() == null)
            {
                b.gameObject.AddComponent<CtrlObstacle>();
            }
        }
  //      init();
    }
    public void init()
    {

        passedCollider = new List<Collider>();
		if (GData.playMode == GData.PlayMode.Replayer)
			return;
        MeshRenderer[] mr = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer m in mr)
        {
            m.enabled = false;
            Collider c = m.gameObject.transform.parent.gameObject.GetComponentInChildren<Collider>();
            c.isTrigger = true;
        }
        NavMeshObstacle[] nmos = GetComponentsInChildren<NavMeshObstacle>();
        foreach(NavMeshObstacle nmo in nmos)
        {
            nmo.carving = false;
        }
    }
    public void OnPassed(Collider c)
    {
		if (GData.playMode == GData.PlayMode.Replayer)
			return;
        MissionSystem missionSys = FindObjectOfType<MissionSystem>();
        if (missionSys.Get_Current_Mission().bGo )
        {
            passedCollider.Add(c);
            Debug.Log("Passed:" + c.transform.parent.name);
        }
    }
    public int GetPassedObstaclesNum()
    {
        return passedCollider.Count;
    }
    public void ActivatePassedObstacles(int num)
    {
        int passedNum = GetPassedObstaclesNum();
        if (num > passedNum)
            num = passedNum;
        List<Collider> shuffled = Shuffle(passedCollider, new System.Random());
        for(int i=0;i<num;i++)
        {
            shuffled[i].isTrigger = false;
            MeshRenderer[] mr = shuffled[i].transform.parent.GetComponentsInChildren<MeshRenderer>();
            foreach (MeshRenderer m in mr)
            {
                if (m.gameObject != shuffled[i].gameObject)
                    m.enabled = true;
            }
            NavMeshObstacle nmo = shuffled[i].GetComponent<NavMeshObstacle>();
            nmo.carving = true;
            Debug.Log("Activate Obstacle : " + shuffled[i].transform.parent.gameObject.name);
        }
    }
    public void ActivateObstacles(int num)
    {
        Collider[] cdr = GetComponentsInChildren<Collider>();
        List<Collider> list = new List<Collider>(cdr);

        int passedNum = list.Count;
        if (num > passedNum)
            num = passedNum;
        List<Collider> shuffled = Shuffle(list, new System.Random());
        for (int i = 0; i < num; i++)
        {
            shuffled[i].isTrigger = false;
            MeshRenderer[] mr = shuffled[i].transform.parent.GetComponentsInChildren<MeshRenderer>();
            foreach(MeshRenderer m in mr )
            {
                if (m.gameObject != shuffled[i].gameObject)
                    m.enabled = true;
            }
            NavMeshObstacle nmo = shuffled[i].GetComponent<NavMeshObstacle>();
            nmo.carving = true;
            Debug.Log("Activate Obstacle : " + shuffled[i].transform.parent.gameObject.name);
        }
    }
    List<Collider> Shuffle(List<Collider> org, System.Random rng)
    {
        List<Collider> list = new List<Collider>(org);
        int n = list.Count;
        while (n > 1)
        {
            n--;
            int k = rng.Next(n + 1);
            Collider value = list[k];
            list[k] = list[n];
            list[n] = value;
        }
        return list;
    }
}

