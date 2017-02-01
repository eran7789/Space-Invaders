using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Screens;

namespace Infrastructure.ObjectModel
{
    public class Sprite : DrawableGameComponent, IDrawable
    {
        protected readonly string r_AssetName;
        protected Vector2 m_Position;
        protected float m_Height;
        protected float m_Width;
        protected Texture2D m_Texture;
        protected Color m_Color;
        private SpriteBatch m_SpriteBatch;
        protected Vector2 m_Velocity;
        protected Color[] m_Pixels;
        protected List<Vector2> m_BorderPixelsPosition;
        protected Rectangle m_SourceRectangle;
        protected bool isUsingSourceRectangle = false;
        protected CompositeAnimator m_Animator;
        protected Vector2 m_RotationOrigin = Vector2.Zero;
        protected float m_Rotation = 0;
        protected Vector2 m_Scales = Vector2.One;
        private bool m_IsAlive = true;
        private float m_Alpha = 1f;
        private GameScreen m_Screen;

        public GameScreen Screen
        {
            get { return m_Screen; }
            set
            {
                m_Screen = value;
                m_Screen.Add(this);
            }
        }
        
        public Vector2 RotationOrigin
        {
            get { return m_RotationOrigin; }
            set { m_RotationOrigin = value; }
        }

        public Vector2 SourceRectangleCenter
        {
            get { return new Vector2((float)(this.m_Width / 2), (float)(this.m_Height / 2)); }
        }

        public CompositeAnimator Animations
        {
            get { return m_Animator; }
        }
        public bool IsAlive
        {
            get { return m_IsAlive; }
            set { m_IsAlive = value; }
        }

        public Vector2 Scales
        {
            get { return m_Scales; }
            set
            {
                if (m_Scales != value)
                {
                    m_Scales = value;
                }
            }
        }

        public float Rotation
        {
            get { return m_Rotation; }
            set { m_Rotation = value; }
        }
        
        public bool IsUsingSourceRectangle
        {
            get { return isUsingSourceRectangle; }
        }

        public Color[] Pixels
        {
            get { return m_Pixels; }
            set
            {
                m_Pixels = value;
                this.m_Texture.SetData(value);
                mapBorderPixels();
            }
        }

        public List<Vector2> BorderPixels
        {
            get { return m_BorderPixelsPosition; }
        }

        public float Height
        {
            get { return m_Height; }
        }

        public float Width
        {
            get { return m_Width; }
        }

        public Vector2 Position
        {
            get { return m_Position; }
            set { m_Position = value; }
        }

        public Color Color
        {
            get { return m_Color; }
        }

        public float Alpha
        {
            get { return m_Alpha; }
            set
            {
                m_Alpha = MathHelper.Clamp(value, 0, 1);
                m_Color.A = Convert.ToByte(m_Alpha * 255);
            }
        }

        public virtual SpriteBatch SpriteBatch
        {
            get { return m_SpriteBatch; }
            set { m_SpriteBatch = value; }
        }

        public Rectangle Bounds
        {
            get { return new Rectangle((int)Position.X, (int)Position.Y, (int)Width, (int)Height); }
        }

        public Sprite(Game game, GameScreen i_Screen, string i_AssetName) : 
            this(game, i_Screen, i_AssetName, Color.White)
        { }

        public Sprite(Game game, GameScreen i_Screen, string i_AssetName, Rectangle i_SourceRectangle) : 
            this(game, i_Screen, i_AssetName, Color.White)
        {
            m_SourceRectangle = i_SourceRectangle;
            isUsingSourceRectangle = true;
        }

        public Sprite(Game game, GameScreen i_Screen, string i_AssetName, Color i_Color) : base(game)
        {
            r_AssetName = i_AssetName;
            m_Color = i_Color;
            m_Animator = new CompositeAnimator(this);
            m_Animator.Enabled = true;
            m_Screen = i_Screen;
        }

        public override void Initialize()
        {
            this.Rotation = 0;
            this.Scales = new Vector2(1);

            base.Initialize();
            Visible = true;
            m_IsAlive = true;
        }

        public virtual void InitBounds()
        {
            m_Position = new Vector2(0, 0);
            if (!isUsingSourceRectangle)
            {
                m_Height = m_Texture.Bounds.Height;
                m_Width = m_Texture.Bounds.Width;
            }
            else
            {
                m_Height = m_SourceRectangle.Height;
                m_Width = m_SourceRectangle.Width;
            }

            this.m_RotationOrigin = new Vector2(this.Width / 2, this.Height / 2);
        }

        protected override void LoadContent()
        {
            m_Texture = Game.Content.Load<Texture2D>(r_AssetName);
            if (m_SpriteBatch == null)
            {
                m_SpriteBatch = new SpriteBatch(Game.GraphicsDevice);
            }

            InitBounds();
            m_Pixels = new Color[(int)m_Width * (int)m_Height];
            this.m_Texture.GetData(m_Pixels);
            mapBorderPixels();
        }

        public override void Draw(GameTime gameTime)
        {
            if (Visible)
            {
                if (isUsingSourceRectangle)
                {
                    m_SpriteBatch.Draw(
                        m_Texture, m_Position + m_RotationOrigin, m_SourceRectangle, m_Color, 
                        Rotation, m_RotationOrigin, Scales, SpriteEffects.None, 0);
                }
                else
                {
                    m_SpriteBatch.Draw(
                        m_Texture, m_Position + m_RotationOrigin, null, m_Color,
                        Rotation, m_RotationOrigin, Scales, SpriteEffects.None, 0);
                }
            }
        }

        public bool isOnScreen(Vector2 i_Position, bool i_AllowEdgesOut)
        {
            bool isOnScreen = true;
            if (Game.Window != null)
            {
                if (i_Position.X > Game.Window.ClientBounds.Width - m_Width && !i_AllowEdgesOut)
                {
                    isOnScreen = false;
                }
                else if (i_Position.Y > Game.Window.ClientBounds.Height - m_Height && !i_AllowEdgesOut)
                {
                    isOnScreen = false;
                }
                else if ((i_Position.Y < 0 || i_Position.X < 0) && !i_AllowEdgesOut)
                {
                    isOnScreen = false;
                }
                else if (i_Position.Y > Game.Window.ClientBounds.Height || i_Position.X > Game.Window.ClientBounds.Width)
                {
                    isOnScreen = false;
                }
                else if ((i_Position.X < 0 - m_Width || i_Position.Y < 0 - m_Height) && i_AllowEdgesOut)
                {
                    isOnScreen = false;
                }
            }
            else
            {
                isOnScreen = false;
            }

            return isOnScreen;
        }
        
        private void mapBorderPixels()
        {
            List<Vector2> borderPixels = new List<Vector2>();
            for (int i = 0; i < m_Pixels.Length; i++)
            {
                if (m_Pixels[i].A != 0)
                {
                    borderPixels.Add(new Vector2(i % m_Width, i / m_Width));
                }
            }

            m_BorderPixelsPosition = borderPixels;
        }

        public bool isHitPixelwise(Sprite i_Sprite)
        {
            bool isShoot = false;

            foreach (Vector2 spritePixel in i_Sprite.BorderPixels)
            {
                if (isShoot)
                {
                    break;
                }

                foreach (Vector2 myPixel in this.BorderPixels)
                {
                    Vector2 spritePixelScreenPosition = new Vector2(
                        (float)Math.Round(spritePixel.X + i_Sprite.Position.X),
                        (float)Math.Round(spritePixel.Y + i_Sprite.Position.Y));
                    Vector2 bulletPixelScreenPosition = new Vector2(
                        (float)Math.Round(myPixel.X + this.Position.X),
                        (float)Math.Round(myPixel.Y + this.Position.Y));
                    if (spritePixelScreenPosition.Equals(bulletPixelScreenPosition))
                    {
                        isShoot = true;
                        break;
                    }
                }
            }

            return isShoot;
        }

        public Sprite ShallowClone()
        {
            return (Sprite)this.MemberwiseClone();
        }

        public virtual void MoveToLevel(int i_Level)
        {
            this.Initialize();
        }
    }
}
