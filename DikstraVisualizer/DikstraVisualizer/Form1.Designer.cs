namespace DikstraVisualizer
{
    partial class Form1
    {
        /// <summary>
        ///  Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        ///  Clean up any resources being used.
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
        ///  Required method for Designer support - do not modify
        ///  the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.Background = new System.Windows.Forms.PictureBox();
            this.Picture = new System.Windows.Forms.PictureBox();
            this.HeuristicsPicker = new System.Windows.Forms.ComboBox();
            this.Directions = new System.Windows.Forms.TextBox();
            this.PrevWalls = new System.Windows.Forms.Button();
            this.SearchTimer = new System.Windows.Forms.Timer(this.components);
            this.OneIteration = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.Background)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).BeginInit();
            this.SuspendLayout();
            // 
            // Background
            // 
            this.Background.Location = new System.Drawing.Point(-3, -17);
            this.Background.Name = "Background";
            this.Background.Size = new System.Drawing.Size(0, 0);
            this.Background.TabIndex = 0;
            this.Background.TabStop = false;
            // 
            // Picture
            // 
            this.Picture.Dock = System.Windows.Forms.DockStyle.Fill;
            this.Picture.Location = new System.Drawing.Point(0, 0);
            this.Picture.Name = "Picture";
            this.Picture.Size = new System.Drawing.Size(884, 550);
            this.Picture.TabIndex = 1;
            this.Picture.TabStop = false;
            this.Picture.MouseClick += new System.Windows.Forms.MouseEventHandler(this.Picture_MouseClick);
            // 
            // HeuristicsPicker
            // 
            this.HeuristicsPicker.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.HeuristicsPicker.FormattingEnabled = true;
            this.HeuristicsPicker.ImeMode = System.Windows.Forms.ImeMode.Disable;
            this.HeuristicsPicker.Location = new System.Drawing.Point(799, 0);
            this.HeuristicsPicker.Name = "HeuristicsPicker";
            this.HeuristicsPicker.Size = new System.Drawing.Size(85, 23);
            this.HeuristicsPicker.TabIndex = 2;
            this.HeuristicsPicker.SelectedIndexChanged += new System.EventHandler(this.HeuristicsPicker_SelectedIndexChanged);
            // 
            // Directions
            // 
            this.Directions.Location = new System.Drawing.Point(799, 58);
            this.Directions.MaximumSize = new System.Drawing.Size(120, 120);
            this.Directions.Name = "Directions";
            this.Directions.ReadOnly = true;
            this.Directions.Size = new System.Drawing.Size(100, 23);
            this.Directions.TabIndex = 3;
            // 
            // PrevWalls
            // 
            this.PrevWalls.Location = new System.Drawing.Point(809, 111);
            this.PrevWalls.Name = "PrevWalls";
            this.PrevWalls.Size = new System.Drawing.Size(75, 47);
            this.PrevWalls.TabIndex = 4;
            this.PrevWalls.Text = "Paste Prev walls";
            this.PrevWalls.UseVisualStyleBackColor = true;
            this.PrevWalls.Click += new System.EventHandler(this.PrevWalls_Click);
            // 
            // SearchTimer
            // 
            this.SearchTimer.Interval = 20;
            this.SearchTimer.Tick += new System.EventHandler(this.SearchTimer_Tick);
            // 
            // OneIteration
            // 
            this.OneIteration.Location = new System.Drawing.Point(809, 203);
            this.OneIteration.Name = "OneIteration";
            this.OneIteration.Size = new System.Drawing.Size(75, 59);
            this.OneIteration.TabIndex = 5;
            this.OneIteration.Text = "For One Iteration of Finder";
            this.OneIteration.UseVisualStyleBackColor = true;
            this.OneIteration.Click += new System.EventHandler(this.OneIteration_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(884, 550);
            this.Controls.Add(this.OneIteration);
            this.Controls.Add(this.PrevWalls);
            this.Controls.Add(this.Directions);
            this.Controls.Add(this.HeuristicsPicker);
            this.Controls.Add(this.Picture);
            this.Controls.Add(this.Background);
            this.KeyPreview = true;
            this.Name = "Form1";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.Form1_KeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.Background)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.Picture)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private PictureBox Background;
        private PictureBox Picture;
        private ComboBox HeuristicsPicker;
        private TextBox Directions;
        private Button PrevWalls;
        private System.Windows.Forms.Timer SearchTimer;
        private Button OneIteration;
    }
}