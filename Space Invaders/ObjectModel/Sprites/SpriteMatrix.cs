using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public abstract class SpriteMatrix : Sprite
    {
        protected float m_RowSpace;
        protected float m_ColSpace;
        protected int m_Rows;
        protected int m_Cols;
        protected int m_MinRowIndex = 0;
        protected int m_MinColIndex = 0;
        protected Sprite[,] m_Matrix;
        protected float m_SpriteHeight;
        protected float m_SpriteWidth;
        protected int m_SpriteCount;

        public override SpriteBatch SpriteBatch
        {
            get { return base.SpriteBatch; }
            set
            {
                base.SpriteBatch = value;
                foreach (Sprite sprite in m_Matrix)
                {
                    sprite.SpriteBatch = value;
                }
            }
        }

        public List<Sprite> Sprites
        {
            get
            {
                List<Sprite> spriteList = new List<Sprite>();
                foreach (Sprite sprite in m_Matrix)
                {
                    spriteList.Add(sprite);
                }

                return spriteList;
            }
        }

        public SpriteMatrix(
            Game i_Game, 
            GameScreen i_Screen, 
            int i_NumOfRows, 
            int i_NumOfCols, 
            float i_RowSpace, 
            float i_ColSpace)
            : this(i_Game, i_Screen, i_NumOfRows, i_NumOfCols)
        {
            m_RowSpace = i_RowSpace;
            m_ColSpace = i_ColSpace;
        }

        public SpriteMatrix(
            Game i_Game, GameScreen i_Screen, int i_NumOfRows, int i_NumOfCols)
            : base(i_Game, i_Screen, null)
        {
            m_Rows = i_NumOfRows;
            m_Cols = i_NumOfCols;
        }

        protected override void LoadContent()
        {
            if (SpriteBatch == null)
            {
                SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            }
        }

        public override void Initialize()
        {
            this.Visible = true;
            initSprites();
            initBounds();
            initPosition();
            updateSpritePositions();
            initSpriteBatch();
            m_SpriteCount = m_Rows * m_Cols;
        }

        private void initSprites()
        {
            foreach (Sprite sprite in m_Matrix)
            {
                sprite.Initialize();
            }
        }

        private void initSpriteBatch()
        {
            foreach (Sprite sprite in m_Matrix)
            {
                sprite.SpriteBatch = SpriteBatch;
            }
        }

        private void updateSpritePositions()
        {
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    m_Matrix[i, j].Position =
                        new Vector2(
                        (j * (m_ColSpace + m_SpriteWidth)) + Position.X,
                            (i * (m_RowSpace + m_SpriteHeight)) + Position.Y);
                }
            }
        }

        protected void updatePosition(Vector2 i_NewPosition)
        {
            m_Position = i_NewPosition;
            updateSpritePositions();
        }

        public override void Draw(GameTime i_GameTime)
        {
            foreach (Sprite sprite in m_Matrix)
            {
                sprite.Draw(i_GameTime);
            }
        }

        public void individualUpdate(GameTime i_GameTime)
        {
            foreach (Sprite sprite in m_Matrix)
            {
                sprite.Update(i_GameTime);
            }
        }

        protected void minimizeBounds()
        {
            int maxColIndex = 0, minColIndex = m_Cols, maxRowIndex = 0, minRowIndex = m_Rows;
            for (int i = 0; i < m_Rows; i++)
            {
                for (int j = 0; j < m_Cols; j++)
                {
                    if (m_Matrix[i, j].Visible)
                    {
                        if (maxColIndex < j)
                        {
                            maxColIndex = j;
                        }

                        if (minColIndex > j)
                        {
                            minColIndex = j;
                        }

                        if (maxRowIndex < i)
                        {
                            maxRowIndex = i;
                        }

                        if (minRowIndex > i)
                        {
                            minRowIndex = i;
                        }
                    }
                }
            }

            m_Width = (maxColIndex + 1) * (m_SpriteWidth + m_ColSpace);
            m_Height = (maxRowIndex + 1) * (m_SpriteHeight + m_RowSpace);
            m_MinRowIndex = minRowIndex;
            m_MinColIndex = minColIndex;
        }

        protected void initBounds()
        {
            m_SpriteHeight = m_Matrix[0, 0].Height;
            m_SpriteWidth = m_Matrix[0, 0].Width;
            m_Height = (int)(m_Rows * (m_SpriteHeight + m_RowSpace));
            m_Width = (int)(m_Cols * (m_SpriteWidth + m_ColSpace));
        }

        public override void MoveToLevel(int i_Level)
        {
            foreach (Sprite sprite in m_Matrix)
            {
                sprite.MoveToLevel(i_Level);
            }
            base.MoveToLevel(i_Level);
        }

        protected abstract void initPosition();
    }
}
