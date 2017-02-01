using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework.Audio;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class PlayerSpaceShip : SpaceShip
    {
        private const int k_StartingNumOfSouls = 3;
        private const int k_DrawOrder = 1;
        private readonly Vector2 k_Velocity = new Vector2(160f, 0);
        private int m_NumOFSouls;
        private MouseState m_PrevMouseState;
        private KeyboardState m_PrevKeyboardState;
        private bool m_IsFirstPlayer;
        private Soul[] m_SoulsSprites;
        private int m_Score;

        public int Score
        {
            get { return m_Score; }
            set { m_Score = value; }
        }

        public Soul[] Souls
        {
            get { return m_SoulsSprites; }
        }

        public PlayerSpaceShip(
            Game i_Game, 
            GameScreen i_Screen, 
            ShootsManager i_ShootsManager, 
            bool i_IsFirstPlayer) 
            : this(i_Game, i_Screen, @"Sprites/Ship01_32x32", i_ShootsManager, i_IsFirstPlayer)
        { }

        public PlayerSpaceShip(
            Game i_Game, 
            GameScreen i_Screen, 
            string i_GameAsset, 
            ShootsManager i_ShootsManager, 
            bool i_IsFirstPlayer)
            : base(i_Game, i_Screen, i_GameAsset, i_ShootsManager)
        {
            m_NumOFSouls = k_StartingNumOfSouls;
            DrawOrder = k_DrawOrder;
            m_IsFirstPlayer = i_IsFirstPlayer;
            m_Animator = new CompositeAnimator(this);
            CompositeAnimator lostSoulAnimation = new CompositeAnimator("lostSoul", TimeSpan.FromSeconds(1.6), this,
                new BlinkAnimator("lostSoulBlink", TimeSpan.FromSeconds(1/14), TimeSpan.FromSeconds(2.4)));
            CompositeAnimator dieAnimation = new CompositeAnimator("dying", TimeSpan.FromSeconds(1.6), this,
                new RotateAnimator("dyingRotate", TimeSpan.FromSeconds(2.4), 4), 
                new FadeOutAnimator("dyingFade", TimeSpan.FromSeconds(2.4)));
            m_Animator.Add(dieAnimation);
            m_Animator.Add(lostSoulAnimation);
            dieAnimation.Enabled = false;
            lostSoulAnimation.Enabled = false;
            m_SoulsSprites = new Soul[3];
            m_SoulsSprites[0] = new Soul(i_Game, i_Screen, i_GameAsset);
            m_SoulsSprites[1] = new Soul(i_Game, i_Screen, i_GameAsset);
            m_SoulsSprites[2] = new Soul(i_Game, i_Screen, i_GameAsset);
            foreach (Soul soul in m_SoulsSprites)
            {
                Screen.Add(soul);
            }
        }

        public override void Initialize()
        {
            float yPosition;

            base.Initialize();
            m_Velocity = new Vector2(0, 0);
            yPosition = (Game.GraphicsDevice.Viewport.Height)-2 * this.Height;
            Position = new Vector2(0, yPosition);
            if (this.m_IsFirstPlayer)
            {
                positionSouls(10);
            }
            else
            {
                positionSouls(10 + m_SoulsSprites[0].Height);
            }
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_DieSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.SoulDie);
            m_ShotSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.PlayerShot);
        }

        private void positionSouls(float i_YPosition)
        {
            m_SoulsSprites[0].Position = new Vector2(
                Game.Window.ClientBounds.Width - (this.Width) - 10,
                i_YPosition);
            m_SoulsSprites[1].Position = new Vector2(
                Game.Window.ClientBounds.Width - (this.Width * 2) - 10,
                i_YPosition);
            m_SoulsSprites[2].Position = new Vector2(
                Game.Window.ClientBounds.Width - (this.Width * 3) - 10,
                i_YPosition);
        }

        public override void Update(GameTime i_GameTime)
        {
            KeyboardState keyboardState = Keyboard.GetState();
            MouseState mouseState = Mouse.GetState();
            Vector2 newPosition;
            const bool k_DontAllowEdges = false;
            
            newPosition = doPlayerMoveLogic(i_GameTime, keyboardState, mouseState);
            
            if (isOnScreen(newPosition, k_DontAllowEdges))
            {
                m_Position = newPosition;
            }

            doPlayerShootLogic(keyboardState, mouseState);

            m_PrevKeyboardState = keyboardState;
            if (m_IsFirstPlayer)
            {
                m_PrevMouseState = mouseState;
            }

            base.Update(i_GameTime);
            if (IsAlive && m_Animator.Enabled == false)
            {
                if (m_ShootsManager.IsPlayerShoot(this))
                {
                    m_Score = m_Score - 1200;
                    if (m_Score < 0)
                    {
                        m_Score = 0;
                    }

                    m_NumOFSouls--;
                    if (m_NumOFSouls == 0)
                    {
                        playDieAnimationAndSound();
                    }
                    else
                    {
                        playSoulLostAnimationAndSound();
                    }
                }
            }

            m_Animator.Update(i_GameTime);
        }

        private void playDieAnimationAndSound()
        {
            m_Animator.Enabled = true;
            m_Animator["dying"].Finished += kill;
            m_Animator["dying"].Restart();
        }

        private void playSoulLostAnimationAndSound()
        {
            m_DieSound.Play();
            m_Animator.Enabled = true;
            m_Animator["lostSoul"].Finished += finshedLostSoulAnimation;
            m_Animator["lostSoul"].Restart();
        }

        private void kill(object i_Sender, EventArgs i_Args)
        {
            Visible = false;
            IsAlive = false;
            m_Animator.Enabled = false;
            m_SoulsSprites[m_NumOFSouls].Visible = false;
        }

        private void finshedLostSoulAnimation(object i_Sender, EventArgs i_Args)
        {
            Visible = true;
            m_Animator.Enabled = false;
            m_SoulsSprites[m_NumOFSouls].Visible = false;
            this.Initialize();
        }

        private void doPlayerShootLogic(KeyboardState keyboardState, MouseState mouseState)
        {
            if (m_Animator.Enabled == false && IsAlive == true)
            {
                if (m_IsFirstPlayer)
                {
                    if ((keyboardState.IsKeyDown(Keys.Up) && m_PrevKeyboardState.IsKeyUp(Keys.Up))
                        || mouseState.RightButton == ButtonState.Pressed && m_PrevMouseState.RightButton == ButtonState.Released)
                    {
                        Shoot();
                    }
                }
                else
                {
                    if (keyboardState.IsKeyDown(Keys.R) && m_PrevKeyboardState.IsKeyUp(Keys.R))
                    {
                        Shoot();
                    }
                }
            }
        }

        private Vector2 doPlayerMoveLogic(GameTime i_GameTime, KeyboardState i_KeyboardState, MouseState i_MouseState)
        {
            Vector2 newPosition;

            if (m_PrevKeyboardState == null)
            {
                m_PrevKeyboardState = i_KeyboardState;
            }

            if (m_IsFirstPlayer)
            {
                if (m_PrevMouseState == null)
                {
                    m_PrevMouseState = i_MouseState;
                }

                if (i_KeyboardState.IsKeyDown(Keys.Left) || i_KeyboardState.IsKeyDown(Keys.Right))
                {
                    newPosition = handleKeyboardMove(i_KeyboardState, i_GameTime);
                }
                else
                {
                    newPosition = new Vector2(
                        m_Position.X + i_MouseState.Position.X - m_PrevMouseState.Position.X, m_Position.Y);
                }
            }
            else
            {
                if (i_KeyboardState.IsKeyDown(Keys.G) || i_KeyboardState.IsKeyDown(Keys.D))
                {
                    newPosition = handleKeyboardMove(i_KeyboardState, i_GameTime);
                }
                else
                {
                    newPosition = Position;
                }
            }

            return newPosition;
        }
        
        private Vector2 handleKeyboardMove(KeyboardState i_KeyboardState, GameTime i_GameTime)
        {
            if (m_IsFirstPlayer)
            {
                if (i_KeyboardState.IsKeyDown(Keys.Left))
                {
                    m_Velocity = k_Velocity * -1;
                }
                else if (i_KeyboardState.IsKeyDown(Keys.Right))
                {
                    m_Velocity = k_Velocity;
                }
            }
            else
            {
                if (i_KeyboardState.IsKeyDown(Keys.G))
                {
                    m_Velocity = k_Velocity;
                }
                else if (i_KeyboardState.IsKeyDown(Keys.D))
                {
                    m_Velocity = k_Velocity * -1;
                }
            }

            return this.m_Position + (this.m_Velocity * (float)i_GameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override void Shoot()
        {
            if (m_ShootsManager.CanPlayerShoot(m_IsFirstPlayer))
            {
                Vector2 bulletPosition = new Vector2(this.Position.X + (this.Width / 2), this.Position.Y);
                Bullet theBullet = new Bullet(Game, this.Screen, Color.Red, SpriteBatch);
                m_ShootsManager.PlayerFired(theBullet, m_IsFirstPlayer, this.Screen);
                theBullet.Position = bulletPosition;
                m_ShotSound.Play();
            }
        }

        public void EnemyGotShot(Object Sender, bool i_IsFirstPlayer)
        {
            if (m_IsFirstPlayer == i_IsFirstPlayer)
            {
                m_Score += (Sender as EnemySpaceShip).PointsForKill;
            }
        }


        public override void MoveToLevel(int i_Level)
        {
            base.MoveToLevel(i_Level);
            m_NumOFSouls = k_StartingNumOfSouls;
        }
    }
}
