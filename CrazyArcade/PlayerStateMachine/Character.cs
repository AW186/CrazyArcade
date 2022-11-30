﻿using System;
using CrazyArcade.BombFeature;
using CrazyArcade.CAFramework;
using CrazyArcade.Demo1;
using CrazyArcade.GameGridSystems;
using Microsoft.Xna.Framework;
using CrazyArcade.Items;
using CrazyArcade.CAFrameWork.GameStates;
using CrazyArcade.PlayerStateMachine.PlayerItemInteractions;
using CrazyArcade.Blocks;
using System.Diagnostics;
using CrazyArcade.UI;
using CrazyArcade.UI.GUI_Compositions;
using CrazyArcade.CAFrameWork.Transition;
using CrazyArcade.UI.GUI_Components;

namespace CrazyArcade.PlayerStateMachine
{
    /*
     * State machine is implemented here
     * 
     */
    public class Character: CharacterBase, IBombCollectable, IExplosionCollidable, IPlayerCollisionBehavior
    {
		public SpriteAnimation[] spriteAnims;
        public CAScene parentScene;
        public ICharacterState playerState;
        public int animationHandleInt;
        public int CurrentBlastLength { get => playerItems.BlastModifier; set { playerItems.BlastModifier = value; } }
        public int BombCapacity {get => playerItems.BombModifier; set { playerItems.BombModifier = value; } }
        public int FreeModifiedSpeed { get => playerItems.SpeedModifier; }
        private int bombOut;
        public int BombsOut => bombOut;
        static int CCount = 0;
        private int loseRideFlag = 5;
        public int lives;
        public int shields;
        public int needles;
        private int score = 0;
        public bool invincible = false;
        private int ICounter = 0;

        public override SpriteAnimation SpriteAnim => spriteAnims[animationHandleInt];
        
        public Character(bool isPirate) : base(isPirate)
        {
            //ModifiedSpeed = DefaultSpeed;
            this.isPirate = isPirate;
            playerState = new CharacterStateFree(this, isPirate);
            spriteAnims = playerState.SetSprites();
            playerState.SetSpeed();
            direction = Dir.Down;
            bombOut = 0;
            GameCoord = new Vector2(3, 3);
            //currentBlastLength = defaultBlastLength;
            DrawOrder = 1;
            lives = 2;
            Console.WriteLine("Count: " + ++CCount);
            needles = 2;
            shields = 2;
        }
        public override void Update(GameTime time)
        {
            ProcessInvincibility();
            playerState.ProcessState(time);
            //Console.WriteLine("bombsOut: " + BombsOut);
            base.Update(time);
        }
        private void ProcessInvincibility()
        {
            if(ICounter > 0)
            {
                invincible = true;
                ICounter--;
                if(ICounter <= 30)
                {
                    invincible = false;
                    ICounter = 0;
                }
            }
        }
        public void CollisionHaltLogic(Point move)
        {
            //GameCoord -= Trans.RevScale(new Vector2(move.X, move.Y));
        }

        //@implement IPlayerCollisionBehavior
        public void CollisionDestroyLogic()
        {
            if (this.playerState is CharacterStateBubble || invincible) return;
            if (this.playerState is CharacterStateTurtle )
            {
                
                this.playerState = new CharacterStateFree(this, isPirate);
                loseRideFlag = 0;
                ICounter = 30;
            }
            else if (loseRideFlag >= 5)
            {
                this.playerState = new CharacterStateBubble(this, isPirate);
            }
            else
            {
                loseRideFlag++;
            }
            
            this.spriteAnims = this.playerState.SetSprites();
            this.playerState.SetSpeed();
        }
        public override void Load()
        {
            if (!isPirate) UI_Singleton.ChangeComponentText("needle", "count", "X" + needles);
        }

        //@Implement IBombCollectable
        public void RecollectBomb()
        {
            bombOut = bombOut-- >= 0 ? bombOut-- : 0;
            Console.WriteLine("Recollect: " + BombsOut);
        }
        //@implement IItemCollidable
        public bool CanHaveItem()
        {
            return (playerState is CharacterStateFree || playerState is CharacterStateTurtle);
        }
        public void IncreaseBlastLength()
        {
            playerItems.AddItem(new BlastLengthModifier());
        }
        public void EnableKick()
        {
            playerItems.AddItem(new KickModifier());
        }
        public void SwitchToMountedState()
        {
            this.playerState = new CharacterStateTurtle(this, isPirate);
            spriteAnims = this.playerState.SetSprites();
            this.playerState.SetSpeed();
        }
        public void IncreaseSpeed()
        {
            playerItems.AddItem(new SpeedModifier());
        }
        public void IncreaseBombCount()
        {
            playerItems.AddItem(new BombCountModifier());
        }
        public void IncreaseScore(int score)
        {
            this.score += score;
			if (!isPirate) UI_Singleton.ChangeComponentText("score", "scoreText", "Score : " + this.score);
        }

        public void SpendBomb()
        {
            bombOut++;
            Console.WriteLine("Spend: " + BombsOut);
        }

        public bool Collide(IExplosion bomb)
        {
            CollisionDestroyLogic();
            return true;
        }
        public void Save(LevelPersnstance level)
        {
            //pirates should not be able to save at all, even if they somehow live
            if (isPirate) return;
            level.SavedStatInt.Add("playerScore", score);
            level.SavedStatInt.Add("playerLives", lives);
            level.SavedStatInt.Add("needle", needles);
            level.SavedStatInt.Add("shield", shields);
        }
        public void Load(LevelPersnstance level)
        {
            if (isPirate) return;
            if (level.SavedStatInt.ContainsKey("playerScore"))
            {
                score = level.SavedStatInt["playerScore"];
                UI_Singleton.ChangeComponentText("score", "scoreText", "Score : " + this.score);
            }
            if (level.SavedStatInt.ContainsKey("playerLives"))
            {
                lives = level.SavedStatInt["playerLives"];
				UI_Singleton.ChangeComponentText("lifeCounter", "count", "Lives: " + lives);
            }
            if (level.SavedStatInt.ContainsKey("needle"))
            {
                lives = level.SavedStatInt["shield"];
				UI_Singleton.ChangeComponentText("needle", "count", "X" + needles);
            }
            if (level.SavedStatInt.ContainsKey("shield"))
            {
                lives = level.SavedStatInt["shield"];
                UI_Singleton.ChangeComponentText("shield", "count", "X" + shields);
            }
        }
        public void SetInvincibilityTime(int iTime)
        {
            shields--;
            UI_Singleton.ChangeComponentText("shield", "itemCount", "X" + shields);
            //add shield effect here
            SceneDelegate.ToAddEntity(new IncincibilityBubble(this, iTime));
            ICounter = iTime;
        }
    }
}

