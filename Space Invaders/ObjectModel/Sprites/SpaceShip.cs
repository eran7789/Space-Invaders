using Microsoft.Xna.Framework;
using Space_Invaders.Managers;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;
using Microsoft.Xna.Framework.Audio;

namespace Space_Invaders.ObjectModel.Sprites
{
    public abstract class SpaceShip : Sprite, IShootable
    {
        protected ShootsManager m_ShootsManager;
        protected SoundEffectsManager m_SoundManager;
        protected SoundEffectInstance m_DieSound;
        protected SoundEffectInstance m_ShotSound;

        public ShootsManager ShootsManager
        {
            get { return m_ShootsManager; }
            set { m_ShootsManager = value; }
        }

        public SpaceShip(
            Game i_Game, 
            GameScreen i_Screen, 
            string i_AssetName, 
            ShootsManager i_ShootsManager) : 
            base(i_Game, i_Screen, i_AssetName)
        {
            m_ShootsManager = i_ShootsManager;
            m_SoundManager = SoundEffectsManager.GetInstance(i_Game);
        }

        public SpaceShip(
            Game i_Game, 
            GameScreen i_Screen, 
            string i_AssetName, 
            ShootsManager i_ShootsManager, 
            Color i_Color) : 
            base(i_Game, i_Screen, i_AssetName, i_Color)
        {
            m_ShootsManager = i_ShootsManager;
            m_SoundManager = SoundEffectsManager.GetInstance(i_Game);
        }

        protected abstract void Shoot();
    }
}
