using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleScalePulse : MonoBehaviour {

    Vector3 scale;
    [SerializeField]
    float magnitude = 0.1f;
    [SerializeField]
    float speed = 50;

	// Use this for initialization
	void Start () {
        scale = transform.localScale;

    }
	
	// Update is called once per frame
	void Update () {
        transform.localScale = scale * (1+(Mathf.Sin(Time.time * speed) * magnitude));
	}
}
