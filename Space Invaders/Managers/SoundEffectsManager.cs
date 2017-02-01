using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Space_Invaders.Managers
{
    public class SoundEffectsManager
    {
        public enum eSounds
        {
            PlayerShot,
            EnemyShot,
            EnemySSKil,
            MotherShipKill,
            BarrierHit,
            GameOver,
            LevelWin,
            SoulDie,
            MenuMove,
            BGMusic
        }

        private static SoundEffectsManager m_Instance;
        public static SoundEffectsManager GetInstance(Game i_Game)
        {
            if (m_Instance == null)
            {
                m_Instance = new SoundEffectsManager(i_Game);
            }

            return m_Instance;
        }

        private const int k_NumOfSounds = 10;
        private Dictionary<eSounds, SoundEffect> m_SoundEffects;
        private Game m_Game;
        private List<SoundEffectInstance> m_FXInstances;
        private List<SoundEffectInstance> m_BGMusicInstances;

        private SoundEffectsManager(Game i_Game)
        {
            m_SoundEffects = new Dictionary<eSounds, SoundEffect>();
            m_Game = i_Game;
            m_FXInstances = new List<SoundEffectInstance>();
            m_BGMusicInstances = new List<SoundEffectInstance>();
        }

        public float SoundEffectsVolume
        {
            set
            {
                value = MathHelper.Clamp(value, 0, 1);
                foreach(SoundEffectInstance instance in m_FXInstances)
                {
                    instance.Volume = value;
                }
            }
            get
            {
                float volume = 1;
                if (m_FXInstances[0] != null)
                {
                    volume = m_FXInstances[0].Volume;
                }

                return volume;
            }
        }

        public float BGMusicVolume
        {
            set
            {
                value = MathHelper.Clamp(value, 0, 1);
                foreach (SoundEffectInstance instance in m_BGMusicInstances)
                {
                    instance.Volume = value;
                }
            }
            get
            {
                float volume = 1;
                if (m_BGMusicInstances[0] != null)
                {
                    volume = m_BGMusicInstances[0].Volume;
                }

                return volume;
            }
        }

        public void LoadContent()
        {
            m_SoundEffects.Add(eSounds.BarrierHit, 
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}BarrierHit", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.BGMusic,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}BGMusic", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.EnemyShot,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}EnemyGunShot", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.EnemySSKil,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}EnemyKill", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.GameOver,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}GameOver", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.MenuMove,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}MenuMove", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.MotherShipKill,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}MotherShipKill", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.PlayerShot,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}SSGunShot", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.SoulDie,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}LifeDie", SpaceInvaders.k_SoundPath)));
            m_SoundEffects.Add(eSounds.LevelWin,
                this.m_Game.Content.Load<SoundEffect>(string.Format("{0}LevelWin", SpaceInvaders.k_SoundPath)));
        }

        public SoundEffectInstance GetInstaceOf(eSounds i_SoundName)
        {
            SoundEffectInstance instance = m_SoundEffects[i_SoundName].CreateInstance();
            if (i_SoundName == eSounds.BGMusic)
            {
                m_BGMusicInstances.Add(instance);
            }
            else
            {
                m_FXInstances.Add(instance);
            }

            return instance;
        }
    }
}
