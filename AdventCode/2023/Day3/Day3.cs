
using AdventCode.Interface;
using System.Reflection;

namespace AdventCode.Days
{
	public class Day3 : IDay
	{
		public void Run()
		{
			var input = File.ReadAllLines("2023\\Day3\\Input.txt");

			var numbers = new List<Number>();
			var symbols = new List<Symbol>();

			for (var row = 0; row < input.Length; row++)
			{
				var num = new Number();
				var digits = new List<int>();

				for (var col = 0; col < input[row].Length; col++)
				{	
					if (Char.IsDigit(input[row][col]))
					{
						// Adiciona o digito a uma lista de digitos consecutivos
						int digit = int.Parse(input[row][col].ToString());
						digits.Add(digit);

						// I Se for o primeiro digito da lista adiciona as cordenadas de inicio do numero
						if (digits.Count == 1)
						{
							num.Start = (row, col);
						}

						// Find all consecutive digits that form a number
						while (col < input[row].Length - 1 && int.TryParse(input[row][col + 1].ToString(), out digit))
						{
							digits.Add(digit);
							col++;
						}

						num.End = (row, col);
						num.Value = int.Parse(string.Join("", digits));
						numbers.Add(num);
						num = new Number();
						digits.Clear();
					}
					else if(input[row][col] != '.')
					{
						symbols.Add(new Symbol
						{
							Value = input[row][col],
							Position = (row, col)
						});
					}
				}
			}

			var part1 = numbers
				.Where(number => symbols.Any(symbol => AreAdjacent(number, symbol)))
				.Sum(number => number.Value);

			var part2 = symbols
				.Where(symbol => symbol.Value == '*')
				.Select(symbol => numbers.Where(number => AreAdjacent(number, symbol)).ToArray())
				.Where(gears => gears.Length == 2)
				.Sum(gears => gears[0].Value * gears[1].Value);

			Console.WriteLine($"Sum of the part numbers: {part1}");
			Console.WriteLine($"Gear Ratio: {part2}");
		}

		static bool AreAdjacent(Number number, Symbol symbol)
		{
			return symbol.Position.Row - number.Start.Row <= 1
				&& symbol.Position.Column >= number.Start.Column - 1
				&& symbol.Position.Column <= number.End.Column + 1;
		}

		struct Number
		{
			public int Value { get; set; }
			public (int Row, int Column) Start { get; set; }
			public (int Row, int Column) End { get; set; }
		}

		struct Symbol
		{
			public char Value { get; set; }
			public (int Row, int Column) Position { get; set; }
		}
	}
}
