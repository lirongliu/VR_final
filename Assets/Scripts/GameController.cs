using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class GameController : MonoBehaviour {

	public static bool play_audio=true;

	public string currScene = Constants.backyardSceneName;
	
	public static Text tb_instruction;
	public static Text cb_instruction;

	void Start () {
		DontDestroyOnLoad (this);

		tb_instruction=GameObject.FindWithTag ("instruction").transform.Find("Text").GetComponent<Text>();

		tb_instruction.text="Task 1:\nWork with your partner to find the exit of the maze!\nPlease note: neither of you can see the whole scene of the maze.";
		StartCoroutine(wait5s());

		print (Utility.FindTransform (GameObject.FindGameObjectWithTag ("cardboardCamera").transform, "Canvas"));

		//cb_instruction.text="Task 1:\nWork with your partner to find the exit of the maze!\nPlease note: neither of you can see the whole scene of the maze.";
		//StartCoroutine(wait5s());
		//print ("cb_instruction"+cb_instruction.text);
	}
	

	IEnumerator wait5s() {
		yield return new WaitForSeconds(5);
		tb_instruction.text = "";
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
