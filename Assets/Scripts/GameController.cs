using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameController : Photon.MonoBehaviour {
	
	public static bool play_audio=true;
	public string currScene = Constants.backyardSceneName;


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
