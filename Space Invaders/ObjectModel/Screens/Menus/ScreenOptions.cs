using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Screens.Menus;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Screens.Menus
{
    public class ScreenOptions : SpaceInvadersMenu
    {
        public ScreenOptions(Game i_Game, GameScreen i_Caller) : base(i_Game, "Screen Options")
        {
            ToggleMenuItem isMouseVisible = 
                new ToggleMenuItem(Game, this, "Is Mouse Visible On Screen", SpaceInvadersSettings.GetInstance(Game).IsMouseVisibile);
            isMouseVisible.ValueChanged += mouseValueChanged;
            this.AddItem(isMouseVisible);
            ToggleMenuItem isWindowResizeAllowed = 
                new ToggleMenuItem(Game, this, "Is Window Resize Allowed", SpaceInvadersSettings.GetInstance(Game).IsWindowResizeAllowed);
            isWindowResizeAllowed.ValueChanged += windowResizeValueChanged;
            this.AddItem(isWindowResizeAllowed);
            ToggleMenuItem isFullScreen = 
                new ToggleMenuItem(Game, this, "Is On Full Screen Mode", SpaceInvadersSettings.GetInstance(Game).IsOnFullScreenMode);
            isFullScreen.ValueChanged += fullScreenValueChanged;
            this.AddItem(isFullScreen);
            
            this.AddItem(new MenuItem(Game, this, i_Caller, "Done"));
        }

        private void mouseValueChanged(bool i_Value)
        {
            SpaceInvadersSettings.GetInstance(Game).IsMouseVisibile = i_Value;
        }

        private void windowResizeValueChanged(bool i_Value)
        {
            SpaceInvadersSettings.GetInstance(Game).IsWindowResizeAllowed = i_Value;
        }

        private void fullScreenValueChanged(bool i_Value)
        {
            SpaceInvadersSettings.GetInstance(Game).IsOnFullScreenMode = i_Value;
        }
    }
}
