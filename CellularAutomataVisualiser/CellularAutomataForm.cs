using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using CellularAutomata;

namespace CellularAutomataVisualiser
{
    public partial class CellularAutomataForm : Form
    {
        ICellularAutomaton cellularAutomaton = null;
        MultiColorCellsGridDrawer multicolourCellsGridDrawer = null;
        SeveralColorsCellsGridDrawer severalColorCellsGridDrawer = null;

        private Graphics g;

        public CellularAutomataForm()
        {
            InitializeComponent();

            FHP fhp = new FHP(300, 200);
            fhp.SetRandomCellsState(5, FHPCellState.Particle);
            cellularAutomaton = fhp;
            
            //ForestFire forestFire = new ForestFire(150, 100);
            //forestFire.SetRandomCellsState(50, ForestFireCellState.Tree);
            //cellularAutomaton = forestFire;

            //SandPile sandPile = new SandPile(400, 300, ColorMode.Uniform);
            //sandPile.SetRandomCellsState(25, SandPileCellState.Sand);
            //sandPile.NeighboringRemainAtRestProbability = new Fraction(1, 4);
            //cellularAutomaton = sandPile;

            int cellWidth = 3;
            int cellHeight = 3;

            multicolourCellsGridDrawer = new MultiColorCellsGridDrawer(cellularAutomaton, cellWidth, cellHeight);
            severalColorCellsGridDrawer = new SeveralColorsCellsGridDrawer(cellularAutomaton, cellWidth, cellHeight);

            int width = severalColorCellsGridDrawer.CellWidth * cellularAutomaton.Width;
            int height = severalColorCellsGridDrawer.CellHeight * cellularAutomaton.Height;
            cellsGrid_pb.ClientSize = ClientSize = new Size(width, height);
            cellsGrid_pnl.ClientSize = new Size(width, height);

            //cellsGrid_pnl.BringToFront();
            cellsGrid_pb.BringToFront();

            cellsGrid_pb.Image = new Bitmap(width, height);
            g = Graphics.FromImage(cellsGrid_pb.Image);

            
            EngineTimer.Start();
            //cellsGrid_pb.Click += new EventHandler((s, e) => DrawingPerformaneTest());
        }

        private void DrawingPerformaneTest()
        {
            int steps = 200;
            var sw = System.Diagnostics.Stopwatch.StartNew();
            for (int i = 0; i < steps; i++)
            {
                cellularAutomaton.NextStep();
                cellsGrid_pb.Refresh();
            }
            long time = sw.ElapsedMilliseconds;
            sw.Stop();
            MessageBox.Show($"Steps = {steps}\nTime = {time}");
        }

        //private void Start()
        //{
        //    while (true)
        //    {
        //        cellsGrid_pb.Refresh();
        //        cellularAutomaton.NextStep();
        //    }
        //}

        private void EngineTimer_Tick(object sender, EventArgs e)
        {
            //cellsGridDrawer.Draw(g);
            cellsGrid_pb.Refresh();
            cellularAutomaton.NextStep();
        }

        private void cellsGrid_pb_Paint(object sender, PaintEventArgs e)
        {
            //multicolourCellsGridDrawer.Draw(e.Graphics);
            severalColorCellsGridDrawer.Draw(e.Graphics);
        }

        private void cellsGrid_pnl_Paint(object sender, PaintEventArgs e)
        {
            //cellsGridDrawer.Draw(e.Graphics);
        }

        private void cellsGrid_pb_Click(object sender, EventArgs e)
        {
            EngineTimer.Start();
        }

        //private void EngineTimer_Tick(object sender, EventArgs e)
        //{
        //    cellsGrid_pnl.Refresh();
        //    cellularAutomaton.NextStep();
        //}

        //private void cellsGrid_pb_Paint(object sender, PaintEventArgs e)
        //{
        //    //cellsGridDrawer.Draw(e.Graphics);
        //}

        //private void cellsGrid_pnl_Paint(object sender, PaintEventArgs e)
        //{
        //    cellsGridDrawer.Draw(e.Graphics);
        //}
        
    }
}
