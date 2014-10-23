using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;

namespace RamEater
{
	class Program
	{
		static void Main (string[] args)
		{
			Console.WriteLine (
				"This program will incrementally allocate as much RAM as possible. Save any open documents first as this " +
				"might lead to severe slowdown or even crashes. Press any key to start.");
			Console.ReadKey (true);
			Thread.CurrentThread.Priority = ThreadPriority.Lowest;

			var list = new List<byte[]> ();
			long nAllocated = 0;
			int size = 1024 * 1024 * 1024;

			while (true) {
				long used = GC.GetTotalMemory (false);
				var gb = used / 1024.0 / 1024.0 / 1024.0;
				Console.Write ("Total RAM: " + used + "b (" + gb.ToString ("0.00") + "GB); GC: ");
				for (int gen = 0; gen < GC.MaxGeneration; gen++) {
					Console.Write (gen + ":" + GC.CollectionCount (gen) + " ");
				}
				Console.WriteLine ();

				try {
					Console.Write ("Trying to grab " + size + "b...");
					var alloc = new byte[size];
					Console.Write (" ok! Filling it... 00");
					list.Add (alloc);
					for (long i = 0; i < alloc.Length; i += 1024) {
						if (i % (10 * 1024 * 1024) == 0) {
							var p = (100 * i / alloc.Length).ToString ("00");
							Console.Write ("\b\b" + p);
						}
						alloc[i] = (byte) i;
					}
					Console.WriteLine ("\b\b100 ok!");
					nAllocated += alloc.Length;

					Console.WriteLine ("Allocated " + (nAllocated / 1024.0 / 1024.0).ToString ("0.0") + "MB of RAM");
					Thread.Sleep (100);
				}
				catch (OutOfMemoryException) {
					Console.WriteLine ("Out of memory. Press any key to resume with smaller allocation sizes.");
					Console.ReadKey (true);
					if (size > 16 * 1024) {
						size = size / 2;
					}
				}

				Console.WriteLine ();
			}
		}
	}
}
