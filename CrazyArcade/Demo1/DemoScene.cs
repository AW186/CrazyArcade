﻿using CrazyArcade.Boss;
using CrazyArcade.Levels;
using CrazyArcade.BombFeature;
using CrazyArcade.CAFramework;
using CrazyArcade.CAFramework.Controller;
using CrazyArcade.PlayerStateMachine;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CrazyArcade.Enemies;
using CrazyArcade.CAFrameWork.CollisionSystem;
using CrazyArcade.GameGridSystems;
using Microsoft.Xna.Framework;
using System.Diagnostics;

namespace CrazyArcade.Demo1
{
    public class DemoScene : CAScene
    {

        public DemoScene(Game1 game)
        {
            gameRef = game;
        }
        public override void LoadSystems()
        {
            //this.systems.Add(new BlockCollisionSystem());
            this.systems.Add(new ItemCollisionSystem(this));
            this.systems.Add(new CAControllerSystem());
            this.systems.Add(new CAGameLogicSystem());

            this.systems.Add(new BombCollisionSystem(this));
            this.systems.Add(new PlayerCollisionSystem());
            
            this.systems.Add(new CAGameGridSystems(new Vector2(0, 0), 40));
            this.systems.Add(new LevelManager(this));
        }

        public override void LoadSprites()
        {
            this.AddSprite(new PlayerCharacter(new DemoController(), this));
        }

    }
}
