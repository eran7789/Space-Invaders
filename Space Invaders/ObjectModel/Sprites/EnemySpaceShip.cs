using System;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework.Audio;

namespace Space_Invaders.ObjectModel.Sprites
{
    public abstract class EnemySpaceShip : SpaceShip
    {
        protected static Random m_Rnd = new Random();
        protected readonly int r_PointsForKill;
        private const int k_DrawOrder = 2;

        private int m_PointsForKill;
        protected int m_NumOfUpdatesToTryToShoot;
        protected int m_NextShootAttemptCounter;
        protected Rectangle[] m_SourceRectangles;
        protected Bullet m_MyBullet = null;

        public event EventHandler GotHit;

        public int PointsForKill
        {
            get { return m_PointsForKill; }
            set { m_PointsForKill = value; }
        }

        public EnemySpaceShip(
            Game i_Game, 
            GameScreen i_Screen, 
            string i_AssetName, 
            int i_PointsForKill, 
            ShootsManager i_ShootsManager) : 
                base(i_Game, i_Screen, i_AssetName, i_ShootsManager)
        {
            r_PointsForKill = i_PointsForKill;
            m_PointsForKill = r_PointsForKill;
            DrawOrder = k_DrawOrder;
            m_NumOfUpdatesToTryToShoot = m_Rnd.Next(0, 2000);
            m_NextShootAttemptCounter = 0;
        }

        public EnemySpaceShip(
            Game i_Game, 
            GameScreen i_Screen, 
            string i_AssetName, 
            int i_PointsForKill, 
            ShootsManager i_ShootsManager,
            Color i_Color) : 
                base(i_Game, i_Screen, i_AssetName, i_ShootsManager, i_Color)
        {
            r_PointsForKill = i_PointsForKill;
            m_PointsForKill = r_PointsForKill;
            DrawOrder = k_DrawOrder;
        }

        public EnemySpaceShip(
           Game i_Game, 
           GameScreen i_Screen, 
           string i_AssetName, 
           int i_PointsForKill, 
           ShootsManager i_ShootsManager,
           Rectangle i_SourceRectangle) :
               this(i_Game, i_Screen, i_AssetName, i_PointsForKill, i_ShootsManager)
        {
            this.m_SourceRectangle = i_SourceRectangle;
        }

        public override void Initialize()
        {
            base.Initialize();
            m_NumOfUpdatesToTryToShoot = m_Rnd.Next(0, 2000);
            m_NextShootAttemptCounter = 0;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_ShotSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.EnemyShot);
        }

        protected override void Shoot()
        {
            Vector2 bulletPosition = new Vector2(m_Position.X + (Width / 2), m_Position.Y + Height);
            m_ShootsManager.EnemyFired(m_MyBullet, this.Screen);
            m_MyBullet.Position = bulletPosition;
            m_MyBullet.Visible = true;
            m_ShotSound.Play();

        }

        public void SwitchSourceRectangle()
        {
            if (isUsingSourceRectangle)
            {
                if (m_SourceRectangle == m_SourceRectangles[0])
                {
                    m_SourceRectangle = m_SourceRectangles[1];
                }
                else
                {
                    m_SourceRectangle = m_SourceRectangles[0];
                }
            }
        }

        protected void invokeGotHitEvent()
        {
            GotHit?.Invoke(this, new EventArgs());
        }

        protected void playDieAnimationAndSound()
        {
            m_Animator.Enabled = true;
            m_Animator["dying"].Finished += kill;
            m_Animator["dying"].Enabled = true;
            m_DieSound.Play();
        }

        protected void kill(object i_Sender, EventArgs i_Args)
        {
            Visible = false;
            IsAlive = false;
            m_Animator.Reset();
            m_Animator.Enabled = false;
        }

        public override void MoveToLevel(int i_Level)
        {
            base.MoveToLevel(i_Level);
            int level = i_Level % 4;
            PointsForKill = r_PointsForKill + 70 * level;
        }
    }
}
