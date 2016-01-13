using System;
using VisualEditor.Logic.Controls.Docking.Documents;
using VisualEditor.Logic.IO;
using System.Text.RegularExpressions;
using System.Xml;

namespace VisualEditor.Logic.Course.Items
{
    internal class Response : CourseItem
    {
        public Response()
        {
            ImageIndex = SelectedImageIndex = 9;

            XmlReader = new ResponseXmlReader(this);
        }

        public Guid Id { get; set; }
        public string NativeId { get; set; }

        public ResponseDocument ResponseDocument { get; set; }
        public string DocumentHtml { get; set; }
        public string PreviewHtml { get; set; }
        protected bool isFixed;
        public ResponseXmlReader XmlReader;
        protected Enums.ResponseType type;
        /// <summary>
        /// Тип элемента ответа.
        /// </summary>
        public Enums.ResponseType Type
        {
            get { return type; }
        }

        /// <summary>
        /// Фиксирован ли элемент ответа. Используется для IMS QTI.
        /// </summary>
        public bool IsFixed
        {
            get { return isFixed; }
            set { isFixed = value; }
        }

        protected string identifier;
        /// <summary>
        /// Идентификатор элемента ответа.
        /// </summary>
        public string Identifier
        {
            get { return identifier; }
            set { identifier = value; }
        }

        protected double mappedValue;
        /// <summary>
        /// Доля правильности элемента ответа.
        /// </summary>
        public double MappedValue
        {
            get { return mappedValue; }
            set { mappedValue = value; }
        }

        public static Response Clone(Response response)
        {
            var newResponse = new Response
            {
                Text = response.Text,
                Id = Guid.NewGuid(),
                DocumentHtml = string.Copy(response.DocumentHtml)
            };

            return newResponse;
        }



    }
}