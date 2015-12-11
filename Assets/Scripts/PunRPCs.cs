﻿using UnityEngine;
using System.Collections;

public class PunRPCs : MonoBehaviour {
	
	public GameObject Enemy;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
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
	void generateEnemy(int x_position,int z_position){
		GameObject enemy = (GameObject)Instantiate (Enemy, new Vector3 (x_position,1,z_position), Quaternion.identity);
		
		
		//enemy.GetComponent<EnemyController> ().id = EnemyController.total_enemy_count;
		//EnemyController.total_enemy_count++;
		NetworkController.enemyList.Add (enemy);
		
		//print("generate enemy:"+NetworkController.enemyList.IndexOf (enemy)+"\tcount:"+NetworkController.enemyList.Count);
	}
	
	[PunRPC]
	void decreseTabletSpotlightIntensity(){
		
		GameObject tbNetworkedPlayer = GameObject.FindWithTag (Constants.tbNetworkedPlayerTag);
		
		if (tbNetworkedPlayer != null) {
			Transform spotLight = Utility.FindTransform (tbNetworkedPlayer.transform, "Spotlight");
			
			spotLight.GetComponent<Light> ().intensity -= 0.01f;
		}
	}
	
	[PunRPC]
	void loadScene(string sceneName) {
		Application.LoadLevel (sceneName);

		GameObject cbNetworkedPlayer = GameObject.FindWithTag (Constants.cbNetworkedPlayerTag);
		GameObject tbNetworkedPlayer = GameObject.FindWithTag (Constants.tbNetworkedPlayerTag);

		if (sceneName == "BossScene") {
			if (cbNetworkedPlayer != null) {
				cbNetworkedPlayer.GetComponent<DarkSceneCbPlayer>().enabled = false;
				BossSceneCbPlayer bossSceneCbPlayerScript = cbNetworkedPlayer.GetComponent<BossSceneCbPlayer>();
				bossSceneCbPlayerScript.enabled = true;
				PhotonView cbPhotonView = cbNetworkedPlayer.GetComponent<PhotonView>();
				cbPhotonView.photonView.ObservedComponents.Add(bossSceneCbPlayerScript);
			}

			if (tbNetworkedPlayer != null) {
				tbNetworkedPlayer.GetComponent<DarkSceneTbPlayer>().enabled = false;
				BossSceneTbPlayer bossSceneTbPlayerScript = tbNetworkedPlayer.GetComponent<BossSceneTbPlayer>();
				bossSceneTbPlayerScript.enabled = true;
				PhotonView tbPhotonView = tbNetworkedPlayer.GetComponent<PhotonView>();
				tbPhotonView.photonView.ObservedComponents.Add(bossSceneTbPlayerScript);
			}
		} else if (sceneName == "DarkBackyardScene") {
		}

	}
}
