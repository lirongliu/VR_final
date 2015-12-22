		using UnityEngine;
		using System.Collections;

		public class NormalEnemyController : EnemyController {
			private GameObject cbPlayer, tbPlayer;
			private Transform cbAvatar, tbAvatar;

	private Vector3 chasingDir = Vector3.zero;	//	only useful for type == "chaseBoth"

	private GameObject chasingAvatar;

	private Transform chasingTarget = null; //      for scene 3 only

	override public void config(float maxLife, string type, int id) {
		this.maxLife = maxLife;
		this.life = maxLife;
		this.type = type;
		this.id = id;
	}

	// only used by tbplayer
	public void setChasingDir(Vector3 dir) {
		print ("setChasingDir " + dir);
		this.chasingDir = dir;
	}
	// only used by tbplayer
	public void setChasingTarget(int playerId) {
		if (playerId == Constants.cbPlayerID) {
			chasingTarget = GameObject.FindWithTag(Constants.cbNetworkedPlayerTag).transform.Find("Avatar");
		} else {
			chasingTarget = GameObject.FindWithTag(Constants.tbNetworkedPlayerTag).transform.Find("Avatar");
		}
		print ("setChasingTarget " + playerId);
	}

	void Start () {

		this.revive ();
		
		if (cbAvatar == null) {
			cbPlayer = GameObject.FindWithTag(Constants.cbNetworkedPlayerTag);
			if (cbPlayer != null) {
				cbAvatar=cbPlayer.transform.Find("Avatar");
			}
		}
		
		if (tbAvatar == null) {
			tbPlayer = GameObject.FindWithTag(Constants.tbNetworkedPlayerTag);
			if (tbPlayer != null) {
				tbAvatar=tbPlayer.transform.Find("Avatar");
			}
		}
	}
	

	void Update () {
	}

	void FixedUpdate() {
		if (type == "chaseCb") {
			if (chasingDir == Vector3.zero) {
				if (NetworkController.whoAmI == Constants.cbPlayerID) {
				chasingDir = cbAvatar.position - this.transform.position;
				chasingDir = new Vector3 (chasingDir.x, 0, chasingDir.z);
				chasingDir.Normalize ();

				this.transform.forward = chasingDir;
					CbNetworkedPlayer cbNetworkedPlayer = Utility.getCbNetworkedPlayerScript();
					cbNetworkedPlayer.assignEnemyChasingDir(chasingDir, id);
				}
			}
			this.transform.position += chasingDir * Time.deltaTime * 10;

			Vector3 pos = this.transform.position;
			if (Vector3.Distance (pos, cbAvatar.transform.position) > 40) {
				//			if (pos.x < -35 || pos.x > 35 || pos.z < -27 || pos.z > 27) {
	//				print ("destroy");
				Destroy (this.gameObject);
			}

		} else if (type == "chaseBoth") {
			Vector3 dir;

			if (chasingTarget == null) {
				if (NetworkController.whoAmI == Constants.cbPlayerID) {
					CbNetworkedPlayer cbNetworkedPlayer = Utility.getCbNetworkedPlayerScript();
					if (Random.Range (-1f, 1f) > -0.5f) {
						chasingTarget = cbAvatar;
//						print ("chasing CB");
						cbNetworkedPlayer.assignEnemyChasingTarget(Constants.cbPlayerID, id);

					} else {
						chasingTarget = tbAvatar;
						cbNetworkedPlayer.assignEnemyChasingTarget(Constants.tbPlayerID, id);

//						print ("chasing TB");
					}
				}
			}
			if (chasingTarget != null) {
				dir = (chasingTarget.transform.position - this.transform.position).normalized;
				this.transform.position += dir * Time.deltaTime * Constants.defaultMovingSpeed * 0.25f;
				//					this.transform.position = Vector3.Lerp (this.transform.position, chasingTarget.position, Time.deltaTime * 0.5f);
				this.transform.forward = (chasingTarget.transform.position - this.transform.position).normalized;
			}
		}

		if (cbAvatar != null) {
			Vector3 cbPos = cbAvatar.transform.position;
			Vector3 myPos = this.transform.position;
	//			print ("dist " + Vector2.Distance (new Vector2 (cbPos.x, cbPos.z), new Vector2 (myPos.x, myPos.z)));
			if (Vector2.Distance (new Vector2 (cbPos.x, cbPos.z), new Vector2 (myPos.x, myPos.z)) < 0.4f) {
			
				if (NetworkController.whoAmI == Constants.cbPlayerID) {

					CbNetworkedPlayer cbNetworkedPlayer = Utility.getCbNetworkedPlayerScript();
					
					if (cbNetworkedPlayer != null) {
						cbNetworkedPlayer.damage(Constants.ghostDamage);
					cbNetworkedPlayer.killEnemy( id);// NetworkController.enemyList.IndexOf (this.gameObject));

					}
	//					photonView.RPC ("destroyEnemy", PhotonTargets.All, NetworkController.enemyList.IndexOf (this.gameObject));
					print ("hit cb...");
				}

			} 
		} 
		if (tbAvatar != null) {
			Vector3 tbPos = tbAvatar.transform.position;
			Vector3 myPos = this.transform.position;
			if( Vector2.Distance (new Vector2(tbPos.x, tbPos.z), new Vector2(myPos.x, myPos.z)) < 0.4f) {
				if (NetworkController.whoAmI == Constants.tbPlayerID) {
					TbNetworkedPlayer tbNetworkedPlayer = Utility.getTbNetworkedPlayerScript();
					
					if (tbNetworkedPlayer != null) {
						tbNetworkedPlayer.damage(Constants.ghostDamage);
					tbNetworkedPlayer.killEnemy(id);// NetworkController.enemyList.IndexOf (this.gameObject));
					}
					print ("hit tb...");
				}

			}
		}
	}

	// NOT WORKING...
void OnCollisionEnter(Collision collision) {
	print ("Ghost OnCollisionEnter!");
}
}
