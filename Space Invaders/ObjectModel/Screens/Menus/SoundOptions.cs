using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens.Menus;
using Microsoft.Xna.Framework.Audio;
using Infrastructure.ObjectModel.Screens;
using Space_Invaders.Managers;

namespace Space_Invaders.ObjectModel.Screens.Menus
{
    public class SoundOptions : SpaceInvadersMenu
    {
        public SoundOptions(Game i_Game, GameScreen i_Caller) : base(i_Game, "Sound Options")
        {
            PrecentValueMenuItem bgMusicVolume =
                new PrecentValueMenuItem(Game, this, "Background Music Volume", SpaceInvadersSettings.GetInstance(Game).BGMusicVolume);
            bgMusicVolume.ValueChanged += bgMusicVolumeValueChanged;
            this.AddItem(bgMusicVolume);
            PrecentValueMenuItem soundFxVolume =
                new PrecentValueMenuItem(Game, this, "Sound Effects Volume", SpaceInvadersSettings.GetInstance(Game).SoundFXVolume);
            soundFxVolume.ValueChanged += soundFxVolumeValueChanged;
            this.AddItem(soundFxVolume);
            ToggleMenuItem soundOff =
                new ToggleMenuItem(Game, this, "Is Sound Off", SpaceInvadersSettings.GetInstance(Game).IsSoundOff);
            soundOff.ValueChanged += soundOffValueChanged;
            this.AddItem(soundOff);

            this.AddItem(new MenuItem(Game, this, i_Caller, "Done"));
        }

        private void bgMusicVolumeValueChanged(int i_Value)
        {
            SpaceInvadersSettings.GetInstance(Game).BGMusicVolume = i_Value;
            SoundEffectsManager.GetInstance(Game).BGMusicVolume = (float)i_Value / 100;
        }

        private void soundFxVolumeValueChanged(int i_Value)
        {
            SpaceInvadersSettings.GetInstance(Game).SoundFXVolume = i_Value;
            SoundEffectsManager.GetInstance(Game).SoundEffectsVolume = (float)i_Value / 100;
        }

        private void soundOffValueChanged(bool i_Value)
        {
            SpaceInvadersSettings.GetInstance(Game).IsSoundOff = i_Value;
        }
    }
}

