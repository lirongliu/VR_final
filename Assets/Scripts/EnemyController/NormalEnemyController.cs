	using UnityEngine;
	using System.Collections;

	public class NormalEnemyController : EnemyController {
		private GameObject cbPlayer, tbPlayer;
		private Transform cbAvatar, tbAvatar;

		private Vector3 chasingDir = Vector3.zero;	//	only useful for type == "chaseBoth"

		private GameObject chasingAvatar;

		private Transform chasingTarget = null; //      for scene 3 only

		override public void config(float maxLife, string type) {
			this.maxLife = maxLife;
			this.life = maxLife;
			this.type = type;

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
				chasingDir = cbAvatar.position - this.transform.position;
				chasingDir = new Vector3 (chasingDir.x, 0, chasingDir.z);
				chasingDir.Normalize ();

				this.transform.forward = chasingDir;
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
				if (Random.Range (-1f, 1f) > 0) {
					chasingTarget = cbAvatar;
					print ("chasing CB");
				} else {
					chasingTarget = tbAvatar;
					print ("chasing TB");
				}
			}
			if (chasingTarget != null) {
				dir = (chasingTarget.transform.position - this.transform.position).normalized;
				this.transform.position += dir * Time.deltaTime * Constants.defaultMovingSpeed * 0.5f;
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

					CbNetworkedPlayer[] scripts = GameObject.FindWithTag (Constants.cbNetworkedPlayerTag).GetComponents<CbNetworkedPlayer> ();
					CbNetworkedPlayer cbNetworkedPlayer = null;
					for (int i = 0;i < scripts.Length;i++) {
						if (scripts[i].enabled) {
							cbNetworkedPlayer = scripts[i];
							break;
						}
					}
					
					if (cbNetworkedPlayer != null) {
						cbNetworkedPlayer.damage(Constants.ghostDamage);
						cbNetworkedPlayer.killEnemy( NetworkController.enemyList.IndexOf (this.gameObject));

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
					TbNetworkedPlayer[] scripts = GameObject.FindWithTag (Constants.tbNetworkedPlayerTag).GetComponents<TbNetworkedPlayer> ();
					TbNetworkedPlayer tbNetworkedPlayer = null;
					for (int i = 0;i < scripts.Length;i++) {
						if (scripts[i].enabled) {
							tbNetworkedPlayer = scripts[i];
							break;
						}
					}
					if (tbNetworkedPlayer != null) {
						tbNetworkedPlayer.damage(Constants.ghostDamage);
					tbNetworkedPlayer.killEnemy( NetworkController.enemyList.IndexOf (this.gameObject));
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
