using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Infrastructure.ObjectModel.Animators;

namespace Infrastructure.ObjectModel.Animators.ConcreteAnimators
{
    public class RotateAnimator : SpriteAnimator
    {
        private readonly double r_RotationsperSecond;

        public RotateAnimator(string i_Name, TimeSpan i_AnimationLength, double i_RotationPerSecond) : 
            base(i_Name, i_AnimationLength)
        {
            r_RotationsperSecond = i_RotationPerSecond;
        }

        protected override void DoFrame(GameTime i_GameTime)
        {
            this.BoundSprite.Rotation += (float)(r_RotationsperSecond * Math.PI * 2 * i_GameTime.ElapsedGameTime.TotalSeconds);
            this.BoundSprite.Rotation %= (float)Math.PI * 2f;
        }

        protected override void RevertToOriginal()
        {
            this.BoundSprite.Rotation = 0;
        }
    }
}
