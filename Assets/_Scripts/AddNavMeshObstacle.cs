using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[ExecuteInEditMode]
public class AddNavMeshObstacle : MonoBehaviour {

    void Awake()
    {
        Debug.Log("Awake AddNavMeshObstacle");
        BoxCollider[] bc = GetComponentsInChildren<BoxCollider>();
        foreach(BoxCollider b in bc)
        {
            Debug.Log("Check navmeshobstacle : " + b.transform.parent.gameObject.name);
            if (b.gameObject.GetComponent<NavMeshObstacle>() == null)
            {
                NavMeshObstacle nmo = b.gameObject.AddComponent<NavMeshObstacle>();
                nmo.carving = false;
                Debug.Log("Add navmeshobstacle : " + b.transform.parent.gameObject.name);
            }
        }
    }

}
