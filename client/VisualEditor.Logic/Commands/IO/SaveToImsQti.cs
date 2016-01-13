using System.IO;
using VisualEditor.Logic.Controls.Docking;
using System.Collections;
using VisualEditor.Logic.Course.Items;
using System.Collections.Generic;
using System.Xml;
using ICSharpCode.SharpZipLib.Zip;
using VisualEditor.Logic.IO.Wrappers;
using System;
using System.Windows.Forms;
using VisualEditor.Logic.Helpers.AppSettings;

//using VisualEditor.Utils.Controls.HtmlEditing;

namespace VisualEditor.Logic.Commands.IO
{

    class SaveToImsQti : AbstractCommand
    {
        private const string operationCantBePerformedMessage = "Невозможно выполнить операцию. Попробуйте повтротить снова.";
        public List<Question> questions;
        public List<Group> groups;
        public TestModule tm;
        private string fileName;        // имя файла для экспорта в IMS QTI
        private string identificator;   // короткий идентификатор теста вида VETEST-<number>. Используется в формате IMS QTI
        private static int testNumber = 1;  // для идентификатора
        public int defaultValue;       // значение оценки по умолчанию
        private Enums.NavigationMode navigationMode;     // режим навигации (может ли пользователь свободно перемещаться по тесту)
        private Enums.SubmissionMode submissionMode;     // режим оценки вопросов
        string s = "ImsQtiPackage";//куда сохраняем тест
        string path = Warehouse.Warehouse.ProjectTrueLocation;


        /// <summary>
        /// Имя файла теста.
        /// </summary>
        public string FileName
        {
            get
            {
                return fileName;
            }
            set
            {
                fileName = value;
            }
        }

        public string Identificator
        {
            get
            {
                return identificator;
            }
        }

        public SaveToImsQti()
        {
            name = CommandNames.SaveToImsQti;
            text = CommandTexts.SaveToImsQti;
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

            //var hquest = (Question)Warehouse.Warehouse.Instance.CourseTree.CurrentNode;
           // string sks = hquest.DocumentHtml;
            /*
            #region Сохранение контента редакторов

            foreach (var tmd in DockContainer.Instance.TrainingModuleDocuments)
            {
                if (tmd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    tmd.TrainingModule.DocumentHtml = tmd.HtmlEditingTool.BodyInnerHtml;
                }
            }

            foreach (var qd in DockContainer.Instance.QuestionDocuments)
            {
                if (qd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    qd.Question.DocumentHtml = qd.HtmlEditingTool.BodyInnerHtml;
                }
            }

            foreach (var rd in DockContainer.Instance.ResponseDocuments)
            {
                if (rd.HtmlEditingTool.Mode == Utils.Controls.HtmlEditing.Enums.HtmlEditingToolMode.Design)
                {
                    rd.Response.DocumentHtml = rd.HtmlEditingTool.BodyInnerHtml;
                }
            }

            #endregion
            */
            
                // Сохраняет xml в ProjectTrueLocation.
           //     CommandManager.Instance.GetCommand(CommandNames.SaveToXml).Execute(null);
            //    CommandManager.Instance.GetCommand(CommandNames.SaveToHtp).Execute(null);
          
            
            if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is TestModule)
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Экспортировать модуль-контроль в IMS QTI";
                    saveFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();
                    saveFileDialog.Filter = "ZIP (*.zip)|*.zip";


                    if (saveFileDialog.ShowDialog().Equals(DialogResult.OK))
                    {
                        path = Path.GetDirectoryName(saveFileDialog.FileName);
                        if (Path.GetFileNameWithoutExtension(saveFileDialog.FileName).Length > 0)
                        {
                            s = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                        }
                        Directory.SetCurrentDirectory(path);
                        ExportTestToQti(s);
                    }
                }

               
               

            }
            else if (Warehouse.Warehouse.Instance.CourseTree.CurrentNode is Question)
            {
                using (var saveFileDialog = new SaveFileDialog())
                {
                    saveFileDialog.Title = "Экспортировать вопрос в IMS QTI";
                    saveFileDialog.InitialDirectory = AppSettingsHelper.GetInitialDirectory();
                    saveFileDialog.Filter = "XML (*.xml)|*.xml";
                    
                    if (saveFileDialog.ShowDialog().Equals(DialogResult.OK))
                    {
                        path = Path.GetDirectoryName(saveFileDialog.FileName);
                        if (Path.GetFileNameWithoutExtension(saveFileDialog.FileName).Length > 0)
                        {
                            s = Path.GetFileNameWithoutExtension(saveFileDialog.FileName);
                        }
                        Directory.SetCurrentDirectory(path);
                        var quest = (Question)Warehouse.Warehouse.Instance.CourseTree.CurrentNode;
                        string ss = quest.DocumentHtml;
                        quest.WriteQti(s + ".xml");
                    }
                }               
                
            }

            IsBusy = false;
        }

        #region запись в ImsQti2.1
        public void ExportTestToQti(string archiveName)
        {
            tm = (TestModule)Warehouse.Warehouse.Instance.CourseTree.CurrentNode;
            groups = tm.Groups;
            questions = tm.Questions;
            identificator = "VETEST" + testNumber.ToString();
            fileName = "resources\\" + identificator + ".xml";


            //массив всех вопросов в тесте 
            ArrayList allQuestions = new ArrayList(0);
            //имя папки, в которую сохраняются промежуточные файлы
            string folderName = System.Guid.NewGuid().ToString() + "\\";
            //создаём папку ресурсов  
            Directory.CreateDirectory(folderName + "resources");
            //добавляем все вопросы, включая содержащиеся в группах 
            allQuestions.AddRange(questions);

            foreach (Group group in groups)
            {
                allQuestions.AddRange(group.Questions);
            }
            //записываем файлы вопросов 
            foreach (Question question in allQuestions)
            {
                question.WriteQti(folderName + question.FileName);//"\\" + "resources" + "\\" + question.Text);
            }
            WriteManifestFile(folderName); //записываем файл манифеста            
            WriteQti(folderName);          //записываем файл теста            
            //пока можем создать только zip-архив
            if (Path.GetExtension(archiveName).ToLower() != ".zip")
            {
                if (Path.HasExtension(archiveName))
                {
                    archiveName = Path.ChangeExtension(archiveName, ".zip");
                }
                else
                {
                    archiveName += ".zip";
                }
            }
            //создаём архив
            FastZip fz = new FastZip
            {
                CreateEmptyDirectories = true
            };
            fz.CreateZip(archiveName, folderName, true, "");
            //удаляем временную папку
            Directory.Delete(folderName, true);
           // string t =
           // File.Move(Path.GetDirectoryName(archiveName), path);

        }
        #endregion

        #region пииишем в ImsQti
        public void WriteQti(string path)
        {
            XmlTextWriter writer;

            // формируем имя файла
            if (fileName == null || fileName == "")
            {
                writer = new XmlTextWriter(path + this.fileName, null);
            }
            else
            {
                if (Path.GetExtension(fileName).ToLower() == ".xml")
                {
                    writer = new XmlTextWriter(path + fileName, null);
                }
                else
                {
                    if (Path.HasExtension(fileName))
                    {
                        fileName = Path.ChangeExtension(fileName, ".xml");
                        writer = new XmlTextWriter(path + fileName, null);
                    }
                    else
                    {
                        writer = new XmlTextWriter(path + fileName + ".xml", null);
                    }
                }
            }

            writer.Formatting = Formatting.Indented;

            writer.WriteStartDocument();
            writer.WriteStartElement("assessmentTest");
            writer.WriteAttributeString("xmlns",
                    @"http://www.imsglobal.org/xsd/imsqti_v2p1");
            writer.WriteAttributeString("xmlns:xsi",
                    @"http://www.w3.org/2001/XMLSchema-instance");
            writer.WriteAttributeString("xsi:schemaLocation",
                    @"http://www.imsglobal.org/xsd/imsqti_v2p1 http://www.imsglobal.org/xsd/imsqti_v2p1.xsd");
            writer.WriteAttributeString("identifier", identificator);
            writer.WriteAttributeString("title", Warehouse.Warehouse.Instance.CourseTree.CurrentNode.Text);//Text);
            writer.WriteStartElement("outcomeDeclaration");
            writer.WriteAttributeString("baseType", "float");
            writer.WriteAttributeString("cardinality", "single");
            writer.WriteAttributeString("identifier", "SCORE");
            writer.WriteStartElement("defaultValue");
            writer.WriteElementString("value", defaultValue.ToString());
            writer.WriteFullEndElement(); //defaultValue
            writer.WriteFullEndElement(); //outcomeDeclaration            
            writer.WriteStartElement("testPart");
            writer.WriteAttributeString("identifier", "part01");
            writer.WriteAttributeString("navigationMode", navigationMode.ToString());
            writer.WriteAttributeString("submissionMode", submissionMode.ToString());
            foreach (Group group in groups)
            {
                string ident = group.Text;
                while (ident.IndexOf(" ") != -1)
                {
                    ident = ident.Remove(ident.IndexOf(" "), 1);
                }
                writer.WriteStartElement("assessmentSection");
                writer.WriteAttributeString("identifier", ident);
                writer.WriteAttributeString("title", group.Text);
                writer.WriteAttributeString("visible", "true");
                if (group.TimeRestriction != 0)
                {
                    writer.WriteStartElement("timeLimits");
                    writer.WriteAttributeString("maxTime", group.TimeRestriction.ToString());
                    writer.WriteFullEndElement(); //timeLimits 
                }
                writer.WriteStartElement("rubricBlock");
                writer.WriteAttributeString("view", "candidate");
                writer.WriteElementString("p", "Инструкции для " + group.Text.Replace("Группа", "Группы"));
                writer.WriteFullEndElement(); //rubricBlock                
                foreach (Question question in group.Questions)
                {
                    string n = Path.GetFileName(question.FileName);

                    writer.WriteStartElement("assessmentItemRef");
                    writer.WriteAttributeString("identifier", question.identifier);
                    writer.WriteAttributeString("href", n);
                    writer.WriteFullEndElement(); //assessmentItemRef     
                }
                writer.WriteFullEndElement(); //assessmentSection
            }
            foreach (Question question in questions)
            {
                string n = Path.GetFileName(question.FileName);

                writer.WriteStartElement("assessmentItemRef");
                writer.WriteAttributeString("identifier", question.identifier);
                writer.WriteAttributeString("href", n);
                if (question.TimeRestriction != 0)
                {
                    writer.WriteStartElement("timeLimits");
                    writer.WriteAttributeString("maxTime", question.TimeRestriction.ToString());
                    writer.WriteFullEndElement(); //timeLimits 
                }
                writer.WriteFullEndElement(); //assessmentItemRef     
            }
            writer.WriteFullEndElement(); //testPart
            writer.WriteStartElement("outcomeProcessing");
            writer.WriteStartElement("setOutcomeValue");
            writer.WriteAttributeString("identifier", "SCORE");
            writer.WriteStartElement("sum");
            foreach (Group group in groups)
            {
                foreach (Question question in group.Questions)
                {
                    string n = Path.GetFileName(question.FileName);
                    n = Path.ChangeExtension(n, ".SCORE");

                    writer.WriteStartElement("variable");
                    writer.WriteAttributeString("identifier", n);
                    writer.WriteFullEndElement(); //variable
                }

            }
            foreach (Question question in questions)
            {
                string n = Path.GetFileName(question.FileName);
                n = Path.ChangeExtension(n, ".SCORE");

                writer.WriteStartElement("variable");
                writer.WriteAttributeString("identifier", n);
                writer.WriteFullEndElement(); //variable
            }
            writer.WriteFullEndElement(); //sum
            writer.WriteFullEndElement(); //setOutcomeValue
            writer.WriteFullEndElement(); //outcomeProcessing
            writer.WriteFullEndElement(); //assessmentTest
            writer.WriteEndDocument();

            writer.Close();
        }
        #endregion

        #region создаем файл манифеста

        /// <summary>
        /// Запись файла манифеста.
        /// </summary>
        /// <param name="questionFiles">список вопросов теста</param>
        /// <remarks>Используется в экспорте теста в формате IMS QTI.</remarks>

        private void WriteManifestFile(string path)
        {
            XmlTextWriter imsManifestWriter = new XmlTextWriter(path + "imsmanifest.xml", null);
            // форматирование файла 
            imsManifestWriter.Formatting = Formatting.Indented;
            // создание файла imsmanifest.xml            
            imsManifestWriter.WriteStartDocument();
            imsManifestWriter.WriteStartElement("manifest");
            imsManifestWriter.WriteAttributeString("xmlns",
                @"http://www.imsglobal.org/xsd/imscp_v1p1");
            imsManifestWriter.WriteAttributeString("xmlns:imsmd",
                @"http://www.imsglobal.org/xsd/imsmd_v1p2");
            imsManifestWriter.WriteAttributeString("xmlns:xsi",
                @"http://www.w3.org/2001/XMLSchema-instance");
            imsManifestWriter.WriteAttributeString("xmlns:imsqti",
                @"http://www.imsglobal.org/xsd/imsqti_v2p1");
            imsManifestWriter.WriteAttributeString("identifier",
                "MANIFEST-" + System.Guid.NewGuid().ToString());
            imsManifestWriter.WriteAttributeString("xsi:schemaLocation",
                @"http://www.imsglobal.org/xsd/imscp_v1p1" +
                @"http://www.imsglobal.org/xsd/imscp_v1p2.xsd" +
                @"http://www.imsglobal.org/xsd/imsmd_v1p2" +
                @"http://www.imsglobal.org/xsd/imsmd_v1p2p4.xsd" +
                @"http://www.imsglobal.org/xsd/imsqti_v2p1" +
                @"http://www.imsglobal.org/xsd/imsqti_v2p1.xsd");
            imsManifestWriter.WriteStartElement("metadata");
            imsManifestWriter.WriteElementString("schema", "IMS Content");
            imsManifestWriter.WriteElementString("schemaVersion", "1.2");
            imsManifestWriter.WriteStartElement("imsmd:lom");
            imsManifestWriter.WriteStartElement("imsmd:general");
            imsManifestWriter.WriteStartElement("imsmd:title");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "ru");
            imsManifestWriter.WriteString("Упаковка IMS QTI");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:title
            imsManifestWriter.WriteElementString("imsmd:language", "ru");
            imsManifestWriter.WriteStartElement("imsmd:description");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "ru");
            imsManifestWriter.WriteString("Эта упаковка содержит файл теста и файлы вопросов");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:description
            imsManifestWriter.WriteFullEndElement(); // imsmd:general
            imsManifestWriter.WriteStartElement("imsmd:lifecycle");
            imsManifestWriter.WriteStartElement("imsmd:version");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "ru");
            imsManifestWriter.WriteString("1.0");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:version
            imsManifestWriter.WriteStartElement("imsmd:status");
            imsManifestWriter.WriteStartElement("imsmd:source");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "ru");
            imsManifestWriter.WriteString("LOMv1.0");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:source
            imsManifestWriter.WriteStartElement("imsmd:value");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "x-none");
            imsManifestWriter.WriteString("Final");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:value
            imsManifestWriter.WriteFullEndElement(); // imsmd:status
            imsManifestWriter.WriteFullEndElement(); // imsmd:lifecycle 
            imsManifestWriter.WriteStartElement("imsmd:metametadata");
            imsManifestWriter.WriteElementString("imsmd:metadatascheme", "LOMv1.0");
            imsManifestWriter.WriteElementString("imsmd:metadatascheme", "QTIv2.1");
            imsManifestWriter.WriteElementString("imsmd:language", "en");
            imsManifestWriter.WriteFullEndElement(); // imsmd:metametadata   
            imsManifestWriter.WriteStartElement("imsmd:technical");
            imsManifestWriter.WriteElementString("imsmd:format", "text/x-imsqti-test-xml");
            imsManifestWriter.WriteElementString("imsmd:format", "text/x-imsqti-item-xml");
            imsManifestWriter.WriteElementString("imsmd:format", "image/png");
            imsManifestWriter.WriteElementString("imsmd:format", "image/jpeg");
            imsManifestWriter.WriteElementString("imsmd:format", "image/bmp");
            imsManifestWriter.WriteElementString("imsmd:format", "image/gif");
            imsManifestWriter.WriteFullEndElement(); // imsmd:technical 
            imsManifestWriter.WriteStartElement("imsmd:rights");
            imsManifestWriter.WriteStartElement("imsmd:description");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "en");
            imsManifestWriter.WriteString("(c) 2005 - 2008, IMS Global Learning Consortium; Visual Editor");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:description 
            imsManifestWriter.WriteFullEndElement(); // imsmd:rights 
            imsManifestWriter.WriteFullEndElement(); // imsmd:lom 
            imsManifestWriter.WriteFullEndElement(); // metadata
            imsManifestWriter.WriteStartElement("organizations");
            imsManifestWriter.WriteFullEndElement(); // organizations  
            imsManifestWriter.WriteStartElement("resources");
            //файл теста
            imsManifestWriter.WriteStartElement("resource");
            imsManifestWriter.WriteAttributeString("identifier", "RES-" + Guid.NewGuid().ToString());//не уверена
            imsManifestWriter.WriteAttributeString("type", "imsqti_test_xmlv2p1");
            imsManifestWriter.WriteAttributeString("href", FileName);
            imsManifestWriter.WriteStartElement("metadata");
            imsManifestWriter.WriteStartElement("imsmd:lom");
            imsManifestWriter.WriteStartElement("imsmd:general");
            imsManifestWriter.WriteStartElement("imsmd:title");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "ru");
            imsManifestWriter.WriteString("Тест в формате IMS QTI");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:title
            imsManifestWriter.WriteElementString("imsmd:language", "ru");
            imsManifestWriter.WriteStartElement("imsmd:description");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "ru");
            imsManifestWriter.WriteString("Это тест в формате IMS QTI");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:description
            imsManifestWriter.WriteFullEndElement(); // imsmd:general
            imsManifestWriter.WriteStartElement("imsmd:lifecycle");
            imsManifestWriter.WriteStartElement("imsmd:version");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "ru");
            imsManifestWriter.WriteString("1.0");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:version
            imsManifestWriter.WriteStartElement("imsmd:status");
            imsManifestWriter.WriteStartElement("imsmd:source");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "ru");
            imsManifestWriter.WriteString("LOMv1.0");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:source
            imsManifestWriter.WriteStartElement("imsmd:value");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "x-none");
            imsManifestWriter.WriteString("Final");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:value
            imsManifestWriter.WriteFullEndElement(); // imsmd:status
            imsManifestWriter.WriteFullEndElement(); // imsmd:lifecycle 
            imsManifestWriter.WriteStartElement("imsmd:metametadata");
            imsManifestWriter.WriteElementString("imsmd:metadatascheme", "LOMv1.0");
            imsManifestWriter.WriteElementString("imsmd:language", "en");
            imsManifestWriter.WriteFullEndElement(); // imsmd:metametadata   
            imsManifestWriter.WriteStartElement("imsmd:technical");
            imsManifestWriter.WriteElementString("imsmd:format", "text/x-imsqti-test-xml");
            imsManifestWriter.WriteFullEndElement(); // imsmd:technical 
            imsManifestWriter.WriteStartElement("imsmd:rights");
            imsManifestWriter.WriteStartElement("imsmd:description");
            imsManifestWriter.WriteStartElement("imsmd:langstring");
            imsManifestWriter.WriteAttributeString("xml:lang", "en");
            imsManifestWriter.WriteString("(c) 2005 - 2008, IMS Global Learning Consortium; Visual Editor");
            imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
            imsManifestWriter.WriteFullEndElement(); // imsmd:description 
            imsManifestWriter.WriteFullEndElement(); // imsmd:rights 
            imsManifestWriter.WriteFullEndElement(); // imsmd:lom 
            imsManifestWriter.WriteFullEndElement(); // metadata
            imsManifestWriter.WriteStartElement("file");
            imsManifestWriter.WriteAttributeString("href", fileName);
            imsManifestWriter.WriteFullEndElement(); // file
            foreach (Group group in groups)
            {
                foreach (Question question in group.Questions)
                {
                    imsManifestWriter.WriteStartElement("dependency");
                    imsManifestWriter.WriteAttributeString("identifierref", "RES-" + question.Id.ToString());//guid меняю на Id наверное так
                    imsManifestWriter.WriteFullEndElement(); // dependency
                }
            }
            foreach (Question question in questions)
            {
                imsManifestWriter.WriteStartElement("dependency");
                imsManifestWriter.WriteAttributeString("identifierref", "RES-" + question.Id.ToString());
                imsManifestWriter.WriteFullEndElement(); // dependency
            }
            imsManifestWriter.WriteFullEndElement(); // resource
            //~~~файл теста
            //---файлы вопросов
            foreach (Group group in groups)
            {
                foreach (Question question in group.Questions)
                {
                    imsManifestWriter.WriteStartElement("resource");
                    imsManifestWriter.WriteAttributeString("identifier", "RES-" + question.Id.ToString());
                    imsManifestWriter.WriteAttributeString("type", "imsqti_item_xmlv2p1");
                    imsManifestWriter.WriteAttributeString("href", question.FileName);
                    imsManifestWriter.WriteStartElement("metadata");
                    imsManifestWriter.WriteStartElement("imsmd:lom");
                    imsManifestWriter.WriteStartElement("imsmd:general");
                    imsManifestWriter.WriteElementString("imsmd:identifier", question.identifier);
                    imsManifestWriter.WriteStartElement("imsmd:title");
                    imsManifestWriter.WriteStartElement("imsmd:langstring");
                    imsManifestWriter.WriteAttributeString("xml:lang", "ru");
                    imsManifestWriter.WriteString(question.Text);
                    imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                    imsManifestWriter.WriteFullEndElement(); // imsmd:title
                    imsManifestWriter.WriteElementString("imsmd:language", "ru");
                    imsManifestWriter.WriteStartElement("imsmd:description");
                    imsManifestWriter.WriteStartElement("imsmd:langstring");
                    imsManifestWriter.WriteAttributeString("xml:lang", "ru");
                    imsManifestWriter.WriteString(question.Description);
                    imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                    imsManifestWriter.WriteFullEndElement(); // imsmd:description
                    imsManifestWriter.WriteFullEndElement(); // imsmd:general
                    imsManifestWriter.WriteStartElement("imsmd:lifecycle");
                    imsManifestWriter.WriteStartElement("imsmd:version");
                    imsManifestWriter.WriteStartElement("imsmd:langstring");
                    imsManifestWriter.WriteAttributeString("xml:lang", "ru");
                    imsManifestWriter.WriteString("1.0");
                    imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                    imsManifestWriter.WriteFullEndElement(); // imsmd:version
                    imsManifestWriter.WriteStartElement("imsmd:status");
                    imsManifestWriter.WriteStartElement("imsmd:source");
                    imsManifestWriter.WriteStartElement("imsmd:langstring");
                    imsManifestWriter.WriteAttributeString("xml:lang", "x-none");
                    imsManifestWriter.WriteString("LOMv1.0");
                    imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                    imsManifestWriter.WriteFullEndElement(); // imsmd:source
                    imsManifestWriter.WriteStartElement("imsmd:value");
                    imsManifestWriter.WriteStartElement("imsmd:langstring");
                    imsManifestWriter.WriteAttributeString("xml:lang", "x-none");
                    imsManifestWriter.WriteString("Draft");
                    imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                    imsManifestWriter.WriteFullEndElement(); // imsmd:value
                    imsManifestWriter.WriteFullEndElement(); // imsmd:status
                    imsManifestWriter.WriteFullEndElement(); // imsmd:lifecycle                      
                    imsManifestWriter.WriteStartElement("imsmd:technical");
                    imsManifestWriter.WriteElementString("imsmd:format", "text/x-imsqti-item-xml");
                    imsManifestWriter.WriteFullEndElement(); // imsmd:technical 
                    imsManifestWriter.WriteStartElement("imsmd:rights");
                    imsManifestWriter.WriteStartElement("imsmd:description");
                    imsManifestWriter.WriteStartElement("imsmd:langstring");
                    imsManifestWriter.WriteAttributeString("xml:lang", "en");
                    imsManifestWriter.WriteString("(c) Visual Editor");
                    imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                    imsManifestWriter.WriteFullEndElement(); // imsmd:description 
                    imsManifestWriter.WriteFullEndElement(); // imsmd:rights 
                    imsManifestWriter.WriteFullEndElement(); // imsmd:lom 
                    imsManifestWriter.WriteStartElement("imsmd:qtiMetadata");
                    imsManifestWriter.WriteElementString("imsmd:timeDependent", question.IsTimeDependent.ToString());
                    imsManifestWriter.WriteElementString("imsmd:interactionType", question.InteractionType.ToString());
                    if (question.IsAdaptive)
                    {
                        imsManifestWriter.WriteElementString("imsmd:feedbackType", "adaptive");
                    }
                    else
                    {
                        imsManifestWriter.WriteElementString("imsmd:feedbackType", "none");
                    }
                    imsManifestWriter.WriteElementString("imsmd:solutionAvailable", "true");
                    imsManifestWriter.WriteFullEndElement(); // imsmd:qtiMetadata 
                    imsManifestWriter.WriteFullEndElement(); // metadata
                    imsManifestWriter.WriteStartElement("file");
                    imsManifestWriter.WriteAttributeString("href", question.FileName);
                    imsManifestWriter.WriteFullEndElement(); // file
                    foreach (string img in question.MmediaFiles)
                    {
                        imsManifestWriter.WriteStartElement("file");
                        imsManifestWriter.WriteAttributeString("href", "resources\\" + Path.GetFileName(img));
                        imsManifestWriter.WriteFullEndElement(); // file
                    }
                    imsManifestWriter.WriteFullEndElement(); // resource
                }
            }
            foreach (Question question in questions)
            {
                imsManifestWriter.WriteStartElement("resource");
                imsManifestWriter.WriteAttributeString("identifier", "RES-" + question.Id.ToString());
                imsManifestWriter.WriteAttributeString("type", "imsqti_item_xmlv2p1");
                imsManifestWriter.WriteAttributeString("href", question.FileName);
                imsManifestWriter.WriteStartElement("metadata");
                imsManifestWriter.WriteStartElement("imsmd:lom");
                imsManifestWriter.WriteStartElement("imsmd:general");
                imsManifestWriter.WriteElementString("imsmd:identifier", question.identifier);
                imsManifestWriter.WriteStartElement("imsmd:title");
                imsManifestWriter.WriteStartElement("imsmd:langstring");
                imsManifestWriter.WriteAttributeString("xml:lang", "ru");
                imsManifestWriter.WriteString(question.Text);
                imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                imsManifestWriter.WriteFullEndElement(); // imsmd:title
                imsManifestWriter.WriteElementString("imsmd:language", "ru");
                imsManifestWriter.WriteStartElement("imsmd:description");
                imsManifestWriter.WriteStartElement("imsmd:langstring");
                imsManifestWriter.WriteAttributeString("xml:lang", "ru");
                imsManifestWriter.WriteString(question.Description);
                imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                imsManifestWriter.WriteFullEndElement(); // imsmd:description
                imsManifestWriter.WriteFullEndElement(); // imsmd:general
                imsManifestWriter.WriteStartElement("imsmd:lifecycle");
                imsManifestWriter.WriteStartElement("imsmd:version");
                imsManifestWriter.WriteStartElement("imsmd:langstring");
                imsManifestWriter.WriteAttributeString("xml:lang", "ru");
                imsManifestWriter.WriteString("1.0");
                imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                imsManifestWriter.WriteFullEndElement(); // imsmd:version
                imsManifestWriter.WriteStartElement("imsmd:status");
                imsManifestWriter.WriteStartElement("imsmd:source");
                imsManifestWriter.WriteStartElement("imsmd:langstring");
                imsManifestWriter.WriteAttributeString("xml:lang", "x-none");
                imsManifestWriter.WriteString("LOMv1.0");
                imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                imsManifestWriter.WriteFullEndElement(); // imsmd:source
                imsManifestWriter.WriteStartElement("imsmd:value");
                imsManifestWriter.WriteStartElement("imsmd:langstring");
                imsManifestWriter.WriteAttributeString("xml:lang", "x-none");
                imsManifestWriter.WriteString("Draft");
                imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                imsManifestWriter.WriteFullEndElement(); // imsmd:value
                imsManifestWriter.WriteFullEndElement(); // imsmd:status
                imsManifestWriter.WriteFullEndElement(); // imsmd:lifecycle                      
                imsManifestWriter.WriteStartElement("imsmd:technical");
                imsManifestWriter.WriteElementString("imsmd:format", "text/x-imsqti-item-xml");
                imsManifestWriter.WriteFullEndElement(); // imsmd:technical 
                imsManifestWriter.WriteStartElement("imsmd:rights");
                imsManifestWriter.WriteStartElement("imsmd:description");
                imsManifestWriter.WriteStartElement("imsmd:langstring");
                imsManifestWriter.WriteAttributeString("xml:lang", "en");
                imsManifestWriter.WriteString("(c) Visual Editor");
                imsManifestWriter.WriteFullEndElement(); // imsmd:langstring
                imsManifestWriter.WriteFullEndElement(); // imsmd:description 
                imsManifestWriter.WriteFullEndElement(); // imsmd:rights 
                imsManifestWriter.WriteFullEndElement(); // imsmd:lom 
                imsManifestWriter.WriteStartElement("imsmd:qtiMetadata");
                imsManifestWriter.WriteElementString("imsmd:timeDependent", question.IsTimeDependent.ToString());
                imsManifestWriter.WriteElementString("imsmd:interactionType", question.InteractionType.ToString());
                if (question.IsAdaptive)
                {
                    imsManifestWriter.WriteElementString("imsmd:feedbackType", "adaptive");
                }
                else
                {
                    imsManifestWriter.WriteElementString("imsmd:feedbackType", "none");
                }
                imsManifestWriter.WriteElementString("imsmd:solutionAvailable", "true");
                imsManifestWriter.WriteFullEndElement(); // imsmd:qtiMetadata 
                imsManifestWriter.WriteFullEndElement(); // metadata
                imsManifestWriter.WriteStartElement("file");
                imsManifestWriter.WriteAttributeString("href", question.FileName);
                imsManifestWriter.WriteFullEndElement(); // file                
                foreach (string img in question.MmediaFiles)
                {
                    imsManifestWriter.WriteStartElement("file");
                    imsManifestWriter.WriteAttributeString("href", "resources\\" + Path.GetFileName(img));
                    imsManifestWriter.WriteFullEndElement(); // file
                }
                imsManifestWriter.WriteFullEndElement(); // resource
            }
            //~~~файлы вопросов
            imsManifestWriter.WriteFullEndElement(); // resources
            imsManifestWriter.WriteFullEndElement(); // manifest
            imsManifestWriter.WriteEndDocument();
            imsManifestWriter.Close();
        }
        #endregion

    }
}
