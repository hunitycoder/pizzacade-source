using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class GameManager : MonoBehaviour
{
    // Start is called before the first frame update

    public string myTeam = "blue";

    public static GameManager Instance;

    public Text RedScore;
    public Text BlueScore;

    int redScore;
    int blueScore;
    public Text turnIndicator;
    public Text roomIDIndicator;
    public GameObject WaitingPanel;

    public bool bGameOver = false;
    public WordCard[] wordcards;
    public GameObject nextGameButton;
    public string currentTurn = "red";
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        nextGameButton.SetActive(false);
        if (GlobalValue.IsPokerController)
        {
            myTeam = "red";
        }
        else
        {
            myTeam = "blue";
        }
        LoadGameData();

        WaitingPanel.SetActive(true);
        
        turnIndicator.gameObject.SetActive(false);
        

        if (PhotonNetwork.connected)
        {
            StartCoroutine(WaitingForOtherPlayer());

            if (PhotonNetwork.room.Name.Contains("Friend")){
                roomIDIndicator.text = "RoomID: " +PhotonNetwork.room.Name.Substring(6);
            }
            else
            {
                roomIDIndicator.text = "RandomRoom";
            }
        }

    }

    IEnumerator WaitingForOtherPlayer()
    {
        yield return new WaitForEndOfFrame();
        while ( true)
        {
            if (PUNMenu.Instant.PlayersJoined() >= 8)
            {
                WaitingPanel.SetActive(false);
                break;
            }

            yield return new WaitForSeconds(1);

        }

        StartGame();
    }

    void StartGame()
    {
        if(GlobalValue.IsPokerController)
        {
            PUNMenu.Instant.SendTurn("red");
        }
    }

    public bool IsMyTurn()
    {
        if (myTeam != currentTurn) return false;
        return true;
    }

    public void RemoteWordClicked( string playerColor, int wordID)
    {
        if( wordcards[wordID].wordID == wordID)
        {
            wordcards[wordID].cardState = WordCard.CardState.Clicked;
            wordcards[wordID].Show();
        }
        if(wordcards[wordID].myColor == "red")
        {
            redScore--;
        }else if(wordcards[wordID].myColor == "blue")
        {
            blueScore--;
        }

        UpdateScore();
        
        if( wordcards[wordID].myColor == "spy")
        {
            if(GlobalValue.IsPokerController)
            {
                PUNMenu.Instant.SendGameOver(playerColor);
            }
        }else if( redScore == 0)
        {
            if (GlobalValue.IsPokerController)
            {
                PUNMenu.Instant.SendGameOver("blue");
            }
        }else if( blueScore == 0)
        {
            if (GlobalValue.IsPokerController)
            {
                PUNMenu.Instant.SendGameOver("red");
            }
        }
        else
        {
            if(GlobalValue.IsPokerController)
            {
                if (wordcards[wordID].myColor != playerColor)
                {
                    if (playerColor == "blue")
                    {
                        PUNMenu.Instant.SendTurn("red");
                    }
                    else if(playerColor == "red")
                    {
                        PUNMenu.Instant.SendTurn("blue");
                    }
                }
            }
            
        }
    }

    public void UpdateScore()
    {
        RedScore.text = redScore.ToString();
        BlueScore.text = blueScore.ToString();
    }

    public void RemoteGameOver( string looserColor)
    {
        bGameOver = true;
        if( looserColor == myTeam)
        {
            turnIndicator.text = "you lose";
        }
        else
        {
            turnIndicator.text = "you win";
        }

        for( int i = 0; i < wordcards.Length; i++)
        {
            if( wordcards[i].cardState == WordCard.CardState.Idle)
            {
                wordcards[i].cardState = WordCard.CardState.GameOver;
                wordcards[i].Show();
            }
        }

        nextGameButton.SetActive(true);
    }


    public void RemoteTurn(string turnColor)
    {
        currentTurn = turnColor;

        turnIndicator.gameObject.SetActive(true);

        if( currentTurn == myTeam)
        {
            turnIndicator.text = "your turn";
        }
        else
        {
            turnIndicator.text = "opponent turn";
        }

        if( currentTurn == "blue")
        {
            turnIndicator.color = Color.blue;
        }else if( currentTurn == "red")
        {
            turnIndicator.color = Color.red;
        }
    }
    void LoadGameData()
    {
        TextAsset tlevelData = Resources.Load<TextAsset>("Levels/level1");
        string leveldata = tlevelData.text;
        string[] lines= leveldata.Split('\n');
        string[] words = lines[0].Split(',');
        string[] wordcolors = lines[1].Split(',');
        Debug.Log(words.Length);
        Debug.Log(wordcolors.Length);
        
        for ( int i = 0; i < wordcards.Length; i++)
        {
            wordcards[i].SetWordCard(words[i], wordcolors[i]);
        }

        redScore = blueScore = 0;
        for (int i = 0; i < wordcards.Length; i++)
        {
            if(wordcolors[i] == "blue")
            {
                blueScore++;
            }else if( wordcolors[i] == "red")
            {
                redScore++;
            }
        }

        RedScore.text = redScore.ToString();
        BlueScore.text = blueScore.ToString();


    }
    // Update is called once per frame
    void Update()
    {
        
    }

    public void SetTeam( string team)
    {
        myTeam = team;
    }

    public void NextGame()
    {
        Application.ExternalCall("RefreshPage");
    }
}
