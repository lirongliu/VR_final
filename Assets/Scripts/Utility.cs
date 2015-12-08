using UnityEngine;
using System.Collections;

public class Utility : MonoBehaviour {

	
	public static Transform FindTransform(Transform parent, string name)
	{
		if (parent.name.Equals(name)) return parent;
		foreach (Transform child in parent)
		{
			Transform result = FindTransform(child, name);
			if (result != null) return result;
		}
		return null;
	}

	public static bool checkTag(Transform objTransform, string tag){
		while (objTransform != objTransform.root) {
			if (objTransform.tag == tag) {
				return true;
			}
			objTransform = objTransform.parent;
		}
		
		if (objTransform.tag == tag) {
			return true;
		}
		
		return false;
		
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
