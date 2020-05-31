namespace PuzzleExtractor.Forms
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
            this.button1 = new System.Windows.Forms.Button();
            this.areaLabel = new System.Windows.Forms.Label();
            this.imageBox1 = new Cyotek.Windows.Forms.ImageBox();
            this.batchMode = new System.Windows.Forms.RadioButton();
            this.comparisonMode = new System.Windows.Forms.RadioButton();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.button2 = new System.Windows.Forms.Button();
            this.button3 = new System.Windows.Forms.Button();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            this.cornerBatchViewMode = new System.Windows.Forms.RadioButton();
            this.contourBatchViewMode = new System.Windows.Forms.RadioButton();
            this.sourceBatchViewMode = new System.Windows.Forms.RadioButton();
            this.label2 = new System.Windows.Forms.Label();
            this.cornerThresholdTextBox = new System.Windows.Forms.TextBox();
            this.segmentBatchViewMode = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(83, 836);
            this.button1.Margin = new System.Windows.Forms.Padding(6);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(310, 45);
            this.button1.TabIndex = 3;
            this.button1.Text = "Save image";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // areaLabel
            // 
            this.areaLabel.AutoSize = true;
            this.areaLabel.Location = new System.Drawing.Point(77, 54);
            this.areaLabel.Margin = new System.Windows.Forms.Padding(6, 0, 6, 0);
            this.areaLabel.Name = "areaLabel";
            this.areaLabel.Size = new System.Drawing.Size(93, 32);
            this.areaLabel.TabIndex = 5;
            this.areaLabel.Text = "label2";
            // 
            // imageBox1
            // 
            this.imageBox1.Location = new System.Drawing.Point(650, 54);
            this.imageBox1.Margin = new System.Windows.Forms.Padding(6);
            this.imageBox1.Name = "imageBox1";
            this.imageBox1.Size = new System.Drawing.Size(2334, 1494);
            this.imageBox1.TabIndex = 7;
            this.imageBox1.Text = "imageBox1";
            // 
            // batchMode
            // 
            this.batchMode.AutoSize = true;
            this.batchMode.Location = new System.Drawing.Point(13, 46);
            this.batchMode.Name = "batchMode";
            this.batchMode.Size = new System.Drawing.Size(125, 36);
            this.batchMode.TabIndex = 8;
            this.batchMode.TabStop = true;
            this.batchMode.Text = "Batch";
            this.batchMode.UseVisualStyleBackColor = true;
            this.batchMode.CheckedChanged += new System.EventHandler(this.batchMode_CheckedChanged);
            // 
            // comparisonMode
            // 
            this.comparisonMode.AutoSize = true;
            this.comparisonMode.Location = new System.Drawing.Point(13, 104);
            this.comparisonMode.Name = "comparisonMode";
            this.comparisonMode.Size = new System.Drawing.Size(205, 36);
            this.comparisonMode.TabIndex = 9;
            this.comparisonMode.TabStop = true;
            this.comparisonMode.Text = "Comparison";
            this.comparisonMode.UseVisualStyleBackColor = true;
            this.comparisonMode.CheckedChanged += new System.EventHandler(this.comparisonMode_CheckedChanged);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.batchMode);
            this.groupBox1.Controls.Add(this.comparisonMode);
            this.groupBox1.Location = new System.Drawing.Point(83, 297);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(285, 148);
            this.groupBox1.TabIndex = 10;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "View mode";
            // 
            // button2
            // 
            this.button2.Location = new System.Drawing.Point(83, 192);
            this.button2.Name = "button2";
            this.button2.Size = new System.Drawing.Size(166, 50);
            this.button2.TabIndex = 11;
            this.button2.Text = "Previous";
            this.button2.UseVisualStyleBackColor = true;
            this.button2.Click += new System.EventHandler(this.previousButton_Click);
            // 
            // button3
            // 
            this.button3.Location = new System.Drawing.Point(267, 192);
            this.button3.Name = "button3";
            this.button3.Size = new System.Drawing.Size(142, 50);
            this.button3.TabIndex = 12;
            this.button3.Text = "Next";
            this.button3.UseVisualStyleBackColor = true;
            this.button3.Click += new System.EventHandler(this.nextButton_Click);
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.segmentBatchViewMode);
            this.groupBox2.Controls.Add(this.cornerBatchViewMode);
            this.groupBox2.Controls.Add(this.contourBatchViewMode);
            this.groupBox2.Controls.Add(this.sourceBatchViewMode);
            this.groupBox2.Location = new System.Drawing.Point(83, 502);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(326, 307);
            this.groupBox2.TabIndex = 13;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Batch view mode";
            // 
            // cornerBatchViewMode
            // 
            this.cornerBatchViewMode.AutoSize = true;
            this.cornerBatchViewMode.Location = new System.Drawing.Point(13, 174);
            this.cornerBatchViewMode.Name = "cornerBatchViewMode";
            this.cornerBatchViewMode.Size = new System.Drawing.Size(138, 36);
            this.cornerBatchViewMode.TabIndex = 2;
            this.cornerBatchViewMode.TabStop = true;
            this.cornerBatchViewMode.Text = "Corner";
            this.cornerBatchViewMode.UseVisualStyleBackColor = true;
            this.cornerBatchViewMode.CheckedChanged += new System.EventHandler(this.cornerBatchViewMode_CheckedChanged);
            // 
            // contourBatchViewMode
            // 
            this.contourBatchViewMode.AutoSize = true;
            this.contourBatchViewMode.Location = new System.Drawing.Point(13, 112);
            this.contourBatchViewMode.Name = "contourBatchViewMode";
            this.contourBatchViewMode.Size = new System.Drawing.Size(153, 36);
            this.contourBatchViewMode.TabIndex = 1;
            this.contourBatchViewMode.TabStop = true;
            this.contourBatchViewMode.Text = "Contour";
            this.contourBatchViewMode.UseVisualStyleBackColor = true;
            this.contourBatchViewMode.CheckedChanged += new System.EventHandler(this.contourBatchViewMode_CheckedChanged);
            // 
            // sourceBatchViewMode
            // 
            this.sourceBatchViewMode.AutoSize = true;
            this.sourceBatchViewMode.Location = new System.Drawing.Point(13, 51);
            this.sourceBatchViewMode.Name = "sourceBatchViewMode";
            this.sourceBatchViewMode.Size = new System.Drawing.Size(142, 36);
            this.sourceBatchViewMode.TabIndex = 0;
            this.sourceBatchViewMode.TabStop = true;
            this.sourceBatchViewMode.Text = "Source";
            this.sourceBatchViewMode.UseVisualStyleBackColor = true;
            this.sourceBatchViewMode.CheckedChanged += new System.EventHandler(this.sourceBatchViewMode_CheckedChanged);
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(77, 122);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(234, 32);
            this.label2.TabIndex = 14;
            this.label2.Text = "Corner threshold:";
            // 
            // cornerThresholdTextBox
            // 
            this.cornerThresholdTextBox.Location = new System.Drawing.Point(354, 119);
            this.cornerThresholdTextBox.Name = "cornerThresholdTextBox";
            this.cornerThresholdTextBox.Size = new System.Drawing.Size(128, 38);
            this.cornerThresholdTextBox.TabIndex = 15;
            this.cornerThresholdTextBox.Leave += new System.EventHandler(this.cornerThresholdTextBox_Leave);
            // 
            // segmentBatchViewMode
            // 
            this.segmentBatchViewMode.AutoSize = true;
            this.segmentBatchViewMode.Location = new System.Drawing.Point(13, 235);
            this.segmentBatchViewMode.Name = "segmentBatchViewMode";
            this.segmentBatchViewMode.Size = new System.Drawing.Size(166, 36);
            this.segmentBatchViewMode.TabIndex = 3;
            this.segmentBatchViewMode.TabStop = true;
            this.segmentBatchViewMode.Text = "Segment";
            this.segmentBatchViewMode.UseVisualStyleBackColor = true;
            this.segmentBatchViewMode.CheckedChanged += new System.EventHandler(this.segmentBatchViewMode_CheckedChanged);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(16F, 31F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(3072, 1626);
            this.Controls.Add(this.cornerThresholdTextBox);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.groupBox2);
            this.Controls.Add(this.button3);
            this.Controls.Add(this.button2);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.imageBox1);
            this.Controls.Add(this.areaLabel);
            this.Controls.Add(this.button1);
            this.Margin = new System.Windows.Forms.Padding(6);
            this.Name = "Form1";
            this.Text = "Form1";
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Label areaLabel;
        private Cyotek.Windows.Forms.ImageBox imageBox1;
        private System.Windows.Forms.RadioButton batchMode;
        private System.Windows.Forms.RadioButton comparisonMode;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Button button2;
        private System.Windows.Forms.Button button3;
        private System.Windows.Forms.GroupBox groupBox2;
        private System.Windows.Forms.RadioButton cornerBatchViewMode;
        private System.Windows.Forms.RadioButton contourBatchViewMode;
        private System.Windows.Forms.RadioButton sourceBatchViewMode;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox cornerThresholdTextBox;
        private System.Windows.Forms.RadioButton segmentBatchViewMode;
    }
}

