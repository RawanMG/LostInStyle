using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class AD : MonoBehaviour 
{
	public enum Type
	{
		Only_Image,
		Only_Text,
		Either,
		Both
	}
	public Type type;

	public bool autoGenerate = false;


	public float delayTime = 10.0f;
	float currentTime = 0.0f;

	public float randomlyTime = 5.0f;

	// fixed material
	public Material mat;


	[Multiline (5)]
	public string[] ADInfos; // this is for showing the ADs

	Text info;
	public Image img; // must sign this value

	public Sprite[] ADSprites;
	MeshRenderer render;



	// Use this for initialization
	void Awake () 
	{
		// fixed material
		render = GetComponent<MeshRenderer> ();
		render.material = mat;

		info = gameObject.GetComponentInChildren<Text>()	;
		if (info != null)
			info.text = "";
		if (!img)
			img = gameObject.GetComponentInChildren<Image>();

		if (autoGenerate)
			NextAD();
	}
	
	// Update is called once per frame
	void Update () 
	{
//		return;
//		render.enabled = true;
//
//		if (currentTime > 0.0f)
//		{
//			currentTime -= Time.deltaTime;
//		}
//		else
//		{
//			NextAD();
//		}
	}

	// *************************************************************************************************************
	// next AD
	// *************************************************************************************************************

	void NextAD()
	{
		img.sprite = null;
		if (info != null)
			info.text = "";
		img.enabled = false;

		switch (type)
		{
		case Type.Only_Image:
		{
			img.enabled = true;
			img.sprite = ADSprites[Random.Range(0, ADSprites.Length)];

		}
			break;

		case Type.Only_Text:
		{
			info.text = ADInfos[Random.Range(0, ADInfos.Length)];
		}
			break;

		case Type.Either:
		{
			if (Random.Range(0,2) == 1)
			{
				img.enabled = true;
				img.sprite = ADSprites[Random.Range(0, ADSprites.Length)];
			}
			else
			{
				info.text = ADInfos[Random.Range(0, ADInfos.Length)];
			}
		}
			break;

		case Type.Both:
		{
			img.enabled = true;
			img.sprite = ADSprites[Random.Range(0, ADSprites.Length)];
			info.text = ADInfos[Random.Range(0, ADInfos.Length)];
		}
			break;
		}

		// reset the time
		currentTime = Random.Range(0,randomlyTime) + delayTime;
	}


}
