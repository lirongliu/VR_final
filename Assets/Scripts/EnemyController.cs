using UnityEngine;
using System.Collections;

public class EnemyController : Photon.MonoBehaviour {
	private GameObject cbPlayer, tbPlayer;
	private Transform cbAvatar, tbAvatar;
	
	private float maxLife;
	private float life;
	private string type;

	public void config(float maxLife, string type) {
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
		
		if (type == "boss") {
		} else if (type == "chaseCb") {
			if (cbAvatar != null) {
				this.transform.position = Vector3.Lerp (this.transform.position, cbAvatar.position, Time.deltaTime * 0.1f);
			}
		} else if (type == "chaseBoth") {
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
