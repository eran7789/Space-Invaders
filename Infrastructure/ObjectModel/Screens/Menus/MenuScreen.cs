using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Graphics;

namespace Infrastructure.ObjectModel.Screens.Menus
{
    public abstract class MenuScreen : GameScreen
    {
        private readonly string r_Headline;
        private List<MenuItem> m_Items;
        private int m_ActiveItemIndex;
        private SoundEffectInstance m_ActiveChangedSound;
        private SpriteFont m_FontMedium;
        private SpriteFont m_FontSmall;
        private float totalItemsHeight;
        private bool m_InitializedPositions;

        public MenuScreen(Game i_Game, string i_HeadLine) : base(i_Game)
        {
            r_Headline = i_HeadLine;
            this.m_Items = new List<MenuItem>();
            this.IsModal = true;
            this.IsOverlayed = true;
        }

        public void AddItem(MenuItem i_Item)
        {
            this.Add(i_Item);
            m_Items.Add(i_Item);
        }

        public void RemoveItem(MenuItem i_Item)
        {
            this.Remove(i_Item);
            m_Items.Add(i_Item);
        }

        public override void Initialize()
        {
            base.Initialize();
            BlackTintAlpha = 0.85f;

            if (!m_InitializedPositions)
            {
                initializePostions();
                m_InitializedPositions = true;
            }
        }

        private void initializePostions()
        {
            foreach (MenuItem item in m_Items)
            {
                totalItemsHeight += item.Height;
            }

            float nextYPosition = CenterOfViewPort.Y - totalItemsHeight / 2;
            foreach (MenuItem item in m_Items)
            {
                item.Position = new Vector2(CenterOfViewPort.X - item.Width / 2, nextYPosition);
                nextYPosition += item.Height + 10;
            }
        }

        protected override void OnActivated()
        {
            base.OnActivated();
            foreach (MenuItem item in m_Items)
            {
                item.IsActive = false;
            }

            if (m_Items.Count > 0)
            {
                m_Items[0].IsActive = true;
                m_ActiveItemIndex = 0;
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_FontMedium = ContentManager.Load<SpriteFont>(@"Fonts/ConsolasMedium");
            m_FontSmall = ContentManager.Load<SpriteFont>(@"Fonts/Consolas");
        }

        public override void Update(GameTime gameTime)
        {
            base.Update(gameTime);

            if (InputManager.KeyPressed(Keys.Up))
            {
                m_Items[m_ActiveItemIndex].IsActive = false;
                m_ActiveItemIndex--;
                if (m_ActiveItemIndex < 0)
                {
                    m_ActiveItemIndex = m_Items.Count - 1;
                }
                m_Items[m_ActiveItemIndex].IsActive = true;
                ActiveItemChanged();
            }
            else if (InputManager.KeyPressed(Keys.Down))
            {
                m_Items[m_ActiveItemIndex].IsActive = false;
                m_ActiveItemIndex++;
                if (m_ActiveItemIndex >= m_Items.Count)
                {
                    m_ActiveItemIndex = 0;
                }
                m_Items[m_ActiveItemIndex].IsActive = true;
                ActiveItemChanged();
            }

            if (InputManager.KeyPressed(Keys.Enter))
            {
                if (m_Items[m_ActiveItemIndex].ChoiseScreen != null)
                {
                    ExitScreen();
                    ScreensManager.SetCurrentScreen(m_Items[m_ActiveItemIndex].ChoiseScreen);
                }
                else if (m_Items[m_ActiveItemIndex] is FunctionMenuItem)
                {
                    (m_Items[m_ActiveItemIndex] as FunctionMenuItem).MyFunction.Invoke();
                }
            }

            if (InputManager.KeyPressed(Keys.PageDown))
            {
                m_Items[m_ActiveItemIndex].ValueDown();
            }
            else if (InputManager.KeyPressed(Keys.PageUp))
            {
                m_Items[m_ActiveItemIndex].ValueUp();
            }
        }

        public override void Draw(GameTime i_GameTime)
        {
            Vector2 headlineMeasures = m_FontMedium.MeasureString(r_Headline);

            base.Draw(i_GameTime);
            m_SpriteBatch.Begin();
            m_SpriteBatch.DrawString(
                m_FontMedium, 
                r_Headline, 
                new Vector2(CenterOfViewPort.X - (headlineMeasures.X / 2), 10), 
                Color.White);
            m_SpriteBatch.End();
        }

        protected abstract void ActiveItemChanged();
    }
}
