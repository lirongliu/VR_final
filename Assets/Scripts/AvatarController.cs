using UnityEngine;
using System.Collections;

public class AvatarController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		
		Transform avatarBodyTransform = this.transform.Find ("AvatarBody");
		if (avatarBodyTransform.localPosition.y < -1.4f) {
			
			Vector3 offset = new Vector3(0, avatarBodyTransform.localPosition.y + 1.4f, 0);
			
			avatarBodyTransform.localPosition = new Vector3(0, 3f, 0);
			this.transform.localPosition += offset;
		}
		if (avatarBodyTransform.localPosition.x != 0 || avatarBodyTransform.localPosition.z != 0) {
			
			Vector3 offset = new Vector3(avatarBodyTransform.localPosition.x, 0, avatarBodyTransform.localPosition.z);
			
			avatarBodyTransform.localPosition = avatarBodyTransform.localPosition - offset;
			this.transform.localPosition += offset;
		}
	}
	
	public void Move(Vector3 vec) {

		/* prevent the separation between parent and child. */
		Transform avatarBodyTransform = this.transform.Find ("AvatarBody");
		this.transform.localPosition += vec;

		Vector3 offset = avatarBodyTransform.localPosition;

		this.transform.localPosition += offset;
		avatarBodyTransform.localPosition = Vector3.zero;

	}
}
