using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using poker;
public class BettingManager : MonoBehaviour
{
    // Start is called before the first frame update

    public static BettingManager Instance;

    public GameObject BetPanel;
    public GameObject RaisePanel;

    public Button HoldButton;

    public Button CheckButton;
    public Button CallButton;

    public Button RaiseButton;
    public Button AllInButton;

    public Slider RaiseSlider;
    public Button RaisePotButton;

    public Text RaiseBetAmount;
    int BetAmount = 0;

    int BetTime = 10;
    int bettimer = 0;
    int _myBetAmount;
    private void Awake()
    {
        Instance = this;
    }
    void Start()
    {
        BetPanel.SetActive(false);
        RaisePanel.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartBetTurn()
    {
        StopAllCoroutines();
        int maxBet = 0;
        for( int i = 0; i < TexasHoldEm.Instance._players.Count; i++)
        {
            int aa = TexasHoldEm.Instance._players[i].GetBetAmount();
            if( maxBet < aa)
            {
                maxBet = aa;
            }
        }

        Show(maxBet, TexasHoldEm.Instance.GetMyPlayer().GetMyView().GetBetAmount());
    }

    

    int needBetMinAmount = 0;
    public void Show(int maxBetAmount, int myBetAmount)
    {
        CallButton.interactable = true;
        CheckButton.interactable = true;
        _myBetAmount = myBetAmount;
        if (maxBetAmount == 0)
        {
            
            if (TexasHoldEm.Instance.CashInBank == 0)
            {
                CheckButton.gameObject.SetActive(false);
                CallButton.gameObject.SetActive(true);
                CallButton.transform.GetChild(0).GetComponent<Text>().text = "Blind";
                CallButton.transform.GetChild(1).GetComponent<Text>().text = "";
                needBetMinAmount = 10;
            }
            else
            {
                CheckButton.gameObject.SetActive(true);
                CallButton.gameObject.SetActive(false);
                needBetMinAmount = 0;
            }
            
        }
        else
        {
            if (maxBetAmount == myBetAmount)
            {
                CheckButton.gameObject.SetActive(true);
                CallButton.gameObject.SetActive(false);
            }
            else
            {
                CheckButton.gameObject.SetActive(false);
                CallButton.gameObject.SetActive(true);
            }
            needBetMinAmount = maxBetAmount - myBetAmount;
            if( maxBetAmount == 0)
            {
                CallButton.transform.GetChild(0).GetComponent<Text>().text = "Blind";
                CallButton.transform.GetChild(1).GetComponent<Text>().text = "";
            }
            else
            {
                CallButton.transform.GetChild(0).GetComponent<Text>().text = "Call";
                CallButton.transform.GetChild(1).GetComponent<Text>().text = needBetMinAmount.ToString();
            }
            RaiseBetAmount.text = needBetMinAmount.ToString();
        }
        
        RaiseBetAmount.text = BetAmount.ToString();
        BetPanel.SetActive(true);

        if( needBetMinAmount > GlobalValue.Coins)
        {
            needBetMinAmount = GlobalValue.Coins;
            CallButton.interactable = false;
            CheckButton.interactable = false;
            RaiseButton.gameObject.SetActive(false);
            AllInButton.gameObject.SetActive(true);
        }
        else
        {
            RaiseButton.gameObject.SetActive(true);
            AllInButton.gameObject.SetActive(false);
        }

        TexasHoldEm.Instance.GetMyPlayer().GetMyView().StartTimer();
    }

   
    public void OnTimerOver()
    {
        if( needBetMinAmount == 0)
        {
            OnClickCheck();
        }
        else
        {
            OnClickHold();
        }
    }
    public void OnClickBet()
    {
        if(RaisePotButton.transform.GetChild(0).GetComponent<Text>().text == "All\nIn")
        {
            
            BetAmount = GlobalValue.Coins;
            
        }
        FinalBet(BetAmount);
    }

    public void OnClickCall()
    {
        FinalBet(needBetMinAmount);
    }

    public void OnClickCheck()
    {
        FinalBet(0);
    }

    public void OnClickRaise()
    {
        RaisePanel.SetActive(true);
        RaiseSlider.value = 0.5f;
        OnChangeSlider();
    }

    public void OnClickAllIn()
    {
        FinalBet(GlobalValue.Coins, true);
    }

    public void FinalBet( int betamount, bool ballin = false)
    {
        BetPanel.SetActive(false);
        RaisePanel.SetActive(false);
        TexasHoldEm.Instance.GetMyPlayer().GetMyView().StopTimer();
        PUNMenu.Instant.SendBetAmount(TexasHoldEm.Instance.GetMyPlayer().SeatID, betamount, ballin);
        
    }

    public void OnClickHold()
    {
        FinalBet(-1);
    }

    
    public void OnChangeSlider()
    {
        if(RaiseSlider.value == 1)
        {
            RaisePotButton.transform.GetChild(0).GetComponent<Text>().text = "All\nIn";
        }
        else
        {
            RaisePotButton.transform.GetChild(0).GetComponent<Text>().text = "Bet";
        }
        int tBetAmount = needBetMinAmount + (int)((GlobalValue.Coins - needBetMinAmount) * RaiseSlider.value);
        if( tBetAmount >= needBetMinAmount)
        {
            BetAmount = tBetAmount;
        }
        else
        {
            BetAmount = GlobalValue.Coins;
        }
        
        RaiseBetAmount.text = BetAmount.ToString();
    }
}
