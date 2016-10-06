namespace RhymesWithOrange
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
            this.getRhymesButton = new System.Windows.Forms.Button();
            this.wordTextBox = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.resultsLabel = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // getRhymesButton
            // 
            this.getRhymesButton.Location = new System.Drawing.Point(26, 40);
            this.getRhymesButton.Name = "getRhymesButton";
            this.getRhymesButton.Size = new System.Drawing.Size(214, 23);
            this.getRhymesButton.TabIndex = 0;
            this.getRhymesButton.Text = "What rhymes with Orange?";
            this.getRhymesButton.UseVisualStyleBackColor = true;
            this.getRhymesButton.Click += new System.EventHandler(this.button1_Click);
            // 
            // wordTextBox
            // 
            this.wordTextBox.Location = new System.Drawing.Point(108, 14);
            this.wordTextBox.Name = "wordTextBox";
            this.wordTextBox.Size = new System.Drawing.Size(132, 20);
            this.wordTextBox.TabIndex = 1;
            this.wordTextBox.Text = "Orange";
            this.wordTextBox.TextChanged += new System.EventHandler(this.textBox1_TextChanged);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(23, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(79, 15);
            this.label1.TabIndex = 2;
            this.label1.Text = "Enter a word ";
            // 
            // resultsLabel
            // 
            this.resultsLabel.AutoSize = true;
            this.resultsLabel.Location = new System.Drawing.Point(20, 80);
            this.resultsLabel.Name = "resultsLabel";
            this.resultsLabel.Size = new System.Drawing.Size(0, 15);
            this.resultsLabel.TabIndex = 4;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(284, 261);
            this.Controls.Add(this.resultsLabel);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.wordTextBox);
            this.Controls.Add(this.getRhymesButton);
            this.Name = "Form1";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button getRhymesButton;
        private System.Windows.Forms.TextBox wordTextBox;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label resultsLabel;
    }
}

