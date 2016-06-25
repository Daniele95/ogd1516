using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using System.Text.RegularExpressions;

public class JoinManager : MonoBehaviour {
    public Canvas btnCanvas;
    public Canvas classMenuManager;
    public Canvas classMenuUIManager;
    public InputField inputField;

    private GameObject canvas;
    private Text lbl;
    private BtnManager btnManager;
    private UsernameChanger usernameChanger;
    private string input;

    // Use this for initialization
    void Start () {
        canvas = this.gameObject;
        lbl = canvas.GetComponentInChildren<Text>();
        lbl.gameObject.SetActive(false);
        inputField.gameObject.SetActive(false);
        btnManager = btnCanvas.GetComponent<BtnManager>();
        usernameChanger = GameObject.Find("TextUsername").GetComponent<UsernameChanger>();
    }
	
	public void WakeUp()
    {
        inputField.gameObject.SetActive(true);
        lbl.gameObject.SetActive(true);
        inputField.Select();
        inputField.ActivateInputField();
        classMenuManager.gameObject.SetActive(false);
        classMenuUIManager.gameObject.SetActive(false);

        usernameChanger.currentMenu = UsernameChanger.JOIN;
    }

    public void GoToSleep()
    {
        inputField.gameObject.SetActive(false);
        lbl.gameObject.SetActive(false);
    }

    public void Send()
    {
        classMenuManager.GetComponent<ClassMenuManager>().host = false;

        if (inputField.text.Equals(""))
            classMenuManager.GetComponent<ClassMenuManager>().ipAddress = "127.0.0.1";
        else
            classMenuManager.GetComponent<ClassMenuManager>().ipAddress = inputField.text;

        usernameChanger.currentMenu = UsernameChanger.CLASSES;

        GoToSleep();

        classMenuManager.gameObject.SetActive(true);
        classMenuUIManager.gameObject.SetActive(true);
    }

    void Update()
    {
       inputField.text = Regex.Replace(inputField.text, @"[^0-9 .]", "");
       if(Input.GetButtonDown("XboxA") && inputField.isActiveAndEnabled && inputField.isFocused)
       {
            GameObject.Find("Cnvs_main").GetComponent<AudioSource>().Play();
            Send();
       }
       if(Input.GetButtonDown("XboxB") && inputField.isActiveAndEnabled && inputField.isFocused)
       {
            GameObject.Find("Cnvs_main").GetComponent<AudioSource>().Play();
            btnManager.ReturnToPrivateGameMenu();
            GoToSleep();
        }
    }
}
