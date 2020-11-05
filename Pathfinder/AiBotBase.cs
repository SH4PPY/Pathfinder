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
    //aiBotBase is the base class for all ai objects. It encapsulates the position, movement animation, and validation 
    //of new positions, but leaves the actual behaviour function as an abstract member that must be implmented by a 
    //dervied class 

    abstract class AiBotBase
    {
        //gridPosition is the current location of the bot.
        //targetPosition is the next chosen location of the bot = the position it is moving towards
        //screen position is the position the sprite is drawn to - somewhere between gridlocation and targetlocation
        private Coord2 gridPosition; //X position, Y position on grid
        private Coord2 targetPosition; //X position, Y position on grid
        private Coord2 screenPosition; //X position, Y position on grid
        int timerMs;
        const int moveTime = 200; //miliseconds

        //accessors
        public Coord2 GridPosition
        {
            get { return gridPosition; }
        }
        public Coord2 ScreenPosition
        {
            get { return screenPosition; }
        }

        //constructor: requires initial position
        public AiBotBase(int x, int y)
        {
            gridPosition = new Coord2(x, y);
            targetPosition = new Coord2(x, y);
            screenPosition = new Coord2(x, y);
            timerMs = moveTime;
        }

        //sets target position: the next grid location to move to
        //need to validate this position - so must be within 1 cell of current position(in x and y directions)
        //and must also be valid on the map: greater than 0, less than mapsize, and not a wall
        public bool SetNextGridPosition(Coord2 pos, Level level)
        {
            if (pos.X < (gridPosition.X - 1)) return false;
            if (pos.X > (gridPosition.X + 1)) return false;
            if (pos.Y < (gridPosition.Y - 1)) return false;
            if (pos.Y > (gridPosition.Y + 1)) return false;
            if (!level.ValidPosition(pos)) return false;
            targetPosition = pos;
            return true;
        }

        //Handles animation moving from current grid position (gridLocation) to next grid position (targetLocation)
        //When target location is reached, sets grid location to targetLocation, and then calls ChooseNextGridLocation
        //and resets animation timer
        public void Update(GameTime gameTime, Level level, Player plr)
        {
            timerMs -= gameTime.ElapsedGameTime.Milliseconds;
            if (timerMs <= 0)
            {
                gridPosition = targetPosition;
                ChooseNextGridLocation(level, plr);
                timerMs = moveTime;
            }
            //calculate screen position
            screenPosition = (gridPosition * 15) + ((((targetPosition * 15) - (gridPosition * 15)) * (moveTime - timerMs)) / moveTime);
        }

        //this function is filled in by a derived class: must use SetNextGridLocation to actually move the bot
        protected abstract void ChooseNextGridLocation(Level level, Player plr);
    }
}
