using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invaders
{
    public class SpaceInvadersSettings
    {
        private static SpaceInvadersSettings m_Instace;
        public static SpaceInvadersSettings GetInstance(Game i_Game)
        {
            if (m_Instace == null)
            {
                m_Instace = new SpaceInvadersSettings(i_Game);
            }

            return m_Instace;
        }

        private Game m_Game;

        private GraphicsDeviceManager m_GraphicsManager;
        public GraphicsDeviceManager GraphicsManager
        {
            set { m_GraphicsManager = value; }
        }

        private bool m_TwoPlayers;
        public bool TwoPlayers
        {
            get { return m_TwoPlayers; }
            set { m_TwoPlayers = value; }
        }

        private int m_BGMusicVolume;
        public int BGMusicVolume
        {
            get { return m_BGMusicVolume; }
            set
            {
                m_BGMusicVolume = value;
                m_BGMusicVolume = MathHelper.Clamp(m_BGMusicVolume, 0, 100);
            }
        }

        private int m_SoundFXVolume;
        public int SoundFXVolume
        {
            get { return m_SoundFXVolume; }
            set
            {
                m_SoundFXVolume = value;
                m_SoundFXVolume = MathHelper.Clamp(m_SoundFXVolume, 0, 100);
            }
        }

        private float m_MasterVolume;
        public bool IsSoundOff
        {
            get
            {
                if (SoundEffect.MasterVolume == 0)
                {
                    return true;
                }
                else
                {
                    return false;
                }
            }
            set
            {
                if (value)
                {
                    SoundEffect.MasterVolume = 0;
                }
                else
                {
                    SoundEffect.MasterVolume = m_MasterVolume; 
                }
            }
        }

        public bool IsMouseVisibile
        {
            get { return m_Game.IsMouseVisible; }
            set { m_Game.IsMouseVisible = value; }
        }

        public bool IsWindowResizeAllowed
        {
            get { return m_Game.Window.AllowUserResizing; }
            set { m_Game.Window.AllowUserResizing = value; }
        }
        
        public bool IsOnFullScreenMode
        {
            get { return m_GraphicsManager.IsFullScreen; }
            set { m_GraphicsManager.ToggleFullScreen(); }
        }

        private SpaceInvadersSettings(Game i_Game)
        {
            m_Game = i_Game;
            m_TwoPlayers = true;
            m_BGMusicVolume = 100;
            m_SoundFXVolume = 100;
            m_MasterVolume = SoundEffect.MasterVolume;
        }
    }
}
