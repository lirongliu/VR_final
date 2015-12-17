using UnityEngine;
using System.Collections;

public class NormalEnemyController : EnemyController {
	private GameObject cbPlayer, tbPlayer;
	private Transform cbAvatar, tbAvatar;

	private Vector3 chasingDir = Vector3.zero;	//	only useful for type == "chaseBoth"

	private GameObject chasingAvatar;

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

	void FixedUpdate(){
		if (type == "chaseCb") {
			if (chasingDir == Vector3.zero) {
				chasingDir = cbAvatar.position - this.transform.position;
				chasingDir = new Vector3(chasingDir.x, 0, chasingDir.z);
				chasingDir.Normalize();
			}
			this.transform.position += chasingDir * Time.deltaTime * 10;

			Vector3 pos = this.transform.position;
			if (pos.x < -35 || pos.x > 35 || pos.z < -27 || pos.z > 27) {
//				print ("destroy");
				Destroy(this.gameObject);
			}

		} else if (type == "chaseBoth") {
			//	TODO: change it so that it chases both
//			if (chasingAvatar == null) {
//				if (Random.Range(-1, 1) > 0) {
//					chasingAvatar = tbAvatar;
//				} else {
//					chasingAvatar = cbAvatar;
//				}
//
//			}
//
//			if (chasingAvatar != null) {
//				
//				this.transform.position=Vector3.Lerp(this.transform.position, chasingAvatar.transform.position, Time.deltaTime*0.2f);

				if (cbAvatar != null) {
					this.transform.position = Vector3.Lerp (this.transform.position, cbAvatar.position, Time.deltaTime * 0.1f);
				}
//			}
		}
	}
}
