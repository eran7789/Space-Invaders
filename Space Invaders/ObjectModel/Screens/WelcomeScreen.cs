//*** Guy Ronen © 2008-2011 ***//
using System;
using Space_Invaders.ObjectModel.Sprites;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.ObjectModel.Screens.Menus;

namespace Space_Invaders.ObjectModel.Screens
{
    public class WelcomeScreen : GameScreen
    {
        Sprite m_WelcomeMessage;
        SpriteFont m_Font;
        Background m_Background;
        private LevelTransitionScreeen m_TransitionionScreen;
        private PlayGameScreen m_GameScreen;

        public WelcomeScreen(Game i_Game, PlayGameScreen i_GameScreen, LevelTransitionScreeen i_TransitionScreen)
            : base (i_Game)
        {
            m_GameScreen = i_GameScreen;
            m_TransitionionScreen = i_TransitionScreen;
            m_Background = new Background(i_Game, this, @"Sprites\BG_Space01_1024x768", 1);
            this.Add(m_Background);

            m_WelcomeMessage = new Sprite(this.Game, this, @"Sprites\SpaceInvadersLogo");
            this.Add(m_WelcomeMessage);
        }

        public override void Initialize()
        {
            base.Initialize();

            m_WelcomeMessage.Animations.Add(new PulseAnimator("Pulse", TimeSpan.Zero, 1.05f, 0.7f));
            m_WelcomeMessage.Animations.Enabled = true;
            m_WelcomeMessage.Animations["Pulse"].Enabled = true;
            m_WelcomeMessage.RotationOrigin = m_WelcomeMessage.SourceRectangleCenter;
            m_WelcomeMessage.RotationOrigin = m_WelcomeMessage.SourceRectangleCenter;
            m_WelcomeMessage.Position = CenterOfViewPort - m_WelcomeMessage.RotationOrigin - new Vector2(0, 100);           
        }

        protected override void LoadContent()
        {
            base.LoadContent();

            m_Font = ContentManager.Load<SpriteFont>(@"Fonts/Consolas");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Enter))
            {
                ExitScreen();
            }

            if (InputManager.KeyPressed(Keys.M))
            {
                MainMenu mainMenu = new MainMenu(Game, m_TransitionionScreen, m_GameScreen);
                ScreensManager.SetCurrentScreen(mainMenu);
            }

            if (InputManager.KeyPressed(Keys.Escape))
            {
                this.Game.Exit();
            }

            if (this.TransitionPosition != 1 && this.TransitionPosition != 0)
            {
                m_Background.Alpha = this.TransitionPosition;
                m_WelcomeMessage.Alpha = this.TransitionPosition;
            }
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(
                 m_Font,
                 "Press Enter To Start The Game",
                 this.CenterOfViewPort + new Vector2(-130, 100),
                 Color.DarkRed);
            m_SpriteBatch.DrawString(
                 m_Font,
                 "Press esc To Exit",
                 this.CenterOfViewPort + new Vector2(-130, 120),
                 Color.DarkRed);
            m_SpriteBatch.DrawString(
                 m_Font,
                 "Press M To Show Main Menu",
                 this.CenterOfViewPort + new Vector2(-130, 140),
                 Color.DarkRed);
            m_SpriteBatch.End();
        }
    }
}
