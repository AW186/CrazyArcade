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
    public class Tree : Block
    {
        public Tree(Vector2 position, CreateLevel.LevelItem type) : base(position, getSource(type), TextureSingleton.GetDesertBlocks())
        {
        }
        private static Rectangle getSource(CreateLevel.LevelItem type)
        {
            switch (type)
            {
                case CreateLevel.LevelItem.DarkTreePosition:
                    return new Rectangle(83, 127, 63, 80);
                //Default is Light tree
                default:
                    return new Rectangle(10, 127, 63, 80);
            }
        }
        public override void Load()
        {
            base.Load();
            base.spriteAnimation.Position.Y -= 18;
        }
    }
    public class VendingBlock : Block
    {
        public VendingBlock(Vector2 position, CreateLevel.LevelItem type) : base(position, getSource(type), TextureSingleton.GetDesertBlocks())
        {
        }
        private static Rectangle getSource(CreateLevel.LevelItem type)
        {
            switch (type)
            {
                case CreateLevel.LevelItem.BlueVendingPosition:
                    return new Rectangle(10, 481, 40, 67);
                case CreateLevel.LevelItem.RedVendingPosition:
                    return new Rectangle(60, 481, 40, 67);
                default: //Default is orange vending machine
                    return new Rectangle(110, 481, 40, 67);
            }
        }
        public override void Load()
        {
            base.Load();
            base.spriteAnimation.Position.Y -= 22;
        }
    }
}
