using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsernameChanger : MonoBehaviour {

    private Text username;

    public string currentMenu;
    public Canvas usernameCanvas;
    public Canvas ClassesUICanvas;
    public Canvas ClassesMeshesCanvas;
    public InputField usernameInputField;
    public Text lblInputField;

    private bool isUsername;

    private const string FORMATTED_UNRANKED = " <color=#808080ff><size=19>unranked</size></color>";
    public const string FIRST = "first";
    public const string SECOND = "second";
    public const string THIRD = "third";
    public const string FOURTH = "fourth";
    public const string JOIN = "join";
    public const string CLASSES = "classes";

    void Start ()
    {
        usernameCanvas.gameObject.SetActive(false);
        username = gameObject.GetComponent<Text>();
        username.text = GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().player + FORMATTED_UNRANKED;
        currentMenu = FIRST;
        isUsername = true;
    }

    void Update ()
    {
        if (Input.GetButtonDown("XboxY"))
        {
            openUsernameCanvas();
            GameObject.Find("Cnvs_main").GetComponent<AudioSource>().Play();
        }

        if (Input.GetButtonDown("XboxA") && usernameInputField.isActiveAndEnabled && usernameInputField.isFocused)
        {
            GameObject.Find("Cnvs_main").GetComponent<AudioSource>().Play();
            closeUsernameCanvas(true);
        }

        if (Input.GetButtonDown("XboxB") && usernameInputField.isActiveAndEnabled && usernameInputField.isFocused)
        {
            GameObject.Find("Cnvs_main").GetComponent<AudioSource>().Play();
            closeUsernameCanvas(false);
        }
    }

    public void changeUsername(string newUsername)
    {
        GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().player = newUsername;
        username.text = newUsername + FORMATTED_UNRANKED;
        usernameCanvas.gameObject.SetActive(true);
    }

    public void openUsernameCanvas()
    {
        switch(currentMenu)
        {
            case FIRST:
            case SECOND:
            case THIRD:
            case FOURTH:
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().CloseEverything();
                break;
            case JOIN:
                GameObject.Find("Cnvs_join").GetComponent<JoinManager>().GoToSleep();
                break;
            case CLASSES:
                ClassesUICanvas.gameObject.SetActive(false);
                ClassesMeshesCanvas.gameObject.SetActive(false);
                break;
        }
        usernameCanvas.gameObject.SetActive(true);
        for (int i = 0; i < usernameCanvas.transform.childCount; ++i)
        {
            usernameCanvas.transform.GetChild(i).gameObject.SetActive(true);
        }

        if (isUsername)
            usernameInputField.textComponent.text = GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().player;
        else
            usernameInputField.textComponent.text = GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().ipAddress;

        if (isUsername)
            lblInputField.text = "Username";
        else
            lblInputField.text = "Host Ip Address";
        usernameInputField.Select();
        usernameInputField.ActivateInputField();
    }

    public void closeUsernameCanvas(bool submitted)
    {
        if (submitted && isUsername)
            changeUsername(usernameInputField.text);
        else
            if (submitted && !isUsername)
            {
                if (usernameInputField.text.Equals(""))
                    ClassesMeshesCanvas.GetComponent<ClassMenuManager>().ipAddress = "127.0.0.1";
                else
                    ClassesMeshesCanvas.GetComponent<ClassMenuManager>().ipAddress = usernameInputField.text;

                this.currentMenu = UsernameChanger.CLASSES;

                isUsername = true;
                usernameCanvas.gameObject.SetActive(false);

                ClassesMeshesCanvas.gameObject.SetActive(true);
                ClassesUICanvas.gameObject.SetActive(true);

                return;
            }

        switch(currentMenu)
        {
            case FIRST:
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenFirstMenu();
                break;
            case SECOND:
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenFirstMenu();
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenNewGameMenu();
                break;
            case THIRD:
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenFirstMenu();
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenNewGameMenu();
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenPrivateGameMenu();
                break;
            case FOURTH:
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenFirstMenu();
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenNewGameMenu();
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenPrivateGameMenu();
                GameObject.Find("Cnvs_btns").GetComponent<BtnManager>().OpenGameModeMenu();
                break;
            case JOIN:
                GameObject.Find("Cnvs_join").GetComponent<JoinManager>().WakeUp();
                break;
            case CLASSES:
                ClassesUICanvas.gameObject.SetActive(true);
                ClassesMeshesCanvas.gameObject.SetActive(true);
                break;
        }
        isUsername = true;
        usernameCanvas.gameObject.SetActive(false);
    }

    public void openJoinCanvas()
    {
        isUsername = false;
        this.openUsernameCanvas();
    }
}
