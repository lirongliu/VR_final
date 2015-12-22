using UnityEngine;
using System.Collections;

public class Constants : MonoBehaviour {

	public static int tbPlayerID = 0;
	public static int cbPlayerID = 1;

	public static float minOrthographicSize = 25;

	public static float cbAvatarHeight = 1.5f;
	public static float tbSpotlightHight = 50;

	public static float cbMaxSpotlightAngle = 80;
	public static float cbSpotlightIntensity = 4;
	public static float tbMaxSpotlightIntensity = 5;

	public static string tbPlayerAvatarTag = "iPadNetworkedPlayerAvatar";
	public static string cbPlayerAvatarTag = "cbNetworkedPlayerAvatar";
	
	public static string tbNetworkedPlayerTag = "iPadNetworkedPlayer";
	public static string cbNetworkedPlayerTag = "cbNetworkedPlayer";

	public static string bossTag = "boss";

	public static string backyardSceneName = "BackyardScene";
	public static string darkBackyardSceneName = "DarkBackyardScene";
	public static string bossSceneName = "BossScene";

	public static float defaultMovingSpeed = 6f;
	
	public static Vector3 backyardStartCoord = new Vector3 (-6.5f, 2, 6.5f);
	//	public static Vector3 backyardDestinationCoord = new Vector3 (68, 1, -93);
	public static Vector3 backyardDestinationCoord = new Vector3 (50, 2, -35);

	public static Vector3 darkBackyardStartCoord = backyardDestinationCoord;
	public static Vector3 darkBackyardDestinationCoord = backyardStartCoord;

	public static int ghostDamage = 20;

	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
