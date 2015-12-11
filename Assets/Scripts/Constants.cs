using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {

	public static int IS_IPAD_PLAYER = 0;
	public static int IS_CB_PLAYER = 1;

	public static float minOrthographicSize = 10;

	public static float cbAvatarHeight = 1;

	public static float cbSpotlightAngle = 50;

	public static string tbPlayerAvatarTag = "iPadNetworkedPlayerAvatar";
	public static string cbPlayerAvatarTag = "cbNetworkedPlayerAvatar";
	
	public static string tbNetworkedPlayerTag = "iPadNetworkedPlayer";
	public static string cbNetworkedPlayerTag = "cbNetworkedPlayer";

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
