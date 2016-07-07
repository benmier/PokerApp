using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        //string[] cards = { "1H", "2H", "3H", "4H", "5H", "6H", "7H", "8H", "9H", "10H", "JH", "QH", "KH", "1D", "2D", "3D", "4D", "5D", "6D", "7D", "8D", "9D", "10D", "JD", "QD", "KD", "1S", "2S", "3S", "4S", "5S", "6S", "7S", "8S", "9S", "10S", "JS", "QS", "KS", "1C", "2C", "3C", "4C", "5C", "6C", "7C", "8C", "9C", "10C", "JC", "QC", "KC" };
        public enum HandType { HighCard, Pair, TwoPair, ThreeOfAKind, Straight, Flush, FullHouse, FourOfAKind, StraightFlush, RoyalFlush, }
        static void Main(string[] args)
        {
            string[] hand1 = { "1H", "3S", "5H", "7H", "10H" }, hand2 = { "1S", "3H", "5S", "7S", "9S" };
            HandType player1 = Poker.Play(hand1), player2 = Poker.Play(hand2);
            string winner = player1 > player2 ? "Player 1 wins" : player1 < player2 ? "Player 2 wins" : Poker.TieBreaker(player1, hand1, hand2);
            Console.WriteLine(winner);
        }
    }
    class Poker
    {
        public static string[] GetCardsFromUser()
        {
            Console.WriteLine("Type 5 cards");
            string input = Console.ReadLine();
            string[] cards = input.Split(' ');
            return cards;
        }
        public static Program.HandType Play(string[] Hand)
        {
            int Hearts = 0, Clubs = 0, Spades = 0, Diamonds = 0, inARow = 0, Pairs = 0, Trips = 0, Quads = 0;
            bool isStraight = false;
            Dictionary<string, int> Values = GetMultiples(Hand);
            Program.HandType State = Program.HandType.HighCard;
            for (int i = 0; i < Hand.Length; i++) { int suits = Hand[i][Hand[i].Length - 1] == 'H' ? Hearts++ : Hand[i][Hand[i].Length - 1] == 'D' ? Diamonds++ : Hand[i][Hand[i].Length - 1] == 'C' ? Clubs++ : Hand[i][Hand[i].Length - 1] == 'S' ? Spades++ : 0; }
            foreach (var card in Values)
            {
                inARow = card.Value == 0 ? 0 : inARow + 1;
                if (inARow == 5) { isStraight = true; }
                if (card.Value == 2) Pairs++;
                if (card.Value == 3) Trips++;
                if (card.Value == 4) Quads++;
            }
            if (Pairs == 1)
                State = Program.HandType.Pair;
            if (Pairs == 2)
                State = Program.HandType.TwoPair;
            if (Trips == 1)
                State = Program.HandType.ThreeOfAKind;
            if (isStraight)
                State = Program.HandType.Straight;
            if ((Hearts == 5 || Diamonds == 5 || Spades == 5 || Clubs == 5))
                State = State == Program.HandType.Straight ? Program.HandType.StraightFlush : Program.HandType.Flush;
            if (Pairs == 1 && Trips == 1)
                State = Program.HandType.FullHouse;
            if (Quads == 1)
                State = Program.HandType.FourOfAKind;
            Console.WriteLine(State);
            return State;
        }
        public static int GetHighCard(string[] hand, int ignore)
        {
            int high = 0;
            foreach (string card in hand)
            {
                if (card == null) continue;
                int temp = card == "Aces" || (card[0] == '1' && card[1] != '0') ? 1 : card == "Twos" || card[0] == '2' ? 2 : card == "Threes" || card[0] == '3' ? 3 : card == "Fours" || card[0] == '4' ? 4 : card == "Fives" || card[0] == '5' ? 5 : card == "Sixes" || card[0] == '6' ? 6 : card == "Sevens" || card[0] == '7' ? 7 : card == "Eights" || card[0] == '8' ? 8 : card == "Nines" || card[0] == '9' ? 9 : card == "Tens" || (card[0] == '1' && card[1] == '0') ? 10 : card == "Jacks" || card[0] == 'J' ? 11 : card == "Queens" || card[0] == 'Q' ? 12 : card == "Kings" || card[0] == 'K' ? 13 : 0;
                if (temp > high && temp < ignore) high = temp;
            }
            return high;
        }
        public static Dictionary<string, int> GetMultiples(string[] Hand)
        {
            Dictionary<string, int> Values = new Dictionary<string, int>() { { "Aces", 0 }, { "Twos", 0 }, { "Threes", 0 }, { "Fours", 0 }, { "Fives", 0 }, { "Sixes", 0 }, { "Sevens", 0 }, { "Eights", 0 }, { "Nines", 0 }, { "Tens", 0 }, { "Jacks", 0 }, { "Queens", 0 }, { "Kings", 0 } };
            for (int i = 0; i < Hand.Length; i++) { int numbs = Hand[i][0] == '1' && Hand[i][1] != '0' ? Values["Aces"]++ : Hand[i][0] == '2' ? Values["Twos"]++ : Hand[i][0] == '3' ? Values["Threes"]++ : Hand[i][0] == '4' ? Values["Fours"]++ : Hand[i][0] == '5' ? Values["Fives"]++ : Hand[i][0] == '6' ? Values["Sixes"]++ : Hand[i][0] == '7' ? Values["Sevens"]++ : Hand[i][0] == '8' ? Values["Eights"]++ : Hand[i][0] == '9' ? Values["Nines"]++ : Hand[i][0] == '1' && Hand[i][1] == '0' ? Values["Tens"]++ : Hand[i][0] == 'J' ? Values["Jacks"]++ : Hand[i][0] == 'Q' ? Values["Queens"]++ : Hand[i][0] == 'K' ? Values["Kings"]++ : 0; }
            return Values;
        }
        public static string TieBreaker(Program.HandType type, string[] hand1, string[] hand2)
        {
            if (type == Program.HandType.Flush || type == Program.HandType.Straight || type == Program.HandType.StraightFlush || type == Program.HandType.HighCard)
            {
                int hand1high = 0, hand2high = 0, ignore = 14, count = 1;
                while (hand1high == hand2high)
                {
                    hand1high = GetHighCard(hand1, ignore);
                    hand2high = GetHighCard(hand2, ignore);
                    ignore = hand1high;
                    if (count == 5) break; else count++;
                }
                return hand1high > hand2high ? "Player 1 wins" : hand1high == hand2high ? "Tie" : "Player 2 wins";
            }
            else
            {
                int NumberOfMultiples = type == Program.HandType.Pair ? 2 : type == Program.HandType.TwoPair ? 2 : type == Program.HandType.FullHouse ? 3 : type == Program.HandType.ThreeOfAKind ? 3 : 4;
                Dictionary<string, int> Values1 = GetMultiples(hand1), Values2 = GetMultiples(hand2);
                string[] pairs1 = new string[2], pairs2 = new string[2];
                int counter1 = 0, counter2 = 0;
                foreach (var key in Values1.Keys)
                    if (Values1[key] == NumberOfMultiples) pairs1[counter1++] = key;
                foreach (var key in Values2.Keys)
                    if (Values2[key] == NumberOfMultiples) pairs2[counter2++] = key;
                int hand1high = 0, hand2high = 0, ignore = 14, count = 1;
                while (hand1high == hand2high)
                {
                    hand1high = GetHighCard(pairs1, ignore);
                    hand2high = GetHighCard(pairs2, ignore);
                    ignore = hand1high;
                    if (count == 5) break; else count++;
                }
                return hand1high > hand2high ? "Player 1 wins" : hand1high == hand2high ? "Tie" : "Player 2 wins";
            }
        }
    }
}
