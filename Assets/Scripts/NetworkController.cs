using UnityEngine;
using System.Collections;


public class NetworkController : MonoBehaviour
{
	public bool iPadTest = false;
	public bool cbTest = false;
	string _room = "Cardboard_Networking";
	public static int whoAmI;
	public static ArrayList enemyList;
	public GameObject Enemy;

	private GameObject enemy1,enemy2,enemy3;

	void Start(){
		enemyList = new ArrayList ();

		PhotonNetwork.ConnectUsingSettings("0.1");
		Debug.Log ("Connected");
		
	}
	
	void OnJoinedLobby(){
		Debug.Log("joined lobby");
		
		RoomOptions roomOptions = new RoomOptions() { };
		PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);
	}
	
	
	void OnJoinedRoom(){


		//set cameras for different users
		if (iPadTest) {
			GameObject.Find ("CardboardMain").SetActive (false);
			GameObject.Find ("iPadCamera").SetActive (true);
			whoAmI = Constants.IS_IPAD_PLAYER;
		} else if (cbTest) {
			GameObject.Find ("CardboardMain").SetActive (true);
			GameObject.Find ("iPadCamera").SetActive (false);
			whoAmI = Constants.IS_CB_PLAYER;
		} else {
			if (PhotonNetwork.playerList.Length == 1) {        //    iPad
				
				GameObject.Find ("CardboardMain").SetActive (false);
				GameObject.Find ("iPadCamera").SetActive (true);
				whoAmI = Constants.IS_IPAD_PLAYER;
				//            PhotonNetwork.Instantiate("NetworkedPlayer", Vector3.zero, Quaternion.identity, 0);
			} else {        // cardboard
				
				GameObject.Find ("CardboardMain").SetActive (true);
				GameObject.Find ("iPadCamera").SetActive (false);
				whoAmI = Constants.IS_CB_PLAYER;
			}
		}

		Vector3 originalTablet = new Vector3 (5, 2, -1);
		Vector3 originalCB = new Vector3 (-5, 1, -2);
		//instantiate cardboard & ipad players, and some enemies
		if (whoAmI == Constants.IS_CB_PLAYER) {
			GameObject networkedPlayer = PhotonNetwork.Instantiate("cbNetworkedPlayer",Vector3.zero , Quaternion.identity, 0);
//			GameObject networkedPlayer = PhotonNetwork.Instantiate("cbNetworkedPlayer",originalCB , Quaternion.identity, 0);


		} else {        
			GameObject networkedPlayer = PhotonNetwork.Instantiate("iPadNetworkedPlayer", Vector3.zero, Quaternion.identity, 0);
//			GameObject networkedPlayer = PhotonNetwork.Instantiate("iPadNetworkedPlayer", originalTablet, Quaternion.identity, 0);



		}
		//PhotonNetwork.Instantiate("CardboardMain", new Vector3(Random.Range(-5.0F, 5.0F), 0, Random.Range(-5.0F, 5.0F)), Quaternion.identity, 0);
		enemy1 = (GameObject)Instantiate (Enemy, new Vector3 (15, 1, -9), Quaternion.identity);
		enemy2 = (GameObject)Instantiate (Enemy, new Vector3 (3.5f, 1, -21), Quaternion.identity);
		enemy3 = (GameObject)Instantiate (Enemy, new Vector3 (-13.5f, 1, -11), Quaternion.identity);

		enemyList.Add(enemy1);
		enemyList.Add(enemy2);
		enemyList.Add(enemy3);
		print("NetworkController enemyList:"+enemyList[0]+"\t"+enemyList[1]+"\t"+enemyList[2]);
	}

	[PunRPC]
	void createEnemy(){


	}
}