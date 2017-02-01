using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class EnemySmallSpaceShip : EnemySpaceShip
    {
        public const int k_RndMaxValueLevel1 = 4000;
        private int m_RndMaxValue;

        public EnemySmallSpaceShip(
            Game i_Game, 
            GameScreen i_Screen, 
            ShootsManager i_ShootsManager,
            string i_AssetName, 
            Color i_Color, 
            int i_PointsForKill, 
            params Rectangle[] i_SourceRectangle) :
            base(i_Game, i_Screen, @"Sprites/EnemySquares", i_PointsForKill, i_ShootsManager, i_Color)
        {
            m_SourceRectangles = new Rectangle[2];
            m_SourceRectangles[0] = i_SourceRectangle[0];
            m_SourceRectangles[1] = i_SourceRectangle[1];
            m_SourceRectangle = m_SourceRectangles[0];
            isUsingSourceRectangle = true;
            m_Animator = new CompositeAnimator(this);
            CompositeAnimator dieAnimation = new CompositeAnimator("dying", TimeSpan.FromSeconds(1.6), this,
                new ShrinkAnimator("dyingShrink", TimeSpan.FromSeconds(1.6)),
                new RotateAnimator("dyingRotate", TimeSpan.FromSeconds(1.6), 6));
            m_Animator.Add(dieAnimation);
            m_RndMaxValue = k_RndMaxValueLevel1;
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_DieSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.EnemySSKil);
        }

        public override void Update(GameTime i_GameTime)
        {
            if (IsAlive)
            {
                if (m_MyBullet == null)
                {
                    m_MyBullet = new Bullet(Game, this.Screen, Color.Blue, SpriteBatch);
                    m_MyBullet.Visible = false;
                }

                m_NextShootAttemptCounter++;
                if (m_NextShootAttemptCounter == m_NumOfUpdatesToTryToShoot && m_MyBullet.Visible == false)
                {
                        Shoot();
                        m_NextShootAttemptCounter = 0;
                        m_NumOfUpdatesToTryToShoot = m_Rnd.Next(0, m_RndMaxValue);
                }

                if (m_ShootsManager.IsEnemyShoot(this))
                {
                    invokeGotHitEvent();
                    IsAlive = false;
                    playDieAnimationAndSound();
                }
            }

            this.m_Animator.Update(i_GameTime);
            base.Update(i_GameTime);
        }

        public override void MoveToLevel(int i_Level)
        {
            base.MoveToLevel(i_Level);
            int level = i_Level % 4;
            this.m_RndMaxValue = k_RndMaxValueLevel1 + level * (k_RndMaxValueLevel1 / 5);
        }
    }
}
