using UnityEngine;
using System.Collections;
using System.Diagnostics;

public class NetworkedPlayer : Photon.MonoBehaviour
{
	
	public GameObject avatar;
	public Transform playerGlobal;
	public Transform playerLocal;

	private GameObject hitObject;


	protected Vector3 correctAvatarPos = Vector3.zero; //We lerp towards this
	protected Quaternion correctAvatarRot = Quaternion.identity; //We lerp towards this

	Stopwatch stopWatch;

	void Update(){


		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
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
							if (stopWatch.ElapsedMilliseconds > 300) {
								//Destroy (hit.collider.gameObject);

								//RPCs only support basic data like floats, ints, bools, Vector2 and 3 and Quaternions.
								//can not send RaycastHit as a parameter

								if (NetworkController.enemyList.IndexOf (hit.collider.gameObject)!=null){
									//print("NetworkedPlayer enemyList:"+NetworkController.enemyList[0]+"\t"+NetworkController.enemyList[1]+"\t"+NetworkController.enemyList[2]);
									//print("INDEX:"+NetworkController.enemyList.IndexOf (hit.collider.gameObject));
									photonView.RPC ("destroyEnemy",PhotonTargets.All,NetworkController.enemyList.IndexOf(hit.collider.gameObject));
								}


							} else {
								if (hitObject.tag == "Enemy") {
									Renderer r = hitObject.GetComponent<Renderer> ();
									r.material.color = Color.yellow;

//									print("INDEX:"+NetworkController.enemyList.IndexOf (hit.Object));
//									NetworkController.enemyList.IndexOf (hitObject);
//									photonView.RPC ("changeColor",PhotonTargets.All,NetworkController.enemyList.IndexOf(hitObject));

								}
							}
						} else {
							if (hit.collider.gameObject.tag == "Enemy") {
								hitObject = hit.collider.gameObject;
								Renderer r = hitObject.GetComponent<Renderer> ();
								r.material.color = Color.white;
							} else {
								stopWatch.Reset ();
								stopWatch.Start ();
							}
						}
					} else {

						hitObject = null;
						stopWatch.Reset ();
					}
				}
			}
		}

	}
	
	void Start ()
	{
		stopWatch = new Stopwatch ();
		//Debug.Log("i'm instantiated");

		if (photonView.isMine && NetworkController.whoAmI == Constants.IS_IPAD_PLAYER) {
			//Debug.Log ("player is mine. I am the iPad player.");

			this.transform.localPosition = new Vector3(5,2,-1);

			Transform spotlight = this.transform.Find("Spotlight");
			spotlight.SetParent(avatar.transform);


		} else if (photonView.isMine && NetworkController.whoAmI == Constants.IS_CB_PLAYER) {
			//Debug.Log ("player is mine. I am the cardboard player.");

			GameObject cb = GameObject.Find ("CardboardMain");
			cb.transform.SetParent(avatar.transform);
			cb.transform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);

			this.transform.localPosition = new Vector3(-5,1,-2);
		}
		playerGlobal = avatar.transform;
	}
	
	protected void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
	{
		if (stream.isWriting){
			stream.SendNext(playerGlobal.position);
			stream.SendNext(playerGlobal.rotation);

		}
		else{
			correctAvatarPos = (Vector3)stream.ReceiveNext();
			correctAvatarRot = (Quaternion)stream.ReceiveNext();

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
			playerGlobal.Rotate (new Vector3 (0, 2, 0));
		}
		if (Input.GetKey ("left")) {
			playerGlobal.Rotate (new Vector3 (0, -2, 0));
		}
		
		if (Input.GetKey ("down")) {
			playerGlobal.Rotate (new Vector3 (2, 0, 0));
		}
		
		if (Input.GetKey ("up")) {
			playerGlobal.Rotate (new Vector3 (-2, 0, 0));
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


}