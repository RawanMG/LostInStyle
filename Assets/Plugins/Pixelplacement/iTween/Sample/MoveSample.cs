using UnityEngine;
using System.Collections;

public class MoveSample : MonoBehaviour
{	
	void Start(){
		iTween.MoveBy(gameObject, iTween.Hash("x", 2, "time", 10,"easeType", "easeInOutExpo", "loopType", "pingPong"));
	}
}

