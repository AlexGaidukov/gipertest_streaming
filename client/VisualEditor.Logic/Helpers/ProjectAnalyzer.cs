using System.Collections.Generic;
using System.Windows.Forms;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Course.Items.Questions;

namespace VisualEditor.Logic.Helpers
{
    internal class ProjectAnalyzer
    {
        private const string missedProfileMessage = " - не назначен оцениваемый элемент профиля";
        private const string zeroMarksMessage = " - не назначено количество баллов";
        private const string emptyGroupMessage = " - в группе отсутствуют вопросы";
        private const string zeroChosenQuestionsCountMessage = " - не указано количество выбираемых из группы вопросов";
        private const string noResponsesMessage = " - у вопроса отсутствуют ответы";
        private const string noResponseVariantsMessage = " - у вопроса отсутствуют варанты ответа";
        private const string emptyTestModuleMessage = " - в контроле отсутствуют группы/вопросы";

        private static ProjectAnalyzer instance;
        private static List<WarningNode> warningNodes;

        private ProjectAnalyzer()
        {
            warningNodes = new List<WarningNode>();
        }

        public static ProjectAnalyzer Instance
        {
            get { return instance ?? (instance = new ProjectAnalyzer()); }
        }

        public List<WarningNode> Analyze()
        {
            warningNodes.Clear();

            if (Warehouse.Warehouse.IsProjectBeingDesigned)
            {
                WalkTree(Warehouse.Warehouse.Instance.CourseTree.Nodes);
            }

            return warningNodes;
        }

        public void FillWarningTree(List<WarningNode> warningItems)
        {
            var warningsTree = Warehouse.Warehouse.Instance.WarningTree;
            warningsTree.Nodes.Clear();
            warningsTree.Nodes.AddRange(warningItems.ToArray());
        }

        private static void WalkTree(TreeNodeCollection tnc)
        {
            foreach (TreeNode tn in tnc)
            {
                if (tn.Nodes.Count != 0)
                {
                    WalkTree(tn.Nodes);
                }

                if (tn is TestModule)
                {
                    var tm = tn as TestModule;

                    if (tm.Groups.Count == 0 &&
                        tm.Questions.Count == 0)
                    {
                        //var warningNode = new WarningNode(Enums.WarningType.EmptyTestModule);
                        //warningNode.Text = string.Concat(tm.Text, emptyTestModuleMessage);
                        //warningNode.WarningTestModule = tm;
                        //warningNodes.Add(warningNode);
                    }
                }

                if (tn is Group)
                {
                    var g = tn as Group;

                    // Checks for empty group.
                    if (g.Questions.Count == 0)
                    {
                        var warningNode = new WarningNode(Enums.WarningType.EmptyGroup);
                        warningNode.Text = string.Concat(g.Text, emptyGroupMessage);
                        warningNode.WarningGroup = g;
                        warningNodes.Add(warningNode);
                    }

                    // Checks for zero chosen questions count.
                    if (g.ChosenQuestionsCount == 0)
                    {
                        var warningNode = new WarningNode(Enums.WarningType.ZeroChosenQuestionsCount);
                        warningNode.Text = string.Concat(g.Text, zeroChosenQuestionsCountMessage);
                        warningNode.WarningGroup = g;
                        warningNodes.Add(warningNode);
                    }

                    // Checks if profile was not set.
                    if (g.Profile == null)
                    {
                        var warningNode = new WarningNode(Enums.WarningType.MissedProfile);
                        warningNode.Text = string.Concat(g.Text, missedProfileMessage);
                        warningNode.WarningGroup = g;
                        warningNodes.Add(warningNode);
                    }

                    // Checks for zero marks.
                    if (g.Marks == 0)
                    {
                        var warningNode = new WarningNode(Enums.WarningType.ZeroMarks);
                        warningNode.Text = string.Concat(g.Text, zeroMarksMessage);
                        warningNode.WarningGroup = g;
                        warningNodes.Add(warningNode);
                    }
                }

                if (tn is Question)
                {
                    var q = tn as Question;

                    // Checks the number of responses.
                    if (q.Responses.Count == 0 &&
                        !(q is OpenQuestion) &&
                        !(q is OuterQuestion))
                    {
                        var warningNode = new WarningNode(Enums.WarningType.NoResponses);
                        warningNode.Text = string.Concat(q.Text, noResponsesMessage);
                        warningNode.WarningQuestion = q;
                        warningNodes.Add(warningNode);
                    }

                    if (!(tn.Parent is Group))
                    {
                        // Checks if profile was not set.
                        if (q.Profile == null)
                        {
                            var warningNode = new WarningNode(Enums.WarningType.MissedProfile);
                            warningNode.Text = string.Concat(q.Text, missedProfileMessage);
                            warningNode.WarningQuestion = q;
                            warningNodes.Add(warningNode);
                        }

                        // Checks for zero marks.
                        if (q.Marks == 0)
                        {
                            var warningNode = new WarningNode(Enums.WarningType.ZeroMarks);
                            warningNode.Text = string.Concat(q.Text, zeroMarksMessage);
                            warningNode.WarningQuestion = q;
                            warningNodes.Add(warningNode);
                        }
                    }

                    // Checks the number of response variants.
                    if (q.ResponseVariants.Count == 0 &&
                        !(q is OpenQuestion) &&
                        !(q is OuterQuestion))
                    {
                        var warningNode = new WarningNode(Enums.WarningType.NoResponseVariants);
                        warningNode.Text = string.Concat(q.Text, noResponseVariantsMessage);
                        warningNode.WarningQuestion = q;
                        warningNodes.Add(warningNode);
                    }
                }
            }
        }
    }
}