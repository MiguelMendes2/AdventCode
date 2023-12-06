using AdventCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Diagnostics;

namespace AdventCode.Days
{
	public class Day5 : IDay
	{

		public void Run()
		{
			string[] input = File.ReadAllLines("2023\\Day5\\Input.txt");
			
			Console.WriteLine($"Part 1: {PartOne(input)}");
			Console.WriteLine($"Part 2: {PartTwo(input)}");
		}

		internal long PartOne(string[] input)
		{
			List<long> seeds = input[0].Split(':')[1].Split(' ')
				.Where(x => !String.IsNullOrEmpty(x)).Select(long.Parse).ToList();
			List<Map> typeMaps = GetMap(input);
			long min = long.MaxValue;
			foreach (var seed in seeds)
			{
				long val = GetLocation(typeMaps, seed);
				if(val < min) 
					min = val;
			}
			return min;
		}

		internal long GetLocation(List<Map> typeMaps, long location)
		{
			string typeName = "seed";
			Map nextType = typeMaps.Where(x => x.PrevTypeName == typeName).FirstOrDefault();
			do
			{
				var matchs = typeMaps.Where(x => x.Source <= location
					&& x.Source + x.Range >= location && x.PrevTypeName == typeName).ToList();

				if (matchs.Count > 0)
				{
					long min = -1;
					foreach (var match in matchs)
					{
						long diff = location - match.Source;
						long local = match.Destination + diff;
						if (local < min || min < 0)
						{
							min = local;
						}
					}
					location = min;
				}
				typeName = nextType.TypeName;
				nextType = typeMaps.Where(x => x.PrevTypeName == typeName).FirstOrDefault();

			} while (nextType.TypeName != null);
			return location;
		}

		internal List<Map> GetMap(string[] lines)
		{
			int i = 2;
			List<Map> typeMaps = new();
			string type = "";
			string prevType = "";
			while (i < lines.Length)
			{
				if (lines[i].Split("-to-").Length == 2)
				{
					string[] types = lines[i].Split("-to-");
					prevType = types[0];
					type = types[1].Split(' ')[0];
				}
				else if (lines[i] != "")
				{
					List<long> nums = lines[i].Split(' ').Select(long.Parse).ToList();
					typeMaps.Add(new()
					{
						TypeName = type,
						PrevTypeName = prevType,
						Destination = nums[0],
						Source = nums[1],
						Range = nums[2]
					});
				}
				i++;
			}
			return typeMaps;
		}


		internal long PartTwo(string[] input)
		{
			List<Range> seedRanges = GetRangedSeeds(input);
			List<Map> typeMaps = GetMap(input).OrderBy(x => x.Source).ToList();

			foreach(var maps in typeMaps.GroupBy(x => x.TypeName))
			{
				List<Range> newRange = new();
				foreach(var seedRange in seedRanges)
				{
					var range = seedRange;
					foreach(var map in maps)
					{
						// obter zonas de exclusão em que o map não tem source, logo o start da seed vai ser igual à destination
						if(range.Start < map.Source)
						{
							newRange.Add( new Range(range.Start, Math.Min(range.End, map.Destination - 1)));
							range.End = map.Destination;
							// valida se é zona de exclusão 
							if (range.Start > map.Destination)
								break;
						}

						if (range.Start <= map.Destination)
						{
							newRange.Add(new Range(range.Start + map.Range, Math.Min(range.End, map.Destination) + map.Range));
							range.End = map.Destination + 1;
							// valida se é zona de exclusão 
							if (range.Start > map.Destination)
								break;
						}
					}
					if (range.Start <= range.End)
						newRange.Add(range);
				}
				seedRanges = newRange;
			}
			return seedRanges.Min(x => x.Start);

			//foreach (var seed in seeds)
			//{
			//	long newSeed = seed;
			//	foreach (var map in typeMaps)
			//	{
			//		if(seed >= map.Destination && seed < map.Source + map.Range)
			//		{
			//			newSeed = map.Source + (seed - map.Destination);
			//		}
			//	}
			//}
			return 0;//seeds.Min();
		}

		internal List<Range> GetRangedSeeds(string[] input)
		{
			List<Range> rangedSeeds = new();
			string[] seedLine = input[0].Split(' ');
			for (int i = 1; i < seedLine.Length; i += 2)
			{
				rangedSeeds.Add(new Range(long.Parse(seedLine[i]), long.Parse(seedLine[i + 1])));
			}
			return rangedSeeds;
		}

		internal struct Map
		{
			public string TypeName { get; set; }
			public string PrevTypeName { get; set; }
			public long Source { get; set; }
			public long Destination { get; set; }
			public long Range { get; set; }
		}

		internal struct Range(long start, long end)
		{
			public long Start { get; set; } = start;
			public long End { get; set; } = end;
		}
	}
}
