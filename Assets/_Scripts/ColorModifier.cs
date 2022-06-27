using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[ExecuteInEditMode]
public class ColorModifier : MonoBehaviour {
    public Material originalMat;
    public int div_x = 2;
    public int div_y = 2;

    private void Awake()
    {
        if (originalMat == null)
            return;
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        Bounds bounds = new Bounds(Vector3.zero, Vector3.zero);
        foreach(MeshRenderer mr in mrs)
        {
            bounds.Encapsulate(mr.bounds);
        }
        float width_x = bounds.size.x / (float)div_x;
        float width_y = bounds.size.z / (float)div_y;

        Material[,] mats = new Material[div_x, div_y];
        Bounds[,] bnds = new Bounds[div_x, div_y];
        for(int i=0;i<div_x;i++)
        {
            for(int j=0;j<div_y;j++)
            {
                Material m = Material.Instantiate(originalMat);
                m.name = originalMat.name + "<" + i + "," + j + ">";
                print(m.name);
                Color col = m.color;
                int sc = 1;
                if (i == 1 && j == 1)
                    sc = 2;
                col.r -= ((float)(255/sc / div_x * i)) / 255f;
                col.b -= ((float)(255/sc / div_y * j)) / 255f;
                print(col);
                m.color = col;
                mats[i, j] = m;

                Vector3 c = bounds.center;

                c.x = bounds.center.x - bounds.extents.x;
                c.x += width_x * (float)i + width_x / 2f;
                c.z = bounds.center.z - bounds.extents.z;
                c.z += width_y * (float)j + width_y / 2f;
                Vector3 s = bounds.size;
                s.x = width_x;
                s.z = width_y;
                bnds[i,j] = new Bounds(c, s);
            }
        }
        foreach (MeshRenderer mr in mrs)
        {
            if (mr.GetComponent<Advertisement>() != null)
                continue;
            int i, j;
            if(GetIndex(bnds,mr.transform.position,out i,out j))
            {
                mr.material = mats[i, j];
//                for(int k=0;k<mr.materials.Length;k++)
//                {
//                    if(true ||mr.materials[k].name == originalMat.name)
//                    {
////                        mr.materials[k] = mats[i, j];
//                        mr.sharedMaterials[k] = mats[i, j];
//                        mr.material = mats[i, j];
//                    }
//                }
            }
        }
    }

    bool GetIndex(Bounds[,] bnds,Vector3 p, out int i,out int j)
    {
        for(int x=0;x<div_x;x++)
        {
            for(int y=0;y<div_y;y++)
            {
                i = x;
                j = y;
                if (bnds[i, j].Contains(p))
                    return true;
            }
        }
        i = -1;
        j = -1;
        return false;
    }

    // Use this for initialization
    void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
    private void OnDisable()
    {
        if (originalMat == null)
            return;
        MeshRenderer[] mrs = GetComponentsInChildren<MeshRenderer>();
        foreach (MeshRenderer mr in mrs)
        {
            if (mr.GetComponent<Advertisement>() != null)
                continue;
            mr.material = originalMat;
        }
    }
    private void OnEnable()
    {
        Awake();
    }
}
