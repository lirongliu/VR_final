using UnityEngine;
using System.Collections;


public class NetworkController : Photon.PunBehaviour
{
	public bool iPadTest = false;
	public bool cbTest = false;
	string _room = "Cardboard_Networking";
	public static int whoAmI;
	public static ArrayList enemyList;
	public GameObject Enemy;

	private GameObject enemy1,enemy2,enemy3;


	void Start(){
		DontDestroyOnLoad (this);
		enemyList = new ArrayList ();

		PhotonNetwork.ConnectUsingSettings("0.1");
		Debug.Log ("Connected");
	}

	void Update() {
	}
	
	public override void OnJoinedLobby() {
		Debug.Log("joined lobby");
		
		RoomOptions roomOptions = new RoomOptions() { };
		PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);
	}
	
	public override void OnJoinedRoom() {
		Debug.Log("joined room");

		//set cameras for different users
		if (iPadTest) {
			GameObject.Find ("CardboardMain").SetActive (false);
			GameObject.Find ("TabletCamera").SetActive (true);
			whoAmI = Constants.IS_IPAD_PLAYER;
		} else if (cbTest) {
			GameObject.Find ("CardboardMain").SetActive (true);
			GameObject.Find ("TabletCamera").SetActive (false);
			whoAmI = Constants.IS_CB_PLAYER;
		} else {
			if (PhotonNetwork.playerList.Length == 1) {        //    iPad
				
				GameObject.Find ("CardboardMain").SetActive (false);
				GameObject.Find ("TabletCamera").SetActive (true);
				whoAmI = Constants.IS_IPAD_PLAYER;
			} else {        // cardboard
				
				GameObject.Find ("CardboardMain").SetActive (true);
				GameObject.Find ("TabletCamera").SetActive (false);
				whoAmI = Constants.IS_CB_PLAYER;
			}
		}

		Vector3 originalTablet = new Vector3 (5, 2, -1);
		Vector3 originalCB = new Vector3 (-5, 1, -2);
		//instantiate cardboard & ipad players, and some enemies
		if (whoAmI == Constants.IS_CB_PLAYER) {
			GameObject networkedPlayer = PhotonNetwork.Instantiate("cbNetworkedPlayer",Vector3.zero , Quaternion.identity, 0);
		} else {        
			GameObject networkedPlayer = PhotonNetwork.Instantiate("iPadNetworkedPlayer", Vector3.zero, Quaternion.identity, 0);
		}

	}

	void OnPhotonPlayerDisconnected(PhotonPlayer player)
	{
		Debug.Log("OnPhotonPlayerDisconnected: " + player);
		reset ();
	}

	
	void reset() {
		foreach (GameObject enemy in enemyList) {
			Destroy(enemy);
		}
		enemyList.Clear ();
	}
}