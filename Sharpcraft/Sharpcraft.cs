/* 
 * Sharpcraft
 * Copyright (c) 2012 by Sijmen Schoon and Adam Hellberg.
 * All Rights Reserved.
 */

using System;
using System.Linq;
using System.Collections.Generic;

using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.GamerServices;

using Keys = Microsoft.Xna.Framework.Input.Keys;
using ButtonState = Microsoft.Xna.Framework.Input.ButtonState;

using Sharpcraft.Steam;
using Sharpcraft.Forms;
using Sharpcraft.Logging;
using Sharpcraft.Networking;

namespace Sharpcraft
{
	/// <summary>
	/// Main class of Sharpcraft, this is the game itself.
	/// </summary>
	/// <remarks>Most documentation in this class comes from XNA.</remarks>
	public class Sharpcraft : Game
	{
		/// <summary>
		/// Log object for this class.
		/// </summary>
		private readonly log4net.ILog _log;

		/// <summary>
		/// The graphics device manager.
		/// </summary>
		private GraphicsDeviceManager _graphics;
		/// <summary>
		/// Sprite batch.
		/// </summary>
		private SpriteBatch _spriteBatch;

		private bool _gameMenuOpen;
		private bool _menuToggling;
		private bool _inServer = true;

		private User _user;

		/// <summary>
		/// Initializes a new instance of Sharpcraft.
		/// </summary>
		public Sharpcraft(User user)
		{
			_log = LogManager.GetLogger(this);
			_log.Debug("Initializing graphics device.");
			_graphics = new GraphicsDeviceManager(this);
			_graphics.PreferredBackBufferWidth = 1280;
			_graphics.PreferredBackBufferHeight = 720;
			_log.Debug("Setting content directory.");
			Content.RootDirectory = "content";
		}

		/// <summary>
		/// Allows the game to perform any initialization it needs to before starting to run.
		/// This is where it can query for any required services and load any non-graphic
		/// related content.  Calling base.Initialize will enumerate through any components
		/// and initialize them as well.
		/// </summary>
		protected override void Initialize()
		{
			_log.Debug("Initialize();");
			_log.Info("Sharpcraft is initializing!");
			// TODO: Add your initialization logic here

			base.Initialize();

			/* /!\ Steam hardcore loading action /!\ */
			_log.Info("Loading Steam components...");
			if (SteamManager.Init())
			{
				//SteamManager.FriendList.LoadFriends(); // Should load automatically now
				//Application.EnableVisualStyles();
				_log.Info("Creating Steam GUI.");
				// TODO: Find a way to set the start location of SteamGUI to be next to Game Window.
				var steamGUI = new SteamGUI();
				if (!steamGUI.Visible)
					steamGUI.Show();
				_log.Info("Steam components loaded!");
			}
			else
			{
				_log.Info("Steam not installed or not running, Steam functionality will NOT be available.");
			}

			/* Commented out by Vijfhoek:
			 * Removed code seeing that we will add it somewhere else later.
			_log.Debug("Creating protocol...");
			var protocol = new Protocol("localhost", 25565);

			_log.Debug("Sending handshake packet.");
			protocol.PacketHandshake("Sharpcraft");
			protocol.GetPacket();
			_log.Debug("Sending login request.");
			protocol.PacketLoginRequest(22, "Sharpcraft");
			protocol.GetPacket();
			*/
		}

		/// <summary>
		/// LoadContent will be called once per game and is the place to load
		/// all of your content.
		/// </summary>
		protected override void LoadContent()
		{
			_log.Debug("LoadContent();");
			_log.Info("!!! GAME LOAD !!!");
			// Create a new SpriteBatch, which can be used to draw textures.
			_spriteBatch = new SpriteBatch(GraphicsDevice);

			// TODO: use this.Content to load your game content here
		}

		/// <summary>
		/// UnloadContent will be called once per game and is the place to unload
		/// all content.
		/// </summary>
		protected override void UnloadContent()
		{
			_log.Info("!!! GAME UNLOAD !!!");
			_log.Debug("UnloadContent();");
			// TODO: Unload any non ContentManager content here
			SteamManager.Close();
		}

		/// <summary>
		/// Allows the game to run logic such as updating the world,
		/// checking for collisions, gathering input, and playing audio.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Update(GameTime gameTime)
		{
			// Allows the game to exit
			if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
				Exit();
			if (Keyboard.GetState().IsKeyDown(Keys.Escape))
				ToggleGameMenu();
			if (Keyboard.GetState().IsKeyUp(Keys.Escape))
				_menuToggling = false;

			// TODO: Add your update logic here

			base.Update(gameTime);
		}

		private void ToggleGameMenu()
		{
			if (!_inServer || _menuToggling)
				return;
			_menuToggling = true;
			_gameMenuOpen = !_gameMenuOpen;
			_log.Debug("Game menu is now " + (_gameMenuOpen ? "open" : "closed"));
		}

		/// <summary>
		/// This is called when the game should draw itself.
		/// </summary>
		/// <param name="gameTime">Provides a snapshot of timing values.</param>
		protected override void Draw(GameTime gameTime)
		{
			GraphicsDevice.Clear(Color.CornflowerBlue);

			// TODO: Add your drawing code here

			base.Draw(gameTime);
		}
	}
}
