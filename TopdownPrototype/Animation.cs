using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace TopdownPrototype
{
    public class Animation
    {
        public Texture2D Texture { get; }
        public Rectangle SourceRectangle { get; private set; }
        private readonly List<Rectangle> sourceRectangles = new();
        private readonly int frames;
        private int frame;
        private readonly float frameTime;
        private float frameTimeLeft;
        private bool active = true;

        public Animation(Texture2D texture, int frames, float frameTime)
        {
            Texture = texture;
            this.frameTime = frameTime;
            frameTimeLeft = this.frameTime;
            this.frames = frames;
            int frameWidth = texture.Width / this.frames;
            int frameHeight = texture.Height;

            for (int i = 0; i < frames; i++)
            {
                sourceRectangles.Add(new(i * frameWidth, 0, frameWidth, frameHeight));
            }
            SourceRectangle = new(0, 0, frameWidth, frameHeight);
        }

        public void Start()
        {
            active = true;
        }

        public void Reset()
        {
            frame = 0;
            frameTimeLeft = frameTime;
            SourceRectangle = sourceRectangles[frame];
        }

        public void Stop()
        {
            active = false;
        }

        public void Update(GameTime gameTime)
        {
            if (active)
            {
                float deltaTime = (float)gameTime.ElapsedGameTime.TotalSeconds;
                frameTimeLeft -= deltaTime;

                if (frameTimeLeft <= 0)
                {
                    frameTimeLeft = frameTime;

                    //frame = (frame + 1) % frames; // less readable, more compact, but compiler optimizes regardless

                    frame++;
                    if (frame >= frames)
                    {
                        frame = 0;
                    }
                    SourceRectangle = sourceRectangles[frame];
                }
            }
        }
    }
}
