#region Using Statements
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
//using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
//using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;

#endregion

namespace Pathfinder
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Game
    {   
        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;

        //sprite texture for tiles, player, and ai bot
        Texture2D tile1Texture;
        Texture2D tile2Texture;
        Texture2D aiTexture;
        Texture2D playerTexture;

        //objects representing the level map, bot, and player 
        private Level level;
        private AiBotBase bot;
        private Player player;
		public bool hasAStar = false;
		public bool hasDijkstra = false;
		public bool hasScentMap = false;
        public bool liveUpdate = false;
        //screen size and frame rate
        private const int TargetFrameRate = 60;
        private const int BackBufferWidth = 600;
        private const int BackBufferHeight = 600;

        public Game1()
        {
            //constructor
            graphics = new GraphicsDeviceManager(this);
            graphics.PreferredBackBufferHeight = BackBufferHeight;
            graphics.PreferredBackBufferWidth = BackBufferWidth;
            Window.Title = "Pathfinder - SHA12307610";
            Content.RootDirectory = "../../../Content";
            //set frame rate
            TargetElapsedTime = TimeSpan.FromTicks(TimeSpan.TicksPerSecond / TargetFrameRate);
            //load level map
            level = new Level();
            level.Loadmap("../../../Content/5.txt");
            //instantiate bot and player objects
            bot = new AiBotStatic(10, 20);
            player = new Player(30, 20);
        }

        protected override void Initialize()
        {
            base.Initialize();
        }

        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);
            //load the sprite textures
            Content.RootDirectory = "../../../Content";
            tile1Texture = Content.Load<Texture2D>("tile1");
            tile2Texture = Content.Load<Texture2D>("tile2");
            aiTexture = Content.Load<Texture2D>("ai");
            playerTexture = Content.Load<Texture2D>("target");
        }

        protected override void UnloadContent()
        {
        }

        protected override void Update(GameTime gameTime)
        {
            // Allows the game to exit
            //if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            //    this.Exit();

            //player movement: read keyboard
            KeyboardState keyState = Keyboard.GetState();
            Coord2 currentPos = new Coord2();
            currentPos = player.GridPosition;

            if(keyState.IsKeyDown(Keys.Up))
            {
                currentPos.Y -= 1;
                player.SetNextLocation(currentPos, level);
                if ((hasAStar == true) && (liveUpdate == true))
                {
                    level.aStar.Build(level, bot, player);
                }
                if ((hasDijkstra == true) && (liveUpdate == true))
                {
                    level.dijkstra.Build(level, bot, player);
                }
            }
            else if (keyState.IsKeyDown(Keys.Down))
            {
                currentPos.Y += 1;
                player.SetNextLocation(currentPos, level);
                if ((hasAStar == true) && (liveUpdate == true))
                {
                    level.aStar.Build(level, bot, player);
                }
                if ((hasDijkstra == true) && (liveUpdate == true))
                {
                    level.dijkstra.Build(level, bot, player);
                }
            }
            else if (keyState.IsKeyDown(Keys.Left))
            {
                currentPos.X -= 1;
                player.SetNextLocation(currentPos, level);
                if ((hasAStar == true) && (liveUpdate == true))
                {
                    level.aStar.Build(level, bot, player);
                }
                if ((hasDijkstra == true) && (liveUpdate == true))
                {
                    level.dijkstra.Build(level, bot, player);
                }
            }
            else if (keyState.IsKeyDown(Keys.Right))
            {
                currentPos.X += 1;
                player.SetNextLocation(currentPos, level);
                if ((hasAStar == true) && (liveUpdate == true))
                {
                    level.aStar.Build(level, bot, player);
                }
                if ((hasDijkstra == true) && (liveUpdate == true))
                {
                    level.dijkstra.Build(level, bot, player);
                }
            }
            //toggle liveUpdate
            else if ((keyState.IsKeyDown(Keys.L)) && (liveUpdate == false))
            {
                liveUpdate = true;
            }
            else if ((keyState.IsKeyDown(Keys.L)) && (liveUpdate == true))
            {
                liveUpdate = false;
            }
            //use dijkstra
            else if (keyState.IsKeyDown(Keys.D))
			{
				bot = new AiBotStatic(10, 20);
				level.dijkstra.Build(level, bot, player);
				hasScentMap = false;
				hasDijkstra = true;
				hasAStar = false;
			}
			//use AStar
			else if (keyState.IsKeyDown(Keys.A))
			{
				bot = new AiBotStatic(10, 20);
				level.aStar.Build(level, bot, player);
				hasScentMap = false;
				hasAStar = true;
				hasDijkstra = false;
			}
			//Use Scent Map
			else if (keyState.IsKeyDown(Keys.S))
			{
				bot = new AiBotStatic(10, 20);
				level.scentMap.Update(level, player, bot);
				hasScentMap = true;
				hasDijkstra = false;
				hasAStar = false;
			}
			//Reset
			else if (keyState.IsKeyDown(Keys.F))
			{
				bot = new AiBotStatic(10, 20);
				hasScentMap = false;
				hasDijkstra = false;
				hasAStar = false;
			}
			//Exit
			else if (keyState.IsKeyDown(Keys.Escape))
			{
				Exit();
			}

            //update bot and player
            bot.Update(gameTime, level, player);
            player.Update(gameTime, level);
			//Update Scent Map
			level.scentMap.Update(level, player, bot);
            base.Update(gameTime);
        }

        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            //draw level map
            DrawGrid();
            //draw bot
            spriteBatch.Draw(aiTexture, bot.ScreenPosition, Color.White);
            //drawe player
            spriteBatch.Draw(playerTexture, player.ScreenPosition, Color.White);
            spriteBatch.End();

            base.Draw(gameTime);
        }

        private void DrawGrid()
        {
            //draws the map grid
            int sz = level.GridSize;
            for (int x = 0; x < sz; x++)
            {
                for (int y = 0; y < sz; y++)
                {
                    Coord2 pos = new Coord2((x*15), (y*15));
					if (level.tiles[x, y] == 0)
					{
						if (hasAStar)
						{
							//showing the route and open/closed lists
							if (level.aStar.inPath[x, y])
							{
								spriteBatch.Draw(tile1Texture, pos, Color.Red);
							}
							else if (level.aStar.cost[x,y] < float.MaxValue && level.aStar.IsOpen[x,y])
							{
								spriteBatch.Draw(tile1Texture, pos, Color.LightBlue);
							}
							else if (!level.aStar.IsOpen[x, y])
							{
								spriteBatch.Draw(tile1Texture, pos, Color.Blue);
							}
							else
							{
								spriteBatch.Draw(tile1Texture, pos, Color.White);
							}
						}
						if (hasDijkstra)
						{
							//showing the route and open/closed lists
							if (level.dijkstra.inPath[x, y])
							{
								spriteBatch.Draw(tile1Texture, pos, Color.Red);
							}
							else if (level.dijkstra.cost[x, y] < float.MaxValue && level.dijkstra.IsOpen[x, y])
							{
								spriteBatch.Draw(tile1Texture, pos, Color.LightBlue);
							}
							else if (!level.dijkstra.IsOpen[x, y])
							{
								spriteBatch.Draw(tile1Texture, pos, Color.Blue);
							}
							else
							{
								spriteBatch.Draw(tile1Texture, pos, Color.White);
							}
						}
						if (hasScentMap)
						{
							//Shows scent flowing out
							spriteBatch.Draw(tile1Texture, pos, Color.Lerp(Color.LimeGreen, Color.Magenta, Math.Min((float)Math.Sin(level.scentMap.buffer1[x, y] * 0.1f), 10)));
						}
						else if (!hasAStar && !hasDijkstra && !hasScentMap)
						{
							spriteBatch.Draw(tile1Texture, pos, Color.White);
						}
					}
					else
					{
						spriteBatch.Draw(tile2Texture, pos, Color.White);
					}
                }
            }
        }
	}
}