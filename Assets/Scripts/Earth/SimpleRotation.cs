using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleRotation : MonoBehaviour {

    [SerializeField]
    Vector3 axis;
    [SerializeField]
    float anglePerFrame;


	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        transform.Rotate(axis, anglePerFrame);
	}
}
