using UnityEngine;
using System.Collections;

public class BossController : EnemyController {
	private GameObject cbPlayer;
	private Transform cbAvatar, tbAvatar;

	private float currBossPosDegree = 0f;
	private float thisRoundBossRevolveTime = 0f;
	private float thisRoundBossRevolveSpeed = 0f;
	private int bossMovementDir = 1;	//	-1 or 1

	override public void config(float maxLife, string type, int id) {
		this.maxLife = maxLife;
		this.life = maxLife;
		this.type = type;
		this.id = id;
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
//		print ("thisRoundBossRevolveTime " + thisRoundBossRevolveTime);
//		print ("bossMovementDir " + bossMovementDir);
		if (thisRoundBossRevolveTime < 0) {
			thisRoundBossRevolveTime = Random.Range(0.3f, 2f);
			bossMovementDir = Random.Range(-1f, 1f) > 0 ? 1 : -1;
//			thisRoundBossRevolveSpeed = Random.Range(0.7f, 1.5f) * Mathf.PI * Time.fixedDeltaTime;
			thisRoundBossRevolveSpeed = Random.Range(0.2f, 0.35f) * Mathf.PI * Time.fixedDeltaTime;
		}

		currBossPosDegree += thisRoundBossRevolveSpeed * bossMovementDir;
		thisRoundBossRevolveTime -= Time.fixedDeltaTime;
		
		//	TODO: define boss movement
		//this.transform.position = Vector3.Lerp (this.transform.position, cbAvatar.position, Time.deltaTime * 2);
		
		//boss moves around cb player in a random way
//		float degree=Time.frameCount*0.01f;
		float radius = 10 * Mathf.Lerp( 0.7f, 1.4f, Mathf.PerlinNoise (Time.time, 0.0F));
//		Vector3 new_pos=new Vector3( cbAvatar.position.x+ Mathf.Cos(degree)*radius, cbAvatar.position.y+Random.Range(-2,2), cbAvatar.position.z+ Mathf.Sin(degree)*radius);
//		this.transform.position=Vector3.Lerp(this.transform.position, new_pos, Time.deltaTime*0.2f);
		Vector3 new_pos=new Vector3( cbAvatar.position.x+ Mathf.Cos(currBossPosDegree)*radius, 8 * Mathf.PerlinNoise (Time.time, 0.2f) - 3f, cbAvatar.position.z+ Mathf.Sin(currBossPosDegree)*radius);

//		new_pos = new Vector3 (new_pos.x + Mathf.PerlinNoise());
		this.transform.position = new_pos;
		this.transform.LookAt(cbAvatar);
	}

}
