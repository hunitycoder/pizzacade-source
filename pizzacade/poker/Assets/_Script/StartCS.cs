using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCS : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject LoginScreen;
    public GameObject MainMenu;

    public InputField InputUserName;
    public InputField InputRoomID;

    void Start()
    {
        GlobalValue.Coins = 1000;
        MainMenu.SetActive(false);
        LoginScreen.SetActive(true);
  
        InputUserName.text = GlobalValue.ProfileName;
    }

    public void OnClickLoginButton()
    {
        GlobalValue.ProfileName = InputUserName.text;
        if (Application.platform == RuntimePlatform.WebGLPlayer)
        {
            Application.ExternalCall("socket.emit", "login", GlobalValue.ProfileName);
        }

        OpenMainMenu();
    }

    public void OnClickRandom()
    {
        PUNMenu.Instant.JoinRandomGame();
    }

    public void OnClickCreateRoom()
    {
        PUNMenu.Instant.CreatePokerRoom("Friend" + (GlobalValue.MyRandomInt(100)+400)) ;
    }

    public void OnClickJoinButton()
    {
        string roomName = "Friend" + InputRoomID.text;
        PUNMenu.Instant.JoinRoomFriend(roomName);
    }
    public void OpenMainMenu()
    {
        MainMenu.SetActive(true);
        LoginScreen.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
