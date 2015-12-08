﻿using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class NetworkedPlayer : Photon.MonoBehaviour
{
	
	public GameObject avatar;
	public Transform playerGlobal;
	public Transform playerLocal;
	public GameObject Enemy;
	
	private GameObject hitObject;
	
	
	protected Vector3 correctAvatarPos = Vector3.zero; //We lerp towards this
	protected Quaternion correctAvatarRot = Quaternion.identity; //We lerp towards this
	protected Quaternion correctHeadRot = Quaternion.identity; //We lerp towards this
	
	
	protected Transform headTransform;
	
	Stopwatch stopWatch;
	
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
						
						if (hit.collider.gameObject == hitObject) {
							//UnityEngine.Debug.Log("if: hitObject="+hitObject.tag);
							print ("stopWatch.ElapsedMilliseconds" + stopWatch.ElapsedMilliseconds);
							if (stopWatch.ElapsedMilliseconds > 300) {
								//Destroy (hit.collider.gameObject);
								
								//RPCs only support basic data like floats, ints, bools, Vector2 and 3 and Quaternions.
								//can not send RaycastHit as a parameter
								
								if (NetworkController.enemyList.IndexOf (hit.collider.gameObject)!=null && NetworkController.enemyList.IndexOf (hit.collider.gameObject)!=(-1)){
									//print("NetworkedPlayer enemyList:"+NetworkController.enemyList[0]+"\t"+NetworkController.enemyList[1]+"\t"+NetworkController.enemyList[2]);
									//print("INDEX:"+NetworkController.enemyList.IndexOf (hit.collider.gameObject));
									photonView.RPC ("destroyEnemy",PhotonTargets.All,NetworkController.enemyList.IndexOf(hit.collider.gameObject));
									print ("ajsdkflajsdflkj");
								}
							}   else {
								if (hitObject.tag == "Enemy") {
									Renderer r = hitObject.GetComponent<Renderer> ();
									r.material.color = Color.yellow;
								}
							}
						} else {
							print ("tag " + hit.collider.gameObject.tag);
							if (hit.collider.gameObject.tag == "Enemy") {
								hitObject = hit.collider.gameObject;
								Renderer r = hitObject.GetComponent<Renderer> ();
								r.material.color = Color.white;
							} else if(Utility.checkTag(hit.collider.gameObject.transform, "iPadNetworkedPlayerAvatar")) {
								
								//GameObject iPadAvatar=hit.collider.gameObject;
								//Transform spotLight=iPadAvatar.transform.Find("Spotlight");
								//spotLight.GetComponent<Light>().intensity-=0.05f;
								photonView.RPC ("decreseTabletSpotlightIntensity",PhotonTargets.All);
								
								
							} else {
								stopWatch.Reset ();
								stopWatch.Start ();
							}
						}
					}   else {
						
						hitObject = null;
						stopWatch.Reset ();
					}
				}
			}
			
			
			if(Time.frameCount%200==0){
				photonView.RPC ("generateEnemy",PhotonTargets.All,Random.Range(-90,90),Random.Range(-90,90));
			}
		}
		
	}
	
	void Start ()
	{
		stopWatch = new Stopwatch ();
		//Debug.Log("i'm instantiated");
		
		if (this.tag == "cbNetworkedPlayer" && photonView.isMine) {
			GameObject cb = GameObject.Find ("CardboardMain");
			
			Transform avatarHeadTransform = this.transform.Find("AvatarHead");
			Transform avatarBodyTransform = this.transform.Find("AvatarBody");
			
			avatarBodyTransform.SetParent(avatar.transform);
			
			cb.transform.SetParent(avatarBodyTransform);
			avatarHeadTransform.SetParent(cb.transform);
			cb.transform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
			avatarHeadTransform.localPosition = Vector3.zero;
			
			
			//playerLocal = headTransform;
			
			// get head transform
			this.headTransform = cb.transform;
		}   else {
			
			Transform avatarHeadTransform = this.transform.Find("AvatarHead");
			Transform avatarBodyTransform = this.transform.Find("AvatarBody");
			
			avatarBodyTransform.SetParent(avatar.transform);
			avatarHeadTransform.SetParent(avatarBodyTransform);
			avatarHeadTransform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
			
			
			//playerLocal = avatar.transform;
			
			// get head transform
			
			this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");
		}
		
		
		if (this.tag == "cbNetworkedPlayer") {
			avatar.transform.localPosition = new Vector3 (-5, 1, -2);
		}  else {
			avatar.transform.localPosition = new Vector3 (5, 2, -1);
		}
		
		playerLocal = headTransform;
		playerGlobal = avatar.transform;
		
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
		if (Input.GetKey ("q")) {	//	moving towards the viewing direction
			this.translate (playerGlobal.forward);
		}
		
		if (Input.GetKey ("d")) {
			this.translate (new Vector3(1, 0, 0));
		}
		
		if (Input.GetKey ("a")) {
			this.translate (new Vector3(-1, 0, 0));
		}
		
		if (Input.GetKey ("s")) {
			this.translate (new Vector3(0, 0, -1));
		}
		
		if (Input.GetKey ("w")) {
			this.translate (new Vector3(0, 0, 1));
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

		GameObject iPadNetworkedPlayer = GameObject.FindWithTag ("iPadNetworkedPlayer");

		if (iPadNetworkedPlayer != null) {
			Transform spotLight = Utility.FindTransform (iPadNetworkedPlayer.transform, "Spotlight");
		
			spotLight.GetComponent<Light> ().intensity -= 0.01f;
		}
	}
	
	
	
	
	
}