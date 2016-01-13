using System;
using System.Xml;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.IO.Questions;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.IO
{
    internal class ResponseXmlReader
    {
        private Response response;

        public ResponseXmlReader(Response response)
        {
            this.response = response;
        }

        public void ReadXml(XmlTextReader xmlReader)
        {
            try
            {
                bool isEndCycle = false;

                while (xmlReader.Read() && !isEndCycle)
                {
                    if (xmlReader.NodeType == XmlNodeType.Element)
                    {
                        if (xmlReader.Name.Equals("html_text"))
                        {
                            response.DocumentHtml = QuestionXmlReader.XmlToHtml(xmlReader.ReadElementString());
                        }
                    }
                    else if (xmlReader.NodeType == XmlNodeType.EndElement)
                    {
                        if (xmlReader.Name.ToLower().Equals("answer"))
                        {
                            isEndCycle = true;
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                ExceptionManager.Instance.LogException(ex);
            }
        }
    }
}