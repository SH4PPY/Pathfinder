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
    class AiBotRandom: AiBotBase
    {
        Random rnd;
        
        public AiBotRandom(int x, int y): base(x,y)
        {
            rnd = new Random();
        }

        protected override void ChooseNextGridLocation(Level level, Player plr)
        {
            bool ok = false;
            Coord2 pos;// = new Coord2();
            while (!ok)
            {
                pos = GridPosition;
                int x = rnd.Next(0,4);
                switch (x)
                {
                    case (0):
                        pos.X += 1;
                        break;
                    case (1):
                        pos.X -= 1;
                        break;
                    case (2):
                        pos.Y += 1;
                        break;
                    case (3):
                        pos.Y -= 1;
                        break;
                }
                ok = SetNextGridPosition(pos, level);
            }
        }
    }
}
