﻿using UnityEngine;
using System.Collections;

public class DarkBackyardSceneCbPlayer : NetworkedPlayer {

	private GameObject hitEnemy;
	

	void Start ()
	{
		DontDestroyOnLoad (this);

		avatar.transform.localPosition = new Vector3 (-5, 0.5f, -2);

		if (photonView.isMine) {
			GameObject cb = GameObject.Find ("CardboardMain");
			
			// set head transform
			this.headTransform = cb.transform;
		} else {
			
			// set head transform
			this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");
		}
		
		playerLocal = headTransform;
		playerGlobal = avatar.transform;

		
		// enable spotlight
		GameObject spotlight = Utility.FindTransform (this.transform, "Spotlight").gameObject;
		spotlight.GetComponent<Light> ().enabled = true;
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		} else {
			inputHandler ();
			RaycastHit hit;
			Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit);
			this.checkHitEnemies (hit, ref hitEnemy);
			this.checkHitTbPlayer (hit);
			
			// only cb is responsible for generating enemies
			if (Time.frameCount % 100 == 0) {
				photonView.RPC ("generateEnemy", PhotonTargets.All, Random.Range (-30, 30), Random.Range (-30, 30), 100f, "chaseBoth");
			}
		}
	}

}
