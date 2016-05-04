using UnityEngine;
using System.Collections;

public class ControllerGame : MonoBehaviour {
	public int scoreTeam0 = 0;
	public int scoreTeam1 = 0;
	public float timerArena = 100f;

	private float timer = 0f;

	// Use this for initialization
	void Start () {
		timer = timerArena;
	}

	void OnGUI() {
		int w = Screen.width, h = Screen.height;

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
			
		GUI.Label(rect, text, style);
	}

	public void addScoreTeam(int team){
		if (timer > 0f) {
			if (team == 0) {
				scoreTeam0++;
			} else if (team == 1) {
				scoreTeam1++;
			}
		}
	}
	
	// Update is called once per frame
	void Update () {
		timer -= Time.deltaTime;

		if (timer <= 0f) {
			timer = 0f;


		}
	}
}
