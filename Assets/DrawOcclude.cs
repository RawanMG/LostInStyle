using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawOcclude : MonoBehaviour {
	public Transform bTransform;
	public Material lineMat;
	// Use this for initialization
	void Start () {
		lineMat = GetComponent<Material> ();
		drawOccludeFactor (bTransform.position, 100, 5.0f);
	
		
	}

	private float drawOccludeFactor(Vector3 myPos, int sampleSize, float radius){
		float factor = 0;

		for (int i = 0; i < sampleSize; i++) {
			float u = UnityEngine.Random.value;
			float v = UnityEngine.Random.value;
			Object[] mat =  Resources.LoadAll ("Materials", typeof(Material));
			if (lineMat == null)
				foreach(Material m in mat){
					if(m.name == "LineMat")
						lineMat = (Material) m;
				}
			Debug.Log ("material " + lineMat.name);
			float theta = Mathf.Acos (1 - u); 
			float phi = 2 * Mathf.PI * v; 

			float z = Mathf.Sin (theta) * Mathf.Cos (phi);
			float y = Mathf.Sin (theta) * Mathf.Sin (phi);
			float x = Mathf.Cos (theta); 

			Vector3 dir = bTransform.rotation *  new Vector3 (x, y, z);
			RaycastHit hit; 
//			Debug.DrawRay (myPos, radius*dir, Color.blue, 1000);
			GameObject newObj = new GameObject();
			LineRenderer lineRend = newObj.AddComponent<LineRenderer> ();
			lineRend.useWorldSpace = true;
			lineRend.SetWidth (0.1f, 0.1f);
			lineRend.numCapVertices = 2;

			lineRend.material = lineMat;
			Vector3 startP = myPos;
			Vector3 endP = startP + radius * dir.normalized;
			lineRend.SetPosition (0, startP);
			lineRend.SetPosition (1, endP);
			LayerMask ignoreLayer = ~(LayerMask.GetMask("Sign")| LayerMask.GetMask("Ceiling") | LayerMask.GetMask("Airwall") | LayerMask.GetMask("Portal"));
			if (Physics.Raycast (myPos, dir, out hit, radius, ignoreLayer))
			if(!hit.collider.gameObject.CompareTag("ADCase")) //not colliding with itself or other banners
				factor += 1;
		}

		return factor;

	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
