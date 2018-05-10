using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Terraria;
using Terraria.GameContent.UI.States;
using Terraria.UI;
using Terraria.World.Generation;

namespace TerrariaUltraApocalypse.Dimension
{
    class TUADimGenerator
    {
        private int dim_seed = 0;
        private List<GenPass> dimensionGenPass = new List<GenPass>();

        public TUADimGenerator(int seed) {
            dim_seed = seed;
        }

        public void AddPasses(GenPass pass) {
            dimensionGenPass.Add(pass);
        }

        public void GenerateDimension(GenerationProgress progress = null) {
            Stopwatch stopwatch = new Stopwatch();
            float totalWeight = 0;
            foreach (GenPass pass in dimensionGenPass) {
                totalWeight += pass.Weight;
            }
            if (progress == null) {
                progress = new GenerationProgress();
            }
            progress.TotalWeight = totalWeight;
            Main.menuMode = 888;
            Main.MenuUI.SetState((UIState)new UIWorldLoad(progress));
            foreach (GenPass pass in dimensionGenPass) {
                stopwatch.Start();
                progress.Start(pass.Weight);
                pass.Apply(progress);
                progress.End();
                stopwatch.Reset();
            }
        }


    }
}
