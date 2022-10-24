﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyArcade.CAFramework;
using CrazyArcade.Blocks;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CrazyArcade.GameGridSystems;

namespace CrazyArcade.Items
{
    //Interface made to catagorise those that implement it, items
    public interface IItem : IEntity
    {
        
    }
    public abstract class Item : Block, IItem
    {

        public Item(Rectangle destination, Rectangle source, Texture2D texture, int frames, int fps) : base(destination, source, texture, frames, fps)
        {
            spriteAnimation = new SpriteAnimation(texture, frames, fps);
            GameCoord = new Vector2(destination.X, destination.Y);
            ScreenCoord = new Vector2(destination.X, destination.Y);
        }

        public override SpriteAnimation SpriteAnim => this.spriteAnimation;

        public override void Update(GameTime time)
        {

        }
        public override void Load()
        {
        }
    }
}
