using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Space_Invaders.Managers;
using Space_Invaders.ObjectModel;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class EnemySpaceShipsMatrix : SpriteMatrix
    {
        private const int k_DrawOrder = 2;
        private const int k_Level1NumOfRows = 5;
        private const int k_Level1NumOfCols = 9;
        private const float k_RowSpace = 32f * 0.6f;
        private const float k_ColSpace = k_RowSpace;

        private float m_SecondsToJump;
        private float m_SecondsElapsedSinceMove;
        private bool m_ChangeDirection = false;
        private ShootsManager m_ShootsManager;
        private SoundEffectsManager m_SoundManager;
        private GameScreen m_Screen;

        public float XGap
        {
            get
            {
                return m_MinColIndex * (m_SpriteWidth + k_ColSpace);
            }
        }

        public float YGap
        {
            get
            {
                return m_MinRowIndex * (m_SpriteHeight + k_RowSpace);
            }
        }
        
        public EnemySpaceShipsMatrix(
            Game i_Game, 
            GameScreen i_Screen, 
            ShootsManager i_ShootsManager) : 
                base(i_Game, i_Screen, k_Level1NumOfRows, k_Level1NumOfCols, k_RowSpace, k_ColSpace)
        {
            m_ShootsManager = i_ShootsManager;
            m_SoundManager = SoundEffectsManager.GetInstance(i_Game);
            m_Screen = i_Screen;
            DrawOrder = k_DrawOrder;
            allocateAndInitMatrix(k_Level1NumOfRows, k_Level1NumOfCols);
        }

        private void allocateAndInitMatrix(int i_Rows, int i_Cols)
        {
            base.m_Rows = i_Rows;
            base.m_Cols = i_Cols;
            m_Matrix = new EnemySpaceShip[i_Rows, i_Cols];
            for (int i = 0; i < base.m_Rows; i++)
            {
                for (int j = 0; j < base.m_Cols; j++)
                {
                    if (i < 1)
                    {
                        m_Matrix[i, j] = new EnemySpaceShipPink(Game, m_Screen, m_ShootsManager);
                    }
                    else if (i < 3)
                    {
                        m_Matrix[i, j] = new EnemySpaceShipBlue(Game, m_Screen, m_ShootsManager);
                    }
                    else
                    {
                        m_Matrix[i, j] = new EnemySpaceShipYellow(Game, m_Screen, m_ShootsManager);
                    }

                    (m_Matrix[i, j] as EnemySpaceShip).GotHit += spaceShipGotHit;
                }
            }
        }

        public override void Initialize()
        {
            base.Initialize();
            m_SecondsElapsedSinceMove = 0;
            m_SecondsToJump = 0.5f;
            m_Velocity = new Vector2(m_SpriteWidth / 2, 0);
        }        
        
        protected override void initPosition()
        {
            m_Position = new Vector2(0, 3 * m_SpriteHeight);
        }
       
        public override void Update(GameTime i_GameTime)
        {
            if (m_ChangeDirection)
            {
                changeDirecton();
                m_ChangeDirection = !m_ChangeDirection;
            }

            Vector2 newPosition = this.m_Position + this.m_Velocity;
            bool isHeatingTheSide = Game.Window != null ? 
                (newPosition.X + Width >
                            Game.Window.ClientBounds.Width || newPosition.X + XGap < 0) 
                                : true;
            m_SecondsElapsedSinceMove += (float)i_GameTime.ElapsedGameTime.TotalSeconds;
            if (m_SecondsElapsedSinceMove >= m_SecondsToJump)
            {
                switchSourceRectangles();
                if (isHeatingTheSide)
                {
                    jumpDown(i_GameTime);
                    m_ChangeDirection = true;
                }
                else
                {
                    updatePosition(newPosition);
                    m_SecondsElapsedSinceMove -= m_SecondsToJump;
                }
            }

            individualUpdate(i_GameTime);
            
            base.Update(i_GameTime);
        }
        
        private void changeDirecton()
        {
            this.m_Velocity.X *= -1;
            increaseSpeed();
        }

        private void increaseSpeed()
        {
            m_SecondsToJump -= m_SecondsToJump * 0.04f;
            Console.WriteLine("second to jump = " + m_SecondsToJump);
        }

        private void jumpDown(GameTime i_GameTime)
        {
            Vector2 newPosition = new Vector2(this.m_Position.X, this.m_Position.Y + (m_SpriteHeight / 2));
            updatePosition(newPosition);
        }
        
        private void spaceShipGotHit(object i_Sender, EventArgs i_Args)
        {
            increaseSpeed();
            minimizeBounds();
            m_SpriteCount--;
            if (m_SpriteCount == 0)
            {
                this.Visible = false;
            }
        }

        private void switchSourceRectangles()
        {
            foreach (EnemySpaceShip spaceship in m_Matrix)
            {
                spaceship.SwitchSourceRectangle();
            }
        }

        public override void MoveToLevel(int i_Level)
        {
            int level = i_Level % 4;
            switch (level)
            {
                case 0:
                    allocateAndInitMatrix(k_Level1NumOfRows, k_Level1NumOfCols);
                    break;
                case 1:
                    allocateAndInitMatrix(k_Level1NumOfRows, k_Level1NumOfCols + 1);
                    break;
                case 2:
                    allocateAndInitMatrix(k_Level1NumOfRows, k_Level1NumOfCols + 2);
                    break;
                case 3:
                    allocateAndInitMatrix(k_Level1NumOfRows, k_Level1NumOfCols + 3);
                    break;
            }
            this.Initialize();
            base.MoveToLevel(i_Level);
        }
    }
}
