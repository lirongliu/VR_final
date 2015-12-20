using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Utility : MonoBehaviour {
	
	/* get scripts */
	public static GameController getGameController() {
		return GameObject.Find ("GameController").GetComponent<GameController> ();
	}
	public static TbInstructionController getTbInstructionController() {
		return GameObject.FindWithTag (Constants.tbNetworkedPlayerTag).GetComponent<TbInstructionController> ();
	}
	public static CbInstructionController getCbInstructionController() {
		return GameObject.FindWithTag (Constants.cbNetworkedPlayerTag).GetComponent<CbInstructionController> ();
	}
	
	/* get Objects */
	public static GameObject getTabletPlayerAvatar() {
		return GameObject.FindGameObjectWithTag (Constants.tbPlayerAvatarTag);
	}
	
	public static GameObject getCbPlayerAvatar() {
		return GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);
	}
	public static GameObject getCbPlayer() {
		return GameObject.FindGameObjectWithTag (Constants.cbNetworkedPlayerTag);
	}
	public static GameObject getTbPlayer() {
		return GameObject.FindGameObjectWithTag (Constants.tbNetworkedPlayerTag);
	}

//	public static Text getInstructionText() {
//		return FindTransform(GameObject.FindWithTag ("instruction").transform, "Text").GetComponent<Text>();
//	}

	public static bool playersAreReady() {
		return NetworkController.iAmReady && NetworkController.otherPlayerReady;
	}

	public static bool isAncestor(Transform t1, Transform t2) {
		
		while (t2 != t2.root) {
			if (t2.parent == t1) {
				return true;
			}
		}
		return false;
	}

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

	
	public static GameObject CreateEnemy(float x_position,float z_position, float maxLife, string type)
	{
		Object prefab;
		GameObject enemyObj;
		if (type == "boss") {
			prefab = Resources.Load ("boss");
			
			enemyObj = Instantiate (prefab, new Vector3 (x_position,1,z_position), Quaternion.identity) as GameObject;
			
			//		GameObject enemyObj = Instantiate(prefab) as GameObject;
			BossController bossController = enemyObj.GetComponent<BossController>();
			bossController.config(maxLife, type);
		} else {
			prefab = Resources.Load("Enemy");
			
			enemyObj = Instantiate (prefab, new Vector3 (x_position,1,z_position), Quaternion.identity) as GameObject;
			
			//		GameObject enemyObj = Instantiate(prefab) as GameObject;
			NormalEnemyController normalEnemyController = enemyObj.GetComponent<NormalEnemyController>();
			normalEnemyController.config(maxLife, type);
		}
		return enemyObj;
	}

	public static Vector3 movementAdjustedWithFPS(Vector3 displacementVec) {
		return displacementVec * Time.deltaTime;
	}

	// not working...
	public static void resetScene() {
//		GameObject[] GameObjects = (FindObjectsOfType<GameObject>() as GameObject[]);
//		
//		for (int i = 0; i < GameObjects.Length; i++) {
//			Destroy(GameObjects[i]);
//		}
		Application.LoadLevel (0);
	}

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
