using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Space_Invaders.ObjectModel.Sprites;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.Managers
{
    public class ShootsManager
    {
        private List<Bullet> m_EnemyBullets;
        private List<Bullet> m_FirstPlayerBullets;
        private List<Bullet> m_SecondPlayerBullets;
        private Game m_Game;
        private Random m_Rnd;

        public event EventHandler<bool> EnemyGotShot;

        public ShootsManager(Game i_Game)
        {
            m_Game = i_Game;
            Initialize();
            m_Rnd = new Random();
        }

        public void Initialize()
        {
            m_EnemyBullets = new List<Bullet>();
            m_FirstPlayerBullets = new List<Bullet>();
            m_SecondPlayerBullets = new List<Bullet>();
        }

        public bool CanPlayerShoot(bool i_IsFirstPlayer)
        {
            bool retVal;
            if (i_IsFirstPlayer)
            {
                retVal = m_FirstPlayerBullets.Count < 2;
            }
            else
            {
                retVal = m_SecondPlayerBullets.Count < 2;
            }

            return retVal;
        }

        public void PlayerFired(Bullet i_Bullet, bool i_IsFirstPlayer, GameScreen i_Screen)
        {
            if (i_IsFirstPlayer)
            {
                m_FirstPlayerBullets.Add(i_Bullet);
            }
            else
            {
                m_SecondPlayerBullets.Add(i_Bullet);
            }

            if (!i_Screen.Contains(i_Bullet))
            {
                i_Screen.Add(i_Bullet);
            }
        }

        public void EnemyFired(Bullet i_Bullet, GameScreen i_Screen)
        {
            m_EnemyBullets.Add(i_Bullet);
            if (!i_Screen.Contains(i_Bullet))
            {
                i_Screen.Add(i_Bullet);
            }
        }

        private void removeBulletsFrom(List<Bullet> listToRemoveFrom, List<Bullet> listOfBulletsToRemove)
        {
            foreach (Bullet bullet in listOfBulletsToRemove)
            {
                listToRemoveFrom.Remove(bullet);
                bullet.Destroy();
                m_Game.Components.Remove(bullet);
            }
        }

        public void Reset()
        {
            foreach (Bullet bullet in m_FirstPlayerBullets)
            {
                bullet.Visible = false;
                bullet.IsAlive = false;
            }

            foreach (Bullet bullet in m_SecondPlayerBullets)
            {
                bullet.Visible = false;
                bullet.IsAlive = false;
            }

            foreach (Bullet bullet in m_EnemyBullets)
            {
                bullet.Visible = false;
                bullet.IsAlive = false;
            }

            this.Initialize();
        }

        public bool IsPlayerShoot(Sprite i_Sprite) 
        {
            bool amIShoot = false;
            List<Bullet> bulletsToRemove = new List<Bullet>();
            foreach (Bullet bullet in m_EnemyBullets)
            {
                if (bullet.Bounds.Intersects(i_Sprite.Bounds))
                {
                    if (bullet.isHitPixelwise(i_Sprite))
                    {
                        amIShoot = true;
                        bulletsToRemove.Add(bullet);
                    }
                }
            }

            removeBulletsFrom(m_EnemyBullets, bulletsToRemove);
            return amIShoot;
        }

        public bool IsEnemyShoot(Sprite i_Sprite)
        {
            bool amIShoot = false;
            List<Bullet> bulletsToRemove = new List<Bullet>();
            const bool k_IsFirstPlayer = true;

            foreach (Bullet bullet in m_FirstPlayerBullets)
            {
                if (amIShoot)
                {
                    isShootByBullet(bullet, i_Sprite, bulletsToRemove, k_IsFirstPlayer);
                }
                else
                {
                    amIShoot = isShootByBullet(bullet, i_Sprite, bulletsToRemove, k_IsFirstPlayer);
                }
            }

            removeBulletsFrom(m_FirstPlayerBullets, bulletsToRemove);
            bulletsToRemove.Clear();
            foreach (Bullet bullet in m_SecondPlayerBullets)
            {
                if (amIShoot)
                {
                    isShootByBullet(bullet, i_Sprite, bulletsToRemove, !k_IsFirstPlayer);
                }
                else
                {
                    amIShoot = isShootByBullet(bullet, i_Sprite, bulletsToRemove, !k_IsFirstPlayer);
                }
            }

            removeBulletsFrom(m_SecondPlayerBullets, bulletsToRemove);

            return amIShoot;
        }

        private bool isShootByBullet(Bullet i_Bullet, Sprite i_Sprite, List<Bullet> i_BulletsToRemove, bool i_IsFirstPlayer)
        {
            bool amIShoot = false;

            if (i_Bullet.Bounds.Intersects(i_Sprite.Bounds))
            {
                if (i_Bullet.isHitPixelwise(i_Sprite))
                {
                    amIShoot = true;
                    i_BulletsToRemove.Add(i_Bullet);
                    if (i_Sprite is EnemySpaceShip)
                    {
                        if (EnemyGotShot != null)
                        {
                            EnemyGotShot.Invoke(i_Sprite, i_IsFirstPlayer);
                        }
                    }
                    else
                    {
                        (i_Sprite as Wall).GotShot(i_Bullet);
                    }
                }
            }

            return amIShoot;
        }

        private bool isOnSameX(Bullet i_Bullet, Sprite i_Sprite)
        {
            bool isOnSameX = false;
            if (i_Bullet.Position.X > i_Sprite.Position.X && 
                i_Bullet.Position.X < i_Sprite.Position.X + i_Sprite.Width)
            {
                isOnSameX = true;
            }

            return isOnSameX;
        }

        private bool isOnSameY(Bullet i_Bullet, Sprite i_Sprite)
        {
            bool isOnSameY = false;
            if (i_Bullet.Position.Y > i_Sprite.Position.Y &&
                i_Bullet.Position.Y < i_Sprite.Position.Y + i_Sprite.Height)
            {
                isOnSameY = true;
            }

            return isOnSameY;
        }

        public void Update()
        {
            const bool k_AllowEdges = true;
            List<Bullet> enemyBulletsToRemove = new List<Bullet>();
            List<Bullet> firstPlayerBulletsToRemove = new List<Bullet>();
            List<Bullet> secondPlayerBulletsToRemove = new List<Bullet>();
            removeOutOfScreenBullets(m_EnemyBullets, k_AllowEdges);
            removeOutOfScreenBullets(m_FirstPlayerBullets, k_AllowEdges);
            removeOutOfScreenBullets(m_SecondPlayerBullets, k_AllowEdges);
            foreach (Bullet enemyBullet in m_EnemyBullets)
            {
                foreach(Bullet playerBullet in m_FirstPlayerBullets)
                {
                    checkBulletCollision(enemyBullet, playerBullet, enemyBulletsToRemove, firstPlayerBulletsToRemove);
                }
                
                foreach (Bullet playerBullet in m_SecondPlayerBullets)
                {
                    checkBulletCollision(enemyBullet, playerBullet, enemyBulletsToRemove, secondPlayerBulletsToRemove);
                }
            }

            removeBulletsFrom(m_FirstPlayerBullets, firstPlayerBulletsToRemove);
            removeBulletsFrom(m_EnemyBullets, enemyBulletsToRemove);
            removeBulletsFrom(m_SecondPlayerBullets, secondPlayerBulletsToRemove);
        }

        private void checkBulletCollision(
            Bullet i_EnemyBullet, 
            Bullet i_PlayerBullet, 
            List<Bullet> i_EnemyBulletToRemove, 
            List<Bullet> i_PlayerBulletsToRemove)
        {
            if (i_EnemyBullet.Bounds.Intersects(i_PlayerBullet.Bounds))
            {
                i_PlayerBulletsToRemove.Add(i_PlayerBullet);
                if (m_Rnd.Next(10) == 1)
                {
                    i_EnemyBulletToRemove.Add(i_EnemyBullet);
                }
            }
        }

        private void removeOutOfScreenBullets
            (List<Bullet> i_ListToCheck, bool i_IsEdgesOutAllowed)
        {
            List<Bullet> bulletsToRemove = new List<Bullet>();
            foreach (Bullet bullet in i_ListToCheck)
            {
                if (!bullet.isOnScreen(bullet.Position, i_IsEdgesOutAllowed))
                {
                    bulletsToRemove.Add(bullet);
                }
            }

            removeBulletsFrom(i_ListToCheck, bulletsToRemove);
        }
    }
}
