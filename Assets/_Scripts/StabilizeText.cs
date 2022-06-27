using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StabilizeText : MonoBehaviour {
	// Use this for initialization
	void Start () {
		
	}
    RenderTexture rTex;
	// Update is called once per frame
	void Update () {
		
	}
    private void Awake()
    {
        Camera cam = GetComponent<Camera>();
        RenderTexture rt = new RenderTexture(cam.pixelWidth, cam.pixelHeight, 0, RenderTextureFormat.ARGB32);
        rt.Create();
      //  cam.targetTexture = rt;
        rTex = rt;
    }
    
    void OnRenderImage(RenderTexture source, RenderTexture destination)
    {
        //DO NOT WORK ???? Oct 12, 2017
        RenderTexture tmp = RenderTexture.GetTemporary(source.width, source.height, 24, RenderTextureFormat.ARGBHalf);
        RenderTexture.active = tmp;
        GL.Clear(false, true, new Color(1.0f, 0f, 0f, 0f));
        Graphics.Blit(source, destination);
        RenderTexture.ReleaseTemporary(tmp);
    }
    /*
    private void OnPreRender()
    {
        RenderTexture tmp = RenderTexture.GetTemporary(rTex.width, rTex.height, 24, RenderTextureFormat.ARGBHalf);
        RenderTexture.active = tmp;
        GL.Clear(false, true, new Color(1.0f, 0f, 0f, 0f));
        Graphics.Blit(rTex, null as RenderTexture);
        RenderTexture.ReleaseTemporary(tmp);

    }
    */
}
