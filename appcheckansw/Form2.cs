using DocumentFormat.OpenXml;
using DocumentFormat.OpenXml.Packaging;
using DocumentFormat.OpenXml.Spreadsheet;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace appcheckansw
{
    public partial class Form2 : Form
    {
        public Form2()
        {
            InitializeComponent();

        }
        public static List<string>? answers, questions;
        public static string[]? userAnswers;
        public int currentAnswer = 0;

        private void button1_Click(object sender, EventArgs e)
        {
            openFileDialog1.ShowDialog(this);
            (questions, answers) = QAPairLoader.LoadQAPairsFromFile(openFileDialog1.FileName);
            if (questions != null && answers != null)
            {
                for (int i = 0; i < answers.Count; i++)
                {
                    try
                    {
                        if (questions[i] != questions[i - 1])
                        {
                            QATextBox.Text += $"{Environment.NewLine}Вопрос #{i + 1}: {Environment.NewLine}{questions[i]}{Environment.NewLine}";
                            currentAnswer = 0;
                        }
                    }
                    catch { QATextBox.Text += $"Вопрос #{i + 1}: {Environment.NewLine}{questions[i]}{Environment.NewLine}"; }
                    QATextBox.Text += $"Ответ: {Environment.NewLine}{answers[i]}{Environment.NewLine}";
                    currentAnswer++;

                }
                QATextBox.Text = QATextBox.Text[..(QATextBox.Text.Length - 1)];
            }
        }
        public string s = "";
        private void button2_Click(object sender, EventArgs e)
        {
            s = QATextBox.Text;

            QuestionAnswerParser.ParseAndSaveToExcel(s, "QuestionsAnswers.xlsx");
            //s=answers[9];
            //s = "Вопрос #1: \r\nШО ТАКОЕ ПАРИШ\r\nОтвет #1: \r\nПариж является столицей Франции\r\n\r\nВопрос #2: \r\nчто такое фотосинтез\r\nОтвет #1: \r\nхимический процесс преобразования энергии видимого света в энергию химических связей органических веществ при участии фотосинтетических пигментов.\r\n";
        }

        private void QATextBox_TextChanged(object sender, EventArgs e)
        {

        }

        private void QATextBox_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.Enter)
            {
                e.SuppressKeyPress = true; // Отменяем стандартное поведение Enter

                int cursorPos = QATextBox.SelectionStart;
                int currentLine = GetCurrentLineIndex(QATextBox, cursorPos);
                string currentLineText = QATextBox.Lines.Length > currentLine ? QATextBox.Lines[currentLine] : "";

                // Если дважды Enter подряд (пустая строка) — создаём новый вопрос
                if (string.IsNullOrWhiteSpace(currentLineText))
                {
                    InsertNewQuestion();
                    //Thread.Sleep(500);
                    //InsertNewAnswer(currentLineText);
                    QATextBox.AppendText(Environment.NewLine +Environment.NewLine + "Ответ:" + Environment.NewLine);
                }
                // Если строка начинается с "Вопрос #X:" — добавляем "Ответ #X.1:" на новой строке
                else if (currentLineText.Trim().StartsWith("Вопрос #"))
                {
                    InsertNewAnswer(currentLineText);
                }
                else
                {
                    // Иначе просто вставляем перевод строки
                    QATextBox.SelectedText = Environment.NewLine;
                }
            }
        }

        // Определяет номер строки по позиции курсора
        private int GetCurrentLineIndex(TextBox textBox, int cursorPosition)
        {
            int line = 0;
            int currentPos = 0;

            foreach (string lineText in textBox.Lines)
            {
                currentPos += lineText.Length + Environment.NewLine.Length;
                if (cursorPosition < currentPos)
                    return line;
                line++;
            }

            return line;
        }

        // Вставляет новый вопрос (если дважды нажат Enter)
        private void InsertNewQuestion()
        {
            // Находим последний номер вопроса
            int lastQuestionNumber = 1;
            foreach (string line in QATextBox.Lines)
            {
                if (line.Trim().StartsWith("Вопрос #"))
                {
                    string numStr = line.Split('#')[1].Split(':')[0];
                    if (int.TryParse(numStr, out int num) && num >= lastQuestionNumber)
                        lastQuestionNumber = num + 1;
                }
            }

            // Добавляем вопрос с новой строки
            QATextBox.AppendText(Environment.NewLine + $"Вопрос #{lastQuestionNumber}:");//раньше было еще раз{Environment.NewLine} в конце
            QATextBox.SelectionStart = QATextBox.TextLength;
        }

        // Вставляет новый ответ на текущий вопрос
        private void InsertNewAnswer(string questionLine)
        {
            // Извлекаем номер вопроса
            string numStr = questionLine.Split('#')[1].Split(':')[0];
            if (!int.TryParse(numStr, out int questionNumber))
                questionNumber = 1;

            // Находим последний номер ответа для этого вопроса
            int lastAnswerNumber = 1;
            bool foundQuestion = false;

            foreach (string line in QATextBox.Lines)
            {
                if (line.Trim().StartsWith($"Вопрос #{questionNumber}:"))
                {
                    foundQuestion = true;
                }
                else if (foundQuestion && line.Trim().StartsWith($"Ответ #{questionNumber}."))
                {
                    string ansNumStr = line.Split('#')[1].Split('.')[0];
                    if (int.TryParse(ansNumStr, out int ansNum) && ansNum >= lastAnswerNumber)
                        lastAnswerNumber = ansNum + 1;
                }
            }

            // Добавляем ответ с новой строки

            QATextBox.AppendText(Environment.NewLine + $"Ответ:" + Environment.NewLine);

            //QATextBox.AppendText(Environment.NewLine + $"Ответ #{questionNumber}.{lastAnswerNumber}:" + Environment.NewLine);//так и не заработало
            QATextBox.SelectionStart = QATextBox.TextLength;
        }
    }

    public class QuestionAnswerParser
    {
        public static void ParseAndSaveToExcel(string inputText, string outputFilePath)
        {
            var questions = new Dictionary<int, (string Question, List<string> Answers)>();

            using (var reader = new StringReader(inputText))
            {
                string line;
                int currentQuestionNumber = -1;
                int currentAnswerNumber = -1;
                string currentQuestion = null;
                List<string> currentAnswers = null;

                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim();

                    if (line.StartsWith("Вопрос "))
                    {

                        // Обработка новой строки с вопросом
                        var parts = line.Split(new[] { ':', '#' }, StringSplitOptions.RemoveEmptyEntries);
                        if (parts.Length >= 2 && int.TryParse(parts[1].Trim(), out int questionNumber))
                        {
                            // Сохраняем предыдущий вопрос, если есть
                            if (currentQuestionNumber != -1)
                            {
                                questions[currentQuestionNumber] = (currentQuestion, currentAnswers);
                            }

                            // Начинаем новый вопрос
                            currentQuestionNumber = questionNumber;
                            currentQuestion = reader.ReadLine()?.Trim() ?? string.Empty;
                            currentAnswers = new List<string>();
                            currentAnswerNumber = -1;
                        }
                    }
                    else if (line.StartsWith("Ответ:"))
                    {
                        // Обработка строки с ответом (без номера)
                        var answer = reader.ReadLine()?.Trim() ?? string.Empty;
                        if (!string.IsNullOrEmpty(answer))
                        {
                            currentAnswers?.Add(answer);
                        }
                    }
                }

                // Добавляем последний вопрос
                if (currentQuestionNumber != -1)
                {
                    questions[currentQuestionNumber] = (currentQuestion, currentAnswers);
                }
            }

            // Создаем XLSX файл
            CreateExcelFile(questions, outputFilePath);
        }

        private static void CreateExcelFile(Dictionary<int, (string Question, List<string> Answers)> questions, string filePath)
        {
            using (var spreadsheet = SpreadsheetDocument.Create(filePath, SpreadsheetDocumentType.Workbook))
            {
                // Добавляем рабочую книгу
                var workbookPart = spreadsheet.AddWorkbookPart();
                workbookPart.Workbook = new Workbook();

                // Добавляем лист
                var worksheetPart = workbookPart.AddNewPart<WorksheetPart>();
                worksheetPart.Worksheet = new Worksheet(new SheetData());

                var sheets = spreadsheet.WorkbookPart.Workbook.AppendChild(new Sheets());
                sheets.Append(new Sheet()
                {
                    Id = spreadsheet.WorkbookPart.GetIdOfPart(worksheetPart),
                    SheetId = 1,
                    Name = "Вопросы и ответы"
                });

                var sheetData = worksheetPart.Worksheet.GetFirstChild<SheetData>();

                // Добавляем заголовки
                var headerRow = new Row();
                /*headerRow.Append(
                    new Cell() { CellValue = new CellValue("Вопрос"), DataType = CellValues.String },
                    new Cell() { CellValue = new CellValue("Ответы"), DataType = CellValues.String }
                );*/
                //sheetData.AppendChild(headerRow);

                // Добавляем данные
                foreach (var kvp in questions)
                {
                    var row = new Row();
                    var answers = string.Join(",", kvp.Value.Answers.Select(a => $"\"{a}\""));

                    row.Append(
                        new Cell() { CellValue = new CellValue(kvp.Value.Question), DataType = CellValues.String },
                        new Cell() { CellValue = new CellValue(answers), DataType = CellValues.String }
                    );

                    sheetData.AppendChild(row);
                }

                workbookPart.Workbook.Save();
            }
        }
    }
}
