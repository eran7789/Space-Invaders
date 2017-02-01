using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel;
using Infrastructure.ObjectModel.Screens;

namespace Space_Invaders.ObjectModel.Sprites
{
    public class Soul : Sprite
    {
        public Soul(Game i_Game, GameScreen i_Screen, string i_AssetName) :
            base (i_Game, i_Screen, i_AssetName)
        { }

        public override void Initialize()
        {
            base.Initialize();

            this.Alpha = 0.5f;
            this.Scales = new Vector2(0.5f);
            this.DrawOrder = int.MaxValue;
        }
    }
}
