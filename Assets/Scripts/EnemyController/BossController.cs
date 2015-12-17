using UnityEngine;
using System.Collections;

public class BossController : EnemyController {
	private GameObject cbPlayer;
	private Transform cbAvatar, tbAvatar;


	override public void config(float maxLife, string type) {
		this.maxLife = maxLife;
		this.life = maxLife;
		this.type = type;

	}

	void Start () {
		DontDestroyOnLoad(this);

		this.revive ();
		
		if (cbAvatar == null) {
			cbPlayer = GameObject.FindWithTag(Constants.cbNetworkedPlayerTag);
			if (cbPlayer != null) {
				cbAvatar=cbPlayer.transform.Find("Avatar");
			}
		}
	}
	

	void Update () {
	}

	void FixedUpdate(){
		
		//	TODO: define boss movement
		//this.transform.position = Vector3.Lerp (this.transform.position, cbAvatar.position, Time.deltaTime * 2);
		
		//boss moves around cb player in a random way
		float degree=Time.frameCount*0.05f;
		float radius=40;
		Vector3 new_pos=new Vector3( cbAvatar.position.x+ Mathf.Cos(degree)*radius, cbAvatar.position.y+Random.Range(-2,2), cbAvatar.position.z+ Mathf.Sin(degree)*radius);
		
		this.transform.position=Vector3.Lerp(this.transform.position, new_pos, Time.deltaTime*0.2f);
		this.transform.LookAt(cbAvatar);
	}
	
//	public void revive() {
//		Renderer r = GetComponent<Renderer> ();
//		r.material.color = Color.gray;
//
//		life = maxLife;
//	}
//
//	public void getHit(float damage) {
//		Renderer r = GetComponent<Renderer> ();
//		r.material.color = Color.red;
//
//		life -= damage * Time.deltaTime;
//	}
}
