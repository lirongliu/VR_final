using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class PunRPCs : MonoBehaviour {
	
	public GameObject Enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	[PunRPC]
	void resetScene() {
		Utility.resetScene();
	}
	
//	
//	IEnumerator showInstructionRoutine(string instructionText, int seconds, GameObject instructionObj) {
//		yield return new WaitForSeconds(seconds);
//		//		print ("showInstruction ends");
//		Text text = Utility.FindTransform(instructionObj.transform, "Text").GetComponent<Text>();
//		text.text = "";
//		instructionObj.SetActive (false);
//
//		Utility.getInstructionController ().ShowingInstruction = false;
//
//	}
//	
//	[PunRPC]
//	void showInstruction(string instructionText, int seconds) {
//		GameObject instructionObj = Utility.getInstructionController ().instructionObj;
//		instructionObj.SetActive (true);
//
//		Text text = Utility.FindTransform(instructionObj.transform, "Text").GetComponent<Text>();
//		text.text = instructionText;
//
//		Utility.getInstructionController ().ShowingInstruction = true;
////		StartCoroutine (showInstructionRoutine(instructionText, seconds, instructionObj));
//	}
	
	[PunRPC]
	void destroyEnemy(int item_index){
		Destroy ((UnityEngine.GameObject)NetworkController.enemyList[item_index]);
	}
	
	[PunRPC]
	void changeColor(int item_index){
		GameObject local_hitObject = (UnityEngine.GameObject)NetworkController.enemyList [item_index];
		Renderer r = local_hitObject.GetComponent<Renderer> ();
		r.material.color = Color.yellow;
	}
	
	//generate enemy at a regular frequency
	
	[PunRPC]
	void generateEnemy(float x_position,float z_position, float maxLife, string type){
		GameObject enemy = Utility.CreateEnemy (x_position, z_position, maxLife, type);
//		GameObject enemy = (GameObject)Instantiate (Enemy, new Vector3 (x_position,1,z_position), Quaternion.identity);
		
		
		//enemy.GetComponent<EnemyController> ().id = EnemyController.total_enemy_count;
		//EnemyController.total_enemy_count++;
		NetworkController.enemyList.Add (enemy);
		
//		print("generate enemy:"+NetworkController.enemyList.IndexOf (enemy)+"\tcount:"+NetworkController.enemyList.Count);
	}
	
	[PunRPC]
	void generateBoss(int x_position,int z_position, float maxLife, string type){
		GameObject boss = Utility.CreateEnemy (x_position, z_position, maxLife, type);
		print ("generate boss");
	}
	
	[PunRPC]
	void decreseTabletSpotlightIntensity(){
		
		GameObject tbNetworkedPlayer = GameObject.FindWithTag (Constants.tbNetworkedPlayerTag);
		
		if (tbNetworkedPlayer != null) {
			Transform spotLight = Utility.FindTransform (tbNetworkedPlayer.transform, "Spotlight");
			
			spotLight.GetComponent<Light> ().intensity -= Constants.tbMaxSpotlightIntensity / 300f;
			spotLight.GetComponent<Light> ().spotAngle *= 0.995f;
		}
	}

	[PunRPC]
	void setOtherPlayerReady(bool isReady) {
		NetworkController.otherPlayerReady = isReady;
	}


	[PunRPC]
	void loadScene(string sceneName) {
		
		print ("sceneName " + sceneName);
		Application.LoadLevel (sceneName);
		Utility.getGameController ().setCurrScene (sceneName);

		GameObject cbNetworkedPlayer = GameObject.FindWithTag (Constants.cbNetworkedPlayerTag);
		GameObject tbNetworkedPlayer = GameObject.FindWithTag (Constants.tbNetworkedPlayerTag);
		
		GameObject tbCamera = GameObject.Find("TabletCamera");

		if (sceneName == Constants.bossSceneName) {
//			GameController.instruction.text = "Task 2:\nYou and your partner are controlling the same character. Tablet player controls the position, cardboard player controls the direction. Stare at the boss for 10 seconds and you will win.";
//			StartCoroutine(wait5s());

			if (cbNetworkedPlayer != null) {
				cbNetworkedPlayer.GetComponent<BackyardSceneCbPlayer> ().enabled = false;
				BossSceneCbPlayer bossSceneCbPlayerScript = cbNetworkedPlayer.GetComponent<BossSceneCbPlayer> ();
				bossSceneCbPlayerScript.enabled = true;
				PhotonView cbPhotonView = cbNetworkedPlayer.GetComponent<PhotonView> ();
				cbPhotonView.photonView.ObservedComponents.Add (bossSceneCbPlayerScript);

			}

			if (tbNetworkedPlayer != null) {
				tbNetworkedPlayer.GetComponent<BackyardSceneTbPlayer> ().enabled = false;
				BossSceneTbPlayer bossSceneTbPlayerScript = tbNetworkedPlayer.GetComponent<BossSceneTbPlayer> ();
				bossSceneTbPlayerScript.enabled = true;
				PhotonView tbPhotonView = tbNetworkedPlayer.GetComponent<PhotonView> ();
				tbPhotonView.photonView.ObservedComponents.Add (bossSceneTbPlayerScript);

				if (tbCamera != null) {
					tbCamera.GetComponent<TbBackyardCameraController> ().enabled = false;
					tbCamera.GetComponent<TbBossSceneCameraController> ().enabled = true;
				}

			}
		} else if (sceneName == Constants.darkBackyardSceneName) {

//			GameController.instruction.text = "Task 3:\nWork with your partner to find the way home. Tablet player can light up the environment with spotlight. Cardboard player can kill enemies with torchlight, but it will also decrease the light of tablet player if you look at him.";
//			StartCoroutine(wait5s());

			if (cbNetworkedPlayer != null) {
				cbNetworkedPlayer.GetComponent<BossSceneCbPlayer> ().enabled = false;
				DarkBackyardSceneCbPlayer darkBackyardSceneCbPlayerScript = cbNetworkedPlayer.GetComponent<DarkBackyardSceneCbPlayer> ();
				darkBackyardSceneCbPlayerScript.enabled = true;
				PhotonView cbPhotonView = cbNetworkedPlayer.GetComponent<PhotonView> ();
				cbPhotonView.photonView.ObservedComponents.Add (darkBackyardSceneCbPlayerScript);
			}
			
			if (tbNetworkedPlayer != null) {
				tbNetworkedPlayer.GetComponent<BossSceneTbPlayer> ().enabled = false;
				DarkBackyardSceneTbPlayer darkBackyardSceneTbPlayerScript = tbNetworkedPlayer.GetComponent<DarkBackyardSceneTbPlayer> ();
				darkBackyardSceneTbPlayerScript.enabled = true;
				PhotonView tbPhotonView = tbNetworkedPlayer.GetComponent<PhotonView> ();
				tbPhotonView.photonView.ObservedComponents.Add (darkBackyardSceneTbPlayerScript);

				if (tbCamera != null) {
					tbCamera.GetComponent<TbBossSceneCameraController> ().enabled = false;
					tbCamera.GetComponent<TbDarkBackyardCameraController> ().enabled = true;
				}
			}
		} 
//		else if (sceneName == Constants.backyardSceneName) {
//			GameObject[] objects = GameObject.FindObjectsOfType(typeof(GameObject)) as GameObject[];
//			foreach (GameObject o in objects) {
//				Destroy(o);
//			}
//		}


	}
}
