using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class ScoreTextUpdater : MonoBehaviour {
    Text textField;

	// Use this for initialization
	void Start () {
        textField = GetComponent<Text>();
	}
	
	// Update is called once per frame
	void Update () {
        textField.text = Mathf.Floor(Score.Value).ToString();
	}
}
