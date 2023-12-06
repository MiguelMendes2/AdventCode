using AdventCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AdventCode.Days
{
	public class Day6 : IDay
	{

		public void Run()
		{
			string[] input = File.ReadAllLines("2023\\Day6\\Input.txt");
			
			Console.WriteLine($"Part 1: {PartOne(input)}");
			Console.WriteLine($"Part 2: {PartTwo(input)}");
		}

		internal long PartOne(string[] input)
		{
			List<Times> times = GetTimes(input);
			long sum = 1;
			foreach(var time  in times)
			{
				sum *= GetPossibilities(time);
			}
			return sum;
		}

		internal long PartTwo(string[] input)
		{
			Times time = GetTime(input);
			return GetPossibilities(time);
		}

		internal List<Times> GetTimes(string[] lines)
		{
			List<int> nums = new List<int>();
			for (int lineNum = 0; lineNum < lines.Length; lineNum++)
			{
				string[] values = lines[lineNum].Split(':')[1].Split(' ');
				foreach(var value in values)
				{
					if(int.TryParse(value, out int num))
					{
						nums.Add(num);
					}
				}
			}

			List<Times> times = new List<Times>();
			for (int i = 0; i < nums.Count / 2; i++)
			{
				times.Add(new Times { Time = nums[i], Distance = nums[i + nums.Count / 2] });
			}
			
			return times;
		}

		internal Times GetTime(string[] lines)
		{
			List<long> nums = new List<long>();
			for (int lineNum = 0; lineNum < lines.Length; lineNum++)
			{
				char[] values = lines[lineNum].Split(':')[1].Trim().Where(c => !Char.IsWhiteSpace(c)).ToArray();
				nums.Add(long.Parse(string.Concat(values)));
			}

			return new Times { Time = nums[0], Distance = nums[1] };
		}

		internal long GetPossibilities(Times time)
		{
			long sum = 0;
			for(int i = 1; i < time.Time; i++)
			{
				if(i * (time.Time - i) > time.Distance)
				{
					sum++;
				}
			}
			return sum;
		}

		internal struct Times
		{
			public long Time { get; set; }
			public long Distance { get; set; }
		}
	}
}
