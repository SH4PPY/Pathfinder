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
	class AiBotSimple_2 : AiBotBase
	{
		public AiBotSimple_2(int x, int y) : base(x, y)
		{

		}
		protected override void ChooseNextGridLocation(Level level, Player plr)
		{
			bool moved = false;
			Coord2 newPos = GridPosition;

			if (GridPosition.X > plr.GridPosition.X)
			{
				newPos.X -= 1;
				if(level.ValidPosition(newPos) == false)
				{
					newPos.X += 1;
					newPos.Y -= 1;
				}
			}
			else if (GridPosition.X < plr.GridPosition.X)
			{ 
				newPos.X += 1;
				if (level.ValidPosition(newPos) == false)
				{
					newPos.X -= 1;
					newPos.Y += 1;
				}
			}
			else if (GridPosition.Y > plr.GridPosition.Y)
			{
				newPos.Y -= 1;
				if (level.ValidPosition(newPos) == false)
				{
					newPos.X -= 1;
					newPos.Y += 1;
				}
			}
			else if (GridPosition.Y < plr.GridPosition.Y)
			{
				newPos.Y += 1;
				if (level.ValidPosition(newPos) == false)
				{
					newPos.X += 1;
					newPos.Y -= 1;
				}
			}
			moved =	SetNextGridPosition(newPos, level);
		}
	}
}

