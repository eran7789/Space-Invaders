using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ObjectModel.Screens.Menus
{
    public class PrecentValueMenuItem : MenuItem
    {
        private int m_Value;
        public int Value
        {
            get { return m_Value; }
        }

        public delegate void PrecentValueChangeEventHandler(int i_Value);
        public event PrecentValueChangeEventHandler ValueChanged;

        public PrecentValueMenuItem(Game i_Game, MenuScreen i_Screen, string i_Description, int i_InitialValue) : 
            base(i_Game, i_Screen, i_Description)
        {
            m_Value = MathHelper.Clamp(i_InitialValue, 0, 100);
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
            m_Value--;
            m_Value = MathHelper.Clamp(m_Value, 0, 100);
            if (ValueChanged != null)
            {
                ValueChanged.Invoke(m_Value);
            }
        }

        public override void ValueUp()
        {
            m_Value++;
            m_Value = MathHelper.Clamp(m_Value, 0, 100);
            if (ValueChanged != null)
            {
                ValueChanged.Invoke(m_Value);
            }
        }
    }
}
