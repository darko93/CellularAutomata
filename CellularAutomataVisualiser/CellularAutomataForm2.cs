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
    public partial class CellularAutomataForm2 : Form
    {
        private int cellSize = 2;
        private int drawersColumnsAmount = 2;
        private int drawersRowsAmount = 2;
        private PictureBox[] pictureBoxes;
        private Graphics[] graphicses;
        private CellsGridDrawer[] cellsGridDrawers;
        private Task[] drawTasks;
        private ICellularAutomaton cellularAutomaton;

        public CellularAutomataForm2()
        {
            InitializeComponent();

            InitializeCellularAutomaton();
            InitializeControls();
            engineTimer.Start();
        }

        private void InitializeCellularAutomaton()
        {
            FHP fhp = new FHP(300, 300);
            fhp.SetRandomCellsState(FHPCellState.Particle, 75, new CellularAutomata.Point(3, 3), new CellularAutomata.Point(75, fhp.Height - 4));
            fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(76, 0), new CellularAutomata.Point(79, (int)(fhp.Height * 0.5 - 15)));
            fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(76, (int)(fhp.Height * 0.5 + 15)), new CellularAutomata.Point(79, fhp.Height - 1));
            fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(0, fhp.Height - 3), new CellularAutomata.Point(fhp.Width - 1, fhp.Height - 1));
            fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(0, 0), new CellularAutomata.Point(2, fhp.Height - 1));
            fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(0, 0), new CellularAutomata.Point(fhp.Width - 1, 2));
            fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(fhp.Width - 3, 0), new CellularAutomata.Point(fhp.Width - 1, fhp.Height - 1));
            //fhp.SetRandomCellsState(FHPCellState.Wall, 100, new CellularAutomata.Point(0, 0), new CellularAutomata.Point(fhp.Width - 1, 0));
            //fhp.SetRandomCellsState(FHPCellState.Particle, 2);
            cellularAutomaton = fhp;

            //ForestFire forestFire = new ForestFire(200, 200);
            ////forestFire.SetRandomCellsState(ForestFireCellState.Tree, 10);
            //forestFire.GrowProbability = new Fraction(1, 150);
            //forestFire.BurnStepsAmount = 10;
            //forestFire.SpontanBurnProbability = new Fraction(1, 350000);
            //forestFire.BurnFromNeighborProbability = new Fraction(1, 5);
            //cellularAutomaton = forestFire;
        }

        private void InitializeControls()
        {
            int drawersAmount = drawersColumnsAmount * drawersRowsAmount;
            pictureBoxes = new PictureBox[drawersAmount];
            graphicses = new Graphics[drawersAmount];
            cellsGridDrawers = new CellsGridDrawer[drawersAmount];
            drawTasks = new Task[drawersAmount];

            int drawerCellWidth = (int)Math.Ceiling((double)(cellularAutomaton.Width / drawersColumnsAmount));
            int drawerCellHeight = (int)Math.Ceiling((double)(cellularAutomaton.Height / drawersRowsAmount));

            int drawerPixelWidth = drawerCellWidth * cellSize;
            int drawerPixelHeight = drawerCellHeight * cellSize;

            Size drawersSize = new Size(drawersColumnsAmount * drawerPixelWidth, drawersRowsAmount * drawerPixelHeight);
            drawers_pnl.ClientSize = drawersSize;
            ClientSize = drawers_pnl.Size;

            for (int i = 0; i < drawersColumnsAmount; i++)
            {
                for (int j = 0; j < drawersRowsAmount; j++)
                {
                    PictureBox ijPictureBox = new PictureBox();

                    ((System.ComponentModel.ISupportInitialize)(ijPictureBox)).BeginInit();

                    ijPictureBox.Location = new System.Drawing.Point(i * drawerPixelWidth, j * drawerPixelHeight);
                    ijPictureBox.Size = new Size(drawerPixelWidth, drawerPixelHeight);
                    ijPictureBox.Image = new Bitmap(drawerPixelWidth, drawerPixelHeight);
                    pictureBoxes[j * drawersColumnsAmount + i] = ijPictureBox;
                    drawers_pnl.Controls.Add(ijPictureBox);

                    ((System.ComponentModel.ISupportInitialize)(ijPictureBox)).EndInit();

                    graphicses[j * drawersColumnsAmount + i] = Graphics.FromImage(ijPictureBox.Image);
                    
                    CellularAutomata.Point upperLeftBound = new CellularAutomata.Point(i * drawerCellWidth, j * drawerCellHeight);
                    int lowerRightX = (i < drawersColumnsAmount - 1) ? i * drawerCellWidth + drawerCellWidth - 1 : cellularAutomaton.Width - 1;
                    int lowerRightY = (j < drawersRowsAmount - 1) ? j * drawerCellHeight + drawerCellHeight - 1 : cellularAutomaton.Height - 1;
                    CellularAutomata.Point lowerRightBound = new CellularAutomata.Point(lowerRightX, lowerRightY);

                    cellsGridDrawers[j * drawersColumnsAmount + i] = new CellsGridDrawer(cellularAutomaton, cellSize, upperLeftBound, lowerRightBound);
                }
            }
        }

        private void engineTimer_Tick(object sender, EventArgs e)
        {
            DrawUsingTasks();
            cellularAutomaton.NextStep();
        }

        private void DrawUsingTasks()
        {
            for (int i = 0; i < cellsGridDrawers.Length; i++)
            {
                int index = i;
                drawTasks[i] = Task.Run(() => cellsGridDrawers[index].Draw(graphicses[index])); // i-ty graphics z i-tej bitmapy
            }
            try
            {
                Task.WaitAll(drawTasks);
                foreach (PictureBox pb in pictureBoxes)
                    pb.Refresh();
            }
            catch (AggregateException aEx)
            {
                StringBuilder messageSB = new StringBuilder();
                foreach (Task drawTask in drawTasks)
                    messageSB.Append(drawTask.Id.ToString() + "    " + drawTask.Status + Environment.NewLine);
                MessageBox.Show(messageSB.ToString());
            }
        }
    }
}
