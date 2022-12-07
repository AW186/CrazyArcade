﻿using CrazyArcade.CAFramework;
using CrazyArcade.Blocks;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using CrazyArcade.Content;
using CrazyArcade.Items;
using CrazyArcade.BombFeature;
using CrazyArcade.Levels;

namespace CrazyArcade.Blocks
{
    public class DefaultBlock : Block
    {
        public DefaultBlock(Vector2 position, CreateLevel.LevelItem type) : base(position, getSource(type), TextureSingleton.GetDesertBlocks())
        {
            
        }
		public DefaultBlock(Vector2 position, Rectangle rectangle) : base(position, rectangle, TextureSingleton.GetDesertBlocks())
		{

		}
		private static Rectangle getSource(CreateLevel.LevelItem type)
        {
            switch(type)
            {
                case CreateLevel.LevelItem.LightTreePosition:
                    return new Rectangle(10, 127, 63, 80);
                case CreateLevel.LevelItem.DarkTreePosition:
                    return new Rectangle(83, 127, 63, 80);
                //Default is Rock
                default:
                    return new Rectangle(110, 10, 40, 47);
            }
        }
    }

    public class LightSandBlock : BreakableBlock
    {
        private static Rectangle source = new Rectangle(10, 10, 40, 44);
        public LightSandBlock(Vector2 position) : base(position, source)
        {
        }
    }
    public class SandBlock : BreakableBlock
    {
        private static Rectangle source = new Rectangle(60, 10, 40, 44);
        public SandBlock(Vector2 position) : base(position, source)
        {
        }
    }
    public class Rock : DefaultBlock
    {
        private static Rectangle source = new Rectangle(110, 10, 40, 47);
        public Rock(Vector2 position) : base(position, source)
        {
        }
    }
    public class Tree : DefaultBlock
    {
        private static Rectangle source = new Rectangle(10, 127, 63, 80);
        public Tree(Vector2 position) : base(position, source)
        {
        }
    }
    public class DarkTree : DefaultBlock
    {
        private static Rectangle source = new Rectangle(83, 127, 63, 80);
        public DarkTree(Vector2 position) : base(position, source)
        {
        }
    }


}
