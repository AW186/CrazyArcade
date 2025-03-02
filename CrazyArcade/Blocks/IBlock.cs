﻿using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using CrazyArcade.CAFramework;
using CrazyArcade.GameGridSystems;
using CrazyArcade.BombFeature;
using CrazyArcade.CAFrameWork.GridBoxSystem;
using CrazyArcade.EnemyCollision;
using CrazyArcade.Enemies;
using System.Collections.Generic;
using CrazyArcade.CAFrameWork.UDPUpdateSystem;

namespace CrazyArcade.Blocks
{
    public abstract class Block : CAGridBoxEntity, IGridable, IExplosionCollidable, IEnemyCollidable, IDeserializable
    {

        //----------IGridable Start------------
        private Vector2 gamePos;
        private Vector2 pos;
        protected Rectangle enemyBoundingBox = new Rectangle(0, 0, 36, 36);
        public override Vector2 ScreenCoord
        {
            get => pos;
            set
            {
                pos = value;
                this.UpdateCoord(value);
            }
        }
        public void UpdateCoord(Vector2 value)
        {
            this.X = (int)value.X;
            this.Y = (int)value.Y;
        }
        public override Vector2 GameCoord
        {
            get => gamePos;
            set
            {
                gamePos = value;
                ScreenCoord = trans.Trans(value);
            }
        }
        private IGridTransform trans = new NullTransform();
        public override IGridTransform Trans { get => trans; set => trans = value; }
        //----------IGridable End------------
        protected SpriteAnimation spriteAnimation;

        private Rectangle internalRectangle = new Rectangle(0, 0, CAGameGridSystems.BlockLength, CAGameGridSystems.BlockLength);

        public Block(Vector2 position, Rectangle source, Texture2D texture) : base(new GridBoxPosition((int)position.X, (int)position.Y, (int)GridObjectDepth.Box))
        {
            spriteAnimation = new SpriteAnimation(texture, source);
            GameCoord = position;
            internalRectangle.X = X;
            internalRectangle.Y = Y;
            enemyBoundingBox.X = X;
            enemyBoundingBox.Y = Y;
        }
        public Block(Vector2 position, Rectangle source, Texture2D texture,int frames, int fps) : base(new GridBoxPosition((int)position.X, (int)position.Y, (int)GridObjectDepth.Box))
        {
            spriteAnimation = new SpriteAnimation(texture, frames, fps);
            GameCoord = position;
            internalRectangle.X = X;
            internalRectangle.Y = Y;
            enemyBoundingBox.X = X;
            enemyBoundingBox.Y = Y;
        }

        public override SpriteAnimation SpriteAnim => this.spriteAnimation;

        public Rectangle boundingBox => internalRectangle;

        public Rectangle EnemyBoundingBox => enemyBoundingBox;

        public override void Update(GameTime time)
        {
            internalRectangle.X = X;
            internalRectangle.Y = Y;
            enemyBoundingBox.X = X;
            enemyBoundingBox.Y = Y;
        }
        public override void Load()
        {
        }

        public virtual bool Collide(IExplosion bomb)
        {
            return false;
        }

        public void EnemyCollisionLogic(IEnemyCollisionBehavior collisionPartner)
        {
            collisionPartner.TurnEnemy();
        }


		public override bool IsSolid(Dir dir, bool couldKick)
        {
            return true;
        }
		public override HashSet<Point> PotentialDangerousTile()
        {
            return new HashSet<Point>();
        }

		public int UpdateFieldWithStream(byte[] stream, int offset)
		{
            return offset + 5;
		}

		int IDeserializable.GetType()
		{
            return UDPUpdateSystem.BlockType();
		}
	}
}