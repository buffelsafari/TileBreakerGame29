using BreakOut.Entities.Static;
using BreakOut.EntityMana;
using Microsoft.Xna.Framework;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.IsolatedStorage;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using static BreakOut.Entities.Static.StaticEntityFactory;
using static TileBreakerGame29.Entities.Static.Effects.EffectFactory;

namespace BreakOut
{
    class Level
    {            
        private List<AbstractStaticEntity> staticEntityList;

        public delegate void LoadingComplete();
        public delegate void SavingComplete();
        public delegate void LoadingFailed();

        public LoadingComplete loadingComplete;        
        public LoadingFailed loadingFailed;

        public Level(Game game, EntityManager entityManager)
        { 
            staticEntityList = new List<AbstractStaticEntity>();
            
            staticEntityList.Add(StaticEntityFactory.Create(game, entityManager, StaticEntityShape.LeftWall, 0, 0, Color.White,1, EffectType.solidWall, ColorType.rainbow));
            staticEntityList.Add(StaticEntityFactory.Create(game, entityManager, StaticEntityShape.RightWall, 0, 0, Color.White,1, EffectType.solidWall, ColorType.rainbow));
            staticEntityList.Add(StaticEntityFactory.Create(game, entityManager, StaticEntityShape.UpperWall, 0, 0, Color.White,1, EffectType.solidWall, ColorType.rainbow));

            for(int counterY=3; counterY< 8 ;counterY++)
            for (int counterX = 0; counterX < 13; counterX++)
            {
                staticEntityList.Add(StaticEntityFactory.Create(game, entityManager, StaticEntityShape.Hexagon, 7+82*counterX, 7+72*counterY, Color.White,2, EffectType.multiBall, ColorType.rainbow));
            }
        }

        public AbstractStaticEntity[] GetTiles()
        {
            return staticEntityList.ToArray();
        }

        public void SetLevel(List<Tile> tiles, UpperWall upperWall, LeftWall leftWall, RightWall rightWall)
        {
            staticEntityList.Clear();
            staticEntityList.Add(leftWall);
            staticEntityList.Add(rightWall);
            staticEntityList.Add(upperWall);
            staticEntityList.AddRange(tiles);
        }

        public void Load(Game game, EntityManager entityManager, string str, bool editorFiles)
        {            
             
            Task.Run(() =>
            {
                try
                {
                    if (editorFiles)
                    {
                        //string dirPath = Game.Activity.ApplicationContext.GetExternalFilesDir(string.Empty).AbsolutePath;
                        //string filePath = Path.Combine(dirPath, str);
                        IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                        using (Stream stream = store.OpenFile(str, FileMode.Open))
                        //using (Stream stream = File.OpenRead(filePath))
                        {
                            staticEntityList.Clear();
                            ReadData(stream, game, entityManager);
                        }
                    }
                    else
                    {
                        string dirPath = game.Content.RootDirectory;                        
                        string filePath = Path.Combine(dirPath, "levels/"+str);                                               
                        using (Stream stream = TitleContainer.OpenStream(filePath))
                        {
                            staticEntityList.Clear();
                            ReadData(stream, game, entityManager);
                        }
                    }

                    loadingComplete?.Invoke();
                    GC.Collect();
                    
                }
                catch (Exception)
                {
                    loadingFailed?.Invoke();                    
                } 
            });
        }

        private void ReadData(Stream stream, Game game, EntityManager entityManager)
        {
            BinaryReader binaryReader = new BinaryReader(stream);
            binaryReader.ReadString();
            int numberOfEntities = (int)binaryReader.ReadInt16();

            for (int i = 0; i < numberOfEntities; i++)
            {
                int x = (int)binaryReader.ReadInt16();
                int y = (int)binaryReader.ReadInt16();

                byte r = binaryReader.ReadByte();
                byte g = binaryReader.ReadByte();
                byte b = binaryReader.ReadByte();
                byte a = binaryReader.ReadByte();

                byte hp = binaryReader.ReadByte();

                StaticEntityShape sh = (StaticEntityShape)binaryReader.ReadInt16();
                EffectType et = (EffectType)binaryReader.ReadInt16();
                ColorType ct = (ColorType)binaryReader.ReadInt16();

                binaryReader.ReadInt32();
                binaryReader.ReadInt32();
                binaryReader.ReadInt32();

                staticEntityList.Add(StaticEntityFactory.Create(game, entityManager, sh, x, y, new Color(r, g, b, a), hp, et, ct));
            }
        }

        public void Delete(Game game, string str)
        {
            
            try
            {
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                if (store.FileExists(str))
                {
                    store.DeleteFile(str);
                }

                
            }
            catch (Exception)
            { 
            
            }
        }

        public void Save(Game game, string str)
        {
            

            try
            {
                IsolatedStorageFile store = IsolatedStorageFile.GetUserStoreForApplication();
                using (Stream stream = store.OpenFile(str, FileMode.Create))                
                {
                    BinaryWriter binaryWriter = new BinaryWriter(stream);
                    binaryWriter.Write("levelfile1.0");
                    binaryWriter.Write((Int16)staticEntityList.Count);
                    foreach (AbstractStaticEntity entity in staticEntityList)
                    {
                        entity.Save(binaryWriter);
                    }
                }
            }
            catch (Exception)
            { 
            
            }


             
        }


    
    }
}