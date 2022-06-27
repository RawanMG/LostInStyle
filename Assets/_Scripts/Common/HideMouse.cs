using UnityEngine;
using System.Collections;

public class HideMouse : MonoBehaviour {

	public Texture2D cursorTex;
	// Use this for initialization
	void Start () 
	{
		Cursor.SetCursor(cursorTex,Vector2.zero, CursorMode.ForceSoftware);
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		//if (Cursor.visible)
		//	Cursor.visible = false;
		Cursor.lockState = CursorLockMode.Locked;
	}

	void OnDestroy() 
	{
		Cursor.SetCursor(null,Vector2.zero, CursorMode.ForceSoftware);
		Cursor.lockState = CursorLockMode.None;
	}


}
