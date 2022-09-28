﻿using CrazyArcade.BombFeature;
using CrazyArcade.CAFramework;
using CrazyArcade.Content;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CrazyArcade.PlayerStateMachine
{
    internal class PlayerStateFree : ICharacterState
    {
        private PlayerCharacter character;
        public SpriteAnimation[] spriteAnims;
        public PlayerStateFree(PlayerCharacter character)
        {
            this.spriteAnims = new SpriteAnimation[4];
            this.character = character;
        }
        public SpriteAnimation[] SetSprites()
        {
            spriteAnims[0] = new SpriteAnimation(TextureSingleton.GetPlayer1(), 12, 14, 44, 56, 6, 4, 10);
            spriteAnims[1] = new SpriteAnimation(TextureSingleton.GetPlayer1(), 12, 78, 44, 56, 6, 4, 10);
            spriteAnims[2] = new SpriteAnimation(TextureSingleton.GetPlayer1(), 12, 142, 44, 56, 6, 4, 10);
            spriteAnims[3] = new SpriteAnimation(TextureSingleton.GetPlayer1(), 12, 206, 44, 56, 6, 4, 10);
            return spriteAnims;
        }
        public void ProcessState(GameTime time)
        {
            character.CalculateMovement();
            character.UpdatePosition();
            character.animationHandleInt = (int)character.direction;
            if (character.CurrentSpeed.X == 0 && character.CurrentSpeed.Y == 0)
            {
                character.SpriteAnim.playing = false;
                character.SpriteAnim.setFrame(0);
            }
            else
            {
                character.SpriteAnim.playing = true;
            }
            if (Keyboard.GetState().IsKeyDown(Keys.X))
            {
                character.playerState = new PlayerStateBubble(character);
                character.spriteAnims = character.playerState.SetSprites();
                character.playerState.SetSpeed();
            }
        }
        public void ProcessItem()
        {
            //nothing yet
        }
        public void ProcessRide()
        {
            //nothing yet
        }
        public int SetSpeed()
        {
            character.ModifiedSpeed = character.DefaultSpeed;
            return 1;
        }
        public void ProcessAttaction()
        {
            character.parentScene.AddSprite(new WaterBomb(character.parentScene, character.X, character.Y, character.currentBlastLength));
        }
    }
}
