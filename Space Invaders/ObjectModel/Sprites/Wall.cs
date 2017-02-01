using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Space_Invaders.Managers;
using Microsoft.Xna.Framework.Graphics;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework.Audio;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class Wall : Sprite, IShootable
    {
        private Color[] m_OriginallTexture;

        private ShootsManager m_ShootsManager;
        private SoundEffectsManager m_SoundManager;
        private SoundEffectInstance m_HitSound;

        public ShootsManager ShootsManager
        {
            get { return m_ShootsManager; }
            set { m_ShootsManager = value; }
        }

        public Wall(Game i_Game, GameScreen i_Screen, ShootsManager i_ShootsManager) : 
            base(i_Game, i_Screen, @"Sprites/Barrier_44x32")
        {
            m_ShootsManager = i_ShootsManager;
            m_SoundManager = SoundEffectsManager.GetInstance(i_Game);
        }
        
        public override void Update(GameTime i_GameTime)
        {
            m_ShootsManager.IsEnemyShoot(this);
            base.Update(i_GameTime);
        }

        public override void Initialize()
        {
            base.Initialize();

            Texture2D texture = new Texture2D(GraphicsDevice, m_Texture.Width, m_Texture.Height);
            texture.SetData(m_Pixels);
            m_Texture = texture;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_OriginallTexture = new Color[m_Pixels.Length];
            m_Pixels.CopyTo(m_OriginallTexture, 0);
            Pixels = m_Pixels;
            m_HitSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.BarrierHit);
        }

        public void GotShot(Bullet i_TheShootBullet)
        {
            foreach (Vector2 pixel in i_TheShootBullet.BorderPixels)
            {
                if (pixel.X + i_TheShootBullet.Position.X < this.Position.X + this.Width &&
                    pixel.X + i_TheShootBullet.Position.X > this.Position.X &&
                    pixel.Y + i_TheShootBullet.Position.Y < this.Position.Y + this.Height &&
                    pixel.Y + i_TheShootBullet.Position.Y > this.Position.Y)
                {
                    for (
                        int i = (int)(pixel.Y + i_TheShootBullet.Position.Y - this.Position.Y);
                        i > (int)(pixel.Y + i_TheShootBullet.Position.Y - this.Position.Y) - (i_TheShootBullet.Height * 0.45) && 
                        i < this.Height && i >= 0;
                        i--)
                        {
                            for (
                                int j = (int)(i_TheShootBullet.Position.X - this.Position.X) - 1; 
                                j <= (int)(i_TheShootBullet.Position.X - this.Position.X) + i_TheShootBullet.Width && j < this.Width && j >= 0;
                                j++)
                                {
                                    this.m_Pixels[j + i * (int)m_Width] = Color.FromNonPremultiplied(0, 0, 0, 0);
                                }
                        }
                }
            }

            Pixels = m_Pixels;
            m_HitSound.Play();
        }

        public void GotHit(Rectangle i_overlapArea)
        {
            for (int i = 0; i < i_overlapArea.Height; i++)
            {
                for (int j = 0; j < i_overlapArea.Width; j++)
                {
                    int x = (int)(i_overlapArea.X - this.Position.X) + j;
                    int y = (int)(i_overlapArea.Y - this.Position.Y) + i;
                    m_Pixels[x + y * (int)m_Width] = Color.FromNonPremultiplied(0, 0, 0, 0);
                }
            }
            Pixels = m_Pixels;
        }


        public override void MoveToLevel(int i_Level)
        {
            m_OriginallTexture.CopyTo(m_Pixels, 0);
            Pixels = m_Pixels;
        }
    }
}
