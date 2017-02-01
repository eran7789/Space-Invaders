using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.ObjectModel.Screens.Menus
{
    public class MenuItem : Sprite
    {
        private const string k_Active = "active";
        private Color m_ActiveColor;
        private Color m_InactiveColor;
        protected string m_Description;
        protected SpriteFont m_Font;
        private GameScreen m_NextMenu;
        
        public ContentManager ContentManager
        {
            get { return this.Game.Content; }
        }

        public GameScreen ChoiseScreen
        {
            get { return m_NextMenu; }
        }

        private bool m_IsActive;
        public bool IsActive
        {
            get { return m_IsActive; }
            set
            {
                m_IsActive = value;
                OnActiveChanged(value);
            }
        }

        public MenuItem(Game i_Game, MenuScreen i_Screen, string i_Description) : 
            this(i_Game, i_Screen, i_Description, Color.White, Color.Red)
        { }

        public MenuItem(Game i_Game, MenuScreen i_Screen, GameScreen i_NextScreen, string i_Description) :
            this(i_Game, i_Screen, i_Description, Color.White, Color.Red)
        {
            m_NextMenu = i_NextScreen;
        }

        public MenuItem(Game i_Game, MenuScreen i_Screen, string i_Description, Color i_InactiveColor, Color i_ActiveColor) : 
            base(i_Game, i_Screen, "")
        {
            m_Color = i_InactiveColor;
            m_Description = i_Description;
            m_InactiveColor = i_InactiveColor;
            m_ActiveColor = i_ActiveColor;
            m_Animator = new CompositeAnimator(this);
            m_Animator.Add(new Animators.ConcreteAnimators.PulseAnimator(k_Active, TimeSpan.Zero, 1.5f, 2)); 
        }

        private void OnActiveChanged(bool value)
        {
            if (value)
            {
                m_Animator.Enabled = true;
                m_Animator[k_Active].Enabled = true;
                m_Color = m_ActiveColor;
            }
            else
            {
                m_Animator.Enabled = false;
                m_Animator[k_Active].Enabled = false;
                m_Color = m_InactiveColor;
            }
        }

        protected override void LoadContent()
        {
            m_Font = ContentManager.Load<SpriteFont>(@"Fonts/Consolas");
            InitBounds();
        }

        public override void InitBounds()
        {
            Vector2 measures = m_Font.MeasureString(m_Description);
            m_Height = measures.Y;
            m_Width = measures.X;
        }

        public override void Draw(GameTime gameTime)
        {

            SpriteBatch.DrawString(m_Font, m_Description, Position, Color);
        }

        public virtual void ValueDown() { }
        public virtual void ValueUp() { }
    }
}
