using poker.view;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
namespace poker
{
    public class TexasHoldEm : MonoBehaviour
    {
        private PokerHandEvaluator _pokerHandEvaluator;
        private Deck _deck;
        [SerializeField]
        public List<PlayerView> _players;
        [SerializeField]
        private TableView _table;
        [SerializeField]
        private Transform _playerContainer;
        private int _maxPlayers;
        private int smallBlind = 50;
        private int bigBlind = 100;
        private int _cashInBank = 0;
        private int maxBetAmount = 0;

        public GameObject StartRoundButton;

        bool calculated = false;
        public int CashInBank
        {
            get
            {
                return _cashInBank;
            }
            set
            {
                _cashInBank = value;
                txtCashInBank.text = _cashInBank.ToString();
            }
        }
        public static TexasHoldEm Instance;

        public Text txtCashInBank;

        private void Awake()
        {
            Instance = this;
            _pokerHandEvaluator = new PokerHandEvaluator();
            _deck = new Deck();
            _players = new List<PlayerView>();
            _maxPlayers = GlobalValue.MaxPlayer;// (_deck.NumberOfCards - 5) / 2;
            CashInBank = 0;
        }

        private void Start()
        {
            InitPlayersRemote();
        }
        public void AddPlayer() {
            return;
            if (_players.Count == 0 || _players[0].GetCards().Length == 0 && _table.GetCards().Length == 0)
                Debug.Log($"Add Player: {TryAddPlayer()}");
            else
                Debug.Log("No adding players in the middle of a round");
        }

        public void Deal(bool NewController = false)
        {

            if (GlobalValue.IsPokerController == false) return;
            if (NewController == true)
            {
                ReaddPlayers();
                Debug.Log("Players: " + _players.Count);
            }
            if (_players.Count < 2)
            {
                
                if (PTurnManager.Instance.RoundIsInProgress)
                {
                    if (calculated)
                    {
                        Reset();
                        return;
                    }
                    else
                    {
                        Calculate();
                        return;
                    }
                }
                else
                {
                    
                    ReaddPlayers();

                    if (_players.Count >= 2)
                    {
                        Deal();
                    }
                    return;
                }
            }

            if (calculated)
            {
                Reset();
                return;
            }
            int numTableCards = _table.GetCards().Length;

            if (_players[0].GetCards().Length == 0)
            {
                BeginRound();
                calculated = false;
            }
            else if (numTableCards == 0)
            {
                Flop();
            }
            else if (numTableCards == 3)
            {
                Turn();
            }
            else if (numTableCards == 4)
            {
                River();
                
            }else if( numTableCards == 5)
            {
                Calculate();
                
            }
            
            

        }

        public void StartTimer(int sid)
        {
            _playerContainer.GetChild(sid).GetComponent<SeatView>().StartTimer();
        }

        public void StopTimer()
        {
            for( int i = 0; i < GlobalValue.MaxPlayer; i++)
            {
                _playerContainer.GetChild(i).GetComponent<SeatView>().StopTimer();
            }
        }

        public void Calculate()
        {
            PUNMenu.Instant.SendCalc();
        }

        public void RemoteCalculate()
        {
            if (_players.Count == 1)
            {
                _players[0].Position = 0;
            }
            else
            {
                RankPlayerHands();
                CalculatePlayerPositions();
                HighlightWinningHand();
            }
            foreach (PlayerView p in _players)
            {
                p.ShowCards();
                p.GetMyView().ResetBet();
            }
            PlayerView tMyplayer = GetMyPlayer();
            if (tMyplayer !=null && tMyplayer.Position == 0)
            {
                GlobalValue.Coins += CashInBank;
            }
            CashInBank = 0;
            StartCoroutine(ReadyReset(1) );
            PTurnManager.Instance.NextDeal(3);
        }
        IEnumerator ReadyReset( float dt)
        {
            yield return new WaitForSeconds(dt);
            calculated = true;
        }
        private void HighlightWinningHand()
        {
            foreach (PlayerView player in _players)
            {
                if (player.Position == 0)
                {
                    foreach (CardView tableCard in player.GetCardViews().Concat(_table.GetCardViews()))
                    {
                        foreach (Card winningHandCard in player.RankedHand.RankCards)
                        {
                            if (tableCard.Card.ToString() == winningHandCard.ToString()) tableCard.HighlightGreen = true;
                        }
                        foreach (Card winningHandCard in player.RankedHand.Kickers)
                        {
                            if (tableCard.Card.ToString() == winningHandCard.ToString()) tableCard.HighlightBlue = true;
                        }
                    }
                }
            }
        }

        private void RankPlayerHands()
        {
            foreach (PlayerView player in _players)
            {
                player.HideRank();
                player.RankedHand = _pokerHandEvaluator.EvaluateHand(_table.GetCards().Concat(player.GetCards()).ToArray());
            }
        }

        public void ReaddPlayers()
        {
           
            _players.Clear();
            for (int i = 0; i < _maxPlayers; i++)
            {
                if (_playerContainer.GetChild(i).GetChild(3).childCount > 0) {
                    _players.Add(_playerContainer.GetChild(i).GetChild(3).GetChild(0).GetComponent<PlayerView>());
                }
            }

           

        }
        private bool TryAddPlayer()
        {
            if (_players.Count < _maxPlayers)
            {
                int playerID = _players.Count;
                GameObject playerGO = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), _playerContainer.GetChild(playerID).GetChild(3));

                _players.Add(playerGO.GetComponent<PlayerView>());
                return true;
            }
            else return false;
        }

        private void BeginRound()
        {
            string cardsStringArray = _deck.Shuffle();
            if (PhotonNetwork.connected)
            {
                if (PUNMenu.Instant != null)
                {
                    PUNMenu.Instant.SendCardData(cardsStringArray);
                }
            }


        }

        void DealCards()
        {
            int seatPosition = (int)PhotonNetwork.player.CustomProperties[PConst.SEATPOS];
            foreach (PlayerView player in _players)
            {
                for (int i = 0; i < 2; i++)
                {
                    player.AddCard(_deck.Draw());
                }
                
                if ( player.SeatID == seatPosition)
                {
                    player.ShowCards();
                }
                else
                {
                    player.HideCards();
                }
            }

        }

        private void Flop()
        {
            PUNMenu.Instant.SendFlop();
        }

        public void RemoteFlop()
        {
            for (int i = 0; i < 3; i++)
            {
                _table.AddCard(_deck.Draw());
            }
            
            PTurnManager.Instance.StartBetting();
        }

        public void ResetBetForallPlayers()
        {
            foreach( PlayerView p in _players)
            {
                p.GetMyView().ResetBet();
            }
        }


        private void Turn()
        {
            PUNMenu.Instant.SendTurn();
        }

        public void RemoteTurn()
        {
            calculated = false;
            _table.AddCard(_deck.Draw());
            PTurnManager.Instance.StartBetting();

        }

        private void River()
        {
            PUNMenu.Instant.SendRiver();

        }

        public void RemoteRiver()
        {
            _table.AddCard(_deck.Draw());
            PTurnManager.Instance.StartBetting();

            
        }

        private void CalculatePlayerPositions()
        {
            int j = 0;
            foreach (int i in _pokerHandEvaluator.EvaluateWinningHands(_players.Select(o => o.RankedHand.Hand).ToArray()))
            {
                _players[j].Position = i;
                j++;
            }
        }

        private void Reset()
        {
            StartCoroutine(IEReset(3));
            
        }

        IEnumerator IEReset( float dt)
        {
            yield return new WaitForSeconds(dt);
            PUNMenu.Instant.SendReset();
        }

        public PlayerView GetMyPlayer()
        {
            int mySeatID = (int)PhotonNetwork.player.CustomProperties[PConst.SEATPOS];
            for( int i = 0; i < _players.Count; i++)
            {
                if( mySeatID == _players[i].SeatID)
                {
                    return _players[i];
                }
            }
            return null;
        }
        public void RemoteReset()
        {
            ReaddPlayers();
            _table.RemoveAllCards();
            foreach (PlayerView player in _players)
            {
                player.RemoveAllCards();
                player.DoneBet = false;
            }
            _deck.Reset();
            foreach (PlayerView player in _players)
            {
                player.GetMyView().ResetBet();
            }
            CashInBank = 0;
            maxBetAmount = 0;
            calculated = false;
            PTurnManager.Instance.EndRound();
        }

        public void SeatPlayer(string playername, int sid, int avatarid, int coins, int bet, int dealer, string action, int prevPos)
        {
            
            _playerContainer.GetChild(sid).GetComponent<SeatView>().Seat(playername, avatarid, coins, bet, dealer, action);
            GameObject playerGO = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), _playerContainer.GetChild(sid).GetChild(3));

            playerGO.GetComponent<PlayerView>().SeatID = sid;

            if (prevPos != -1)
            {
                RemovePlayer(prevPos);
            }
            else
            {
                ReaddPlayers();
            }
            Debug.Log(_players.Count);

            if( _players.Count >= 2)
            {
                if(GlobalValue.IsPokerController && PTurnManager.Instance.RoundIsInProgress == false)
                {
                    OnClickStartRound();
                }
            }
            
        }

        public void OnClickStartRound()
        {
            if (GlobalValue.IsPokerController)
            {
                PTurnManager.Instance.StartRound();
            }
        }

        public void RemovePlayer(int sid)
        {
            
            _playerContainer.GetChild(sid).GetComponent<SeatView>().RemoveSeat();
            for( int i = _playerContainer.GetChild(sid).GetChild(3).childCount-1; i>=0; i--)
            {
                Debug.Log("Remove Player");
                Destroy(_playerContainer.GetChild(sid).GetChild(3).GetChild(i).gameObject);
            }
            
        }

        public void UpdatePlayerCoins( int seatPos, int coins)
        {
            Debug.Log(String.Format( "seatpos {0}", seatPos));
            for ( int i = 0; i < _players.Count; i++)
            {
                if( _players[i].SeatID == seatPos)
                {

                    _players[i].GetMyView().UpdateCoinsRemote(coins);
                    return;
                }
            }
        }

        public void InitPlayersRemote()
        {

            if(PhotonNetwork.connected)
            {
                bool[] bOccupiedPositions = new bool[7];
                for( int i = 0; i <bOccupiedPositions.Length; i++)
                {
                    bOccupiedPositions[i] = false;
                }
                foreach (PhotonPlayer p in PhotonNetwork.playerList)
                {
                    int seatPosition = (int)p.CustomProperties[PConst.SEATPOS];
                    int avatarid = (int)p.CustomProperties[PConst.AVATARID];
                    string name = (string)p.CustomProperties[PConst.NAME];
                    int coins = (int)p.CustomProperties[PConst.COINS];
                    int bet = (int)p.CustomProperties[PConst.BET];
                    int dealer = (int)p.CustomProperties[PConst.DEALER];
                    string action = (string)p.CustomProperties[PConst.ACTION];


                    if( seatPosition != -1)
                    {
                        GameObject playerGO = GameObject.Instantiate(Resources.Load<GameObject>("Prefabs/Player"), _playerContainer.GetChild(seatPosition).GetChild(3));
                        playerGO.GetComponent<PlayerView>().SeatID = seatPosition;
                        bOccupiedPositions[seatPosition] = true;
                        _playerContainer.GetChild(seatPosition).GetComponent<SeatView>().Seat(name, avatarid,coins,bet,dealer,action);
                    }


                }

                for( int i = 0; i < bOccupiedPositions.Length; i++)
                {
                    if(bOccupiedPositions[i] == false)
                    {
                        PUNMenu.Instant.Seat(i);
                        return;
                    }
                }
            }
        }

      
        public void CardDataRemote( string data)
        {
            _deck.RemoteSuffle(data);

            Debug.Log(data);
            ReaddPlayers();

            DealCards();
            StartRoundButton.SetActive(false);

            PTurnManager.Instance.RoundIsInProgress = true;

            PTurnManager.Instance.StartBetting();
        }

        public bool PlayerSeated ( int seatPosition)
        {
            if( _playerContainer.GetChild(seatPosition).GetChild(3).childCount >= 1)
            {
                return true;
            }
            return false;
        }

        public void RemoteBet(int seatid, int betamount, bool ballin)
        {
            int pid = 0;
            for( int i = 0; i < _players.Count; i++)
            {
                if( _players[i].SeatID == seatid)
                {
                    pid = i;
                    break;
                }
            }

            if (betamount == -1) //hold
            {
                _players[pid].RemoveAllCards();
                _players.RemoveAt(pid);
            }
            else
            {
                if (betamount > 0)
                {
                    _players[pid].GetMyView().Bet(betamount);
                }
                _players[pid].DoneBet = true;
                if( ballin)
                {
                    _players[pid].DoneAllIn = true;
                }
                CashInBank += betamount;
            }

            if(GlobalValue.IsPokerController)
            {
                if (ReadyForNextDeal())
                {
                    foreach (PlayerView player in _players)
                    {
                        player.DoneBet = false;
                    }
                    PTurnManager.Instance.NextDeal();
                }
                else
                {
                    PTurnManager.Instance.NextBetting();
                }
            }
        }

        bool ReadyForNextDeal()
        {
            for (int i = 1; i < _players.Count; i++)
            {
                if (_players[i].DoneBet == false) return false;
                if (_players[i].DoneAllIn) continue;
                if (_players[i].GetBetAmount() != _players[0].GetBetAmount())
                {
                    return false;
                }
            }
            return true;
        }

        public bool ReadySeat( int sid)
        {
            for (int i = 0; i < _players.Count; i++)
            {
                if (_players[i].SeatID == sid)
                {
                    return true;
                }
            }
            return false;
        }

    }
}
