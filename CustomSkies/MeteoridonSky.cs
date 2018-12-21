using System;
using System.Collections.Generic;
using System.IO.Pipes;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Terraria.Graphics.Effects;

namespace TerrariaUltraApocalypse.CustomSkies
{
    class MeteoridonSky : CustomSky
    {
        private bool _isLeaving = false;
        private bool _isActive = false;

        public override void Activate(Vector2 position, params object[] args)
        {
            _isActive = true;
            _isLeaving = false;
        }

        public override void Deactivate(params object[] args)
        {
            _isActive = false;
            _isLeaving = true;
        }

        public override void Update(GameTime gameTime)
        {

        }

        public override void Draw(SpriteBatch spriteBatch, float minDepth, float maxDepth)
        {

        }

        public override bool IsActive()
        {
            return _isActive;
        }

        public override void Reset()
        {
            _isActive = false;
        }

        private struct meteor
        {
            private float rotation;
            private float scale;
            private Vector2 velocity;
            private float x;
            private float y;
            private int width;
            private int height;
        }
    }
}
