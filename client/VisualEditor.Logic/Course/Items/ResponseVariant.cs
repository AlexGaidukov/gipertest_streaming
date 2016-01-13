using System.Collections;
using VisualEditor.Logic.IO;

namespace VisualEditor.Logic.Course.Items
{
    internal class ResponseVariant
    {
        private readonly Question question;

        public ResponseVariant(Question question)
        {
            this.question = question;
            Responses = new ArrayList();

            XmlReader = new ResponseVariantXmlReader(question, this);
        }

        public ArrayList Responses;
        public string NextQuestion { get; set; }
        public double Weight { get; set; }
        public string Hint { get; set; }

        public ResponseVariantXmlReader XmlReader;

        public static ResponseVariant Clone(ResponseVariant responseVariant)
        {
            var newResponseVariant = new ResponseVariant(responseVariant.question)
            {
                Responses = (ArrayList)responseVariant.Responses.Clone(),
                Weight = responseVariant.Weight,
                Hint = responseVariant.Hint,
                NextQuestion = responseVariant.NextQuestion
            };

            return newResponseVariant;
        }
    }
}