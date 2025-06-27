using System.Diagnostics;

namespace appcheckansw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            textBox1.Text = "ШО ТАКОЕ ПАРИШ?";

        }
        public string answer = "Париж является столицей Франции.";

        public async Task<string> bublic()
        {
            string text1 = textBox2.Text;
            string text2 = answer;

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = "python";
            start.Arguments = $"{System.IO.Directory.GetCurrentDirectory()}\\dlyacheka.py \"{text1}\" \"{text2}\"";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            //start.CreateNoWindow = true;
            start.WindowStyle = ProcessWindowStyle.Minimized;
            
            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    //MessageBox.Show(result);

                    return result;
                }
            }

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            button1.Enabled = false;
            textBox3.Text = await Task.Run(bublic);
            button1 .Enabled = true;
            //textBox3.Text=backgroundWorker1.DoWork(sender,e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                textBox1.Text += ": " + answer;
            }
            else
            {
                textBox1.Text = textBox1.Text[..(textBox1.TextLength - answer.Length - 2)];
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            bublic();
        }
    }
}
