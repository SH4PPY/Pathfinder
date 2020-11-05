using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;



namespace Pathfinder
{
    struct Coord2
    {
        public int X;
        public int Y;

        public Coord2(int a, int b) { X = a; Y = b; }
        public static Coord2 operator +(Coord2 c1, Coord2 c2)
        {
            Coord2 res;
            res.X = (c1.X+c2.X);
            res.Y = (c1.Y+c2.Y);
            return res;
        }
        public static Coord2 operator -(Coord2 c1, Coord2 c2)
        {
            Coord2 res;
            res.X = (c1.X - c2.X);
            res.Y = (c1.Y - c2.Y);
            return res;
        }
        public static Coord2 operator *(Coord2 c1, int c2)
        {
            Coord2 res;
            res.X = c1.X * c2;
            res.Y = c1.Y * c2;
            return res;
        }
        public static Coord2 operator /(Coord2 c1, int c2)
        {
            Coord2 res;
            res.X = c1.X/c2;
            res.Y = c1.Y/c2;
            return res;
        }
        public static bool operator ==(Coord2 c1, Coord2 c2)
        {
            return ((c1.X == c2.X) && (c1.Y == c2.Y));
        }
        public static bool operator !=(Coord2 c1, Coord2 c2)
        {
            return ((c1.X != c2.X) || (c1.Y != c2.Y));
        }
        public static implicit operator Vector2(Coord2 c1)
        {
            Vector2 res;
            res.X = c1.X;
            res.Y = c1.Y;
            return res;
        }
        public override int GetHashCode()
        {
            return X + Y;
        }
        public override bool Equals(Object obj)
        {
            //Check for null and compare run-time types.
            if (obj == null || GetType() != obj.GetType()) return false;
            Coord2 p = (Coord2)obj;
            return (X == p.X) && (Y == p.Y);
        }
    }
}
