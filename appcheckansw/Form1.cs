using ExcelDataReader;
using System.Diagnostics;
using System.Text.RegularExpressions;

namespace appcheckansw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //questionTextBox.Text = "�� ����� �����?";
            //userAnswerTextBox.Text = "��� ������� �������";
            //textBox2.Text = $"{System.IO.Directory.GetCurrentDirectory()[..(System.IO.Directory.GetCurrentDirectory().Length-25)]}\\dlyacheka.py";
            //checkBox1.Hide();
            checkedAnswerTextBox.Hide();
        }
        public static List<string> answers, questions;
        public static string[] userAnswers;
        public int currentQuestion = 0;

        public async Task<string> bublic()
        {
            string text1 = ""; string text2 = "";
            foreach (var uanswer in userAnswers) { text1 += uanswer + ','; }
            foreach (var answer in answers) { text2 += answer + ","; }
            text1 = text1.TrimEnd(',');
            text2 = text2.TrimEnd(',');

            ProcessStartInfo start = new ProcessStartInfo();
            start.FileName = $"{System.IO.Directory.GetCurrentDirectory()[..(System.IO.Directory.GetCurrentDirectory().Length - 25)]}\\dlyacheka.exe";
            start.Arguments = $"\"{text1}\" \"{text2}\"";
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            //start.CreateNoWindow = true;
            start.WindowStyle = ProcessWindowStyle.Minimized;

            using (Process process = Process.Start(start))
            {
                using (StreamReader reader = process.StandardOutput)
                {
                    string result = reader.ReadToEnd();
                    MessageBox.Show(result);

                    return result;
                }
            }

        }
        private async void button1_Click(object sender, EventArgs e)
        {
            checkAnswerButton.Enabled = false;
            checkedAnswerTextBox.Text = await Task.Run(bublic);
            checkAnswerButton.Enabled = true;
            //textBox3.Text=backgroundWorker1.DoWork(sender,e);
        }

        private void checkBox1_CheckedChanged(object sender, EventArgs e)
        {

            if (checkBox1.Checked)
            {
                questionTextBox.Text += ": " + answers[currentQuestion];
            }
            else
            {
                questionTextBox.Text = questionTextBox.Text[..(questionTextBox.TextLength - answers[currentQuestion].Length - 2)];
            }
        }

        private void backgroundWorker1_DoWork(object sender, System.ComponentModel.DoWorkEventArgs e)
        {
            bublic();
        }

        private void button2_Click(object sender, EventArgs e)//next q
        {
            if (questions != null)
            {
                if (questions.Count - 1 > currentQuestion)
                {
                    currentQuestion++;
                    questionTextBox.Text = questions[currentQuestion];
                    userAnswerTextBox.Text = userAnswers[currentQuestion];
                }
            }
        }

        private void button1_Click_1(object sender, EventArgs e)//prev q
        {
            if (currentQuestion != 0)
            {
                currentQuestion--;
                questionTextBox.Text = questions[currentQuestion];
                userAnswerTextBox.Text = userAnswers[currentQuestion];
            }
        }

        private void loadQuestionsButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
            (questions, answers) = QAPairLoader.LoadQAPairsFromFile(openFileDialog1.FileName);
            if (questions != null && answers != null)
            {
                questionTextBox.Text = questions[currentQuestion];
                userAnswers = new string[answers.Count];
                //userAnswerTextBox.Text += answers.Count;
                //userAnswerTextBox.Text += questions.Count;
            }
        }

        private void questionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void userAnswerTextBox_TextChanged(object sender, EventArgs e)
        {
            userAnswers[currentQuestion] = userAnswerTextBox.Text;
        }

        private void changeQAButton_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2();
            form2.Show();
        }
    }
    public class QAPairLoader
    {
        public static (List<string> questions, List<string> answers) LoadQAPairsFromFile(string filePath)
        {
            var questions = new List<string>();
            var answers = new List<string>();

            string extension = Path.GetExtension(filePath).ToLower();

            try
            {
                if (extension == ".csv")
                {
                    LoadFromCsv(filePath, questions, answers);
                }
                else if (extension == ".xlsx")
                {
                    LoadFromXlsx(filePath, questions, answers);
                }
                else
                {
                    throw new NotSupportedException("�������������� ������ ����� ������� CSV � XLSX");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"������ ��� �������� �����: {ex.Message}");
            }

            return (questions, answers);
        }

        private static void LoadFromCsv(string filePath, List<string> questions, List<string> answers)
        {
            using (var reader = new StreamReader(filePath))
            {
                while (!reader.EndOfStream)
                {
                    var line = reader.ReadLine();
                    // ��������� CSV � ������ ������� (����������� - �������)
                    var values = ParseCsvLine(line);

                    if (values.Count >= 2 && !string.IsNullOrWhiteSpace(values[0]) && !string.IsNullOrWhiteSpace(values[1]))
                    {
                        var question = values[0].Trim();
                        var answerParts = SplitAnswers(values[1].Trim());

                        foreach (var answer in answerParts)
                        {
                            if (!string.IsNullOrWhiteSpace(answer))
                            {
                                questions.Add(question);
                                answers.Add(answer.Trim());
                            }
                        }
                    }
                }
            }
        }

        private static void LoadFromXlsx(string filePath, List<string> questions, List<string> answers)
        {
            System.Text.Encoding.RegisterProvider(System.Text.CodePagesEncodingProvider.Instance);

            using (var stream = File.Open(filePath, FileMode.Open, FileAccess.Read))
            using (var reader = ExcelReaderFactory.CreateReader(stream))
            {
                do
                {
                    while (reader.Read())
                    {
                        try
                        {
                            var question = reader.GetValue(0)?.ToString();
                            var answer = reader.GetValue(1)?.ToString();

                            if (!string.IsNullOrWhiteSpace(question) && !string.IsNullOrWhiteSpace(answer))
                            {
                                var answerParts = SplitAnswers(answer.Trim());

                                foreach (var part in answerParts)
                                {
                                    if (!string.IsNullOrWhiteSpace(part))
                                    {
                                        questions.Add(question.Trim());
                                        answers.Add(part.Trim());
                                    }
                                }
                            }
                        }
                        catch (Exception)
                        {
                            continue;
                        }
                    }
                } while (reader.NextResult());
            }
        }

        // ��������� ������ � �������� �� ��������� ������ (��������� ������� � �������)
        private static List<string> SplitAnswers(string answerString)
        {
            var answers = new List<string>();

            // ������� 1: ������ � �������� ("�����1", "�����2")
            if (answerString.Contains("\""))
            {
                // ������� ��� ������� ����� ��������� � �������� ��� ��������� ���������
                answerString = Regex.Replace(answerString, @"\s*,\s*", ",");
                // ��������� �� ������� ��� �������
                var matches = Regex.Matches(answerString, @"(?:\""([^\""]*)\""|([^,]+))");

                foreach (Match match in matches)
                {
                    var answer = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
                    if (!string.IsNullOrWhiteSpace(answer))
                        answers.Add(answer.Trim());
                }
            }
            // ������� 2: ������� ������ ����� ������� (�����1, �����2)
            else
            {
                var parts = answerString.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                foreach (var part in parts)
                {
                    if (!string.IsNullOrWhiteSpace(part))
                        answers.Add(part.Trim());
                }
            }

            return answers;
        }

        // ������ ������ CSV � ������ ������� (����� ������� ������ ������� �� ��������� �������������)
        private static List<string> ParseCsvLine(string line)
        {
            var values = new List<string>();
            bool inQuotes = false;
            int startIndex = 0;

            for (int i = 0; i < line.Length; i++)
            {
                if (line[i] == '"')
                {
                    inQuotes = !inQuotes;
                }
                else if (line[i] == ',' && !inQuotes)
                {
                    values.Add(line.Substring(startIndex, i - startIndex).Trim(' ', '"'));
                    startIndex = i + 1;
                }
            }

            // ��������� ��������� ��������
            values.Add(line.Substring(startIndex).Trim(' ', '"'));

            return values;
        }
    }
}
