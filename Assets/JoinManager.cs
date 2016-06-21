using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class JoinManager : MonoBehaviour {

    private GameObject canvas;
    private InputField inputField;
    private Text lbl;
    string input;

	// Use this for initialization
	void Start () {
        canvas = this.gameObject;
        inputField = canvas.GetComponentInChildren<InputField>();
        lbl = canvas.GetComponentInChildren<Text>();
        lbl.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
	}
	
	public void WakeUp()
    {
        inputField.gameObject.SetActive(true);
        lbl.gameObject.SetActive(false);
        inputField.Select();
        inputField.ActivateInputField();
    }

    void Update()
    {
        inputField.text = Regex.Replace(inputField.text, @"[^0-9 .]", "");
    }
}
