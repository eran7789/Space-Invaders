using System;
using Microsoft.Xna.Framework;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class EnemySpaceShipPink : EnemySmallSpaceShip
    {
        public const int k_PointsForKill = 240;

        public EnemySpaceShipPink(
            Game i_Game, 
            GameScreen i_Screen, 
            ShootsManager i_ShootsManager) :
            base(i_Game, 
                i_Screen, 
                i_ShootsManager, 
                @"Sprites/EnemySquares", 
                Color.LightPink, k_PointsForKill, 
                new Rectangle(64, 0, 32, 32), 
                new Rectangle(96, 0, 32, 32))
        { }            
    }
}
