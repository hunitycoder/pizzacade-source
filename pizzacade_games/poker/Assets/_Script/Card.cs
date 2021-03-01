namespace poker
{
    public class Card
    {
        public CardValue Value;
        public CardSuit Suit;


        public Card(CardValue value, CardSuit suit)
        {
            Value = value;
            Suit = suit;
        }

        override public string ToString()
        {
            return $"{Value} of {Suit}";
        }

        public void SetCard( int cRank)
        {
            Suit = (CardSuit)(cRank / 13);
            Value = (CardValue)((cRank % 13)+1);
        }
        public string CardRank()
        {
            int val = (int)Value-1;
            int suit = (int)Suit;
            int numVals = 13;
            int index = val + (suit * numVals);
            return index.ToString();
        }
    }
}