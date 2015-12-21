using UnityEngine;
using System.Collections;

public class EnemyController : Photon.MonoBehaviour {
	
	protected float maxLife;
	protected float life;
	protected string type;

	void Start () {

	}
	

	void Update () {
	}

	void FixedUpdate(){

	}

	public virtual void config (float maxLife, string type) {}
	
	public virtual void revive() {
		Renderer r = GetComponent<Renderer> ();
		r.material.color = Color.gray;

		life = maxLife;
	}

	public virtual void getHit(float damage) {
		Renderer r = GetComponent<Renderer> ();
		r.material.color = Color.white;

		life -= damage * Time.deltaTime;
	}

	public bool shouldBeDead() {
		return life <= 0;
	}

	public float getLife() {
		return life;
	}
	

}
