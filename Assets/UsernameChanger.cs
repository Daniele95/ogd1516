using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class UsernameChanger : MonoBehaviour {

    private bool inside;
    private Text username;

    public string currentMenu;
    public Canvas usernameCanvas;
    public Canvas ClassesUICanvas;
    public Canvas ClassesMeshesCanvas;
    public InputField usernameInputField;

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
        inside = false;
    }

    void Update ()
    {
        if (Input.GetButtonDown("XboxY") && !inside)
            openUsernameCanvas();

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

        usernameInputField.textComponent.text = GameObject.Find("NetVehicleContainer").GetComponent<NetVehicleContainer>().player;
        usernameInputField.Select();
        usernameInputField.ActivateInputField();
    }

    public void closeUsernameCanvas(bool submitted)
    {
        if (submitted)
            changeUsername(usernameInputField.text);

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
        usernameCanvas.gameObject.SetActive(false);
    }
}
