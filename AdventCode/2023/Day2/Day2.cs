using AdventCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode.Days
{
	public class Day2 : IDay
	{
		private const int maxRed = 12;
		private const int maxGreen = 13;
		private const int maxBlue = 14;

		public void Run()
		{
			List<Cube> cubes = new();
			string[] input = File.ReadAllLines("2023\\Day2\\Input.txt");
			foreach(string line in input)
			{
				cubes.Add(GetCube(line));
			}

			Console.WriteLine("Part 1: " + cubes.Where(x => x.possible).Sum(x => x.GameId));
			Console.WriteLine("Part 2: " + cubes.Sum(x => x.power));
		}

		internal Cube GetCube(string line)
		{
			Cube cube = new();
			cube.GameId = int.Parse(line.Split(':').First().Split(' ').Last());
			string[] sets = line.Split(':').Last().Split(';');
			int setMaxRed = 0;
			int setMaxBlue = 0;
			int setMaxGreen = 0;
			foreach (string set in sets)
			{
				string[] vals = set.TrimStart().Split(' ');
				for (int i = 0; i < vals.Length; i += 2)
				{
					int value = int.Parse(vals[i]);
					if (vals[i + 1].StartsWith("red") && value > setMaxRed)
					{
						setMaxRed = value;
					}
					else if (vals[i + 1].StartsWith("blue") && value > setMaxBlue)
					{
						setMaxBlue = value;
					}
					else if (vals[i + 1].StartsWith("green") && value > setMaxGreen)
					{
						setMaxGreen = value;
					}
				}
			}
			cube.power = setMaxRed * setMaxBlue * setMaxGreen;
			cube.possible = setMaxRed <= maxRed && setMaxBlue <= maxBlue && setMaxGreen <= maxGreen;
			return cube;
		}

		internal struct Cube
		{
			public int GameId;
			public int power;
			public bool possible;
		}
	}
}
