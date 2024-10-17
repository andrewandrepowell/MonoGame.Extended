using System;
using System.Linq;
using Microsoft.Xna.Framework;

namespace MonoGame.Extended.Sprites
{
    public class AnimatedSprite : Sprite
    {
        private readonly SpriteSheet _spriteSheet;
        private SpriteSheetAnimation _currentAnimation;        
        private class Functor0
        {
            public SpriteSheet SpriteSheet;
            public readonly Func<SpriteSheetAnimationFrame, TextureAtlases.TextureRegion2D> Func;
            public Functor0()
            {
                Func = f => SpriteSheet.TextureAtlas[f.Index];
            }
        }
        private static readonly Functor0 _functor0 = new();

        public AnimatedSprite(SpriteSheet spriteSheet, string playAnimation = null)
            : base(spriteSheet.TextureAtlas[0])
        {
            _spriteSheet = spriteSheet;

            if (playAnimation != null)
                Play(playAnimation);
        }

        public SpriteSheetAnimation Play(string name, Action onCompleted = null)
        {
            if (_currentAnimation == null || _currentAnimation.IsComplete || _currentAnimation.Name != name)
            {
                var cycle = _spriteSheet.Cycles[name];
                _functor0.SpriteSheet = _spriteSheet;
                var keyFrames = cycle.Frames.Select(_functor0.Func).ToArray();
                _currentAnimation = new SpriteSheetAnimation(name, keyFrames, cycle.FrameDuration, cycle.IsLooping, cycle.IsReversed, cycle.IsPingPong);

                if(_currentAnimation != null)
                    _currentAnimation.OnCompleted = onCompleted;
            }

            return _currentAnimation;
        }

        public void Update(float deltaTime)
        {
            if (_currentAnimation != null && !_currentAnimation.IsComplete)
            {
                _currentAnimation.Update(deltaTime);
                TextureRegion = _currentAnimation.CurrentFrame;
            }
        }

        public void Update(GameTime gameTime)
        {
            Update(gameTime.GetElapsedSeconds());
        }
    }
}
