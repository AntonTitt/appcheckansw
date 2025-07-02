namespace appcheckansw
{
    partial class Form2
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
            button1 = new Button();
            QATextBox = new TextBox();
            openFileDialog1 = new OpenFileDialog();
            button2 = new Button();
            saveFileDialog1 = new SaveFileDialog();
            checkBox1 = new CheckBox();
            SuspendLayout();
            // 
            // button1
            // 
            button1.Location = new Point(12, 12);
            button1.Name = "button1";
            button1.Size = new Size(94, 29);
            button1.TabIndex = 0;
            button1.Text = "загрузить";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click;
            // 
            // QATextBox
            // 
            QATextBox.Location = new Point(12, 47);
            QATextBox.Multiline = true;
            QATextBox.Name = "QATextBox";
            QATextBox.ScrollBars = ScrollBars.Vertical;
            QATextBox.Size = new Size(776, 391);
            QATextBox.TabIndex = 1;
            QATextBox.TextChanged += QATextBox_TextChanged;
            QATextBox.KeyDown += QATextBox_KeyDown;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // button2
            // 
            button2.Location = new Point(112, 12);
            button2.Name = "button2";
            button2.Size = new Size(94, 29);
            button2.TabIndex = 2;
            button2.Text = "сохранить";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Checked = true;
            checkBox1.CheckState = CheckState.Checked;
            checkBox1.Location = new Point(212, 15);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(173, 24);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "создать новый файл";
            checkBox1.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 450);
            Controls.Add(checkBox1);
            Controls.Add(button2);
            Controls.Add(QATextBox);
            Controls.Add(button1);
            Name = "Form2";
            Text = "Редактирование";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button button1;
        private TextBox QATextBox;
        private OpenFileDialog openFileDialog1;
        private Button button2;
        private SaveFileDialog saveFileDialog1;
        private CheckBox checkBox1;
    }
}