using UnityEngine;
using System.Collections;

public class DarkSceneCbPlayer : NetworkedPlayer {

	private GameObject hitEnemy;
	

	void Start ()
	{
		DontDestroyOnLoad (this);
		

		avatar.transform.localPosition = new Vector3 (-5, 0.5f, -2);
		
		if (photonView.isMine) {
			GameObject cb = GameObject.Find ("CardboardMain");
			
			Transform avatarHeadTransform = this.transform.Find("AvatarHead");
			Transform avatarBodyTransform = this.transform.Find("AvatarBody");
			
			
			/* hierachy:
			 * avatar
			 * avatarBody
			 * CardboardMain
			 * head
			 */
			avatarBodyTransform.SetParent(avatar.transform);
			avatarBodyTransform.localPosition = Vector3.zero;
			
			cb.transform.SetParent(avatarBodyTransform);
			avatarHeadTransform.SetParent(cb.transform);
			cb.transform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
			avatarHeadTransform.localPosition = Vector3.zero;
			
			// set head transform
			this.headTransform = cb.transform;
		}   else {

			Transform avatarHeadTransform = this.transform.Find("AvatarHead");
			Transform avatarBodyTransform = this.transform.Find("AvatarBody");
			
			/* hierachy:
			 * avatar
			 * avatarBody
			 * head
			 */
			avatarBodyTransform.SetParent(avatar.transform);
			avatarBodyTransform.localPosition = Vector3.zero;
			
			avatarHeadTransform.SetParent(avatarBodyTransform);
			avatarHeadTransform.localPosition = new Vector3(0, Constants.cbAvatarHeight, 0);
			
			// set head transform
			this.headTransform = Utility.FindTransform (avatar.transform, "AvatarHead");
		}
		
		playerLocal = headTransform;
		playerGlobal = avatar.transform;		
	}
	
	void Update(){
		
		if (!photonView.isMine) {
			//Update remote player (smooth this, this looks good, at the cost of some accuracy)
			avatar.transform.localPosition = Vector3.Lerp (avatar.transform.localPosition, correctAvatarPos, Time.deltaTime * 5);
			avatar.transform.localRotation = Quaternion.Lerp (avatar.transform.localRotation, correctAvatarRot, Time.deltaTime * 5);
			headTransform.localRotation = Quaternion.Lerp (headTransform.localRotation, correctHeadRot, Time.deltaTime * 5);
		}
		else {
			inputHandler();
			
			//gaze
			if (NetworkController.whoAmI == Constants.IS_CB_PLAYER) {
				RaycastHit hit;
				if (Physics.Raycast (Camera.main.transform.position, Camera.main.transform.forward, out hit)) {
					if (hit.collider != null) {
						if (hit.collider.gameObject == hitEnemy) {
							EnemyController ec = hitEnemy.GetComponent<EnemyController>();
							ec.getHit(300);
							
							if (ec.shouldBeDead()) {
								
								if (NetworkController.enemyList.IndexOf (hitEnemy)!=null && NetworkController.enemyList.IndexOf (hitEnemy)!=(-1)){
									//print("NetworkedPlayer enemyList:"+NetworkController.enemyList[0]+"\t"+NetworkController.enemyList[1]+"\t"+NetworkController.enemyList[2]);
									//print("INDEX:"+NetworkController.enemyList.IndexOf (hit.collider.gameObject));
									photonView.RPC ("destroyEnemy",PhotonTargets.All,NetworkController.enemyList.IndexOf(hitEnemy));
								}
							}
							
						} else {
							if (hitEnemy != null) {
								EnemyController ec = hitEnemy.GetComponent<EnemyController>();
								ec.revive();
								hitEnemy = null;
							}
							
							//							print ("hit.collider.gameObject.tag " + hit.collider.gameObject.tag);
							if (hit.collider.CompareTag("Enemy")) {
								hitEnemy = hit.collider.gameObject;
							}
						}
					}
				}
				
				// check whether the spotlight hits tablet avatar
				GameObject tabletPlayerAvatar = Utility.getTabletPlayerAvatar();
				if (tabletPlayerAvatar != null) {
					float angle = Utility.getVectorAngle(Camera.main.transform.forward, tabletPlayerAvatar.transform.position - Camera.main.transform.position);
					if(angle < Constants.cbSpotlightAngle / 2) {
						bool hitByLight = false;
						if (hit.collider != null) {
							if (Utility.checkTag(hit.collider.gameObject.transform, Constants.tabletPlayerAvatarTag)) {
								hitByLight = true;
							} else {
								float hitPointDist = Vector3.Distance(Camera.main.transform.position, hit.point);
								float tabletAvatarDist = Vector3.Distance(Camera.main.transform.position, tabletPlayerAvatar.transform.position);
								if (hitPointDist > tabletAvatarDist) {
									hitByLight = true;
								}
							}
						} else {
							hitByLight = true;
						}
						if (hitByLight) {
							print ("hit by spotlight!!!!");
							photonView.RPC ("decreseTabletSpotlightIntensity",PhotonTargets.All);
						}
						
					}
				}
				
				// only cb is responsible for generating enemies
				if(Time.frameCount%100==0){
					photonView.RPC ("generateEnemy",PhotonTargets.All,Random.Range(-90,90),Random.Range(-90,90));
				}
			}
		}
		
	}

}
