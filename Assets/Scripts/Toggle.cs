using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Toggle : MonoBehaviour {

	public void Do()
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
	
}
