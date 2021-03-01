using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace poker.view
{
    public class PlayerView : CardHolder
    {
        [SerializeField]
        private Text rankDisplay;
        [SerializeField]
        private Text positionDisplay;

        RankedHand _rankedHand;

        public int SeatID;

        public bool DoneBet = false;
        public bool DoneAllIn = false;
        public RankedHand RankedHand
        {
            set
            {
                _rankedHand = value;
                if (rankDisplay != null)
                {
                    rankDisplay.text = Enum.GetName(typeof(Rank), value.Rank);
                }
            }
            get
            {
                return _rankedHand;
            }
        }
        private int _position;
        public int Position
        {
            set
            {
                _position = value;
                if (positionDisplay != null)
                {
                    //positionDisplay.text = $"Rank: {value}";
                    positionDisplay.text = "";
                    if( value != 0)
                    {
                        rankDisplay.text = "";
                    }
                }
            }
            get
            {
                return _position;
            }
        }

        public void ToggleDisplayRank()
        {
            if (rankDisplay.color == Color.grey) HideRank();
            else ShowRank();
        }

       
        public void ShowRank()
        {
            //rankDisplay.color = Color.grey;
        }
        public void HideRank()
        {
            rankDisplay.text = "";
        }

        protected override void OnAddCard()
        {
            ShowRank();
        }

        protected override void OnRemoveAllCards()
        {
            HideRank();
            rankDisplay.text = "";
            positionDisplay.text = "";
        }

        public SeatView GetMyView()
        {
            return transform.parent.parent.GetComponent<SeatView>();
        }

        public int GetBetAmount()
        {
            return GetMyView().GetBetAmount();
        }
    }
}