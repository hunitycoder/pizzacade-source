using System;
using System.Collections.Generic;
using Debug = UnityEngine.Debug;

namespace poker
{
    public class Deck
    {
        private static Random rng = new Random();
        private List<Card> _cards;
        private int _currentCardIndex;

        public int NumberOfCards => _cards.Count;

        public Deck()
        {
            _cards = new List<Card>();
            foreach (CardSuit suit in Enum.GetValues(typeof(CardSuit)))
            {
                foreach (CardValue value in Enum.GetValues(typeof(CardValue)))
                {
                    _cards.Add(new Card(value, suit));
                }
            }
        }

        public void Reset()
        {
            _currentCardIndex = 0;
        }

        public void RemoteSuffle(string cdata)
        {
            string[] ccs = cdata.Split(',');
            for (int i = 0; i < _cards.Count; i++)
            {
                _cards[i].SetCard(int.Parse(ccs[i]));
            }

        }
        public string Shuffle()
        {
            string ret = "";
            int n = _cards.Count;
            while (n > 1)
            {
                n--;
                int k = rng.Next(n + 1);
                Card value = _cards[k];
                _cards[k] = _cards[n];
                _cards[n] = value;
            }

            for( int i = 0; i < _cards.Count; i++)
            {
                ret += _cards[i].CardRank() + ",";
            }

            return ret;
        }

        public Card Draw()
        {
            if (_currentCardIndex < _cards.Count)
                return _cards[_currentCardIndex++];
            else
                return null;
        }

        public string GetCardData(int start, int len)
        {
            string ret = "";
            for( int i = start; i < start + len; i++)
            {
                ret += _cards[i].CardRank() + ",";
            }

            return ret;
        }
    }
}