using Infrastructure.ObjectModel.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Space_Invaders.ObjectModel.Screens
{
    public class LevelTransitionScreeen : GameScreen
    {
        private int m_Level;
        private float m_TimeElapsedToOneSecond;
        SpriteFont m_FontHuge;
        SpriteFont m_FontMedium;
        private int m_TimeToShow;
        private const int k_TimeBeforeNextScreen = 3;

        public int Level
        {
            set { m_Level = value; }
        }

        public LevelTransitionScreeen(Game i_Game) : base(i_Game)
        {
            IsOverlayed = true;
            IsModal = true;
        }

        public LevelTransitionScreeen(Game i_Game, int i_Level) : this(i_Game)
        {
            m_Level = i_Level;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_TimeElapsedToOneSecond= 0;
            m_TimeToShow = k_TimeBeforeNextScreen;
            this.BlackTintAlpha = 0.8f;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontHuge = ContentManager.Load<SpriteFont>(@"Fonts/ConsolasHuge");
            m_FontMedium = ContentManager.Load<SpriteFont>(@"Fonts/ConsolasMedium");
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            m_TimeElapsedToOneSecond += (float)i_GameTime.ElapsedGameTime.TotalSeconds;

            if (m_TimeElapsedToOneSecond >= 1)
            {
                m_TimeElapsedToOneSecond -= 1;
                m_TimeToShow--;
            }

            if (m_TimeToShow == 0)
            {
                ExitScreen();
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            base.Draw(i_GameTime);
            string message = String.Format("Starting level {0} in:", m_Level);
            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(
                 m_FontMedium,
                 message,
                 this.CenterOfViewPort - (m_FontMedium.MeasureString(message) / 2) - new Vector2(0, 100),
                 Color.LawnGreen);
            message = string.Format("{0}", m_TimeToShow);
            m_SpriteBatch.DrawString(
                 m_FontHuge,
                 message,
                 this.CenterOfViewPort - (m_FontHuge.MeasureString(message) / 2),
                 Color.LawnGreen);
            m_SpriteBatch.End();
        }
    }
}
