using UnityEngine;
using System.Collections;

public class EnemyController : Photon.MonoBehaviour {
	private GameObject cbPlayer, tbPlayer;
	private Transform cbAvatar, tbAvatar;
	
	private float maxLife;
	private float life;
	private string type;

	private Vector3 chasingDir = Vector3.zero;	//	only useful for type == "chaseBoth"

	public void config(float maxLife, string type) {
		this.maxLife = maxLife;
		this.life = maxLife;
		this.type = type;

//		if (type == "boss") {
//			this.gameObject.tag = Constants.bossTag;
//		} else {
//			
//			this.gameObject.tag = "Enemy";
//		}
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
		
		if (type == "boss") {
			//	TODO: define boss movement
			//this.transform.position = Vector3.Lerp (this.transform.position, cbAvatar.position, Time.deltaTime * 2);

			//boss moves around cb player in a random way
			float degree=Time.frameCount*0.05f;
			float radius=40;
			Vector3 new_pos=new Vector3( cbAvatar.position.x+ Mathf.Cos(degree)*radius, cbAvatar.position.y+Random.Range(-2,2), cbAvatar.position.z+ Mathf.Sin(degree)*radius);

			this.transform.position=Vector3.Lerp(this.transform.position, new_pos, Time.deltaTime*0.2f);
			this.transform.LookAt(cbAvatar);

		} else if (type == "chaseCb") {
			if (chasingDir == Vector3.zero) {
				//print ("chasingDir pre " + chasingDir);
				chasingDir = cbAvatar.position - this.transform.position;
				chasingDir.Normalize();
				//print ("chasingDir post " + chasingDir);
			}
			this.transform.position += chasingDir * Time.deltaTime * 10;
		} else if (type == "chaseBoth") {
			//	TODO: change it so that it chases both
			if (cbAvatar != null) {
				this.transform.position = Vector3.Lerp (this.transform.position, cbAvatar.position, Time.deltaTime * 0.1f);
			}
		}
	}
	
	public void revive() {
		Renderer r = GetComponent<Renderer> ();
		r.material.color = Color.gray;

		life = maxLife;
	}

	public void getHit(float damage) {
		Renderer r = GetComponent<Renderer> ();
		r.material.color = Color.red;

		life -= damage * Time.deltaTime;
	}

	public bool shouldBeDead() {
		return life <= 0;
	}
	

}
