using System;
using Microsoft.Xna.Framework;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel.Animators;
using Infrastructure.ObjectModel.Animators.ConcreteAnimators;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class EnemySpaceShipBlue : EnemySmallSpaceShip
    {
        public const int k_PointsForKill = 170;

        public EnemySpaceShipBlue(Game i_Game, 
            GameScreen i_Screen, 
            ShootsManager i_ShootsManager) : 
            base(
                i_Game, 
                i_Screen, 
                i_ShootsManager,
                @"Sprites/EnemySquares", 
                Color.LightBlue, 
                k_PointsForKill, 
                new Rectangle(0, 0, 32, 32),
                new Rectangle(32, 0, 32, 32))
        { }
    }
}
