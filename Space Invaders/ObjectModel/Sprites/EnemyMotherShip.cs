using System;
using Microsoft.Xna.Framework;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class EnemyMotherShip : EnemySpaceShip
    {
        public const int k_PointsForKill = 650;
        public const int k_RndMaxValueWhenToAppear = 220;

        public EnemyMotherShip(Game i_Game, GameScreen i_Screen, ShootsManager i_ShootsManager) :
            base(i_Game, i_Screen, @"Sprites/MotherShip_32x120", k_PointsForKill, i_ShootsManager, Color.Red)
        {
            m_Animator = new CompositeAnimator(this);
            CompositeAnimator dieAnimation = new CompositeAnimator("dying", TimeSpan.FromSeconds(1.6), this,
                new ShrinkAnimator("dyingShrink", TimeSpan.FromSeconds(2.4)),
                new BlinkAnimator("dyingBlink", TimeSpan.FromSeconds(0.2), TimeSpan.FromSeconds(2.4)),
                new FadeOutAnimator("dyingFade", TimeSpan.FromSeconds(2.4)));
            m_Animator.Add(dieAnimation);
        }

        public override void Initialize()
        {
            base.Initialize();

            m_Position = new Vector2(0 - Width, Height);
            m_Velocity = new Vector2(105f, 0);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_DieSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.MotherShipKill);
        }

        public override void Update(GameTime i_GameTime)
        {
            const bool k_AllowEdgesOut = true;

            Vector2 nextPosition = this.m_Position + (this.m_Velocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
            if (!isOnScreen(nextPosition, k_AllowEdgesOut))
            {
                Visible = false;
                IsAlive = false;
            }
            else if (IsAlive)
            {
                m_Position = nextPosition;
            }

            if (Visible == false)
            {
                int rndNum = m_Rnd.Next(0, k_RndMaxValueWhenToAppear);
                if (rndNum == 1)
                {
                    this.Initialize();
                }
            }
            else if (IsAlive)
            {
                if (m_ShootsManager.IsEnemyShoot(this))
                {
                    IsAlive = false;
                    playDieAnimationAndSound();
                }
            }

            m_Animator.Update(i_GameTime);
        }
    }
}
