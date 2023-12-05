using AdventCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode.Days
{
	public class Day1 : IDay
	{
		public void Run()
		{
			int sumDigits = 0;
			int sumNumbers = 0;
			string[] input = File.ReadAllLines("2023\\Day1\\Input.txt");
			foreach(string line in input)
			{
				sumDigits += FindDigits(line); 
				sumNumbers += FindNumbers(line);
			}
			Console.WriteLine("The sum of the calibrated numbers (only digits): " + sumDigits);
			Console.WriteLine("The sum of the calibrated numbers: " + sumNumbers);
		}

		internal int FindDigits(string line)
		{
			int result = 0;
			List<int> foundNumbers = new();
			foreach (var ch in line)
			{
				if (char.IsDigit(ch))
				{
					foundNumbers.Add(ch - '0');
				}
			}

			if (foundNumbers.Count == 1)
			{
				result = foundNumbers.First() * 10 + foundNumbers.First();
			}
			else
			{
				result = foundNumbers.First() * 10 + foundNumbers.Last();
			}
			return result;
		}

		internal int FindNumbers(string line)
		{
			Dictionary<string, int> numbers = new()
			{
				{"one", 1 },
				{"two", 2 },
				{"three", 3 },
				{"four", 4 },
				{"five", 5 },
				{"six", 6 },
				{"seven", 7 },
				{"eight", 8 },
				{"nine", 9 }
			};
			int result = 0;
			List<int> foundNumbers = new();
			string prev = "";
			foreach (var ch in line)
			{
				prev += ch;
				if (numbers.Any(pv => prev.EndsWith(pv.Key)))
				{
					int val = numbers.First(pv => prev.EndsWith(pv.Key)).Value;
					foundNumbers.Add(val);
				}
				else if (char.IsDigit(ch))
				{
					foundNumbers.Add(ch - '0');
					prev = "";
				}
			}

			if (foundNumbers.Count == 1)
			{
				result = foundNumbers.First() * 10 + foundNumbers.First();
			}
			else
			{
				result = foundNumbers.First() * 10 + foundNumbers.Last();
			}
			return result;
		}
	}
}
