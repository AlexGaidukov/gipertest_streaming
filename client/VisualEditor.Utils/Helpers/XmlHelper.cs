using System;
using System.IO;
using System.Xml;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Utils.Helpers
{
    public class XmlHelper
    {
        private readonly XmlDocument document;

        public XmlHelper()
        {
            document = new XmlDocument();
        }

        public XmlDocument Document
        {
            get { return document; }
        }

        public void AppendNode(string parentNodeName, string childNodeName)
        {
            if (parentNodeName.IsNull())
            {
                throw new ArgumentNullException();
            }

            if (string.IsNullOrEmpty(childNodeName))
            {
                throw new ArgumentNullException();
            }

            // Добавляет корневой узел.
            if (parentNodeName.Equals(string.Empty))
            {
                document.AppendChild(document.CreateElement(childNodeName));
                return;
            }

            // Добавляет узел.
            var nodeList = document.GetElementsByTagName(parentNodeName);
            var parentNode = nodeList[0];
            parentNode.AppendChild(document.CreateElement(childNodeName));
        }

        public void RemoveNode(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName))
            {
                throw new ArgumentNullException();
            }

            var nodeList = document.GetElementsByTagName(nodeName);
            var parentNode = nodeList[0].ParentNode;
            parentNode.RemoveChild(nodeList[0]);
        }

        public void SetNodeValue(string nodeName, string value)
        {
            if (nodeName.IsNull() ||
                value.IsNull())
            {
                throw new ArgumentNullException();
            }

            var nodeList = document.GetElementsByTagName(nodeName);
            var parentNode = nodeList[0];

            // В случае первого обращения к узлу добавляет TextNode
            // и устанавливает его текст.
            if (parentNode.FirstChild == null)
            {
                parentNode.AppendChild(document.CreateTextNode(value));
            }
            else
            {
                (parentNode.FirstChild as XmlText).Data = value;
            }
        }

        public string GetNodeValue(string nodeName)
        {
            if (string.IsNullOrEmpty(nodeName))
            {
                throw new ArgumentNullException();
            }

            var nodeList = document.GetElementsByTagName(nodeName);
            var node = nodeList[0];

            if (node == null || node.FirstChild == null)
            {
                return string.Empty;
            }

            return (node.FirstChild as XmlText).Data;
        }

        public void Load(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }

            try
            {
                document.Load(path);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }
        }

        public void Save(string path)
        {
            if (string.IsNullOrEmpty(path))
            {
                throw new ArgumentNullException();
            }

            try
            {
                File.Delete(path);
                document.Save(path);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }
        }
    }
}