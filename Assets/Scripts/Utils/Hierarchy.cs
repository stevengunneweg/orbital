/** 
 * Written by Simon Karman - www.simonkarman.nl
 * 
 * The Hierarchy class is used to easily find 'singleton' like objects in the Unity Scene.
 * The GetComponentWithTag function will search for a game object with the provided tag or the tag with the same name of the class
 */

using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Hierarchy : MonoBehaviour 
{
	public static T GetComponentWithTag<T>() where T : Component
	{
		return GetComponentWithTag<T>(typeof(T).Name);
	}
	
	public static T GetComponentWithTag<T>(string tag) where T : Component
	{
		foreach (GameObject g in GameObject.FindGameObjectsWithTag(tag))
		{
			T t = g.GetComponent<T>();
			if (t != null)
				return t;
		}
		return null;
	}
	
	
}
