using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartCS : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject MainMenu;

    public InputField InputRoomID;

    void Start()
    {
        MainMenu.SetActive(true);

    }

    public void OnClickRandom()
    {
        PUNMenu.Instant.JoinRandomGame();
    }

    public void OnClickCreateRoom()
    {
        PUNMenu.Instant.CreatePokerRoom("Friend" + GlobalValue.MyRandomInt(100)) ;
    }

    public void OnClickJoinButton()
    {
        string roomName = "Friend" + InputRoomID.text;
        PUNMenu.Instant.JoinRoomFriend(roomName);
    }
    public void OpenMainMenu()
    {
        MainMenu.SetActive(true);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
