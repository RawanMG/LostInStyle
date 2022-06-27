using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaycastSkipTag : MonoBehaviour {
    void Awake()
    {
        gameObject.layer = LayerMask.NameToLayer("Ignore Raycast");
    }
}
