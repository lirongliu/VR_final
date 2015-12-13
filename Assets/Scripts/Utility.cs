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
		return GameObject.FindGameObjectWithTag (Constants.tbPlayerAvatarTag);
	}
	
	public static GameObject getCbPlayerAvatar() {
		return GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);
	}

	
	public static GameObject CreateEnemy(int x_position,int z_position, float maxLife, string type)
	{
		Object prefab;
		if (type == "boss") {
			prefab = Resources.Load ("boss");
		} else {
			prefab = Resources.Load("Enemy");
		}
		GameObject enemyObj = Instantiate (prefab, new Vector3 (x_position,1,z_position), Quaternion.identity) as GameObject;

//		GameObject enemyObj = Instantiate(prefab) as GameObject;
		EnemyController enemyController = enemyObj.GetComponent<EnemyController>();
		enemyController.config(maxLife, type);
		//do additional initialization steps here
		return enemyObj;
	}

	public static GameController getGameController() {
		return GameObject.Find ("GameController").GetComponent<GameController> ();
	}

	public static Vector3 movementAdjustedWithFPS(Vector3 displacementVec) {
		return displacementVec * Time.deltaTime;
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
