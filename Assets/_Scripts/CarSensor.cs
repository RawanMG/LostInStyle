using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CarSensor : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void Awake()
    {

    }
    private void OnTriggerEnter(Collider other)
    {
    //    print(other.name);
        DriveCar dc = transform.parent.GetComponent<DriveCar>();
        if (dc != null)
            dc.OnNewRoad(other.gameObject);
    }
    private void OnTriggerExit(Collider other)
    {
        DriveCar dc = transform.parent.GetComponent<DriveCar>();
        if (dc != null)
            dc.OnExitRoad(other.gameObject);

    }
}
