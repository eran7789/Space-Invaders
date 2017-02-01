using System;
using System.Collections.Generic;
using Microsoft.Xna.Framework;
using Space_Invaders.ObjectModel.Sprites;
using Infrastructure.ObjectModel;

namespace Space_Invaders.Managers
{
    public class CollisionManager
    {
        private List<Sprite> m_Enemys;
        private List<PlayerSpaceShip> m_Players;
        private List<Sprite> m_Walls;
        private Game m_Game;

        public CollisionManager(Game i_Game) 
        {
            m_Game = i_Game;
        }

        public CollisionManager(Game i_Game, List<PlayerSpaceShip> i_Player, List<Sprite> i_Enemys, List<Sprite> i_Walls)
            : this(i_Game)
        {
            Restart(i_Player, i_Enemys, i_Walls);
        }

        public void Restart(List<PlayerSpaceShip> i_Player, List<Sprite> i_Enemys, List<Sprite> i_Walls)
        {
            m_Enemys = i_Enemys;
            m_Players = i_Player;
            m_Walls = i_Walls;
            foreach (Sprite enemy in m_Enemys)
            {
                (enemy as EnemySpaceShip).GotHit += removeHitEnemySpaceShip;
            }
        }

        public void RegisterEnemy(EnemySpaceShip i_Enemy) 
        {
            if (m_Enemys == null)
            {
                m_Enemys = new List<Sprite>();
            }

            m_Enemys.Add(i_Enemy);
            i_Enemy.GotHit += removeHitEnemySpaceShip;
        }

        public void RemoveEnemy(EnemySpaceShip i_Enemy)
        {
            if (m_Enemys != null)
            {
                m_Enemys.Remove(i_Enemy);
            }
        }

        public void RegisterPlayer(PlayerSpaceShip i_Player)
        {
            if (m_Players == null)
            {
                m_Players = new List<PlayerSpaceShip>();
            }

            m_Players.Add(i_Player);
        }

        public void RemovePlayer(PlayerSpaceShip i_Player)
        {
            if (m_Players != null)
            {
                m_Players.Remove(i_Player);
            }
        }

        public void RegisterWall(Wall i_Wall)
        {
            if (m_Walls == null)
            {
                m_Walls = new List<Sprite>();
            }

            m_Walls.Add(i_Wall);
        }

        public void RemoveWall(Wall i_Wall)
        {
            if (m_Walls != null)
            {
                m_Walls.Remove(i_Wall);
            }
        }

        public bool IsCollisionHappend()
        {
            bool isCollisionHappend = false;
            foreach (EnemySpaceShip enemy in m_Enemys)
            {
               if (enemy.Position.Y + enemy.Height >= m_Players[0].Position.Y)
                {
                    isCollisionHappend = true;
                }
            }

            return isCollisionHappend;
        }

        public void Update()
        {
            foreach (Wall wall in m_Walls)
            {
                isWallHit(wall);
            }
        }

        private void isWallHit(Wall i_Wall)
        {
            Rectangle overlapArea;
            foreach(EnemySpaceShip enemy in m_Enemys)
            {
                Rectangle wallRectangle = new Rectangle(
                    i_Wall.Bounds.X, i_Wall.Bounds.Y, i_Wall.Bounds.Width, i_Wall.Bounds.Height);
                Rectangle enemyRectangle = new Rectangle(
                    enemy.Bounds.X, enemy.Bounds.Y, enemy.Bounds.Width, enemy.Bounds.Height);
                Rectangle.Intersect(ref wallRectangle, ref enemyRectangle, out overlapArea);
                if (overlapArea.Height != 0)
                {
                    i_Wall.GotHit(overlapArea);
                }
            }
        }

        private void removeHitEnemySpaceShip(object i_Enemy, EventArgs i_Args)
        {
            RemoveEnemy(i_Enemy as EnemySpaceShip);
        }
    }
}
