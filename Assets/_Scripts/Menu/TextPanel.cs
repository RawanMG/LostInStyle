using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TextPanel : MonoBehaviour 
{
	public Text tipsText{get;set;}

	// Use this for initialization
	void Awake () 
	{
		tipsText = gameObject.GetComponentInChildren<Text>();
	}
	
	// Update is called once per frame
	void Update () 
	{
		// if tips is empty, disactive myself
		if (tipsText.text == "")
		{
			// gameObject.SetActive(false);
		}
	}
}
