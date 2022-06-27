using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class AddAdvertisement : MonoBehaviour {


    private void Awake()
    {
        Advertisement[] ads = GetComponentsInChildren<Advertisement>();
        GameObject[] gos = FindObjectsOfType<GameObject>();
        foreach(GameObject go in gos)
        {
            print(go.name);
            if(go.name.Contains(name) && go.GetComponentInChildren<Advertisement>() == null)
            {
                foreach(Advertisement ad in ads)
                {
                    Vector3 pos = ad.transform.parent.transform.worldToLocalMatrix.MultiplyPoint3x4(ad.transform.position);
                    Quaternion rot = ad.transform.localRotation;
                    Vector3 scale = ad.transform.localScale;

                    GameObject clone = GameObject.Instantiate(ad.gameObject);
                    clone.transform.position = go.transform.localToWorldMatrix.MultiplyPoint3x4(pos);
                    clone.transform.localRotation = go.transform.rotation*rot;
//                    clone.transform.rotation = rot;
//                    clone.transform.localScale = scale;
                    clone.transform.parent = go.transform;
                }
            }
        }
    }
}
