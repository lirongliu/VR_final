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
			if (objTransform.CompareTag(tag)) {
				return true;
			}
			objTransform = objTransform.parent;
		}
		
		if (objTransform.CompareTag(tag)) {
			return true;
		}
		
		return false;
		
	}

	public static float getVectorAngle(Vector3 from, Vector3 to) {
		float angle = Vector3.Angle(from, to);
		return Mathf.Abs (angle);
	}

	public static GameObject getTabletPlayerAvatar() {
		return GameObject.FindGameObjectWithTag (Constants.tabletPlayerAvatarTag);
	}
	
	public static GameObject getCbPlayerAvatar() {
		return GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
