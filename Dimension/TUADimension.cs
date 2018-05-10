using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.Generation;
using Terraria.ID;
using Terraria.IO;
using Terraria.World.Generation;
using TerrariaUltraApocalypse.Dimension.LunarDimension;

namespace TerrariaUltraApocalypse.Dimension
{
    class TUADimension
    {
        private static TUADimGenerator dimGenerator;
        public static List<WorldFileData> dimension = new List<WorldFileData>();
        private static FileMetadata solar;

        private static string currentDimName = "";


        public static void loader() {
            dimension.Clear();
            Directory.CreateDirectory(Main.WorldPath + "/" + Main.ActiveWorldFileData.Name + "/dim/");
            String[] fileName = Directory.GetFiles(Main.WorldPath + "/" + Main.ActiveWorldFileData.Name + "/dim/");
            int fileNumber = Math.Min(fileName.Length, 1);
            for (int i = 0; i < fileNumber; i++) {
                WorldFileData worldData = WorldFile.GetAllMetadata(fileName[i], false);
                dimension.Add(worldData);
            }
        }

        public void CallDimGen(String dimensionName) {

        }

        public void CreateDimension(GenerationProgress progress = null) {

            ThreadPool.QueueUserWorkItem(new WaitCallback(TUADimension.DimensionGenCallBack_), progress);
            
        }

        public static void DimensionGenCallBack_(object threadContext) {
            TUADimension.generateSolar(Main.ActiveWorldFileData.Seed, threadContext as GenerationProgress);
            TUADimFile.saveWorld();
        }

        public static void generateDimension(int seed, GenerationProgress customProgressObject = null) {
            //if (currentDimName == "solar") {
                generateSolar(seed, customProgressObject);
            //}

        }

        private static void generateSolar(int seed, GenerationProgress customProgressObject = null) {
            dimGenerator = new TUADimGenerator(seed);

            TUADimension.AddGenerationPass("An attempt toward dimension?", delegate (GenerationProgress progress) {
                for (int i = 0; i < Solar.maxTilesX; i++)
                {
                    for (int j = 0; j < Solar.maxTilesY / 4; j++)
                    {
                        Solar.tile[i, j] = new Tile();
                        Solar.tile[i, j].type = TileID.Dirt;
                    }
                }
            });

            TUADimension.AddGenerationPass("An attempt toward dimension? - Grass", delegate (GenerationProgress progress) {
                for (int j = 0; j < Solar.maxTilesY / 4; j++)
                {
                    Solar.tile[2400, j].type = TileID.Gold;
                }
            });

            TUADimension.AddGenerationPass("An attempt toward dimension? - Idk", delegate (GenerationProgress progress) {
                for (int i = 0; i < Solar.maxTilesX; i++)
                {
                    Solar.tile[i, 1200].type = TileID.Adamantite;
                    
                }
            });

            dimGenerator.GenerateDimension(customProgressObject);
            solar = FileMetadata.FromCurrentSettings(FileType.World);
            
        }

        private static void AddGenerationPass(string name, WorldGenLegacyMethod method)
        {
            TUADimension.dimGenerator.AddPasses(new PassLegacy(name, method));
        }
    }
}
