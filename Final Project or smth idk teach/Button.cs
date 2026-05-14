using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using System.Collections.Generic;

namespace Final_Project_or_smth_idk_teach
{
    public class Button
    {
        private Texture2D _texture;
        private Vector2 _position;
        private float _scale;

        // These are no longer constants
        private float _normalScale;
        private float _hoverScale;
        private const float LerpSpeed = 0.15f;

        public Rectangle Hitbox { get; private set; }
        public bool IsClicked { get; private set; }

        // Update constructor to accept scale values
        public Button(Texture2D texture, Vector2 position, float normalScale = 0.4f, float hoverScale = 0.6f)
        {
            _texture = texture;
            _position = position;
            _normalScale = normalScale;
            _hoverScale = hoverScale;
            _scale = _normalScale; // Start at normal size
        }

        public void Update(MouseState mouseState, MouseState prevMouseState)
        {
            int scaledWidth = (int)(_texture.Width * _scale);
            int scaledHeight = (int)(_texture.Height * _scale);

            Hitbox = new Rectangle(
                (int)_position.X - (scaledWidth / 2),
                (int)_position.Y - (scaledHeight / 2),
                scaledWidth,
                scaledHeight
            );

            if (Hitbox.Contains(mouseState.Position))
            {
                _scale = MathHelper.Lerp(_scale, _hoverScale, LerpSpeed);
            }
            else
            {
                _scale = MathHelper.Lerp(_scale, _normalScale, LerpSpeed);
            }

            IsClicked = Hitbox.Contains(mouseState.Position) &&
                        mouseState.LeftButton == ButtonState.Pressed &&
                        prevMouseState.LeftButton == ButtonState.Released;
        }

        public void Draw(SpriteBatch spriteBatch)
        {
            Vector2 origin = new Vector2(_texture.Width / 2f, _texture.Height / 2f);
            spriteBatch.Draw(_texture, _position, null, Color.White, 0f, origin, _scale, SpriteEffects.None, 0f);
        }
    }
}