namespace CellularAutomataVisualiser
{
    partial class CellularAutomataForm
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.EngineTimer = new System.Windows.Forms.Timer(this.components);
            this.cellsGrid_pb = new System.Windows.Forms.PictureBox();
            this.cellsGrid_pnl = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.cellsGrid_pb)).BeginInit();
            this.SuspendLayout();
            // 
            // EngineTimer
            // 
            this.EngineTimer.Interval = 1;
            this.EngineTimer.Tick += new System.EventHandler(this.EngineTimer_Tick);
            // 
            // cellsGrid_pb
            // 
            this.cellsGrid_pb.Location = new System.Drawing.Point(0, 0);
            this.cellsGrid_pb.Name = "cellsGrid_pb";
            this.cellsGrid_pb.Size = new System.Drawing.Size(120, 185);
            this.cellsGrid_pb.TabIndex = 0;
            this.cellsGrid_pb.TabStop = false;
            this.cellsGrid_pb.Click += new System.EventHandler(this.cellsGrid_pb_Click);
            this.cellsGrid_pb.Paint += new System.Windows.Forms.PaintEventHandler(this.cellsGrid_pb_Paint);
            // 
            // cellsGrid_pnl
            // 
            this.cellsGrid_pnl.Location = new System.Drawing.Point(0, 0);
            this.cellsGrid_pnl.Name = "cellsGrid_pnl";
            this.cellsGrid_pnl.Size = new System.Drawing.Size(200, 100);
            this.cellsGrid_pnl.TabIndex = 1;
            this.cellsGrid_pnl.Paint += new System.Windows.Forms.PaintEventHandler(this.cellsGrid_pnl_Paint);
            // 
            // CellularAutomataForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(891, 479);
            this.Controls.Add(this.cellsGrid_pnl);
            this.Controls.Add(this.cellsGrid_pb);
            this.Name = "CellularAutomataForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CellularAutomataForm";
            ((System.ComponentModel.ISupportInitialize)(this.cellsGrid_pb)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Timer EngineTimer;
        private System.Windows.Forms.PictureBox cellsGrid_pb;
        private System.Windows.Forms.Panel cellsGrid_pnl;
    }
}