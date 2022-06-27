using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

//******************************************************************************************************************//
// find all the objects with the specify component from the whole scene
//******************************************************************************************************************//
public class Finder : EditorWindow
{

	//Object obj;
	static GameObject[] gameObjs;

//	[MenuItem ("ME/Find Look At This Point")]
//	static void FindLookAtThisPoint () 
//	{
//		// Specify a class here
//		LookAtThisPoint[] monos = GameObject.FindObjectsOfType<LookAtThisPoint>() as LookAtThisPoint[];
//		EditorWindow.GetWindow<MyWindow>().Show();
//		List<GameObject> goes = new List<GameObject>();
//		foreach(LookAtThisPoint mo in monos)
//		{
//			goes.Add(mo.gameObject);
//		}
//		gameObjs = goes.ToArray();
//		Selection.objects = gameObjs;
//	}

	[MenuItem ("Finder/Sensors")]
	static void Find_Sensors ()
	{
		// Specify a class here
		Sensor[] monos = GameObject.FindObjectsOfType<Sensor> () as Sensor[];
		EditorWindow.GetWindow<Finder> ().Show ();
		List<GameObject> goes = new List<GameObject> ();
		foreach (Sensor mo in monos) 
		{
			goes.Add (mo.gameObject);
		}
		gameObjs = goes.ToArray ();
		Selection.objects = gameObjs;
	}

	[MenuItem ("Finder/Disable Mesh Render")]
	static void Find_Disable_Mesh_Render ()
	{
		// Specify a class here
		DisableMeshRender[] monos = GameObject.FindObjectsOfType<DisableMeshRender> () as DisableMeshRender[];
		EditorWindow.GetWindow<Finder> ().Show ();
		List<GameObject> goes = new List<GameObject> ();
		foreach (DisableMeshRender mo in monos) 
		{
			goes.Add (mo.gameObject);
		}
		gameObjs = goes.ToArray ();
		Selection.objects = gameObjs;
	}

	[MenuItem ("Finder/Escalator Level")]
	static void Find_Escalator_Level ()
	{
		// Specify a class here
		Escalator_Level[] monos = GameObject.FindObjectsOfType<Escalator_Level> () as Escalator_Level[];
		EditorWindow.GetWindow<Finder> ().Show ();
		List<GameObject> goes = new List<GameObject> ();
		foreach (Escalator_Level mo in monos) 
		{
			goes.Add (mo.gameObject);
		}
		gameObjs = goes.ToArray ();
		Selection.objects = gameObjs;
	}

	[MenuItem ("Finder/Elevator platform")]
	static void Find_Elevator_Platform ()
	{
		// Specify a class here
		Elevator_Platform[] monos = GameObject.FindObjectsOfType<Elevator_Platform> () as Elevator_Platform[];
		EditorWindow.GetWindow<Finder> ().Show ();
		List<GameObject> goes = new List<GameObject> ();
		foreach (Elevator_Platform mo in monos) 
		{
			goes.Add (mo.gameObject);
		}
		gameObjs = goes.ToArray ();
		Selection.objects = gameObjs;
	}

	void OnGUI ()
	{
		if (gameObjs.Length > 0) {
			foreach (GameObject obj in gameObjs) {
				EditorGUILayout.ObjectField (obj, typeof(GameObject));
			}
		}
		else
		{
			EditorWindow.GetWindow<Finder> ().Close();
		}

	}
}
