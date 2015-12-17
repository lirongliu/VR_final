using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using System.Collections.Generic;


public class GameController : MonoBehaviour {

	public static bool play_audio=true;

	public string currScene = Constants.backyardSceneName;

	public static Text instruction;

	private BackyardSceneController backyardSceneController = new BackyardSceneController();
	private BossSceneController bossSceneController = new BossSceneController();
	private DarkBackyardSceneController darkBackyardSceneController = new DarkBackyardSceneController();

	void Start () {
		DontDestroyOnLoad (this);

		instruction=GameObject.FindWithTag ("instruction").transform.Find("Text").GetComponent<Text>();

//		string text="Task 1:\nWork with your partner to find the exit of the maze!\nPlease note: neither of you can see the whole scene of the maze.";
//		StartCoroutine(showInstruction(text, 5));
//		StartCoroutine (Utility.waitFor (5));

		print (Utility.FindTransform (GameObject.FindGameObjectWithTag ("cardboardCamera").transform, "Canvas"));

		//cb_instruction.text="Task 1:\nWork with your partner to find the exit of the maze!\nPlease note: neither of you can see the whole scene of the maze.";
		//StartCoroutine(wait5s());
		//print ("cb_instruction"+cb_instruction.text);
	}
	

	IEnumerator showInstruction(string text, int seconds) {
		instruction.text = text;
//		print ("showInstruction starts");
		yield return new WaitForSeconds(seconds);
//		print ("showInstruction ends");
		instruction.text = "";
	}

	// Update is called once per frame
	void Update () {
		SceneController sc = null;
		if (NetworkController.whoAmI == Constants.IS_IPAD_PLAYER) {
			if (currScene == Constants.backyardSceneName) {
//				sc = backyardSceneController;
				string currInstruction = backyardSceneController.getCurrentInstruction();
				if (currInstruction != null) {
					print (currInstruction);

					StartCoroutine(showInstruction(currInstruction, 5));
					backyardSceneController.currInstructionIdx++;
				}
			} else if (currScene == Constants.bossSceneName) {
				//				sc = bossSceneController;
				string currInstruction = bossSceneController.getCurrentInstruction();
				if (currInstruction != null) {
					print (currInstruction);
					
					StartCoroutine(showInstruction(currInstruction, 5));
					bossSceneController.currInstructionIdx++;
				}
			} else if (currScene == Constants.darkBackyardSceneName) {
				//				sc = darkBackyardSceneController;
				string currInstruction = darkBackyardSceneController.getCurrentInstruction();
				if (currInstruction != null) {
					print (currInstruction);
					
					StartCoroutine(showInstruction(currInstruction, 5));
					darkBackyardSceneController.currInstructionIdx++;
				}
			}

			// not working
//			if (sc != null) {
//				print ("sc getCurrentInstruction " + sc.getCurrentInstruction());
//
//				if (sc.getCurrentInstruction() != null) {
//					print (sc.getCurrentInstruction());
//
//					showInstruction(sc.getCurrentInstruction(), 5);
//					sc.currInstructionIdx++;
//				}
//			}
		}
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
