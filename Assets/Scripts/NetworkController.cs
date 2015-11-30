using UnityEngine;
using System.Collections;


public class NetworkController : MonoBehaviour
{
	public bool iPadTest = false;
	public bool cbTest = false;
	string _room = "Cardboard_Networking";
	public static int whoAmI;
	
	void Start()
	{
		PhotonNetwork.ConnectUsingSettings("0.1");
		Debug.Log ("Connected");
		
	}
	
	void OnJoinedLobby()
	{
		Debug.Log("joined lobby");
		
		RoomOptions roomOptions = new RoomOptions() { };
		PhotonNetwork.JoinOrCreateRoom(_room, roomOptions, TypedLobby.Default);
	}
	
	
	void OnJoinedRoom()
	{
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
		
		if (whoAmI == Constants.IS_CB_PLAYER) {
			GameObject networkedPlayer = PhotonNetwork.Instantiate("cbNetworkedPlayer", Vector3.zero, Quaternion.identity, 0);
		} else {        
			GameObject networkedPlayer = PhotonNetwork.Instantiate("iPadNetworkedPlayer", Vector3.zero, Quaternion.identity, 0);
		}
		
		//PhotonNetwork.Instantiate("CardboardMain", new Vector3(Random.Range(-5.0F, 5.0F), 0, Random.Range(-5.0F, 5.0F)), Quaternion.identity, 0);
		
	}
}