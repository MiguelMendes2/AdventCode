using AdventCode.Days;
using AdventCode.Interface;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AdventCode._2023
{
	public class Runner : IRunner
	{
		public void RunYear()
		{
			for (int i = 6; i <=6; i++)
			{
				Console.WriteLine($"--- Day {i} ---");
				var service = GetDayService(i);
				service.Run();
			}
		}

		internal static IDay GetDayService(int day)
		{
			switch (day)
			{
				//case 1:
				//	return new Day1();
				//case 2:
				//	return new Day2();
				//case 3:
				//	return new Day3();
				//case 4:
				//	return new Day4();
				case 5:
					return new Day5();
				case 6:
					return new Day6();
				default:
					throw new KeyNotFoundException(); // or maybe return null, up to you
			}
		}
	}
}
