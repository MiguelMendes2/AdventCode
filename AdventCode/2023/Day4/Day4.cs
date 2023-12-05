using AdventCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode.Days
{
	public class Day4 : IDay
	{
		public void Run()
		{
			int points = 0;
			string[] input = File.ReadAllLines("2023\\Day4\\Input.txt");
			List<Card> cards = new();
			foreach (string line in input)
			{
				string[] split = line.Split(':');
				int cardNum = int.Parse(split[0].Split(' ').Last());
				Card card = new Card();
				card.CardNum = cardNum;
				card = getCard(split[1], card);
				int result = getPoints(card);
				card = getInstances(card);
				points += result;
				foreach (var instance in cards.Where(x => x.instancesWon.Contains(cardNum)).ToList())
				{
					cards.Add(card);
				}
				cards.Add(card);
			}
			Console.WriteLine("The sum of the calibrated numbers (only digits): " + points);
		}

		internal Card getCard(string nums, Card card)
		{
			bool winNumbers = true;
			int num = 0;
			for (int i = 0; i < nums.Length; i++)
			{
				if (Char.IsDigit(nums[i]))
				{
					num *= 10;
					num += int.Parse(nums[i].ToString());
				}
				if ((nums[i] == ' ' || i + 1 >= nums.Length) && num > 0)
				{
					if (winNumbers)
					{
						card.winningNumbers.Add(num);
					}
					else
					{
						card.numbers.Add(num);
					}
					num = 0;
				}
				if (nums[i] == '|')
					winNumbers = false;
			}
			return card;
		}

		internal int getPoints(Card card)
		{
			int result = 0;
			int tempPoints = card.numbers.Where(num => card.winningNumbers.Contains(num)).Count();
			for (int i = 1; i <= tempPoints; i++)
			{
				if (i > 1)
					result *= 2;
				else
					result += 1;
			}
			return result;
		}


		internal Card getInstances(Card card)
		{
			int matches = card.numbers.Where(num => card.winningNumbers.Contains(num)).Count();
			for (int i = card.CardNum + 1; i <= card.CardNum + matches; i++)
			{
				card.instancesWon.Add(i);
			}
			return card;
		}

		internal struct Card
		{
			public Card()
			{
				CardNum = 0;
				winningNumbers = new ();
				instancesWon = new();
				numbers = new ();
			}

			public int CardNum { get; set; }
			public List<int> winningNumbers { get; set; }
			public List<int> numbers { get; set; }
			public List<int> instancesWon { get; set; }
		}
	}
}
