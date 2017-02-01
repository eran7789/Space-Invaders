using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class Bullet : Sprite
    {
        public Bullet(Game i_Game, GameScreen i_Screen, Color i_MyColor, SpriteBatch i_SpriteBatch) 
            : base(i_Game, i_Screen, @"Sprites/Bullet", i_MyColor)
        {
            SpriteBatch = i_SpriteBatch;
            DrawOrder = int.MinValue;
            if (i_MyColor == Color.Red)
            {
                m_Velocity = new Vector2(0, -120f);
            }
            else
            {
                m_Velocity = new Vector2(0, 120f);
            }
        }

        public override void Update(GameTime gameTime)
        {
            const bool k_AllowEdgesOut = true;
            this.m_Position += this.m_Velocity * (float)gameTime.ElapsedGameTime.TotalSeconds;
            if (!isOnScreen(m_Position, k_AllowEdgesOut))
            {
                this.Destroy();
            }

            base.Update(gameTime);
        }

        public void Destroy()
        {
            this.Visible = false;
        }

        public override void MoveToLevel(int i_Level)
        {
            base.MoveToLevel(i_Level);
            this.IsAlive = false;
            this.Visible = false;
        }
    }
}
