using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using poker;
public class SeatView : MonoBehaviour
{
    // Start is called before the first frame update

    public int SeatID;
    bool SeatWithPlayer = false;
    private Image Loading;
    private Image avatar;
    private Text playerName;
    private Button seat;
    private Transform playerRoot;

    private Text txtCoins;
    private Text txtBet;

    private Image loadingBar;

    private int betAmount = 0;
    private void Awake()
    {
        Loading = transform.GetChild(2).GetComponent<Image>();
        avatar = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        seat = transform.GetChild(0).GetChild(1).GetComponent<Button>();
        playerRoot = transform.GetChild(3);
        playerName = transform.GetChild(4).GetComponent<Text>();
        txtCoins = transform.GetChild(5).GetComponent<Text>();
        txtBet = transform.GetChild(6).GetComponent<Text>();
        txtCoins.text = "";
        txtBet.text = "";
        playerName.text = "";
        seat.gameObject.SetActive(true);
        avatar.enabled = false;
        Loading.fillAmount = 0;

    }

    public void StartTimer()
    {
        StartCoroutine(IEBettingTimer());
    }
    public void StopTimer()
    {
        StopAllCoroutines();
        Loading.fillAmount = 0;
    }

    IEnumerator IEBettingTimer()
    {
        yield return new WaitForEndOfFrame();
        for( int i = 0; i <= 30; i++)
        {
            Loading.fillAmount = ((float )i)/30;
            yield return new WaitForSeconds(1);
        }

        BettingManager.Instance.OnTimerOver();
    }
    void Start()
    {
        
    }

    public void Bet( int amount)
    {
        int mySeatPosition = (int)PhotonNetwork.player.CustomProperties[PConst.SEATPOS];
        if (SeatID == mySeatPosition)
        {
            GlobalValue.Coins -= amount;
            if( GlobalValue.Coins < 0)
            {
                GlobalValue.Coins = 0;
            }
            txtCoins.text = GlobalValue.Coins.ToString();
        }
        else
        {
            foreach (PhotonPlayer p in PhotonNetwork.playerList)
            {

                int seatPosition = (int)p.CustomProperties[PConst.SEATPOS];
                int coins = (int)p.CustomProperties[PConst.COINS];
                if (SeatID == seatPosition)
                {
                    txtCoins.text = coins.ToString();
                    break;
                }
            }
        }
        betAmount += amount;
        txtBet.text = betAmount.ToString();
        
    }

    public void UpdateCoins()
    {
        //txtCoins.text = GlobalValue.Coins.ToString();
    }

    public void UpdateCoinsRemote(int coins)
    {
        txtCoins.text = coins.ToString();
    }

    public int GetBetAmount()
    {
        return betAmount;
    }
    public void ResetBet()
    {
        betAmount = 0;
        txtBet.text = "";
    }
    public void OnClickSeat()
    {
        if (PTurnManager.Instance.RoundIsInProgress == true) return;
        if( PUNMenu.Instant == null)
        {
            TexasHoldEm.Instance.SeatPlayer(GlobalValue.ProfileName, SeatID, GlobalValue.AvatarID, GlobalValue.Coins, 0, 0,"", -1);
        }
        else
        {
            PUNMenu.Instant.Seat(SeatID);
        }
        
    }

    public void Seat(string playername, int avatarID = 0, int coins = 0, int bet = 0, int dealer = 0, string action= "")
    {
        playerName.text = playername;
        seat.gameObject.SetActive(false);
        avatar.enabled = true;
        txtCoins.text = coins.ToString();
        txtBet.text = "";
        if (bet > 0) txtBet.text = bet.ToString();
        if (avatarID > 0)
        {
            avatar.sprite = Resources.Load<Sprite>("Avatar/Item/1 (" + avatarID + ")");
        }
    }

    public void RemoveSeat()
    {
        Stanup();
        transform.GetChild(3).RemoveAllChildren();
    }

    public void Stanup()
    {
        playerName.text = "";
        txtCoins.text = "";
        txtBet.text = "";
        seat.gameObject.SetActive(true);
        avatar.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
