//*** Guy Ronen © 2008-2011 ***//
using System;
using Space_Invaders.ObjectModel.Sprites;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.ObjectModel.Screens.Menus;

namespace Space_Invaders.ObjectModel.Screens
{
    public class GameOverScreen : GameScreen
    {
        string m_GameOverMessage;
        string m_Player1Score;
        string m_Player2Score;
        SpriteFont m_FontHuge;
        SpriteFont m_FontMedium;
        SpriteFont m_FontSmall;
        private LevelTransitionScreeen m_TransitionionScreen;
        private PlayGameScreen m_GameScreen;

        public string Message
        {
            set { this.m_GameOverMessage = value; }
            get { return this.m_GameOverMessage; }
        }

        public string Player1Score
        {
            get { return m_Player1Score; }
            set { m_Player1Score = value; }
        }

        public string Player2Score
        {
            get { return m_Player2Score; }
            set { m_Player2Score = value; }
        }
        
        public GameOverScreen(Game i_Game, PlayGameScreen i_GameScreen, LevelTransitionScreeen i_TransitionScreen)
            : base(i_Game)
        {
            m_GameScreen = i_GameScreen;
            m_TransitionionScreen = i_TransitionScreen;
            this.IsModal = true;
            this.IsOverlayed = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            BlackTintAlpha = 0.7f;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontMedium = ContentManager.Load<SpriteFont>(@"Fonts/ConsolasMedium");
            m_FontSmall = ContentManager.Load<SpriteFont>(@"Fonts/Consolas");
            m_FontHuge = ContentManager.Load<SpriteFont>(@"Fonts/ConsolasHuge");
        }

        public override void Update(GameTime gameTime)
        {
            if (InputManager.KeyPressed(Keys.Home))
            {
                m_GameScreen.Reset();
                ScreensManager.SetCurrentScreen(m_GameScreen);
                ScreensManager.SetCurrentScreen(m_TransitionionScreen);
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
        }

        public override void Draw(GameTime gameTime)
        {
            base.Draw(gameTime);
            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(
                 m_FontHuge,
                 m_GameOverMessage,
                 new Vector2(this.CenterOfViewPort.X - (m_FontHuge.MeasureString(m_GameOverMessage).X / 2), 0),
                 Color.Red);
            m_SpriteBatch.DrawString(
                 m_FontMedium,
                 m_Player1Score,
                 new Vector2(this.CenterOfViewPort.X - (m_FontMedium.MeasureString(m_Player1Score).X / 2), this.ViewPortBounds.Y / 6),
                 Color.Red);
            m_SpriteBatch.DrawString(
                 m_FontMedium,
                 m_Player2Score,
                 new Vector2(this.CenterOfViewPort.X - (m_FontMedium.MeasureString(m_Player2Score).X / 2), (this.ViewPortBounds.Y / 6) * 2),
                 Color.Red);
            string message = "Press Home To Start Over";
            m_SpriteBatch.DrawString(
                 m_FontSmall,
                 message,
                  new Vector2(this.CenterOfViewPort.X - (m_FontSmall.MeasureString(message).X / 2), (this.ViewPortBounds.Y / 6) * 4),
                 Color.Red);
            message = "Press esc To Exit";
            m_SpriteBatch.DrawString(
                 m_FontSmall,
                 message,
                 new Vector2(this.CenterOfViewPort.X - (m_FontSmall.MeasureString(message).X / 2), (this.ViewPortBounds.Y / 6) * 4 + 20),
                 Color.Red);
            message = "Press M To Show Main Menu";
            m_SpriteBatch.DrawString(
                 m_FontSmall,
                 message,
                  new Vector2(this.CenterOfViewPort.X - (m_FontSmall.MeasureString(message).X / 2), (this.ViewPortBounds.Y / 6) * 4 + 40),
                 Color.Red);
            m_SpriteBatch.End();
        }
    }
}
