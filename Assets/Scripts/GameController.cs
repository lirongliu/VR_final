using UnityEngine;
using System.Collections;

public class GameController : MonoBehaviour {

	public static bool play_audio=false;

	public string currScene = Constants.backyardSceneName;
	

	// Use this for initialization
	void Start () {
		DontDestroyOnLoad (this);
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	public string getNextScene() {
		print ("currScene " + currScene);
		string nextScene = "";
		if (currScene == Constants.backyardSceneName) {
			nextScene = Constants.bossSceneName;
		} else if (currScene == Constants.bossSceneName) {
			nextScene = Constants.darkBackyardSceneName;
		} else if (currScene == Constants.darkBackyardSceneName) {
			nextScene = Constants.backyardSceneName;		//	for testing only... not working now...
		}
		return nextScene;
	}

	public void setCurrScene(string currScene) {
		this.currScene = currScene;
	}
}
