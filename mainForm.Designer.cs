namespace CommunityServiceDBTracker
{
    partial class mainForm
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(mainForm));
            this.toolStrip1 = new System.Windows.Forms.ToolStrip();
            this.recordsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator1 = new System.Windows.Forms.ToolStripSeparator();
            this.studentsButton = new System.Windows.Forms.ToolStripButton();
            this.awardsButton = new System.Windows.Forms.ToolStripButton();
            this.aboutButton = new System.Windows.Forms.ToolStripButton();
            this.toolStripSeparator2 = new System.Windows.Forms.ToolStripSeparator();
            this.reportsButton = new System.Windows.Forms.ToolStripButton();
            this.toolStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // toolStrip1
            // 
            this.toolStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.recordsButton,
            this.toolStripSeparator1,
            this.studentsButton,
            this.awardsButton,
            this.aboutButton,
            this.toolStripSeparator2,
            this.reportsButton});
            this.toolStrip1.LayoutStyle = System.Windows.Forms.ToolStripLayoutStyle.HorizontalStackWithOverflow;
            this.toolStrip1.Location = new System.Drawing.Point(0, 0);
            this.toolStrip1.Name = "toolStrip1";
            this.toolStrip1.Size = new System.Drawing.Size(1053, 38);
            this.toolStrip1.TabIndex = 4;
            this.toolStrip1.Text = "toolStrip1";
            // 
            // recordsButton
            // 
            this.recordsButton.Image = ((System.Drawing.Image)(resources.GetObject("recordsButton.Image")));
            this.recordsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.recordsButton.Name = "recordsButton";
            this.recordsButton.Size = new System.Drawing.Size(53, 35);
            this.recordsButton.Text = "Records";
            this.recordsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.recordsButton.Click += new System.EventHandler(this.recordsButton_Click);
            // 
            // toolStripSeparator1
            // 
            this.toolStripSeparator1.Name = "toolStripSeparator1";
            this.toolStripSeparator1.Size = new System.Drawing.Size(6, 38);
            // 
            // studentsButton
            // 
            this.studentsButton.Image = global::CommunityServiceDBTracker.Properties.Resources.students;
            this.studentsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.studentsButton.Name = "studentsButton";
            this.studentsButton.Size = new System.Drawing.Size(57, 35);
            this.studentsButton.Text = "Students";
            this.studentsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.studentsButton.Click += new System.EventHandler(this.studentsButton_Click);
            // 
            // awardsButton
            // 
            this.awardsButton.Image = global::CommunityServiceDBTracker.Properties.Resources.awards;
            this.awardsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.awardsButton.Name = "awardsButton";
            this.awardsButton.Size = new System.Drawing.Size(50, 35);
            this.awardsButton.Text = "Awards";
            this.awardsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.awardsButton.Click += new System.EventHandler(this.awardsButton_Click);
            // 
            // aboutButton
            // 
            this.aboutButton.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.aboutButton.Image = global::CommunityServiceDBTracker.Properties.Resources.about;
            this.aboutButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.aboutButton.Name = "aboutButton";
            this.aboutButton.Size = new System.Drawing.Size(44, 35);
            this.aboutButton.Text = "About";
            this.aboutButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.aboutButton.Click += new System.EventHandler(this.aboutButton_Click);
            // 
            // toolStripSeparator2
            // 
            this.toolStripSeparator2.Name = "toolStripSeparator2";
            this.toolStripSeparator2.Size = new System.Drawing.Size(6, 38);
            // 
            // reportsButton
            // 
            this.reportsButton.Image = global::CommunityServiceDBTracker.Properties.Resources.reports;
            this.reportsButton.ImageTransparentColor = System.Drawing.Color.Magenta;
            this.reportsButton.Name = "reportsButton";
            this.reportsButton.Size = new System.Drawing.Size(51, 35);
            this.reportsButton.Text = "Reports";
            this.reportsButton.TextImageRelation = System.Windows.Forms.TextImageRelation.ImageAboveText;
            this.reportsButton.Click += new System.EventHandler(this.reportsButton_Click);
            // 
            // mainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.BackgroundImageLayout = System.Windows.Forms.ImageLayout.None;
            this.ClientSize = new System.Drawing.Size(1053, 732);
            this.Controls.Add(this.toolStrip1);
            this.DoubleBuffered = true;
            this.IsMdiContainer = true;
            this.Name = "mainForm";
            this.Text = "Students Community Service Tracking";
            this.Load += new System.EventHandler(this.mainForm_Load);
            this.toolStrip1.ResumeLayout(false);
            this.toolStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }
        #endregion
        private System.Windows.Forms.ToolStrip toolStrip1;
        private System.Windows.Forms.ToolStripButton recordsButton;
        private System.Windows.Forms.ToolStripButton studentsButton;
        private System.Windows.Forms.ToolStripButton awardsButton;
        private System.Windows.Forms.ToolStripButton aboutButton;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator1;
        private System.Windows.Forms.ToolStripSeparator toolStripSeparator2;
        private System.Windows.Forms.ToolStripButton reportsButton;
    }
}



