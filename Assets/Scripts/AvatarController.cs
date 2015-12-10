using UnityEngine;
using System.Collections;

public class AvatarController : MonoBehaviour {

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
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
