using System;
using System.Collections.Generic;
using System.Linq;
using System.Diagnostics;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Audio;

using System.IO;

namespace Pathfinder
{
	class ScentMap : AiBotBase
	{
		public String scentTimer;
		Stopwatch watch = new Stopwatch();
		int gridSize; //the map grid size
		public int[,] buffer1;
		public int[,] buffer2;
		bool running = true;
		int sourceValue;

		Coord2[] PreCalcNeighbours = new Coord2[8];
		Coord2 currentNode = new Coord2();

		public ScentMap(int x, int y) : base(x, y)
		{
			gridSize = 40;
			buffer1 = new int[gridSize, gridSize];
			buffer2 = new int[gridSize, gridSize];
			sourceValue = 0;

			PreCalcNeighbours[0] = new Coord2(0, 1);
			PreCalcNeighbours[1] = new Coord2(1, 0);
			PreCalcNeighbours[2] = new Coord2(0, -1);
			PreCalcNeighbours[3] = new Coord2(-1, 0);
			PreCalcNeighbours[4] = new Coord2(1, 1);
			PreCalcNeighbours[5] = new Coord2(1, -1);
			PreCalcNeighbours[6] = new Coord2(-1, -1);
			PreCalcNeighbours[7] = new Coord2(-1, 1);
		}
		protected override void ChooseNextGridLocation(Level level, Player plr)
		{
			int highestScent = 0;
			Coord2 pos = GridPosition;
			Coord2 nextPos = new Coord2(0, 0);
			for (int i = 0; i < 8; i++)
			{
				int temp = buffer2[pos.X + PreCalcNeighbours[i].X, pos.Y + PreCalcNeighbours[i].Y];
				if (temp > highestScent)
				{
					highestScent = temp;
					nextPos = pos + PreCalcNeighbours[i];
				}
			}
			SetNextGridPosition(nextPos, level);
		}
		public void Update(Level level, Player plr, AiBotBase bot)
		{
			if (running == true)
			{
				watch.Start();
			}
			sourceValue++;
			for (int x = 0; x < 40; x++)
            {
				for (int y = 0; y < 40; y++)
                {
					buffer1[x, y] = buffer2[x, y];
				}
			}
			//Array.Copy(buffer1, buffer2, buffer2.Length);
			buffer1[plr.GridPosition.X, plr.GridPosition.Y] = sourceValue;
			
			for (int xx = 0; xx < 40; xx++)
			{
				for (int yy = 0; yy < 40; yy++)
				{
					if (level.ValidPosition(new Coord2(xx, yy)))
					{
						int highestScent = 0;
						Coord2 pos = new Coord2(xx, yy);						
						for(int i = 0; i < 8; i++)
						{
							Coord2 test = pos + PreCalcNeighbours[i];
							if (test.X >= 0 && test.X < gridSize && test.Y >= 0 && test.Y < gridSize)
							{
								int temp = buffer1[pos.X + PreCalcNeighbours[i].X, pos.Y + PreCalcNeighbours[i].Y];
								if (temp > highestScent)
								{
									highestScent = temp;
								}
							}
							buffer2[pos.X, pos.Y] = highestScent - 1;
						}
					}
				}
			}
			if ((buffer1[bot.GridPosition.X, bot.GridPosition.Y] <= 0))
			{
				running = true;
			}
			else if ((buffer1[bot.GridPosition.X, bot.GridPosition.Y] >= 1))
			{
				running = false;
				if (running == false)
				{
					watch.Stop();
					scentTimer = watch.ElapsedMilliseconds.ToString();
					//Console.WriteLine("Scent time = " + scentTimer + " Milliseconds");
					watch.Reset();
				}
			}
		}
		public void UpdateScent()
		{
			for (int x = 0; x < 8; x++)
			{
				Coord2 neighbourPos = new Coord2(currentNode.X + PreCalcNeighbours[x].X, currentNode.Y + PreCalcNeighbours[x].Y);
			}
		}
	}
}