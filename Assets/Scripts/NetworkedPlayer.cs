using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class NetworkedPlayer : Photon.MonoBehaviour
{
	
	public GameObject avatar;
	public Transform playerGlobal;
	public Transform playerLocal;
	public GameObject Enemy;

	protected Vector3 correctAvatarPos = Vector3.zero; //We lerp towards this
	protected Quaternion correctAvatarRot = Quaternion.identity; //We lerp towards this
	protected Quaternion correctHeadRot = Quaternion.identity; //We lerp towards this

	protected Transform headTransform;

	protected float movingSpeed = Constants.defaultMovingSpeed;

//	private int count = 0;

	void Start () {
	}
	
	void Update() {

	}
	
	protected virtual void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info) {
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
	
	protected virtual void inputHandler() {

		if (Input.GetKey ("l")) {
			string nextScene = Utility.getGameController().getNextScene();
			photonView.RPC("loadScene", PhotonTargets.All, nextScene);
		}

		if (Input.GetKey ("q")) {	//	moving towards the viewing direction
			this.move(Camera.main.transform.forward);
		}
		if (Input.GetKey ("d")) {
			this.move(new Vector3(1, 0, 0));
		}
		
		if (Input.GetKey ("a")) {
			this.move(new Vector3(-1, 0, 0));
		}
		
		if (Input.GetKey ("s")) {
			this.move(new Vector3(0, 0, -1));
		}
		
		if (Input.GetKey ("w")) {
			this.move(new Vector3(0, 0, 1));
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



		
		if (Input.GetKey ("]")) {
			movingSpeed *= 1.5f;

		}
		
		if (Input.GetKey ("[")) {
			movingSpeed /= 1.5f;
		}
	}

	public void move(Vector3 dir) {
		
		AvatarController avatarController = avatar.GetComponent<AvatarController> ();
		avatarController.Move(Utility.movementAdjustedWithFPS(movingSpeed * dir));
	}

	protected void checkHitTbPlayer(RaycastHit hit) {
		// check whether the spotlight hits tablet avatar
		GameObject tabletPlayerAvatar = Utility.getTabletPlayerAvatar();
		if (tabletPlayerAvatar != null) {
			float angle = Utility.getVectorAngle(Camera.main.transform.forward, tabletPlayerAvatar.transform.position - Camera.main.transform.position);
			if(angle < Constants.cbMaxSpotlightAngle / 2) {
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
//					print ("hit by spotlight!!!!");
//					count++;
//					print ("count " + count);
					photonView.RPC ("decreseTabletSpotlightIntensity",PhotonTargets.All);
				}
				
			}
		}
	}

	protected void checkHitEnemies(RaycastHit hit, ref GameObject hitEnemy) {

		if (hit.collider != null) {
			if (hit.collider.gameObject == hitEnemy) {
				EnemyController ec = hitEnemy.GetComponent<EnemyController> ();
				ec.getHit (300);
					
				if (ec.shouldBeDead ()) {
						
					if (NetworkController.enemyList.IndexOf (hitEnemy) != null && NetworkController.enemyList.IndexOf (hitEnemy) != (-1)) {
						//print("NetworkedPlayer enemyList:"+NetworkController.enemyList[0]+"\t"+NetworkController.enemyList[1]+"\t"+NetworkController.enemyList[2]);
						//print("INDEX:"+NetworkController.enemyList.IndexOf (hit.collider.gameObject));
						photonView.RPC ("destroyEnemy", PhotonTargets.All, NetworkController.enemyList.IndexOf (hitEnemy));
					}
				}
					
			} else {
				if (hitEnemy != null) {
					EnemyController ec = hitEnemy.GetComponent<EnemyController> ();
					ec.revive ();
					hitEnemy = null;
				}
					
				//							print ("hit.collider.gameObject.tag " + hit.collider.gameObject.tag);
				if (hit.collider.CompareTag ("Enemy")) {
					hitEnemy = hit.collider.gameObject;
				}
			}
		}
	}

	protected bool arriveInDest(Vector3 dest, float allowance) {
		GameObject cbNetworkedPlayerAvatar = GameObject.FindGameObjectWithTag (Constants.cbPlayerAvatarTag);
		GameObject tbNetworkedPlayerAvatar = GameObject.FindGameObjectWithTag (Constants.tbPlayerAvatarTag);
		if (cbNetworkedPlayerAvatar != null && tbNetworkedPlayerAvatar != null) {
			float dist1 = Vector3.Distance(cbNetworkedPlayerAvatar.transform.position, dest);
			float dist2 = Vector3.Distance(tbNetworkedPlayerAvatar.transform.position, dest);
//			print ("dist1 " + dist1);
//			print ("dist2 " + dist2);
			if (dist1 + dist2 < allowance) {
				return true;
			}
		}
		return false;
	}
}