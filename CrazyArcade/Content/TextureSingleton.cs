﻿using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using System.Collections.Generic;

namespace CrazyArcade.Content
{
    //How to use:
    //in Initialize
    //TextureSingleton.LoadAllTextures(this.Content);
    //In Draw or your method:
    //Texture2D texture = TextureSingleton.GetPlayer1();
    //This only returns Textures2D
    public class TextureSingleton
    {
        public static string[] fileNames;
        public static Texture2D[] spriteSheets;
        //Eager Initialization
        private static TextureSingleton instance = new TextureSingleton();
        private TextureSingleton() { }
        public static TextureSingleton Instance {
            get 
            {
                return instance; 
            } 
        }
        private static void DefineArrays()
        {
            fileNames = new string[] {"Balloons","BombEnemy", "Bubble", "DesertBlocks", "Pirate", "Player1", "Powerups", "Rides"};
            spriteSheets = new Texture2D[fileNames.Length];
        }
        public static void LoadAllTextures(ContentManager content)
        {
            DefineArrays();
            for (int i = 0; i < fileNames.Length; i++)
            {
                spriteSheets[i] = content.Load<Texture2D>(fileNames[i]);
            }
        }
        
        public static Texture2D GetBallons()
        {
            return spriteSheets[0];
        }

        public static Texture2D GetBombEnemy()
        {
            return spriteSheets[1];
        }
        public static Texture2D GetBubble()
        {
            return spriteSheets[2];
        }

        public static Texture2D GetDesertBlocks()
        {
            return spriteSheets[3];
        }

        public static Texture2D GetPirate()
        {
            return spriteSheets[4];

        }

        public static Texture2D GetPlayer1()
        {
            return spriteSheets[5];
        }

        public static Texture2D GetPowerUps()
        {
            return spriteSheets[6];
        }
        public static Texture2D GetRides()
        {
            return spriteSheets[7];
        }
        


    }
}