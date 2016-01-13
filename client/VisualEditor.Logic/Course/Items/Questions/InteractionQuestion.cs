using VisualEditor.Logic.IO.Questions;
using System.Xml;
using System.IO;
using System;
using System.Text.RegularExpressions;
using System.Windows.Forms;
using System.Globalization;
using System.Collections;

namespace VisualEditor.Logic.Course.Items.Questions
{
    class InteractiveQuestion: Question
    {
        public InteractiveQuestion()
        {
            XmlWriter = new MultichoiceQuestionXmlWriter(this);/////////
            XmlReader = new MultichoiceQuestionXmlReader(this);/////////

            type = Enums.QuestionType.Interactive;
            interactionType = Enums.InteractionType.choiceInteraction;
            identifier = identifier + type.ToString();
            cardinality = "interactive";
            baseType = "identifier";
            maxChoices = 0;
            fileName = "resources\\" + identifier + ".xml";
        }

        public override void WriteQti(string fileName)
        { }

        public override bool ReadQti(string qfPath)
        { return true; }
    }
}
