﻿using CrazyArcade.Demo1;
using CrazyArcade.Enemies;
using CrazyArcade.Boss;
using CrazyArcade.CAFramework;
using CrazyArcade.Items;
using CrazyArcade.Blocks;
using CrazyArcade.PlayerStateMachine;
using System.Collections.Generic;
using Vector2 = Microsoft.Xna.Framework.Vector2;
using Rectangle = Microsoft.Xna.Framework.Rectangle;

namespace CrazyArcade.Levels
{
    internal class Level
    {
        

        private List<CAEntity> EntityList;
        private CAScene Scene;
        private CreateLevel currentLevel;
        private Vector2[] itemLocations;
        private CAEntity Entity;
        float scale;
        Vector2 border;
        Vector2 startPosition;

        public Level(CAScene scene, string levelName)
        {
            currentLevel = new CreateLevel(levelName);
            this.Scene = scene;
            EntityList = new List<CAEntity>();
            LoadSprites();
            LoadBorder();
            EntityList.Add(new PlayerCharacter(new DemoController(), Scene));

        }
        public List<CAEntity> DrawLevel()
        {
            return EntityList;
        }
        
        public void DeleteLevel()
        {
            foreach (CAEntity entity in EntityList)
            {
                Scene.RemoveSprite(entity);
            }
        }
        
        
        private void LoadBorder()
        {
            scale = .9f;
            border = currentLevel.GetBorder();
            for (int i = (int)border.X; i >= 0; i--)
            {
                LoadStone(i, -1);
                LoadStone(i - 1, (int)border.Y - 1);
            }
            for (int i = (int)border.Y; i >= 0; i--)
            {
                LoadStone(-1, i - 1);
                LoadStone((int)border.X, i - 1);
            }
        }
        private void LoadStone(int X, int Y)
        {

            startPosition = currentLevel.GetStartPosition(new int[2] { X, Y });
            Entity = new LightSandBlock(startPosition);
            Entity.SpriteAnim.Scale = scale;
            EntityList.Add(Entity);
        }
        private void LoadSprites()
        {

            
            //TODO Find a way to reduce duplicate code
            scale = .9f;
            //IMPORTANT!!!! uncomment tbis when Door Block class is implemented.
            //itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.DoorPosition);

            //foreach (Vector2 vector in itemLocations)
            //{
                
            //    Entity = new Door(vector);
            //    Entity.SpriteAnim.Scale = scale;
            //    EntityList.Add(Entity);
            //}
            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.LightSandPosition);

            foreach (Vector2 vector in itemLocations)
            {

                Entity = new LightSandBlock(vector);
                Entity.SpriteAnim.Scale = scale;
                EntityList.Add(Entity);
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.DarkSandPosition);

            foreach (Vector2 vector in itemLocations)
            {
                
                Entity = new SandBlock(vector);
                Entity.SpriteAnim.Scale = scale;
                EntityList.Add(Entity);
            }
            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.StonePosition);

            foreach (Vector2 vector in itemLocations)
            {
                
                Entity = new Rock(vector);
                Entity.SpriteAnim.Scale = scale;
                EntityList.Add(Entity);
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.LightTreePosition);

            foreach (Vector2 vector in itemLocations)
            {
                
                Entity = new Tree(vector);
                Entity.SpriteAnim.Scale = scale;
                EntityList.Add(Entity);
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.DarkTreePosition);

            foreach (Vector2 vector in itemLocations)
            {
                
                Entity = new DarkTree(vector);
                Entity.SpriteAnim.Scale = .9f;
                EntityList.Add(Entity);
            }


            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.CactusPosition);

            foreach (Vector2 vector in itemLocations)
            {
                
                Entity = new Cactus(vector);
                Entity.SpriteAnim.Scale = .9f;
                EntityList.Add(Entity);
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.CoinBagPosition);

            foreach (Vector2 vector in itemLocations)
            {
               
                Entity = new CoinBag(vector);
                EntityList.Add(Entity);
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.BalloonPosition);

            foreach (Vector2 vector in itemLocations)
            {

                
                EntityList.Add(new Balloon(vector));

            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.SneakerPosition);

            foreach (Vector2 vector in itemLocations)
            {

                
                EntityList.Add(new Sneaker(vector));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.TurtlePosition);

            foreach (Vector2 vector in itemLocations)
            {
                
                EntityList.Add(new Turtle(vector));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.PotionPosition);

            foreach (Vector2 vector in itemLocations)
            {
                
                EntityList.Add(new Potion(vector));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.CoinPosition);

            foreach (Vector2 vector in itemLocations)
            {
                
                EntityList.Add(new Coin(vector));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.BombPosition);

            foreach (Vector2 vector in itemLocations)
            {
                EntityList.Add(new BombEnemySprite((int)vector.X, (int)vector.Y, Scene));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.SquidPosition);

            foreach (Vector2 vector in itemLocations)
            {
                EntityList.Add(new SquidEnemySprite((int)vector.X, (int)vector.Y, Scene));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.BatPosition);

            foreach (Vector2 vector in itemLocations)
            {
                EntityList.Add(new BatEnemySprite((int)vector.X, (int)vector.Y, Scene));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.RobotPosition);

            foreach (Vector2 vector in itemLocations)
            {
                EntityList.Add(new RobotEnemySprite((int)vector.X, (int)vector.Y, Scene));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.OctoBossPosition);

            foreach (Vector2 vector in itemLocations)
            {
                EntityList.Add(new OctopusEnemy((int)vector.X, (int)vector.Y));
            }

            itemLocations = currentLevel.GetItemLocation(CreateLevel.LevelItem.SunBossPosition);

            foreach (Vector2 vector in itemLocations)
            {
                EntityList.Add(new SunBoss(Scene));
            }


        }
    }
}
