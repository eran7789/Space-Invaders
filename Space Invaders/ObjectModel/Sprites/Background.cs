using System;
using System.Collections.Generic;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class Background : Sprite
    {
        public Background(Game i_Game, GameScreen i_Screen, string i_AssetName, int i_Opacity)
            : base(i_Game, i_Screen, i_AssetName)
        {
            this.Alpha = i_Opacity;
            this.DrawOrder = int.MinValue;
        }

        public override void Initialize()
        {
            base.Initialize();
        }
    }
}
