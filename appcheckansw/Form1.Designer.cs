namespace appcheckansw
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
            checkAnswerButton = new Button();
            questionTextBox = new TextBox();
            userAnswerTextBox = new TextBox();
            checkBox1 = new CheckBox();
            checkedAnswerTextBox = new TextBox();
            backgroundWorker1 = new System.ComponentModel.BackgroundWorker();
            button1 = new Button();
            button2 = new Button();
            loadQuestionsButton = new Button();
            openFileDialog1 = new OpenFileDialog();
            changeQAButton = new Button();
            SuspendLayout();
            // 
            // checkAnswerButton
            // 
            checkAnswerButton.Location = new Point(11, 9);
            checkAnswerButton.Name = "checkAnswerButton";
            checkAnswerButton.Size = new Size(86, 29);
            checkAnswerButton.TabIndex = 0;
            checkAnswerButton.Text = "готово";
            checkAnswerButton.UseVisualStyleBackColor = true;
            checkAnswerButton.Click += button1_Click;
            // 
            // questionTextBox
            // 
            questionTextBox.Location = new Point(11, 47);
            questionTextBox.Multiline = true;
            questionTextBox.Name = "questionTextBox";
            questionTextBox.Size = new Size(777, 84);
            questionTextBox.TabIndex = 1;
            questionTextBox.TextChanged += questionTextBox_TextChanged;
            // 
            // userAnswerTextBox
            // 
            userAnswerTextBox.Location = new Point(11, 137);
            userAnswerTextBox.Multiline = true;
            userAnswerTextBox.Name = "userAnswerTextBox";
            userAnswerTextBox.PlaceholderText = "вводи ответ тут";
            userAnswerTextBox.Size = new Size(777, 301);
            userAnswerTextBox.TabIndex = 2;
            userAnswerTextBox.TextChanged += userAnswerTextBox_TextChanged;
            // 
            // checkBox1
            // 
            checkBox1.AutoSize = true;
            checkBox1.Location = new Point(654, 9);
            checkBox1.Name = "checkBox1";
            checkBox1.Size = new Size(134, 24);
            checkBox1.TabIndex = 3;
            checkBox1.Text = "показать ответ";
            checkBox1.UseVisualStyleBackColor = true;
            checkBox1.CheckedChanged += checkBox1_CheckedChanged;
            // 
            // checkedAnswerTextBox
            // 
            checkedAnswerTextBox.Location = new Point(398, 214);
            checkedAnswerTextBox.Multiline = true;
            checkedAnswerTextBox.Name = "checkedAnswerTextBox";
            checkedAnswerTextBox.ScrollBars = ScrollBars.Vertical;
            checkedAnswerTextBox.Size = new Size(390, 224);
            checkedAnswerTextBox.TabIndex = 4;
            // 
            // backgroundWorker1
            // 
            backgroundWorker1.DoWork += backgroundWorker1_DoWork;
            // 
            // button1
            // 
            button1.Location = new Point(103, 7);
            button1.Margin = new Padding(3, 4, 3, 4);
            button1.Name = "button1";
            button1.Size = new Size(112, 32);
            button1.TabIndex = 5;
            button1.Text = "предыдущий";
            button1.UseVisualStyleBackColor = true;
            button1.Click += button1_Click_1;
            // 
            // button2
            // 
            button2.Location = new Point(222, 7);
            button2.Margin = new Padding(3, 4, 3, 4);
            button2.Name = "button2";
            button2.Size = new Size(106, 32);
            button2.TabIndex = 6;
            button2.Text = "следующий";
            button2.UseVisualStyleBackColor = true;
            button2.Click += button2_Click;
            // 
            // loadQuestionsButton
            // 
            loadQuestionsButton.Location = new Point(334, 7);
            loadQuestionsButton.Margin = new Padding(3, 4, 3, 4);
            loadQuestionsButton.Name = "loadQuestionsButton";
            loadQuestionsButton.Size = new Size(86, 31);
            loadQuestionsButton.TabIndex = 7;
            loadQuestionsButton.Text = "загрузить";
            loadQuestionsButton.UseVisualStyleBackColor = true;
            loadQuestionsButton.Click += loadQuestionsButton_Click;
            // 
            // openFileDialog1
            // 
            openFileDialog1.FileName = "openFileDialog1";
            // 
            // changeQAButton
            // 
            changeQAButton.Location = new Point(426, 8);
            changeQAButton.Margin = new Padding(3, 4, 3, 4);
            changeQAButton.Name = "changeQAButton";
            changeQAButton.Size = new Size(86, 31);
            changeQAButton.TabIndex = 8;
            changeQAButton.Text = "изменить";
            changeQAButton.UseVisualStyleBackColor = true;
            changeQAButton.Click += changeQAButton_Click;
            // 
            // Form1
            // 
            AutoScaleDimensions = new SizeF(8F, 20F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 451);
            Controls.Add(changeQAButton);
            Controls.Add(loadQuestionsButton);
            Controls.Add(button2);
            Controls.Add(button1);
            Controls.Add(checkedAnswerTextBox);
            Controls.Add(checkBox1);
            Controls.Add(userAnswerTextBox);
            Controls.Add(questionTextBox);
            Controls.Add(checkAnswerButton);
            Name = "Form1";
            Text = "Ввод и проверка";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Button checkAnswerButton;
        private TextBox questionTextBox;
        private TextBox userAnswerTextBox;
        private CheckBox checkBox1;
        private TextBox checkedAnswerTextBox;
        private System.ComponentModel.BackgroundWorker backgroundWorker1;
        private Button button1;
        private Button button2;
        private Button loadQuestionsButton;
        private OpenFileDialog openFileDialog1;
        private Button changeQAButton;
    }
}
