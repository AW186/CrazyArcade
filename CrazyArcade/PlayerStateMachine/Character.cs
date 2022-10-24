﻿using System;
using CrazyArcade.BombFeature;
using CrazyArcade.CAFramework;
using CrazyArcade.Demo1;
using CrazyArcade.GameGridSystems;
using Microsoft.Xna.Framework;

namespace CrazyArcade.PlayerStateMachine
{
    public abstract class Character : CharacterBase, IBombCollectable
    {
		public SpriteAnimation[] spriteAnims;
        public CAScene parentScene;
        public ICharacterState playerState;
        public int animationHandleInt;
        public int currentBlastLength;
        public int bombCapacity = 4;
        public int bombsOut;

        public override SpriteAnimation SpriteAnim => spriteAnims[animationHandleInt];

        public Character(CAScene scene)
        {
            ModifiedSpeed = DefaultSpeed;
            playerState = new CharacterStateFree(this);
            spriteAnims = playerState.SetSprites();
            playerState.SetSpeed();
            direction = Dir.Down;
            this.parentScene = scene;
            bombsOut = 0;
            GameCoord = new Vector2(3, 3);
            currentBlastLength = defaultBlastLength;
            //this.bboxOffset = new Point(20, 20);
        }
        public override void Update(GameTime time)
        {
            playerState.ProcessState(time);
            base.Update(time);
        }
        public override void CollisionDestroyLogic()
        {
            if (this.playerState is CharacterStateBubble) return;
            this.playerState = new CharacterStateBubble(this);
            this.spriteAnims = this.playerState.SetSprites();
            this.playerState.SetSpeed();
        }
        public override void Load()
        {

        }


        public void recollectBomb()
        {
            bombsOut = bombsOut-- >= 0 ? bombsOut-- : 0;
        }

        public void IncreaseBlastLength()
        {
            this.currentBlastLength++;
        }
        public void SwitchToMountedState()
        {
            this.playerState.ProcessRide();
        }
        public void IncreaseSpeed()
        {
            this.ModifiedSpeed++;
        }
        public void IncreaseBombCount()
        {
            this.bombCapacity++;
        }
        public void AddCoin(int toAdd)
        {

        }
    }
}

