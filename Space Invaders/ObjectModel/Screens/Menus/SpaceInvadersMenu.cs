using Infrastructure.ObjectModel.Screens.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Space_Invaders.Managers;
using Space_Invaders.ObjectModel.Sprites;

namespace Space_Invaders.ObjectModel.Screens.Menus
{
    public class SpaceInvadersMenu : MenuScreen
    {
        private SoundEffectInstance m_ActiveItemChangedSound;
        private SoundEffectsManager m_SoundManager;

        public SpaceInvadersMenu(Game i_Game, string i_Headline) : 
            base(i_Game, i_Headline)
        {
            m_SoundManager = SoundEffectsManager.GetInstance(i_Game);
        }

        protected override void LoadContent()
        {
            base.LoadContent();
            m_ActiveItemChangedSound = m_SoundManager.GetInstaceOf(SoundEffectsManager.eSounds.MenuMove);
        }

        protected override void ActiveItemChanged()
        {
            m_ActiveItemChangedSound.Play();
        }
    }
}
