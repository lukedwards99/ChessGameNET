
namespace Project5Edwards_z1861935
{
    partial class Form1
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
            this.Canvas = new System.Windows.Forms.PictureBox();
            this.Turn_Label = new System.Windows.Forms.Label();
            this.Time_Label = new System.Windows.Forms.Label();
            this.Surrender_Button = new System.Windows.Forms.Button();
            this.ClearSelection_Button = new System.Windows.Forms.Button();
            this.Check_Label = new System.Windows.Forms.Label();
            this.CheckMate_Label = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).BeginInit();
            this.SuspendLayout();
            // 
            // Canvas
            // 
            this.Canvas.BackColor = System.Drawing.Color.White;
            this.Canvas.Location = new System.Drawing.Point(12, 37);
            this.Canvas.Name = "Canvas";
            this.Canvas.Size = new System.Drawing.Size(677, 677);
            this.Canvas.TabIndex = 0;
            this.Canvas.TabStop = false;
            this.Canvas.Paint += new System.Windows.Forms.PaintEventHandler(this.Canvas_Paint);
            this.Canvas.MouseDown += new System.Windows.Forms.MouseEventHandler(this.Canvas_MouseDown);
            // 
            // Turn_Label
            // 
            this.Turn_Label.AutoSize = true;
            this.Turn_Label.BackColor = System.Drawing.Color.White;
            this.Turn_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Turn_Label.ForeColor = System.Drawing.Color.Black;
            this.Turn_Label.Location = new System.Drawing.Point(12, 9);
            this.Turn_Label.Name = "Turn_Label";
            this.Turn_Label.Size = new System.Drawing.Size(144, 25);
            this.Turn_Label.TabIndex = 1;
            this.Turn_Label.Text = "White\'s Turn";
            // 
            // Time_Label
            // 
            this.Time_Label.AutoSize = true;
            this.Time_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Time_Label.ForeColor = System.Drawing.Color.Black;
            this.Time_Label.Location = new System.Drawing.Point(545, 9);
            this.Time_Label.Name = "Time_Label";
            this.Time_Label.Size = new System.Drawing.Size(136, 25);
            this.Time_Label.TabIndex = 2;
            this.Time_Label.Text = "Time: 00:00";
            // 
            // Surrender_Button
            // 
            this.Surrender_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Surrender_Button.Location = new System.Drawing.Point(12, 720);
            this.Surrender_Button.Name = "Surrender_Button";
            this.Surrender_Button.Size = new System.Drawing.Size(113, 32);
            this.Surrender_Button.TabIndex = 3;
            this.Surrender_Button.Text = "Surrender";
            this.Surrender_Button.UseVisualStyleBackColor = true;
            this.Surrender_Button.Click += new System.EventHandler(this.Surrender_Button_Click);
            // 
            // ClearSelection_Button
            // 
            this.ClearSelection_Button.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.ClearSelection_Button.Location = new System.Drawing.Point(550, 720);
            this.ClearSelection_Button.Name = "ClearSelection_Button";
            this.ClearSelection_Button.Size = new System.Drawing.Size(139, 32);
            this.ClearSelection_Button.TabIndex = 4;
            this.ClearSelection_Button.Text = "Clear Selection";
            this.ClearSelection_Button.UseVisualStyleBackColor = true;
            this.ClearSelection_Button.Click += new System.EventHandler(this.ClearSelection_Button_Click);
            // 
            // Check_Label
            // 
            this.Check_Label.AutoSize = true;
            this.Check_Label.BackColor = System.Drawing.Color.White;
            this.Check_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 15.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.Check_Label.ForeColor = System.Drawing.Color.Black;
            this.Check_Label.Location = new System.Drawing.Point(273, 9);
            this.Check_Label.Name = "Check_Label";
            this.Check_Label.Size = new System.Drawing.Size(149, 25);
            this.Check_Label.TabIndex = 5;
            this.Check_Label.Text = "Check_Lable";
            // 
            // CheckMate_Label
            // 
            this.CheckMate_Label.AutoSize = true;
            this.CheckMate_Label.BackColor = System.Drawing.Color.Black;
            this.CheckMate_Label.Font = new System.Drawing.Font("Microsoft Sans Serif", 18F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.CheckMate_Label.ForeColor = System.Drawing.Color.White;
            this.CheckMate_Label.Location = new System.Drawing.Point(129, 284);
            this.CheckMate_Label.Name = "CheckMate_Label";
            this.CheckMate_Label.Size = new System.Drawing.Size(215, 29);
            this.CheckMate_Label.TabIndex = 6;
            this.CheckMate_Label.Text = "CheckMate_label";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(701, 761);
            this.Controls.Add(this.CheckMate_Label);
            this.Controls.Add(this.Check_Label);
            this.Controls.Add(this.ClearSelection_Button);
            this.Controls.Add(this.Surrender_Button);
            this.Controls.Add(this.Time_Label);
            this.Controls.Add(this.Turn_Label);
            this.Controls.Add(this.Canvas);
            this.Name = "Form1";
            this.Text = "Form1";
            ((System.ComponentModel.ISupportInitialize)(this.Canvas)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.PictureBox Canvas;
        private System.Windows.Forms.Label Turn_Label;
        private System.Windows.Forms.Label Time_Label;
        private System.Windows.Forms.Button Surrender_Button;
        private System.Windows.Forms.Button ClearSelection_Button;
        private System.Windows.Forms.Label Check_Label;
        private System.Windows.Forms.Label CheckMate_Label;
    }
}

