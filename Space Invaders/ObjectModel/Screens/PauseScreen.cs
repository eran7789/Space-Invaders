using Infrastructure.ObjectModel.Screens;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Space_Invaders.ObjectModel.Screens
{
    public class PauseScreen : GameScreen
    {
        SpriteFont m_FontMedium;
        SpriteFont m_FontSmall;

        public PauseScreen(Game i_Game) : base(i_Game)
        {
            this.IsModal = true;
            this.IsOverlayed = true;
        }

        public override void Initialize()
        {
            base.Initialize();
            BlackTintAlpha = 0.4f; 
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontSmall = ContentManager.Load<SpriteFont>(@"Fonts/Consolas");
            m_FontMedium = ContentManager.Load<SpriteFont>(@"Fonts/ConsolasMedium");
        }

        public override void Update(GameTime i_GameTime)
        {
            base.Update(i_GameTime);
            if (InputManager.KeyPressed(Keys.R))
            {
                ExitScreen();
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            string message = "Game is Paused";
            Vector2 measures = m_FontMedium.MeasureString(message);

            base.Draw(i_GameTime);
            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(
                 m_FontMedium,
                 message,
                 this.CenterOfViewPort - (measures / 2),
                 Color.LawnGreen);
            message = "press R to continue";
            measures = m_FontSmall.MeasureString(message);
            m_SpriteBatch.DrawString(
                 m_FontSmall,
                 message,
                 new Vector2(this.CenterOfViewPort.X - (measures.X / 2), this.CenterOfViewPort.Y + 92),
                 Color.LawnGreen);
            m_SpriteBatch.End();
        }
    }
}
