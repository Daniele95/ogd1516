using UnityEngine;
using System.Collections;
using UnityEngine.Networking;
using UnityEngine.UI;

public class ControllerGaming : NetworkBehaviour {
	[SyncVar]
	public int scoreTeam0 = 0;
	[SyncVar]
	public int scoreTeam1 = 0;

	public float timerArena = 100f;
	[SyncVar]
	public float timer = 0f;

	private GameObject scoreTextTeam0;
    private GameObject scoreTextTeam1;
    private GameObject timingText;
	private GameObject timingLoader;

	// Use this for initialization
	void Start () {
		timer = timerArena;

        scoreTextTeam0 = GameObject.Find ("ScoreBottomTextTeam0");
        scoreTextTeam1 = GameObject.Find("ScoreBottomTextTeam1");
        timingText = GameObject.Find ("TimingText");
		timingLoader = GameObject.Find ("TimingLoader");
	}

	void OnGUI() {
        /*int w = Screen.width, h = Screen.height;

		GUIStyle style = new GUIStyle();

		Rect rect = new Rect(0, 0, w, h);

		style.alignment = TextAnchor.UpperCenter;
		style.fontSize = h * 10 / 100;
		style.normal.textColor = Color.blue;

		string text = "";

		if (timer <= 0f) {
			if (scoreTeam0 > scoreTeam1)
				text = "Team0 wins";
			else if (scoreTeam0 < scoreTeam1)
				text = "Team1 wins";
			else
				text = "Draw";
		} else {
			int minutes = Mathf.FloorToInt (timer / 60F);
			int seconds = Mathf.FloorToInt (timer - minutes * 60);
			string niceTime = string.Format ("{0:0}:{1:00}", minutes, seconds);

			text = "Time: " + niceTime.ToString ();
		}

		text += " Scores: " + scoreTeam0.ToString() + " - " + scoreTeam1.ToString();

		GUI.Label(rect, text, style);*/

        //if (!isLocalPlayer)
        //	return;

        scoreTextTeam0.GetComponent<Text> ().text = scoreTeam0.ToString ();
        scoreTextTeam1.GetComponent<Text>().text = scoreTeam1.ToString();

        int minutes = Mathf.FloorToInt (timer / 60F);
		int seconds = Mathf.FloorToInt (timer - minutes * 60);
		string niceTime = string.Format ("{0:0}:{1:00}", minutes, seconds);

		timingText.GetComponent<Text>().text = niceTime.ToString ();

		timingLoader.GetComponent<Image> ().fillAmount = timer / timerArena;
	}

	[ClientRpc]
	void RpcTimingArena(float timingArena){
		timer = timingArena;
	}

	[ClientRpc]
	void RpcScore(int scoreTeam0Value, int scoreTeam1Value){
		scoreTeam0 = scoreTeam0Value;
		scoreTeam1 = scoreTeam1Value;
	}

	[Command]
	void CmdAddScoreTeam(int team){
		if (timer > 0f) {
			if (team == 0) {
				scoreTeam0++;
			} else if (team == 1) {
				scoreTeam1++;
			}
		}

		RpcScore (scoreTeam0, scoreTeam1);
	}

	public void addScoreTeam(int team){
		CmdAddScoreTeam (team);
	}

	[Command]
	void CmdUpdateArena()
	{
		timer -= Time.deltaTime;

		if (timer <= 0f) {
			timer = 0f;
		}

		RpcTimingArena (timer);
	}

	// Update is called once per frame
	void Update () {
		if (!isServer)
			return;

		CmdUpdateArena ();
	}
}
