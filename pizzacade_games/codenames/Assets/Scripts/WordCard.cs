using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class WordCard : MonoBehaviour
{
    // Start is called before the first frame update
    public int wordID;
    public string myColor = "";
    Text word;
    Image Bg;
    public enum CardState
    {
        Idle = 0,
        Clicked,
        GameOver
    }

    public CardState cardState = CardState.Idle;
    private void Awake()
    {
        word = transform.GetChild(0).GetComponent<Text>();
        Bg = transform.GetComponent<Image>();
    }
    void Start()
    {
        
    }

    public void SetWordCard(string _word, string _color)
    {
        myColor = _color;
        word.text = _word;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnClickWord()
    {
        if (GameManager.Instance.bGameOver == true) return;
        if (GameManager.Instance.IsMyTurn() == false) return;
        GetComponent<Button>().interactable = false;

        cardState = CardState.Clicked;
        Show();

        PUNMenu.Instant.SendClickedWord(GameManager.Instance.myTeam, wordID);
        
    }

    void GameOver()
    {
        if (cardState == CardState.Idle) cardState = CardState.GameOver;
    }

    public void Show()
    {
        if( cardState == CardState.Idle)
        {
            word.color = Color.black;
            Bg.color = new Color(0.9f, 0.9f, 0.9f, 1);
        }else if( cardState == CardState.Clicked)
        {
            if( myColor == "red")
            {
                word.color = Color.white;
                Bg.color = Color.red;
            }else if( myColor == "blue")
            {
                word.color = Color.white;
                Bg.color = Color.blue;
            }else if( myColor == "spy")
            {
                word.color = Color.black;
                Bg.color = Color.grey;
            }else if( myColor == "black")
            {
                word.color = Color.black;
                Bg.color = new Color(0.7f,0.7f,0.7f,1);
            }

        }else if( cardState == CardState.GameOver)
        {
            if (myColor == "red")
            {
                word.color = Color.white;
                Bg.color = new Color(237 / 255.0F, 172 / 255.0F, 172 / 255.0F, 255 / 255.0F);
            }
            else if (myColor == "blue")
            {
                word.color = Color.white;
                Bg.color = new Color(237 / 255.0F, 226 / 255.0F, 204 / 255.0F, 255 / 255.0F);
            }
            else if (myColor == "spy")
            {
                word.color = Color.black;
                Bg.color = Color.grey;
            }
            else if (myColor == "black")
            {
                word.color = Color.black;
                Bg.color = new Color(232 / 255.0F, 232 / 255.0F, 232 / 255.0F, 255 / 255.0F);
            }
        }
    }
}
