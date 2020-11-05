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
    class Player
    {
        private Coord2 gridPosition;   //X position, Y position on grid
        private Coord2 targetPosition; //X position, Y position on grid
        private Coord2 screenPosition; //X position, Y position on grid
        private int timerMs;
        private const int moveTime = 200; //miliseconds

        //accessors
        public Coord2 GridPosition
        {
            get { return gridPosition; }
        }
        public Coord2 ScreenPosition
        {
            get { return screenPosition; }
        }

        //constructor: takes an initial position as arguments
        public Player(int x, int y)
        {
            gridPosition = new Coord2(x, y);
            targetPosition = new Coord2(x, y);
            screenPosition = new Coord2(x, y);
            timerMs = moveTime;
        }

        //Handles animation moving from current grid position (gridLocation) to next grid position (targetLocation)
        public void Update(GameTime gameTime, Level level)
        {
            if (timerMs > 0)
            {
                timerMs -= gameTime.ElapsedGameTime.Milliseconds;
                if (timerMs <= 0)
                {
                    timerMs = 0;
                    gridPosition = targetPosition;
                }
            }
           
            //calculate screen position
            screenPosition = (gridPosition * 15) + ((((targetPosition * 15) - (gridPosition * 15)) * (moveTime - timerMs)) / moveTime); 
        }

        //sets next position for player to move to: called by keyboard processing functions. validates new position against level,
        //so can't move to blocked position, or position off grid
        public void SetNextLocation(Coord2 newLoc, Level level)
        {
            if (timerMs > 0) return;
            if (level.ValidPosition(newLoc))
            {
                targetPosition = newLoc;
                timerMs = moveTime;
            }
        }

    }
}
