using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;

namespace HTMLEngine.XNA4
{
    /// <summary>
    /// This is the main type for your game
    /// </summary>
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        private const string demo = @"
<p align=center>Hello world!</p><br><br>
<p align=justify valign=middle><div width=200><img src='logos/html'></div><div>Lorem ipsum dolor sit amet, consectetur adipisicing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua. Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat. Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur. Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.</div></p>
<effect name=outline color=black>
<p align=center>Normal text <u>underlined</u><s>striked</s></p>
<p align=center><b>Bold text<u>underlined</u><s>striked</s></b></p>
<p align=center><i>Italic text<u>underlined</u><s>striked</s></i></p>
<p align=center><b><i>Bold and italic text<u>underlined</u><s>striked</s></i></b></p>
</effect>
";

        GraphicsDeviceManager graphics;
        SpriteBatch spriteBatch;
        private HtCompiler compiler;

        public Game1()
        {
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            HtmlDevice.Context = spriteBatch;
            HtmlDevice.Content = Content;

            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
            spriteBatch = new SpriteBatch(GraphicsDevice);

            HtmlDevice.Content = Content;
            HtmlDevice.Context = spriteBatch;
            HtEngine.DefaultFontFace = "Arial";
            HtEngine.DefaultFontSize = 14;
            HtEngine.RegisterDevice(new HtmlDevice());

            compiler = HtEngine.GetCompiler();
            compiler.Compile(demo, GraphicsDevice.Viewport.Width);
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            compiler.Dispose();
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
                this.Exit();

            // TODO: Add your update logic here

            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.CornflowerBlue);

            spriteBatch.Begin();
            HtmlDevice.Context = spriteBatch;
            compiler.Draw(0);
            spriteBatch.End();

            base.Draw(gameTime);
        }
    }
}
