using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using poker;
public class PTurnManager : MonoBehaviour
{
    public static PTurnManager Instance;
    public bool RoundIsInProgress = false;

    public int currentBetPlayer = 0;
    private void Awake()
    {
        Instance = this;
        RoundIsInProgress = false;
    }

    public void StartRound()
    {
        StopAllCoroutines();
        StartCoroutine(CheckToStartRound());
    }

    IEnumerator CheckToStartRound()
    {
        yield return new WaitForSeconds(1);
        if (TexasHoldEm.Instance._players.Count >= 2 && RoundIsInProgress == false)
        {
            Debug.Log("StartRound");
            RoundIsInProgress = true;
            TexasHoldEm.Instance.Deal();

            TexasHoldEm.Instance.StartRoundButton.SetActive(false);
        }

    }

    public void EndRound()
    {

        RoundIsInProgress = false;
        if (GlobalValue.IsPokerController)
        {
            //TexasHoldEm.Instance.StartRoundButton.SetActive(true);
            StartCoroutine(startNewRound());

        }
    }

    IEnumerator startNewRound()
    {
        yield return new WaitForSeconds(1);
        TexasHoldEm.Instance.OnClickStartRound();
    }


    IEnumerator RestartRound()
    {
        yield return new WaitForSeconds(1);
        StartRound();
    }

    public void NextDeal(float dt = 1)
    {
        if (GlobalValue.IsPokerController)
        {
            StartCoroutine(IENextDeal(dt));
        }
    }
    IEnumerator IENextDeal(float dt)
    {
        yield return new WaitForSeconds(dt);

        TexasHoldEm.Instance.Deal();
    }

    public void StartBetting()
    {
        TexasHoldEm.Instance.ResetBetForallPlayers();
        if (GlobalValue.IsPokerController)
        {
            currentBetPlayer = 0;
            int sid = TexasHoldEm.Instance._players[currentBetPlayer].SeatID;
            Debug.Log("SeatID" + sid);
            PUNMenu.Instant.SendBetTurn(sid);
        }
    }

    public void NextBetting()
    {
        if (GlobalValue.IsPokerController)
        {
            currentBetPlayer++;
            if( currentBetPlayer >= TexasHoldEm.Instance._players.Count)
            {
                currentBetPlayer = 0;
               
            }
            int sid = TexasHoldEm.Instance._players[currentBetPlayer].SeatID;
            Debug.Log("SeatID" + sid);
            PUNMenu.Instant.SendBetTurn(sid);
        }
    }
}
