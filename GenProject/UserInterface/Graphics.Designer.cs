namespace GenProject
{
    partial class Graph
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
            System.Windows.Forms.DataVisualization.Charting.ChartArea chartArea1 = new System.Windows.Forms.DataVisualization.Charting.ChartArea();
            System.Windows.Forms.DataVisualization.Charting.Legend legend1 = new System.Windows.Forms.DataVisualization.Charting.Legend();
            System.Windows.Forms.DataVisualization.Charting.Series series1 = new System.Windows.Forms.DataVisualization.Charting.Series();
            this.chart1 = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bDraw = new System.Windows.Forms.Button();
            this.n1 = new System.Windows.Forms.NumericUpDown();
            this.n2 = new System.Windows.Forms.NumericUpDown();
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.n1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.n2)).BeginInit();
            this.SuspendLayout();
            // 
            // chart1
            // 
            chartArea1.Name = "ChartArea1";
            this.chart1.ChartAreas.Add(chartArea1);
            legend1.Name = "Legend1";
            this.chart1.Legends.Add(legend1);
            this.chart1.Location = new System.Drawing.Point(40, 177);
            this.chart1.Name = "chart1";
            series1.ChartArea = "ChartArea1";
            series1.ChartType = System.Windows.Forms.DataVisualization.Charting.SeriesChartType.FastPoint;
            series1.Legend = "Legend1";
            series1.Name = "Found optimal points";
            this.chart1.Series.Add(series1);
            this.chart1.Size = new System.Drawing.Size(679, 300);
            this.chart1.TabIndex = 0;
            this.chart1.Text = "chart1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(204)));
            this.label1.Location = new System.Drawing.Point(37, 32);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(394, 16);
            this.label1.TabIndex = 1;
            this.label1.Text = "Select the numbers of functions to be shown at the graph";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(37, 68);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(140, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "First function number (x-axis)";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(37, 95);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(158, 13);
            this.label3.TabIndex = 3;
            this.label3.Text = "Second function number (y-axis)";
            // 
            // bDraw
            // 
            this.bDraw.Location = new System.Drawing.Point(40, 125);
            this.bDraw.Name = "bDraw";
            this.bDraw.Size = new System.Drawing.Size(122, 23);
            this.bDraw.TabIndex = 6;
            this.bDraw.Text = "Build";
            this.bDraw.UseVisualStyleBackColor = true;
            this.bDraw.Click += new System.EventHandler(this.bDraw_Click);
            // 
            // n1
            // 
            this.n1.Location = new System.Drawing.Point(230, 66);
            this.n1.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.n1.Name = "n1";
            this.n1.Size = new System.Drawing.Size(120, 20);
            this.n1.TabIndex = 7;
            this.n1.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // n2
            // 
            this.n2.Location = new System.Drawing.Point(230, 93);
            this.n2.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.n2.Name = "n2";
            this.n2.Size = new System.Drawing.Size(120, 20);
            this.n2.TabIndex = 8;
            this.n2.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // Graph
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(769, 521);
            this.Controls.Add(this.n2);
            this.Controls.Add(this.n1);
            this.Controls.Add(this.bDraw);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.chart1);
            this.Name = "Graph";
            this.Text = "Graph";
            ((System.ComponentModel.ISupportInitialize)(this.chart1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.n1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.n2)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataVisualization.Charting.Chart chart1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button bDraw;
        private System.Windows.Forms.NumericUpDown n1;
        private System.Windows.Forms.NumericUpDown n2;
    }
}