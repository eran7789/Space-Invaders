using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class FadeOutAnimator : SpriteAnimator
    {
        private const float k_FullVisibleAlpha = 1f;

        public FadeOutAnimator(string i_Name, TimeSpan i_AnimationLength) : 
            base(i_Name, i_AnimationLength)
        { }

        protected override void DoFrame(GameTime i_GameTime)
        {
            BoundSprite.Alpha -= (float)(1/AnimationLength.TotalSeconds * i_GameTime.ElapsedGameTime.TotalSeconds);
        }

        protected override void RevertToOriginal()
        {
            BoundSprite.Alpha = k_FullVisibleAlpha;
        }
    }
}
