using Microsoft.Xna.Framework;
using System.Collections.Generic;

namespace TopdownPrototype
{
    public class AnimationManager
    {
        public Dictionary<string, Animation> Animations { get; set; } = new();
        public Animation CurrentAnimation { get; set; }

        public void SwitchAnimation(string key)
        {
            if (Animations[key] is null) { return; }

            if (CurrentAnimation is not null)
            {
                CurrentAnimation.Stop();
                CurrentAnimation.Reset();
            }

            CurrentAnimation = Animations[key];
            CurrentAnimation.Start();
        }

        public void Update(GameTime gameTime)
        {
            if (CurrentAnimation is not null)
            {
                CurrentAnimation.Update(gameTime);
            }
        }
    }
}
