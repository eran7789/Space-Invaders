using System;
using Microsoft.Xna.Framework;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class EnemySpaceShipYellow : EnemySmallSpaceShip
    {
        public const int k_PointsForKill = 140;

        public EnemySpaceShipYellow(
            Game i_Game, 
            GameScreen i_Screen, 
            ShootsManager i_ShootsManager) :
            base(
                i_Game, 
                i_Screen, 
                i_ShootsManager, 
                @"Sprites/EnemySquares", 
                Color.LightYellow, 
                k_PointsForKill, 
                new Rectangle(128, 0, 32, 32), 
                new Rectangle(160, 0, 32, 32))
        { }
    }
}
