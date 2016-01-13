using System;
using System.Collections.Generic;
using System.Xml;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Course.Items.Questions
{
    internal class ServiceInfo
    {
        /// <summary>
        /// Список тем.
        /// </summary>
        public List<Task> Tasks { get; private set; }

        /// <summary>
        /// Список задач.
        /// </summary>
        public List<Problem> Problems { get; private set; }

        /// <summary>
        /// Список дисциплин.
        /// </summary>
        public List<Externaltest> Externaltests { get; private set; }

        /// <summary>
        /// Номер дисциплины.
        /// </summary>
        public int externaltest_number = 0;

        /// <summary>
        /// Номер темы.
        /// </summary>
        public int task_number = 0;


        /// <summary>
        /// Адрес сервиса.
        /// </summary> 
        public string Url { get; private set; }

        #region Конструктор

        public ServiceInfo(string path)
        {
            Tasks = new List<Task>();
            Problems = new List<Problem>();
            Externaltests = new List<Externaltest>();
            var wsdlFile = new XmlDocument();

            try
            {
                wsdlFile.Load(path);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }

            foreach (XmlElement hypertestexternaltestChild in wsdlFile.DocumentElement.ChildNodes)
            {
                // Если потомок root есть WSDL.
                if (hypertestexternaltestChild.Name.Equals("externaltest"))
                {
                    Url = hypertestexternaltestChild.GetAttribute("wsdl");
                    externaltest_number++;
                    var externaltest = new Externaltest
                    {
                        Login = hypertestexternaltestChild.GetAttribute("login"),
                        Password = hypertestexternaltestChild.GetAttribute("password"),
                        Name = hypertestexternaltestChild.GetAttribute("name"),
                        Webpage = hypertestexternaltestChild.GetAttribute("webpage"),
                        Externaltest_number = externaltest_number.ToString()
                    };
                    Externaltests.Add(externaltest);
                }

                foreach (XmlElement externaltestChild in hypertestexternaltestChild.ChildNodes)
                {
                    if (externaltestChild.Name.Equals("test"))
                    {
                        {
                            if (externaltestChild.Name.Equals("test"))
                            {
                                task_number++;
                                // Cоздает новый класс с полями.
                                var test = new Task
                                {
                                    Id = externaltestChild.GetAttribute("id"),
                                    Name = externaltestChild.GetAttribute("name"),
                                    // номер дисциплины, к которой относится тема
                                    Externaltest_number = externaltest_number.ToString(),
                                    Task_number = task_number.ToString()
                                };

                                foreach (XmlElement testChild in externaltestChild.ChildNodes)
                                {
                                    if (testChild.Name.Equals("problem"))
                                    {
                                        //Создает новый класс с полями
                                        var problem = new Problem
                                        {
                                            Id = testChild.GetAttribute("id"),
                                            Name = testChild.GetAttribute("name"),
                                            Weight = testChild.GetAttribute("weight"),
                                            // номер темы, к которой относится задача
                                            Task_number = task_number.ToString(),
                                            Url = hypertestexternaltestChild.GetAttribute("wsdl")
                                        };
                                        for (var i = 0; i < testChild.ChildNodes.Count; i++)
                                        {
                                            if (testChild.Name.Equals("problem"))
                                            {
                                                problem.Declaration = testChild.ChildNodes[i].InnerText;

                                            }
                                        }
                                        Problems.Add(problem);
                                    }
                                }

                                Tasks.Add(test);
                            }
                        }
                    }

                }
            }

        }

        #endregion

        #region GetQuestions

        /// <summary>
        /// Возвразщает список вопросов по заданным предмету и теме.
        /// </summary>
        /// <param name="subjectName">Название предмета.</param>
        /// <param name="authorName">Тема.</param>
        public List<string> GetQuestions(string subjectName, string authorName)
        {
            var problems = new List<string>();
            if (!subjectName.Equals(string.Empty) && !authorName.Equals(string.Empty))
            {
                for (var j = 0; j < externaltest_number; j++)
                    for (var i = 0; i < task_number; i++)
                    {
                        if (Tasks[i].Externaltest_number == Externaltests[j].Externaltest_number && Tasks[i].Name.Equals(authorName) && Externaltests[j].Name.Equals(subjectName))
                        {
                            for (var k = 0; k < Problems.Count; k++)
                            {
                                if (Tasks[i].Task_number == Problems[k].Task_number)
                                    problems.Add(Problems[k].Name);
                            }
                        }

                    }
            }
            if (subjectName.Equals(string.Empty) && authorName.Equals(string.Empty))
            {
                for (var l = 0; l < Problems.Count; l++)
                {
                    problems.Add(Problems[l].Name);
                }
            }


            if (!subjectName.Equals(string.Empty) && authorName.Equals(string.Empty))
            {
                int t = 0;
                for (var j = 0; j < externaltest_number; j++)
                    for (var i = 0; i < task_number; i++)
                    {
                        if (Tasks[i].Externaltest_number == Externaltests[j].Externaltest_number && Externaltests[j].Name.Equals(subjectName))
                        {
                            for (var k = 0; k < Problems.Count; k++)
                            {
                                if (Tasks[i].Task_number == Problems[k].Task_number)
                                    problems.Add(Problems[k].Name);
                            }
                        }

                    }
            }


            if (subjectName.Equals(string.Empty) && !authorName.Equals(string.Empty))
            {
                for (var j = 0; j < externaltest_number; j++)
                    for (var i = 0; i < task_number; i++)
                    {
                        if (Tasks[i].Externaltest_number == Externaltests[j].Externaltest_number && Tasks[i].Name.Equals(authorName))
                        {
                            for (var k = 0; k < Problems.Count; k++)
                            {
                                if (Tasks[i].Task_number == Problems[k].Task_number)
                                    problems.Add(Problems[k].Name);
                            }
                        }

                    }
            }



            /*

                        for (var i = 0; i < Problems.Count; i++)
                        {
               

                            if (!subjectName.Equals(string.Empty) && !authorName.Equals(string.Empty))
                            {
                                if (Tasks[i].Name.Equals(authorName) /*&& Externaltests[i].Name.Equals(subjectName))
                                {

                                    problems.Add(Problems[i].Name);
                                }
                            }
                            else if (subjectName.Equals(string.Empty) && !authorName.Equals(string.Empty))
                            {
                                if (Externaltests[i].Name.Equals(authorName))
                                {
                                    problems.Add(Problems[i].Name);
                                }
                            }
                            else if (!subjectName.Equals(string.Empty) && authorName.Equals(string.Empty))
                            {
                                if (Externaltests[i].Name.Equals(subjectName))
                                {
                                    problems.Add(Problems[i].Name);
                                }
                            }



                            else if (subjectName.Equals(string.Empty) && authorName.Equals(string.Empty))
                            {
                                //if (Externaltests[i].Name.Equals(authorName))
                                {
                                    problems.Add(Problems[i].Name);
                                }
                            }


                        }
            */
            return problems;
        }

        #endregion

        #region GetSubjects

        /// <summary>
        /// Возвращает дисциплины,у которых существует такая тема. 
        /// </summary> 
        /// <param name="author">Тема, по которой нужно найти дисциплины.</param>
        public List<string> GetSubjects(string author)
        {
            var subjects = new List<string>();

            for (int i = 0; i < Externaltests.Count; i++)
            {
                if (author.Equals(string.Empty))
                {
                    if (!subjects.Contains(Externaltests[i].Name))//если нет этой дисцплине в списке
                    {
                        subjects.Add(Externaltests[i].Name);
                    }
                }
                else
                {
                    for (var j = 0; j < task_number; j++)
                        if (Tasks[j].Name.Equals(author) && !subjects.Contains(Externaltests[i].Name) && Externaltests[i].Externaltest_number.Equals(Tasks[j].Externaltest_number))
                        {
                            subjects.Add(Externaltests[i].Name);
                        }
                }
            }
            return subjects;
        }

        #endregion

        #region GetAuthors

        /// <summary>
        /// Возвращает темы, которые существует в данной дисциплине.
        /// </summary>
        /// <param name="subject">Дисциплина, по которой надо искать темы.</param>
        public List<string> GetAuthors(string subject)
        {
            var authors = new List<string>(0);

            for (var i = 0; i < Tasks.Count; i++)
            {
                if (subject.Equals(string.Empty))// если предмет не выбран
                {
                    if (!authors.Contains(Tasks[i].Name)) //если нет этой темы в списке
                    {
                        authors.Add(Tasks[i].Name);
                    }
                }
                else
                {
                    for (var j = 0; j < externaltest_number; j++)
                    {
                        if (Externaltests[j].Name.Equals(subject) && Tasks[i].Externaltest_number.Equals(Externaltests[j].Externaltest_number) && !authors.Contains(Tasks[i].Name))
                        {
                            authors.Add(Tasks[i].Name);
                        }
                    }
                }
            }

            return authors;
        }

        #endregion
    }
}