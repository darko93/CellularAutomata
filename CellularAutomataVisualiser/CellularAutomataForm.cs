﻿using System;
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

        private int state = 0;

        public CellularAutomataForm()
        {
            InitializeComponent();

            //FHP fhp = new FHP(400, 400);
            //fhp.SetRandomCellsState(FHPCellState.Particle, 75);
            //cellularAutomaton = fhp;
            //CellularAutomata.Point center = new CellularAutomata.Point((int)(fhp.Width * 0.5), (int)(fhp.Height * 0.5));
            //CellularAutomata.Point point = new CellularAutomata.Point(0, 0);
            //for (int x = 0; x < fhp.Width; x++)
            //{
            //    for (int y = 0; y < fhp.Height; y++)
            //    {
            //        point.X = x;
            //        point.Y = y;
            //        if (point.SquaredDistanceFrom(center) <= 900)
            //            //fhp.SetCellState(FHPCellState.Particle, x, y);
            //            fhp.SetParticleCellState(FHPParticleCellState.Northeast | FHPParticleCellState.East | FHPParticleCellState.Southeast |
            //                FHPParticleCellState.Southwest | FHPParticleCellState.West | FHPParticleCellState.Northwest | FHPParticleCellState.Rest, x, y);
            //    }
            //}

            //FHP fhp = new FHP(300, 300);
            //fhp.SetRandomCellsState(FHPCellState.Particle, 75, new CellularAutomata.Point(3, 3), new CellularAutomata.Point(75, fhp.Height - 4));
            //fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(76, 0), new CellularAutomata.Point(79, (int)(fhp.Height * 0.5 - 15)));
            //fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(76, (int)(fhp.Height * 0.5 + 15)), new CellularAutomata.Point(79, fhp.Height - 1));
            //fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(0, fhp.Height - 3), new CellularAutomata.Point(fhp.Width - 1, fhp.Height - 1));
            //fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(0, 0), new CellularAutomata.Point(2, fhp.Height - 1));
            //fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(0, 0), new CellularAutomata.Point(fhp.Width - 1, 2));
            //fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(fhp.Width - 3, 0), new CellularAutomata.Point(fhp.Width - 1, fhp.Height - 1));
            ////fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(0, 0), new CellularAutomata.Point(fhp.Width - 1, 0));
            ////fhp.SetRandomCellsState(FHPCellState.Particle, 2);
            //cellularAutomaton = fhp;

            //ForestFire forestFire = new ForestFire(150, 150);
            //forestFire.SetRandomCellsState(ForestFireCellState.Tree, 10);
            //forestFire.GrowProbability = new Fraction(1, 150);
            ////forestFire.BurnStepsAmount = 10;
            //forestFire.SpontanBurnProbability = new Fraction(1, 250000);
            //forestFire.BurnStepsAmount = 1;
            //forestFire.BurnFromNeighborProbability = new Fraction(1, 1);
            //cellularAutomaton = forestFire;

            //SandPile sandPile = new SandPile(200, 200, ColorMode.SlightlyDifferent);
            //sandPile.SetRandomCellsState(SandPileCellState.SandGrain, 25);
            //sandPile.SetRandomCellsState(SandPileCellState.Wall, 100, new CellularAutomata.Point(0, 0), new CellularAutomata.Point(0, sandPile.Height - 1));
            //sandPile.SetRandomCellsState(SandPileCellState.Wall, 100, new CellularAutomata.Point(sandPile.Width - 1, 0), new CellularAutomata.Point(sandPile.Width - 1, sandPile.Height - 1));
            //sandPile.SetRandomCellsState(SandPileCellState.Wall, 100, new CellularAutomata.Point(0, 0), new CellularAutomata.Point(sandPile.Width - 1, 0));
            //sandPile.SetRandomCellsState(SandPileCellState.Wall, 100, new CellularAutomata.Point(0, sandPile.Height - 1), new CellularAutomata.Point(sandPile.Width - 1, sandPile.Height - 1));
            ////sandPile.NeighboringRemainAtRestProbability = new Fraction(1, 4);
            //cellularAutomaton = sandPile;

            ////SandPile sandPile = new SandPile(201, 351);
            //SandPile sandPile = new SandPile(151, 231, ColorMode.SlightlyDifferent);
            //CellularAutomata.Point center = new CellularAutomata.Point((int)(sandPile.Width * 0.5), (int)(sandPile.Height * 0.5));
            //int counter = 0;
            //int distanceFromCenter = 3;
            //int hourGlassThickness = 7;
            //for (int y = center.Y - 2; y >= 0; y--)
            //{
            //    counter++;
            //    if (counter % 2 == 0)
            //        distanceFromCenter++;
            //    for (int x = center.X - distanceFromCenter - hourGlassThickness; x < center.X - distanceFromCenter; x++)
            //        sandPile.SetCellState(SandPileCellState.Wall, x, y);
            //    for (int x = center.X + distanceFromCenter + hourGlassThickness; x >= center.X + distanceFromCenter; x--)
            //        sandPile.SetCellState(SandPileCellState.Wall, x, y);

            //    if (y > 10)
            //    {
            //        for (int x = center.X - distanceFromCenter; x < center.X + distanceFromCenter; x++)
            //            sandPile.SetCellState(SandPileCellState.SandGrain, x, y);
            //    }
            //}
            //distanceFromCenter = 3;
            //counter = 0;
            //for (int y = center.Y; y < sandPile.Height; y++)
            //{
            //    counter++;
            //    if (counter % 2 == 0)
            //        distanceFromCenter++;
            //    for (int x = center.X - distanceFromCenter - hourGlassThickness; x < center.X - distanceFromCenter; x++)
            //        sandPile.SetCellState(SandPileCellState.Wall, x, y);
            //    for (int x = center.X + distanceFromCenter + hourGlassThickness; x >= center.X + distanceFromCenter; x--)
            //        sandPile.SetCellState(SandPileCellState.Wall, x, y);
            //}
            //cellularAutomaton = sandPile;

            int cellWidth = 2;
            int cellHeight = 2;

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

            
            //EngineTimer.Start();
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
            if (state == 0)
            {
                EngineTimer.Start();
                state = 1;
            }
            else
            {
                EngineTimer.Stop();
                state = 0;
            }
            //StepCA(70);
        }

        private void StepCA(int stepsAmount)
        {
            for (int i = 0; i < stepsAmount; i++)
            {
                cellularAutomaton.NextStep();
                cellsGrid_pb.Refresh();
            }
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
