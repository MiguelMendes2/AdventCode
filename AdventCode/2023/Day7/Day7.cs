using AdventCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;
using static System.Formats.Asn1.AsnWriter;
using System.Reflection;

namespace AdventCode.Days
{
	public class Day7 : IDay
	{
		public enum HandType
		{
			HighCard = 0,
			Pair = 1,
			TwoPair = 2,
			ThreeOfAKind = 3,
			FullHouse = 4,
			FourOfAKind = 5,
			FiveOfAKind = 6
		}

		public void Run()
		{
			string[] input = File.ReadAllLines("2023\\Day7\\Input.txt");

			Console.WriteLine($"Part 1: {Solver(input, "23456789TJQKA", "")}");
			Console.WriteLine($"Part 2: {Solver(input, "J23456789TQKA", "J")}");
		}

		internal long Solver(string[] lines, string labels, string jokerChar)
		{
			List<Card> hands = GetCards(lines, labels, jokerChar);
			var orderedHands = hands.OrderBy(x => x.Type).ThenBy(x => x.Weight);
			var result = orderedHands.Select((hand, index) => hand.Bid * (index + 1)).Sum();
			return result;
		}

		internal List<Card> GetCards(string[] lines, string labels, string jokerChar)
		{
			List<Card> listCards = new ();
			foreach(var line in lines)
			{
				string[] split = line.Trim().Split(' ');
				Card card = new(split[0], int.Parse(split[1]));

				HandType handType = HandType.FiveOfAKind;
				var handWithoutJokers = jokerChar != "" ? card.Hand.Replace(jokerChar, "") : card.Hand;
				var numJokers = card.Hand.Length - handWithoutJokers.Length;

				int[] sizes = handWithoutJokers
				  .GroupBy(x => x)
				  .Select(x => x.Count())
				  .OrderByDescending(x => x)
				  .Concat(new[] { 0 })
				  .ToArray();
				// Add jokers
				sizes[0] += numJokers;

				card.Type = GetHandType(sizes);
				card.Weight = card.Hand.Select((card, index) => labels.IndexOf(card) << (4 * (5 - index))).Sum();

				listCards.Add(card);
			}
			return listCards;
		}

		internal HandType GetHandType(int[] sizes)
		{
			return sizes switch
			{
				[5, ..] => HandType.FiveOfAKind,
				[4, ..] => HandType.FourOfAKind,
				[3, 2, ..] => HandType.FullHouse,
				[3, ..] => HandType.ThreeOfAKind,
				[2, 2, ..] => HandType.TwoPair,
				[2, ..] => HandType.Pair,
				[..] => HandType.HighCard,
			};
		}
	
		internal struct Card(string hand, int bid)
		{
			public string Hand { get; set; } = hand;
			public int Bid { get; set; } = bid;
            public int Weight { get; set; }
            public HandType Type { get; set; }
        }
	}
}
