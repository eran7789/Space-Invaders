using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ObjectModel.Screens.Menus
{
    public class ToggleMenuItem : MenuItem
    {
        private bool m_Value;
        public bool Value
        {
            get { return m_Value; }
        }

        public delegate void ToggleValueChangeEventHandler(bool i_Value);
        public event ToggleValueChangeEventHandler ValueChanged;

        public ToggleMenuItem(Game i_Game, MenuScreen i_Screen, string i_Description, bool i_InitialValue) : 
            base(i_Game, i_Screen, i_Description)
        {
            m_Value = i_InitialValue;
        }

        public override void InitBounds()
        {
            Vector2 measures = m_Font.MeasureString(string.Format("{0}: {1}", m_Description, m_Value.ToString()));
            m_Width = measures.X;
            m_Height = measures.Y;
        }

        public override void Draw(GameTime gameTime)
        {
            SpriteBatch.DrawString(m_Font, string.Format("{0}: {1}", m_Description, m_Value.ToString()), Position, Color);
        }

        public override void ValueDown()
        {
            m_Value = !m_Value;
            if (ValueChanged != null)
            {
                ValueChanged.Invoke(m_Value);
            }
        }

        public override void ValueUp()
        {
            ValueDown();
        }
    }
}
