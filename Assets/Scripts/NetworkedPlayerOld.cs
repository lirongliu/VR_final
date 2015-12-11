using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class NetworkedPlayerOld : Photon.MonoBehaviour
{
	
	public GameObject avatar;
	public Transform playerGlobal;
	public Transform playerLocal;
	public GameObject Enemy;

	private GameObject hitEnemy;
	
	
	protected Vector3 correctAvatarPos = Vector3.zero; //We lerp towards this
	protected Quaternion correctAvatarRot = Quaternion.identity; //We lerp towards this
	protected Quaternion correctHeadRot = Quaternion.identity; //We lerp towards this
	
	
	protected Transform headTransform;

	void Start ()
	{
		DontDestroyOnLoad (this);
		//Debug.Log("i'm instantiated");

		
		if (this.CompareTag(Constants.cbNetworkedPlayerTag)) {
			avatar.transform.localPosition = new Vector3 (-5, 0.5f, -2);
		}  else {
			avatar.transform.localPosition = new Vector3 (5, 0.5f, -1);
		}
		
		if (this.CompareTag(Constants.cbNetworkedPlayerTag) && photonView.isMine) {
			GameObject cb = GameObject.Find ("CardboardMain");
			
			Transform avatarHeadTransform = this.transform.Find("AvatarHead");
			Transform avatarBodyTransform = this.transform.Find("AvatarBody");


			/* hierachy:
			 * avatar
			 * avatarBody
			 * CardboardMain
			 * head
			 */
			avatarBodyTransform.SetParent(avatar.transform);
			avatarBodyTransform.localPosition = Vector3.zero;
			
			cb.transform.SetParent(avatarBodyTransform);
			avatarHeadTransform.SetParent(cb.transform);
			cb.transform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
			avatarHeadTransform.localPosition = Vector3.zero;
			
			// set head transform
			this.headTransform = cb.transform;
		}   else {
			
			Transform avatarHeadTransform = this.transform.Find("AvatarHead");
			Transform avatarBodyTransform = this.transform.Find("AvatarBody");

			/* hierachy:
			 * avatar
			 * avatarBody
			 * head
			 */
			avatarBodyTransform.SetParent(avatar.transform);
			avatarBodyTransform.localPosition = Vector3.zero;

			avatarHeadTransform.SetParent(avatarBodyTransform);
			avatarHeadTransform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);

			// set head transform
			this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");
		}

		playerLocal = headTransform;
		playerGlobal = avatar.transform;		
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		}
		else {
			inputHandler();
			
			//gaze
			if (NetworkController.whoAmI == Constants.IS_CB_PLAYER) {
				RaycastHit hit;
				if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit)) {
					if (hit.collider != null) {
						if (hit.collider.gameObject == hitEnemy) {
							EnemyController ec = hitEnemy.GetComponent<EnemyController>();
							ec.getHit(300);
							
							if (ec.shouldBeDead()) {
								
								if (NetworkController.enemyList.IndexOf (hitEnemy)!=null && NetworkController.enemyList.IndexOf (hitEnemy)!=(-1)){
									//print("NetworkedPlayer enemyList:"+NetworkController.enemyList[0]+"\t"+NetworkController.enemyList[1]+"\t"+NetworkController.enemyList[2]);
									//print("INDEX:"+NetworkController.enemyList.IndexOf (hit.collider.gameObject));
									photonView.RPC ("destroyEnemy",PhotonTargets.All,NetworkController.enemyList.IndexOf(hitEnemy));
								}
							}
							
						} else {
							if (hitEnemy != null) {
								EnemyController ec = hitEnemy.GetComponent<EnemyController>();
								ec.revive();
								hitEnemy = null;
							}

//							print ("hit.collider.gameObject.tag " + hit.collider.gameObject.tag);
							if (hit.collider.CompareTag("Enemy")) {
								hitEnemy = hit.collider.gameObject;
							}
						}
					}
				}

				// check whether the spotlight hits tablet avatar
				GameObject tabletPlayerAvatar = Utility.getTabletPlayerAvatar();
				if (tabletPlayerAvatar != null) {
					float angle = Utility.getVectorAngle(Camera.main.transform.forward, tabletPlayerAvatar.transform.position - Camera.main.transform.position);
					if(angle < Constants.cbSpotlightAngle / 2) {
						bool hitByLight = false;
						if (hit.collider != null) {
							if (Utility.checkTag(hit.collider.gameObject.transform, Constants.tbPlayerAvatarTag)) {
								hitByLight = true;
							} else {
								float hitPointDist = Vector3.Distance(Camera.main.transform.position, hit.point);
								float tabletAvatarDist = Vector3.Distance(Camera.main.transform.position, tabletPlayerAvatar.transform.position);
								if (hitPointDist > tabletAvatarDist) {
									hitByLight = true;
								}
							}
						} else {
							hitByLight = true;
						}
						if (hitByLight) {
							print ("hit by spotlight!!!!");
							photonView.RPC ("decreseTabletSpotlightIntensity",PhotonTargets.All);
						}
						
					}
				}

				// only cb is responsible for generating enemies
				if(Time.frameCount%100==0){
					photonView.RPC ("generateEnemy",PhotonTargets.All,Random.Range(-90,90),Random.Range(-90,90));
				}
			}
			

		}
		
	}


	protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting){
			stream.SendNext(playerGlobal.position);
			stream.SendNext(playerGlobal.rotation);
			stream.SendNext(headTransform.localRotation);
		}
		else{
			correctAvatarPos = (Vector3)stream.ReceiveNext();
			correctAvatarRot = (Quaternion)stream.ReceiveNext();
			correctHeadRot = (Quaternion)stream.ReceiveNext();
		}
	}
	
	protected void translate(Vector3 dir) {
		
		Vector3 oldPos = playerGlobal.localPosition;
		Vector3 newPos = oldPos + dir * 0.1f;
		playerGlobal.localPosition = newPos;
	}
	
	protected void inputHandler() {
		float movingSpeed = 0.2f;

		AvatarController avatarController = avatar.GetComponent<AvatarController> ();
		if (Input.GetKey ("q")) {	//	moving towards the viewing direction
//			this.translate (playerGlobal.forward);
			avatarController.Move(Camera.main.transform.forward * 0.1f);
		}

		if (Input.GetKey ("l")) {
			photonView.RPC("loadScene", PhotonTargets.All, "BossScene");
		}

		if (Input.GetKey ("d")) {
//			this.translate (new Vector3(1, 0, 0));
			avatarController.Move(new Vector3(movingSpeed, 0, 0));
		}
		
		if (Input.GetKey ("a")) {
//			this.translate (new Vector3(-1, 0, 0));
			avatarController.Move(new Vector3(-movingSpeed, 0, 0));
		}
		
		if (Input.GetKey ("s")) {
//			this.translate (new Vector3(0, 0, -1));
			avatarController.Move(new Vector3(0, 0, -movingSpeed));
		}
		
		if (Input.GetKey ("w")) {
//			this.translate (new Vector3(0, 0, 1));
			avatarController.Move(new Vector3(0, 0, movingSpeed));
		}
		
		
		if (Input.GetKey ("right")) {
			playerLocal.Rotate (new Vector3 (0, 2, 0));
		}
		if (Input.GetKey ("left")) {
			playerLocal.Rotate (new Vector3 (0, -2, 0));
		}
		
		if (Input.GetKey ("down")) {
			playerLocal.Rotate (new Vector3 (2, 0, 0));
		}
		
		if (Input.GetKey ("up")) {
			playerLocal.Rotate (new Vector3 (-2, 0, 0));
		}
		
	}

	
//	[PunRPC]
//	void destroyEnemy(int item_index){
//		Destroy ((UnityEngine.GameObject)NetworkController.enemyList[item_index]);
//	}
//	
//	[PunRPC]
//	void changeColor(int item_index){
//		GameObject local_hitObject = (UnityEngine.GameObject)NetworkController.enemyList [item_index];
//		Renderer r = local_hitObject.GetComponent<Renderer> ();
//		r.material.color = Color.yellow;
//	}
//	
//	//generate enemy at a regular frequency
//	
//	[PunRPC]
//	void generateEnemy(int x_position,int z_position){
//		GameObject enemy = (GameObject)Instantiate (Enemy, new Vector3 (x_position,1,z_position), Quaternion.identity);
//		
//		
//		//enemy.GetComponent<EnemyController> ().id = EnemyController.total_enemy_count;
//		//EnemyController.total_enemy_count++;
//		NetworkController.enemyList.Add (enemy);
//		
//		//print("generate enemy:"+NetworkController.enemyList.IndexOf (enemy)+"\tcount:"+NetworkController.enemyList.Count);
//	}
//	
//	[PunRPC]
//	void decreseTabletSpotlightIntensity(){
//		
//		GameObject iPadNetworkedPlayer = GameObject.FindWithTag ("iPadNetworkedPlayer");
//		
//		if (iPadNetworkedPlayer != null) {
//			Transform spotLight = Utility.FindTransform (iPadNetworkedPlayer.transform, "Spotlight");
//			
//			spotLight.GetComponent<Light> ().intensity -= 0.01f;
//		}
//	}
//	
//	[PunRPC]
//	void loadScene(string sceneName) {
//		Application.LoadLevel (sceneName);
//		if (sceneName == "BossScene") {
//		} else if (sceneName == "DarkBackyardScene") {
//		}
//	}

}