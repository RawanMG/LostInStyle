using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(WaypointSystem))]
public class WaypointSystemEditor : Editor {

	public override void OnInspectorGUI ()
	{
		base.OnInspectorGUI ();
		DrawDefaultInspector();

		WaypointSystem wps = (WaypointSystem)target;

		if (GUILayout.Button ("Save To...")) {
			string filePath =  EditorUtility.SaveFilePanel ("Select a file for heatmap",Application.dataPath + "/Path","path","txt");
			if (!string.IsNullOrEmpty (filePath)) {
				wps.SaveToFile (filePath);
			}
		}
	}

}
