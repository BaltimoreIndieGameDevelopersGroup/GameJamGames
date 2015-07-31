using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;
using Zombie.Components.General;

namespace Zombie.Components.Entities
{
    /// <summary>
    /// BIG NOTE: .... this class is WAAAAAAAAYYYYY unstable because it was written in notepad without enough
    /// time to perform analysis and design. It was a PITA to get to compile. It will likely need to be tweaked
    /// but this is the initial Idea.
    /// </summary>
        public class Level{
        private Level(){} //private ctor
        public const string LEVEL_DIRECTORY = "Levels";
        public string Name {get;set;}
        public Texture2D LevelMap {get;set;}
        public Texture2D CollisionMap {get;set;}
        public List<Actor> Actors {get;set;}
        public LeveLData LevelData {get;set;}
        public string RawLevelData {get;set;}     

        // a static method for constructing levels
        public static Level Load(string Name, ScreenManager sm)
        {
            
            //loads levelimage, collision imagage, level data file               
            var content = new ContentManager(sm.Game.Services, "Content");

            Level lvl = new Level();
            lvl.Name = Name;
            lvl.LevelMap = content.Load<Texture2D>(LEVEL_DIRECTORY + "//" + Name); //Required            
            lvl.CollisionMap = content.Load<Texture2D>(LEVEL_DIRECTORY + "//" + Name + "_coll");
            string filePath = content.RootDirectory + "//" + LEVEL_DIRECTORY + "//" + Name + ".lvl";
            lvl.RawLevelData = File.ReadAllText(filePath);
            // lvl.LevelData = JsonConvert.DeserializeObject<Of List<Actor>>(RawLevelData);
            lvl.Actors = lvl.LevelData.Actors;

            return lvl;
            //return null;
        }

    }

    public class LeveLData
    {
        public List<Actor> Actors {get;set;}
        public string MapName {get;set;}
        public Rectangle CameraStart {get;set;}
        public Vector2 PlayerStart {get;set;}        
    }

    
}
