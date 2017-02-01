using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class WallMatrix : SpriteMatrix
    {
        private const int k_NumOfRows = 1;
        private const int k_NumOfCols = 4;
        private const float k_PixelsToMoveInDirection = 22f;

        private float m_PixelsMovedSinceChangeDirection;
        private bool m_ChangeDiretion = false;

        public WallMatrix(
            Game i_Game, 
            GameScreen i_Screen, 
            ShootsManager i_ShootsManager) : 
                base(i_Game, i_Screen, k_NumOfRows, k_NumOfCols)
        {
            m_Matrix = new Wall[k_NumOfRows, k_NumOfCols];
            for (int i = 0; i < k_NumOfRows; i++)
            {
                for (int j = 0; j < k_NumOfCols; j++)
                {
                    m_Matrix[i, j] = new Wall(i_Game, this.Screen, i_ShootsManager);
                }
            }
            m_ColSpace = 44;
            m_Velocity = Vector2.Zero;
            m_PixelsMovedSinceChangeDirection = 0;
        }

        protected override void initPosition()
        {
            Vector2 position = new Vector2(0);
            position.X = (Game.Window.ClientBounds.Width / 2) - (this.Width / 2);
            position.Y = (Game.GraphicsDevice.Viewport.Height) - 4 * 32;
            m_Position = position;
        }

        public override void Update(GameTime i_GameTime)
        {
            Vector2 newPosition = this.Position + (this.m_Velocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            m_PixelsMovedSinceChangeDirection += Math.Abs(this.Position.X - newPosition.X);
            m_ChangeDiretion = m_PixelsMovedSinceChangeDirection >= k_PixelsToMoveInDirection;
            updatePosition(newPosition);
            if (m_ChangeDiretion)
            {
                m_Velocity *= -1;
                m_PixelsMovedSinceChangeDirection -= k_PixelsToMoveInDirection;
            }

            individualUpdate(i_GameTime);
            base.Update(i_GameTime);
        }

        public override void MoveToLevel(int i_Level)
        {
            int level = i_Level % 4;
            switch (level)
            {
                case 0:
                    m_Velocity = Vector2.Zero;
                    break;
                case 1:
                    m_Velocity = new Vector2(60, 0);
                    break;
                case 2:
                    m_Velocity *= 0.94f;
                    break;
                case 3:
                    m_Velocity *= 0.94f;
                    break;
            }
            base.MoveToLevel(i_Level);
        }
    }
}
