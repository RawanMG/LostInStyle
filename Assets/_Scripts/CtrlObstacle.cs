using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class CtrlObstacle : MonoBehaviour
{

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {

    }
    void OnTriggerEnter(Collider other)
    {
        CtrlObstacleCollider coc = GetComponentInParent<CtrlObstacleCollider>();
   //     print(other.gameObject.name);
        if(other.gameObject.GetComponent<User_Object>()!=null)
            coc.OnPassed(GetComponent<Collider>());
    }
}
