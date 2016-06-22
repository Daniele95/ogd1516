using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class JoinManager : MonoBehaviour {
    public Canvas btnCanvas;
    public Canvas classMenuManager;
    public Canvas classMenuUIManager;

    private GameObject canvas;
    private InputField inputField;
    private Text lbl;
    private BtnManager btnManager;
    private string input;

	// Use this for initialization
	void Start () {
        canvas = this.gameObject;
        inputField = canvas.GetComponentInChildren<InputField>();
        lbl = canvas.GetComponentInChildren<Text>();
        lbl.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        btnManager = btnCanvas.GetComponent<BtnManager>();
	}
	
	public void WakeUp()
    {
        inputField.gameObject.SetActive(true);
        lbl.gameObject.SetActive(true);
        inputField.Select();
        inputField.ActivateInputField();
        classMenuManager.gameObject.SetActive(false);
        classMenuUIManager.gameObject.SetActive(false);
    }

    public void GoToSleep()
    {
        inputField.gameObject.SetActive(false);
        lbl.gameObject.SetActive(false);
    }

    public void Send()
    {
        GoToSleep();

        classMenuManager.gameObject.SetActive(true);
        classMenuUIManager.gameObject.SetActive(true);

        classMenuManager.GetComponent<ClassMenuManager>().host = false;
    }

    void Update()
    {
        inputField.text = Regex.Replace(inputField.text, @"[^0-9 .]", "");
       if(Input.GetButtonDown("XboxA") && inputField.isActiveAndEnabled && inputField.isFocused)
       {
           Send();
       }
       if(Input.GetButtonDown("XboxB") && inputField.isActiveAndEnabled && inputField.isFocused)
       {
           btnManager.ReturnToPrivateGameMenu();
           GoToSleep();
        }
    }
}
