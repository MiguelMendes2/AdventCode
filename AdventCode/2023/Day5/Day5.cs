using AdventCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode.Days
{
	public class Day5 : IDay
	{
		public void Run()
		{
			string[] input = File.ReadAllLines("2023\\Day5\\Input.txt");

			string prevType = "seed";
			string type = "";
			List<Map> typeMaps = new();
			List<long> seeds = input[0].Split(':')[1].Split(' ')
				.Where(x => !String.IsNullOrEmpty(x)).Select(long.Parse).ToList();
			
			Console.WriteLine($"{PartOne(seeds, input)}");
			Console.WriteLine($"{PartTwo(seeds, input)}");
		}

		internal long PartOne(List<long> seeds, string[] input)
		{
			string prevType = "seed";
			string type = "";
			List<Map> typeMaps = new();
			foreach (var seed in seeds)
			{
				typeMaps.Add(new Map
				{
					TypeName = prevType,
					Source = seed,
					Destination = seed
				});
			}
			List<string> typeLines = new();
			for (int i = 2; i < input.Length; i++)
			{
				if (input[i].Split('-').Length == 3)
				{
					type = input[i].Split('-')[2].Split(' ')[0];
				}
				else if (input[i]?.Trim() != "")
				{
					typeLines.Add(input[i]);
				}
				else
				{
					typeMaps.AddRange(GetMap(typeLines, type, prevType));
					typeLines = new();
					prevType = type;
				}
			}
			typeMaps.AddRange(GetMap(typeLines, type, prevType));
			List<long> x = new();
			foreach (var seed in seeds)
			{
				x.Add(getLocation(typeMaps, seed));
			}
			return x.Min();
		}

		internal long PartTwo(List<long> seeds, string[] input)
		{
			string prevType = "seed";
			string type = "";
			List<Map> typeMaps = new();
			for(int i = 0; i < seeds.Count; i+= 2)
			{
				typeMaps.Add(new Map
				{
					TypeName = prevType,
					Source = seeds[i],
					Destination = seeds[i] + seeds[i + 1] - 1
				});				
			}
			List<string> typeLines = new();
			for (int i = 2; i < input.Length; i++)
			{
				if (input[i].Split('-').Length == 3)
				{
					type = input[i].Split('-')[2].Split(' ')[0];
				}
				else if (input[i]?.Trim() != "")
				{
					typeLines.Add(input[i]);
				}
				else
				{
					typeMaps.AddRange(GetMap(typeLines, type, prevType));
					typeLines = new();
					prevType = type;
				}
			}
			typeMaps.AddRange(GetMap(typeLines, type, prevType));

			return GetMinSeed(typeMaps);
		}

		internal long getLocation(List<Map> typeMaps, long location)
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
						if(local < min || min < 0)
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

		internal long GetMinSeed(List<Map> typeMaps)
		{
			long result = -1;
			bool foundSeed = false;
			while (!foundSeed)
			{
				string typeName = "location";
				int tries = 0;
				long tempResult = 0;
				long location = typeMaps.Where(x => x.TypeName == typeName).Min(x => x.Destination) + tries;
				Map prevType = typeMaps.Where(x => x.TypeName == typeName).FirstOrDefault();
				do
				{
					var matchs = typeMaps.Where(x => x.Source <= location
					&& x.Source + x.Range >= location && x.PrevTypeName == prevType.PrevTypeName).ToList();

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
						tempResult = min;
					}
					else
					{
						break;
					}
					typeName = prevType.PrevTypeName;
					prevType = typeMaps.Where(x => x.TypeName == typeName).FirstOrDefault();

				} while (result < 0);
				tries++;
			}
			
			return result;
		}

		internal List<Map> GetMap(List<string> lines, string type, string prevType)
		{
			List<Map> typeMaps = new();
			for(int i = 0; i < lines.Count; i++)
			{
				long[] nums = lines[i].Split(' ').Select(long.Parse).ToArray();
				Map map = new()
				{
					TypeName = type,
					PrevTypeName = prevType,
					Destination = nums[0],
					Source = nums[1],
					Range = nums[2]
				};
				typeMaps.Add(map);
			}
			return typeMaps;
		}


		internal struct Map
		{
			public string TypeName { get; set; }
			public string PrevTypeName { get; set; }
			public long Source { get; set; }
			public long Destination { get; set; }
			public long Range { get; set; }
		}
	}
}
