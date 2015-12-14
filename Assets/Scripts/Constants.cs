using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {

	public static int IS_IPAD_PLAYER = 0;
	public static int IS_CB_PLAYER = 1;

	public static float minOrthographicSize = 25;

	public static float cbAvatarHeight = 1.5f;
	public static float tbSpotlightHight = 50;

	public static float cbMaxSpotlightAngle = 50;
	public static float cbMaxSpotlightIntensity = 5;

	public static string tbPlayerAvatarTag = "iPadNetworkedPlayerAvatar";
	public static string cbPlayerAvatarTag = "cbNetworkedPlayerAvatar";
	
	public static string tbNetworkedPlayerTag = "iPadNetworkedPlayer";
	public static string cbNetworkedPlayerTag = "cbNetworkedPlayer";

	public static string bossTag = "boss";

	public static string backyardSceneName = "BackyardScene";
	public static string darkBackyardSceneName = "DarkBackyardScene";
	public static string bossSceneName = "BossScene";

	public static float defaultMovingSpeed = 6f;
	
	public static Vector3 backyardStartCoord = new Vector3 (0, 1, -2);
	public static Vector3 backyardDestinationCoord = new Vector3 (68, 1, -93);

	public static Vector3 darkBackyardStartCoord = new Vector3 (68, 1, -93);
	public static Vector3 darkBackyardDestinationCoord = backyardStartCoord;


	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
