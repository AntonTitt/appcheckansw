using DocumentFormat.OpenXml.Presentation;
using ExcelDataReader;
using System.Diagnostics;
using System.Text.RegularExpressions;
using System.Xml.Xsl;

namespace appcheckansw
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
            //textBox2.Text = $"{System.IO.Directory.GetCurrentDirectory()[..(System.IO.Directory.GetCurrentDirectory().Length-25)]}\\dlyacheka.py";
            checkBox1.Hide();
            checkedAnswerTextBox.Hide();
            userAnswerTextBox.Enabled = false;
            questionTextBox.ReadOnly = true;
        }
        public static List<string> answers, questions;
        public static string[] userAnswers;
        public int currentQuestion = 0, currentAnswer = 0;
        public static int[] de;//для сохранения индекса текущего ответа
        static bool useinterpretetere = false;//использовать интерпритатор (поставить false если надо предкомпилированую)

        public static string bublic()
        {
            string text1 = ""; string text2 = "", preva = "", prevq = "";
            for (int i = 0; i < questions.Count; i++)
            {
                var q = questions[i];
                if (prevq != q)
                {
                    prevq = q;
                    preva = userAnswers[i];
                    text1 += preva + ",";
                }
                else
                {
                    text1 += preva + ",";
                }
            }
            foreach (var answer in answers) { text2 += answer + ","; }
            text1 = text1.TrimEnd(',');
            text2 = text2.TrimEnd(',');

            ProcessStartInfo start = new ProcessStartInfo();
            if (useinterpretetere)
            {
                start.FileName = "python"; // или полный путь к python.exe
                start.Arguments = $"{System.IO.Directory.GetCurrentDirectory()[..(System.IO.Directory.GetCurrentDirectory().Length - 25)]}\\dlyacheka.py \"{text1}\" \"{text2}\"";//python файл
            }
            else
            {
                start.FileName = $"{System.IO.Directory.GetCurrentDirectory()[..(System.IO.Directory.GetCurrentDirectory().Length - 25)]}\\dlyacheka.exe"; // при перемещении файлов проекта могут возникнуть проблемы
                start.Arguments = $"\"{text1}\" \"{text2}\"";
            }
            start.UseShellExecute = false;
            start.RedirectStandardOutput = true;
            //start.CreateNoWindow = true;// не знаю как лучше но пускай пока так
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
            checkAnswerButton.Enabled = false;
            loadQuestionsButton.Enabled = false;

            List<float> result = new List<float>();
            float[] tm = new float[questions.Count];
            string[] answersnew = new string[questions.Count];

            //var (previous, nextUniqueIndex) = FindDifferentAdjacentStringIndices(questions.ToArray(), 0, true);
            int ind = 0;
            string prevq = questions[0];
            for (int i = 0; i < questions.Count; i++)
            {
                if (questions[i] == prevq)
                {
                    answersnew[i] = userAnswers[de[ind]];
                }
                else
                {
                    prevq = questions[i];
                    ind++;
                    answersnew[i] = userAnswers[de[ind]];
                }
            }
            userAnswers = answersnew;//А ЗАЧЕМ??

            string checkedAnsw = await Task.Run(bublic);
            string[] chchc = checkedAnsw.Split('\n');//тут хранится точность при сравнении ответов (в массиве 8 элементов а не 7 по ошибке и это теперь часть программы)

            checkedAnswerTextBox.Text = checkedAnsw;
            checkedAnswerTextBox.Text += $"{Environment.NewLine}{chchc.Length}";
            ISet<string> set = new HashSet<string>(questions);
            float[] answvaluevisible = new float[set.Count], answvalue = new float[chchc.Length - 1];
            for (int i = 0; i < chchc.Length - 1; i++)
            {
                checkedAnsw = chchc[i][(chchc[i].Length - 8)..];
                answvalue[i] = Convert.ToSingle(checkedAnsw.Replace('.', ','));
                checkedAnswerTextBox.Text += $"{checkedAnsw}, ";
            }
            ind = 0;
            prevq = questions[0];
            for (int i = 0; i < questions.Count; i++)
            {


                if (questions[i] == prevq)
                {
                    result.Add(Convert.ToSingle(Convert.ToString(answvalue[i]).Replace(".", ",")));
                }
                else
                {
                    prevq = questions[i];
                    if (ind == 0)
                    {
                        for (int j = 0; j < de[ind + 1]; j++)
                        {
                            tm[j] = result.Max();
                        }

                    }
                    else
                    {
                        for (int j = de[ind]; j < de[ind + 1]; j++)
                        {
                            tm[j] = result.Max();
                        }
                    }
                    ind++;
                    result.Clear();
                    result.Add(Convert.ToSingle(Convert.ToString(answvalue[i]).Replace(".", ",")));
                }
            }
            for (int j = de[ind]; j < questions.Count; j++)
            {
                tm[j] = result.Max();
            }
            //MessageBox.Show($"{answvaluevisible.ToString()}");
            tm = tm.ToHashSet().ToArray();
            string rrrrr = "";
            for (int i = 0; i < tm.Length; i++) { checkedAnswerTextBox.Text += rrrrr += $"Вопрос {i + 1}: {tm[i] > 0.88} {Environment.NewLine}"; }
            MessageBox.Show(rrrrr, "Точность ответов");
            /*
             Потом придумать можно что с ответами делать запись файл там или на сервер переслать
             */


            //foreach (var qwqe in set) { checkedAnswerTextBox.Text += qwqe.ToString(); }


            checkAnswerButton.Enabled = true;
            loadQuestionsButton.Enabled = true;
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
                var (previous, nextUniqueIndex) = FindDifferentAdjacentStringIndices(questions.ToArray(), currentQuestion, true);
                if (nextUniqueIndex > 0)
                {
                    currentAnswer++;
                    currentQuestion = nextUniqueIndex;
                    questionTextBox.Text = questions[currentQuestion];
                    userAnswerTextBox.Text = userAnswers[de[currentAnswer]];
                    checkedAnswerTextBox.Text = de[currentAnswer].ToString();
                    //foreach(var q in questions.ToArray()) {checkedAnswerTextBox.Text += $"{q.ToString()} "; }
                }
                if (previous > 0)
                {

                }
            }
        }


        private void button1_Click_1(object sender, EventArgs e)//prev q
        {
            if (questions != null)
            {
                var (previous, nextUniqueIndex) = FindDifferentAdjacentStringIndices(questions.ToArray(), currentQuestion, true);
                if (nextUniqueIndex > 0)
                {

                }
                if (previous > 0)
                {
                    currentAnswer--;
                    currentQuestion = previous;
                    questionTextBox.Text = questions[currentQuestion];
                    userAnswerTextBox.Text = userAnswers[de[currentAnswer]];
                    checkedAnswerTextBox.Text = de[currentAnswer].ToString();
                    //foreach(var q in questions.ToArray()) {checkedAnswerTextBox.Text += $"{q.ToString()} "; }
                }
            }
        }
        public static (int previousIndex, int nextIndex) FindDifferentAdjacentStringIndices(string[] array, int currentIndex, bool includePrevious = false)
        {
            if (array == null)
                throw new ArgumentNullException(nameof(array));

            if (currentIndex < 0 || currentIndex >= array.Length)
                throw new ArgumentOutOfRangeException(nameof(currentIndex));

            string current = array[currentIndex];
            int previousDifferentIndex = -1;
            int nextDifferentIndex = -1;

            // Поиск предыдущей отличной строки
            if (includePrevious)
            {
                for (int i = currentIndex - 1; i >= 0; i--)
                {
                    if (!string.Equals(array[i], current, StringComparison.Ordinal))
                    {
                        previousDifferentIndex = i;
                        break;
                    }
                }
            }

            // Поиск следующей отличной строки
            for (int i = currentIndex + 1; i < array.Length; i++)
            {
                if (!string.Equals(array[i], current, StringComparison.Ordinal))
                {
                    nextDifferentIndex = i;
                    break;
                }
            }

            return (previousDifferentIndex, nextDifferentIndex);
        }


        private void loadQuestionsButton_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
            if (openFileDialog1.SafeFileName != "openFileDialog1")
            {
                userAnswerTextBox.Enabled = true;
                (questions, answers) = QAPairLoader.LoadQAPairsFromFile(openFileDialog1.FileName);

                de = new int[questions.Count];
                for (int i = 0; i < questions.Count; i++)
                {
                    var (x, y) = FindDifferentAdjacentStringIndices(questions.ToArray(), i);
                    if (y == -1) { y = 0; }
                    de[i] = y;
                }
                de[0] = 1;
                de = de.ToHashSet().ToArray();
                if (questions != null && answers != null)
                {
                    questionTextBox.Text = questions[currentQuestion];
                    userAnswers = new string[answers.Count];
                }
            }
        }

        private void questionTextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void userAnswerTextBox_TextChanged(object sender, EventArgs e)
        {
            userAnswers[de[currentAnswer]] = userAnswerTextBox.Text;
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
                    throw new NotSupportedException("Поддерживаются только файлы формата CSV и XLSX");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка при загрузке файла: {ex.Message}");
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
                    // обработка CSV с учётом кавычек (разделитель - запятая)
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

        private static List<string> SplitAnswers(string answerString)
        {
            var answers = new List<string>();

            if (answerString.Contains("\""))
            {
                answerString = Regex.Replace(answerString, @"\s*,\s*", ",");
                var matches = Regex.Matches(answerString, @"(?:\""([^\""]*)\""|([^,]+))");

                foreach (Match match in matches)
                {
                    var answer = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;
                    if (!string.IsNullOrWhiteSpace(answer))
                        answers.Add(answer.Trim());
                }
            }
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

            values.Add(line.Substring(startIndex).Trim(' ', '"'));

            return values;
        }
    }
}
