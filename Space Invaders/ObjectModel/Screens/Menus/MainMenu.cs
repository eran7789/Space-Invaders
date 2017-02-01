using Infrastructure.ObjectModel.Screens.Menus;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Space_Invaders.Managers;

namespace Space_Invaders.ObjectModel.Screens.Menus
{
    public class MainMenu : SpaceInvadersMenu
    {
        private LevelTransitionScreeen m_TransitionionScreen;
        private PlayGameScreen m_GameScreen;

        public MainMenu(Game i_Game, LevelTransitionScreeen i_TransitionScreen, PlayGameScreen i_GameScreen) : base(i_Game, "Main Menu")
        {
            m_TransitionionScreen = i_TransitionScreen;
            m_GameScreen = i_GameScreen;
            this.AddItem(new MenuItem(Game, this, new SoundOptions(i_Game, this), "Sound Options"));
            this.AddItem(new MenuItem(Game, this, new ScreenOptions(i_Game, this), "Screen Options"));
            ToggleMenuItem TwoPlayers = 
                new ToggleMenuItem(Game, this, "Two Players", SpaceInvadersSettings.GetInstance(Game).TwoPlayers);
            TwoPlayers.ValueChanged += TwoPlayersValueChangedHandler;
            this.AddItem(TwoPlayers);
            this.AddItem(new FunctionMenuItem(Game, this, "Play", play));
            this.AddItem(new FunctionMenuItem(Game, this, "Quit", quit));
        }

        private void TwoPlayersValueChangedHandler(bool i_Value)
        {
            SpaceInvadersSettings.GetInstance(Game).TwoPlayers = i_Value;
        }

        private void play()
        {
            m_GameScreen.Reset();
            ScreensManager.SetCurrentScreen(m_GameScreen);
            ScreensManager.SetCurrentScreen(m_TransitionionScreen);
        }

        private void quit()
        {
            Game.Exit();
        }
    }
}
