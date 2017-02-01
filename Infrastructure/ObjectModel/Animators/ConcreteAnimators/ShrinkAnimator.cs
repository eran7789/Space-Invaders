using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class ShrinkAnimator : SpriteAnimator
    { 
        public ShrinkAnimator(string i_Name, TimeSpan i_AnimationLength) : 
            base(i_Name, i_AnimationLength)
        { }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Scales -= new Vector2((float)(i_GameTime.ElapsedGameTime.TotalSeconds / AnimationLength.TotalSeconds));
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Scales = new Vector2(1);
        }
    }
}
