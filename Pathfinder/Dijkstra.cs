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
    class Dijkstra
    {
		String dijkstraTimer;
		Stopwatch watch = new Stopwatch();

        public bool[,] IsOpen;      //Whether or not the location is closed
        public float[,] cost;       //Cost value for each location
        public Coord2[,] link;      //Link for each location = coords of a neighbouring location
        public bool[,] inPath;      //Whether or not a location is in the final path

        // Movement references
        const float STRAIGHTMOVEMENT = 1;
        const float DIAGONALMOVEMENT = 1.4f;

        Coord2[] PreCalcNeighbours = new Coord2[8]; 

        public Dijkstra()
        {
            IsOpen = new bool[40, 40];
            cost = new float[40, 40];
            link = new Coord2[40, 40];
            inPath = new bool[40, 40];

			//Precalculate the neighbours
            PreCalcNeighbours[0] = new Coord2(0, 1);
            PreCalcNeighbours[1] = new Coord2(1, 0);
            PreCalcNeighbours[2] = new Coord2(0, -1);
            PreCalcNeighbours[3] = new Coord2(-1, 0);
            PreCalcNeighbours[4] = new Coord2(1, 1);
            PreCalcNeighbours[5] = new Coord2(1, -1);
            PreCalcNeighbours[6] = new Coord2(-1, -1);
            PreCalcNeighbours[7] = new Coord2(-1, 1);
        }

        public void Build(Level level, AiBotBase bot, Player plr)
        {
			watch.Start();
            for (int x = 0; x < 40; x++)
            {
                for (int y = 0; y < 40; y++)
                {
                    IsOpen[x, y] = true;   //Set all to open
                    cost[x, y] = float.MaxValue;
                    link[x, y] = new Coord2(-1, -1);
                    inPath[x, y] = false;
                }
            }

            IsOpen[bot.GridPosition.X, bot.GridPosition.Y] = true;
            cost[bot.GridPosition.X, bot.GridPosition.Y] = 0;

            float lowestCost = float.MaxValue;
            Coord2 currentNode = new Coord2();

            //Loop while location occupied by the player is closed
            while (IsOpen[plr.GridPosition.X, plr.GridPosition.Y] == true)
            {
                lowestCost = float.MaxValue;

                //Find the grid location with the lowest cost, that is not closed, and is not blocked
                for (int x = 0; x < 40; x++)
                {
                    for (int y = 0; y < 40; y++)
                    {
                        //Is open
                        if (IsOpen[x, y] == true)
                        {
                            //Is not blocked
                            if(level.ValidPosition(new Coord2(x, y)))
                            {
                                if (cost[x, y] < lowestCost)
                                {
                                    //If the cost is lower, save it to lowest cost
                                    //and remember the position
                                    currentNode = new Coord2(x, y);
                                    lowestCost = cost[x, y];
                                }
                            }
                        }
                    }
                }

                //Mark the lowest cost node as closed
                IsOpen[currentNode.X, currentNode.Y] = false;

                //Calculate new cost for the 8 locations around this nocde
                for (int i = 0; i < 8; i++)
                {
                    //Get the position of each neighbour
                    Coord2 neighbourPosition = new Coord2(currentNode.X + PreCalcNeighbours[i].X, currentNode.Y + PreCalcNeighbours[i].Y);

                    //Is the neighboura valid position
                    if (level.ValidPosition(neighbourPosition))
                    {

                        float newCost;
                        //Straights
                        if (i <= 3)
                        {
                            newCost = cost[currentNode.X, currentNode.Y] + 1; 
                        }
                        else  //Diagonals
                        {
                            newCost = cost[currentNode.X, currentNode.Y] + 1.4f;
                        }

                        //If this new cost is lower than the neighbours orginal cost
                        if (newCost <= cost[neighbourPosition.X, neighbourPosition.Y])
                        {
                            //Set the neighbours cost to be the new one
                            cost[neighbourPosition.X, neighbourPosition.Y] = newCost;

                            //Also set the neighbours link value to be the parent
                            link[neighbourPosition.X, neighbourPosition.Y] = currentNode;
                        }
                    }
                }
            }

            bool done = false;
            Coord2 nextClosed = plr.GridPosition;
            while (!done)
            {
                inPath[nextClosed.X, nextClosed.Y] = true;
                nextClosed = link[nextClosed.X, nextClosed.Y];
                if (nextClosed == bot.GridPosition)
                {
                    done = true;
                }
            }
			watch.Stop();
			dijkstraTimer = watch.ElapsedMilliseconds.ToString();
			Console.WriteLine("Dijkstra time = " + dijkstraTimer + " Milliseconds");
			watch.Reset();
        }
    }
}