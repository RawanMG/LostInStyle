using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class PortalSystem : MonoBehaviour 
{
	Dictionary<string, Portal_Sensor> protals;

	// public string spwan_portal = "";

	public Dictionary<string, Portal_Sensor> Get_Protals() 
	{
		if (protals == null)
		{
			Init();
		}
		return protals;
	}

	// Use this for initialization
	void Awake()
	{

	}


	void Start () 
	{
	
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void Init()
	{
		protals = new Dictionary<string, Portal_Sensor>();
		Portal_Sensor[] ps = GameObject.FindObjectsOfType<Portal_Sensor>();
		foreach(Portal_Sensor p in ps)
		{
			protals.Add(p.name, p);
		}
	}

	public void Place_Player(string portal_name)
	{
		if (protals == null)
		{
			Init();
		}

		if (portal_name != "")
		{
			User_Object po = GameObject.FindObjectOfType<User_Object>() as User_Object;
			if (po)
			{
				if (protals.ContainsKey (portal_name)) {
					if (protals [portal_name].startingPoint != null) {
						po.transform.position = protals [portal_name].startingPoint.transform.position;
						po.transform.rotation = protals [portal_name].startingPoint.transform.rotation;
					}

				} else {
					Debug.Log ("NO portal with the name " + portal_name);
				}
			}
		}

	}


}
