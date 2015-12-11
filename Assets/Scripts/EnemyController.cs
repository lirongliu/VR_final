using UnityEngine;
using System.Collections;

public class EnemyController : Photon.MonoBehaviour {

	private GameObject cbPlayer;
	private Transform avatar;

	private float life;

	void Start () {
		this.revive ();
		
		if (avatar == null) {
			cbPlayer = GameObject.FindWithTag(Constants.cbNetworkedPlayerTag);
			if (cbPlayer != null) {
				avatar=cbPlayer.transform.Find("Avatar");
			}
		}
	}
	

	void Update () {
	}

	void FixedUpdate(){
		
		if (avatar != null) {
			this.transform.position = Vector3.Lerp (this.transform.position, avatar.position, Time.deltaTime * 0.1f);
		}
	}
	
	public void revive() {
		Renderer r = GetComponent<Renderer> ();
		r.material.color = Color.gray;

		life = 100;
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
