using System;
using System.IO;
using System.Windows.Forms;
using VisualEditor.Logic.Warehouse;
using VisualEditor.Utils.Controls.HtmlEditing;
using VisualEditor.Logic.Helpers.AppSettings;
using System.Xml;
using VisualEditor.Logic.Course.Items.Questions;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.IO.Questions;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.Helpers;
using ICSharpCode.SharpZipLib.Zip;
using System.Collections;



namespace VisualEditor.Logic.Commands.IO
{
    internal class LoadFromImsQti : AbstractCommand
    {
        private const string UnableAddMessage = "Невозможно добавить вопрос к учебному модулю. Создайте входной или выходной контроль.";
        //private Question q;
        //private TestModule m;
        // private Group gr;
        private Enums.QuestionSequence questionSequence; // порядок следования вопросов
        private string testName;

        /// <summary>
        /// Группы вопросов, входящие в тест.
        /// </summary>
        public ArrayList groups;

        TestModule tmd;
        Question qn;

        // string s = ""; //правильный вариант ответа
        private string identificator;   // короткий идентификатор теста вида VETEST-<number>. Используется в формате IMS QTI
        private int defaultValue;       // значение оценки по умолчанию
        private Enums.NavigationMode navigationMode;     // режим навигации (может ли пользователь свободно перемещаться по тесту)
        private int questionNumber;     // номер, который присваивается новому вопросу
        private int groupNumber;        // номер, который присваивается новой группе
        /// <summary>
        /// Вопросы, входящие в тест, не принадлежащие группам.
        /// </summary>
        public ArrayList questions;

        /// <summary>
        /// Последовательность вопросов (естественная, случайная или сетевая).
        /// </summary>
        public Enums.QuestionSequence QuestionSequence
        {
            get
            {
                return questionSequence;
            }
            set
            {
                questionSequence = value;
            }
        }

        /// <summary>
        /// Номер, который присваивается новому вопросу.
        /// </summary>
        public int QuestionNumber
        {
            get
            {
                return questionNumber;
            }
            set
            {
                questionNumber = value;
            }
        }

        /// <summary>
        /// Номер, который присваивается новой группе.
        /// </summary>
        public int GroupNumber
        {
            get
            {
                return groupNumber;
            }
            set
            {
                groupNumber = value;
            }
        }


        public LoadFromImsQti()
        {
            name = CommandNames.LoadFromImsQti;
            text = CommandTexts.LoadFromImsQti;
            // image = Properties.Resources.OpenProject;
            groups = new ArrayList(0);
            questions = new ArrayList(0);
            questionNumber = 1;
            groupNumber = 1;
        }

        public static bool IsBusy { get; set; }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            if (IsBusy)
            {
                return;
            }

            IsBusy = true;

            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TrainingModule || Warehouse.Warehouse.Instance.CourseTree.CurrentNode is CourseRoot)
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Импортировать тест из упаковки IMS QTI";
                    openFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();
                    openFileDialog.Filter = "ZIP (*.zip)|*.zip";

                    if (openFileDialog.ShowDialog().Equals(DialogResult.OK))
                    {
                        tmd = new TestModule();
                        string path = openFileDialog.FileName;
                        ImportTestFromQtiPackage(path);

                        tmd.Text = testName;
                        tmd.Id = Guid.NewGuid();
                        tmd.QuestionSequence = questionSequence;
                        tmd.TestType = Enums.TestType.OutTest;

                        if (groups.Count > 0)
                            foreach (Group gr in groups)
                            {
                                if (gr!=null)
                                    tmd.Nodes.Add(gr);
                                if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
                                {
                                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
                                }
                            }

                        if (questions.Count > 0)
                            foreach (Question qu in questions)
                            {
                                if (qu!=null)
                                    tmd.Nodes.Add(qu);
                                if (!Warehouse.Warehouse.Instance.CourseTree.CurrentNode.IsExpanded)
                                {
                                    Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Toggle();
                                }
                            }
                        if (tmd!=null)
                        {
                            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(tmd);
                            Warehouse.Warehouse.IsProjectModified = true;
                        }
                    }
                }
            }


            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TestModule || Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Group)
            {
                using (var openFileDialog = new OpenFileDialog())
                {
                    openFileDialog.Title = "Импортировать вопрос из IMS QTI";
                    openFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();
                    openFileDialog.Filter = "XML (*.xml)|*.xml";

                    if (openFileDialog.ShowDialog().Equals(DialogResult.OK))
                    {
                        tmd = new TestModule();
                        string path = openFileDialog.FileName;
                       
                        qn = ReadQuestionFile(path, tmd);

                        if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Group)
                        {
                            var g = Warehouse.Warehouse.Instance.CourseTree.CurrentNode as Group;
                            qn.Text = string.Concat("Вопрос ", g.Questions.Count + 1);

                            qn.TimeRestriction = g.TimeRestriction;
                            qn.Profile = g.Profile;
                            qn.Marks = g.Marks;
                        }

                        if (qn!=null)
                        {
                            Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Nodes.Add(qn);
                            Warehouse.Warehouse.IsProjectModified = true;
                        }
                    }
                }
               
                
            }

            IsBusy = false;
        }

        /// <summary>
        /// Импорт теста из упаковки формата IMS QTI 2.1
        /// </summary>
        /// <param name="packageFile">Путь к упаковке.</param>
        public bool ImportTestFromQtiPackage(string packageFile)
        {
            FastZip fz = new FastZip();
            fz.ExtractZip(packageFile, Warehouse.Warehouse.ProjectEditorLocation + "\\ImsQtiPackage", "");
            string directory = Warehouse.Warehouse.ProjectEditorLocation + "\\ImsQtiPackage";//распаковываем архив и получаем папку, куда распаковываем архив 

            string testFileName = string.Empty;     //путь к файлу теста
            string manifestFileName = string.Empty; //путь к файлу манифеста            

            if (IsManifestFound(directory, out manifestFileName)) //если нашли файл манифеста...
            {
                if (!(testFileName = ExtractTestFileNameFromManifest(manifestFileName)).Equals(string.Empty))
                {
                    bool isTest = false;
                    XmlTextReader testReader = new XmlTextReader(testFileName);
                    while (testReader.Read())
                    {
                        if (testReader.NodeType == XmlNodeType.Element)
                        {
                            if (testReader.Name.Equals("assessmentTest", StringComparison.OrdinalIgnoreCase))
                            {
                                isTest = true;
                            }
                        }
                    }
                    testReader.Close();
                    if (isTest)
                    {
                        ReadQti(testFileName); //...импортируем файл теста вместе с вопросами и группами 
                    }
                    else
                    {
                        MessageBox.Show("Упаковка содержит неподдерживаемый файл теста.",
                                "Visual Editor",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                        Directory.Delete(directory, true);
                        return false;
                    }
                }
                else
                {
                    Directory.Delete(directory, true);
                    return false;
                }
            }
            else
            {
                MessageBox.Show("Этот файл не является упаковкой IMS QTI",
                                "Visual Editor",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                Directory.Delete(directory, true);
                return false;
            }
            Directory.Delete(directory, true);

            tmd.success = true;
            return true;
        }

        /// <summary>
        /// Импорт файла теста из формата IMS QTI. 
        /// </summary>
        /// <param name="fileName">Имя файла теста.</param>
        public void ReadQti(string fileName)
        {
            XmlTextReader reader = new XmlTextReader(fileName);
            string testFolder = Path.GetDirectoryName(fileName);

            while (reader.Read())
            {
                if (reader.NodeType == XmlNodeType.Element)
                {
                    if (reader.Name.Equals("assessmentTest"))
                    {
                        identificator = reader.GetAttribute("identifier");
                        if (reader.GetAttribute("title") != "")
                            testName = reader.GetAttribute("title");
                        else testName = identificator;

                    }
                    if (reader.Name.Equals("defaultValue"))
                    {
                        bool endCycle = false;
                        // считываем содержимое defaultValue
                        while (!endCycle && reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (reader.Name.Equals("value"))
                                {
                                    int.TryParse(reader.ReadElementString(), out defaultValue);
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                if (reader.Name.Equals("defaultValue"))
                                {
                                    endCycle = true;
                                }
                            }
                        }
                    }
                    if (reader.Name.Equals("testPart"))
                    {
                        if (reader.GetAttribute("navigationMode") == "linear")
                        {
                           // navigationMode = Enums.NavigationMode.linear;
                            questionSequence = Enums.QuestionSequence.Natural;
                        }
                        else if (reader.GetAttribute("navigationMode") == "nonlinear")
                        {
                          //  navigationMode = Enums.NavigationMode.nonlinear;
                            questionSequence = Enums.QuestionSequence.Random;
                        }
                    }
                    if (reader.Name.Equals("assessmentSection"))
                    {
                        Group group = new Group();
                        group.Text = reader.GetAttribute("title");  //хз.. вроде так)
                        bool endCycle = false;

                        while (!endCycle && reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                if (reader.Name.Equals("timeLimits", System.StringComparison.OrdinalIgnoreCase))
                                {
                                    int tr;
                                    int.TryParse(reader.GetAttribute("maxTime"), out tr);
                                    group.TimeRestriction = tr;
                                }

                                if (reader.Name.Equals("assessmentItemRef"))
                                {
                                    Question q = ReadQuestionFile(testFolder + "\\" + reader.GetAttribute("href"), tmd);//(TestModule)Warehouse.Warehouse.Instance.CourseTree.CurrentNode);
                                    if (q != null)
                                    {
                                        if (!reader.IsEmptyElement)
                                        {
                                            bool end = false;
                                            while (!end && reader.Read())
                                            {
                                                if (reader.NodeType == XmlNodeType.Element)
                                                {
                                                    if (reader.Name.Equals("timeLimits", System.StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        int tr;
                                                        int.TryParse(reader.GetAttribute("maxTime"), out tr);
                                                        q.TimeRestriction = tr;
                                                    }
                                                }
                                                else if (reader.NodeType == XmlNodeType.EndElement)
                                                {
                                                    if (reader.Name.Equals("assessmentItemRef", System.StringComparison.OrdinalIgnoreCase))
                                                    {
                                                        end = true;
                                                    }
                                                }
                                            }
                                        }
                                        //пока не поняла, зачем это вообще надо
                                        // q.TestModule = (TestModule)Warehouse.Warehouse.Instance.CourseTree.CurrentNode;//this;
                                        // q.Group = group;
                                        group.Questions.Add(q);
                                        //roups
                                        if (q.Text != "")
                                            group.Nodes.Add(q);//скорее эта строчка правильная)
                                    }
                                }
                            }
                            else if (reader.NodeType == XmlNodeType.EndElement)
                            {
                                if (reader.Name.Equals("assessmentSection"))
                                {
                                    endCycle = true;
                                }
                            }
                        }
                        // group.QuestionNumber = group.questions.Count + 1;
                        groups.Add(group);

                        //  groups.
                    }
                    if (reader.Name.Equals("assessmentItemRef"))
                    {
                        Question q = ReadQuestionFile(testFolder + "\\" + reader.GetAttribute("href"), tmd);//(TestModule)Warehouse.Warehouse.Instance.CourseTree.CurrentNode /*this*/);
                        if (q != null)
                        {
                            if (!reader.IsEmptyElement)
                            {
                                bool end = false;
                                while (!end && reader.Read())
                                {
                                    if (reader.NodeType == XmlNodeType.Element)
                                    {
                                        if (reader.Name.Equals("timeLimits", System.StringComparison.OrdinalIgnoreCase))
                                        {
                                            int tr;
                                            int.TryParse(reader.GetAttribute("maxTime"), out tr);
                                            q.TimeRestriction = tr;
                                        }
                                    }
                                    else if (reader.NodeType == XmlNodeType.EndElement)
                                    {
                                        if (reader.Name.Equals("assessmentItemRef", System.StringComparison.OrdinalIgnoreCase))
                                        {
                                            end = true;
                                        }
                                    }
                                }
                            }
                            //  q.TestModule = this;
                            questions.Add(q);
                            //    TestModule tmd = (TestModule)Warehouse.Warehouse.Instance.CourseTree.CurrentNode;
                            //    tmd.Nodes.Add(q);
                            //   int o = 90;
                            //tmd.Questions.Count+=1;

                        }
                    }
                }
            }
            reader.Close();
            questionNumber = questions.Count + 1;
            groupNumber = groups.Count + 1;
            if (groups.Count != 0)
            {
                QuestionSequence = Enums.QuestionSequence.Random;
            }

        }

        #region читаем вопроса файл
        /// <summary>
        /// считывает файл вопроса
        /// </summary>
        /// <returns>считанный вопрос</returns>
        public Question ReadQuestionFile(string qfPath, TestModule test)
        {
            // пока сделан вариант только для вопроса типов Choice

            // когда будут реализованы все варианты вопросов, сделать Question question = null
            Question question = new MultichoiceQuestion();
            XmlTextReader reader = null;

            string interaction = string.Empty;
            string title = string.Empty;
            string cardinality = string.Empty;
            bool isAdaptive = false;
            bool isSupported = false;

            if (!File.Exists(qfPath))
            {
                MessageBox.Show("Файла " + qfPath + " не существует!");
            }
            else
            {
                try
                {
                    bool isQuestion = false;

                    // проверяем, является ли файл вопросом
                    reader = new XmlTextReader(qfPath);
                    while (reader.Read())
                    {
                        if (reader.NodeType == XmlNodeType.Element)
                        {
                            if (reader.Name.Equals("assessmentItem", StringComparison.OrdinalIgnoreCase))
                            {
                                isQuestion = true;
                            }
                        }
                        if (isQuestion == true) break;
                    }
                    reader.Close();
                    if (isQuestion)
                    {
                        // считываем из файла тип вопроса                 
                        reader = new XmlTextReader(qfPath);
                        while (reader.Read())
                        {
                            if (reader.NodeType == XmlNodeType.Element)
                            {
                                // считываем название вопроса
                                if (reader.Name.Equals("assessmentItem", StringComparison.OrdinalIgnoreCase))
                                {
                                    if (reader.GetAttribute("title") != null)
                                    {
                                        title = reader.GetAttribute("title");
                                    }
                                    if (reader.GetAttribute("adaptive") != null)
                                    {
                                        isAdaptive = bool.Parse(reader.GetAttribute("adaptive"));
                                    }
                                }
                                if (!isAdaptive)
                                {
                                    // считываем признак типа вопроса
                                    if (reader.Name.Equals("responseDeclaration", StringComparison.OrdinalIgnoreCase))
                                    {
                                        cardinality = reader.GetAttribute("cardinality");
                                    }
                                    // вопросы типа "Выбор"
                                    if (reader.Name.Equals("choiceInteraction", StringComparison.OrdinalIgnoreCase))
                                    {
                                        isSupported = true;
                                        // Одновариантный выбор
                                        if (cardinality.Equals("single", StringComparison.OrdinalIgnoreCase))
                                        {
                                            question = new ChoiceQuestion();
                                        }
                                        // Множественный выбор
                                        else if (cardinality.Equals("multiple", StringComparison.OrdinalIgnoreCase))
                                        {
                                            question = new MultichoiceQuestion();
                                        }
                                    }
                                    // вопросы типа "Упорядочивание элементов ответа"
                                    if (reader.Name.Equals("orderInteraction", StringComparison.OrdinalIgnoreCase))
                                    {
                                        isSupported = true;
                                        question = new OrderingQuestion();
                                    }
                                    // Открытые вопросы
                                    if (reader.Name.Equals("textEntryInteraction", StringComparison.OrdinalIgnoreCase))
                                    {
                                        isSupported = true;
                                        question = new OpenQuestion();
                                    }
                                }
                            }
                        }
                        if (!isSupported)
                        {
                            // question = new UnknownQuestion(title);
                            MessageBox.Show("Данный тип вопросов не поддерживается приложением Visual Editor.",
                                    "Visual Editor",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                            question = null;
                        }
                        question.TestModule = test;
                        if (!question.ReadQti(qfPath))
                        {
                            question = null;
                        }
                    }
                    else
                    {
                        MessageBox.Show("Данный файл не является файлом вопроса в стандарте IMS QTI 2.1.",
                                    "Visual Editor",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                        question = null;
                    }
                }
                catch
                {
                    MessageBox.Show("При чтении файла вопроса произошла ошибка. (" + qfPath + ")",
                                    "Visual Editor",
                                    MessageBoxButtons.OK,
                                    MessageBoxIcon.Error);
                    question = null;
                }
                finally
                {
                    if (reader != null)
                    {
                        reader.Close();
                    }
                }
            }

            return question;
        }
        #endregion

        #region поиск файла манифеста в упаковке
        /// <summary>
        /// Процедура поиска файла манифеста в упаковке.
        /// </summary>        
        private bool IsManifestFound(string directory, out string manifestFileName)
        {
            manifestFileName = string.Empty;
            if (Directory.GetFiles(directory).Length != 0)
            {
                foreach (string fileName in Directory.GetFiles(directory))
                {
                    //если нашли файл манифеста...
                    if (Path.GetFileName(fileName) == "imsmanifest.xml")
                    {
                        manifestFileName = fileName;
                        return true; //...выходим из рекурсии
                    }
                }
            }
            if (Directory.GetDirectories(directory).Length != 0)
            {
                foreach (string directoryName in Directory.GetDirectories(directory))
                {
                    //если рекурсивно нашли файл манифеста...
                    if (IsManifestFound(directoryName, out manifestFileName))
                    {
                        return true; //...выходим из рекурсии
                    }
                }
            }

            return false;
        }
        #endregion

        #region поиск имени теста в файле манифеста
        public static string ExtractTestFileNameFromManifest(string manifestFile)
        {
            XmlTextReader reader = new XmlTextReader(manifestFile);

            string testFileName = string.Empty;
            string folder = Path.GetDirectoryName(manifestFile);
            bool testFound = false;

            try
            {
                while (reader.Read())
                {
                    if (reader.NodeType == XmlNodeType.Element)
                    {
                        if (reader.Name.Equals("resource", StringComparison.OrdinalIgnoreCase))
                        {
                            if (reader.GetAttribute("type").Contains("test"))
                            {
                                testFileName = folder + "/" + reader.GetAttribute("href");
                                testFound = true;
                            }
                        }
                    }
                }
            }
            catch
            {
                MessageBox.Show("При чтении файла манифеста произошла ошибка. (" + manifestFile + ")",
                                "Visual Editor",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
            }
            finally
            {
                reader.Close();
                if (!testFound)
                {
                    testFileName = string.Empty;
                    MessageBox.Show("Упаковка не содержит файл теста.",
                                "Visual Editor",
                                MessageBoxButtons.OK,
                                MessageBoxIcon.Error);
                }
            }

            return testFileName;
        }
        #endregion


    }
}
