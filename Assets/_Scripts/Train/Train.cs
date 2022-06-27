using UnityEngine;
using System.Collections;

public class Train : MonoBehaviour {

	public enum Type
	{
		In,
		Out
	}

	public Type type = Type.In;


	public bool atStation = false; // at station at the beginning

	Animator anim;

	public float delayTime = 180.0f;
	public float randomTime = 120.0f;
	public float currentTime = -1.0f;

	// Use this for initialization
	void Start () 
	{
		anim = gameObject.GetComponent<Animator>();

		if (atStation)
		{
			anim.Play(type.ToString(),0, 0.75f);
			
			// reset the time
			currentTime = Random.Range(0.0f,randomTime) + delayTime;
		}

	}
	
	// Update is called once per frame
	void Update () 
	{
		if (currentTime > 0.0f)
		{
			currentTime -= Time.deltaTime;
		}
		else
		{
			anim.Play(type.ToString(),0, 0.0f);
		
			// reset the tim
			currentTime = Random.Range(0.0f,randomTime) + delayTime;
		}
	}


}
