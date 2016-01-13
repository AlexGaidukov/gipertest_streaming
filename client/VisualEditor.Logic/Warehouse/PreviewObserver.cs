using System;
using System.Collections.Generic;
using VisualEditor.Logic.Controls.Docking.Documents;

namespace VisualEditor.Logic.Warehouse
{
    internal class PreviewObserver
    {
        private static PreviewObserver instance;
        private static List<DocumentBase> documents;
        private static int currentDocumentIndex;

        public PreviewObserver Instance
        {
            get { return instance ?? (instance = new PreviewObserver()); }
        }

        public static void AddDocument(DocumentBase document)
        {
            if (documents == null)
            {
                documents = new List<DocumentBase>();
                currentDocumentIndex = -1;       
            }

            if (currentDocumentIndex != documents.Count - 1)
            {
                documents.RemoveRange(currentDocumentIndex + 1, documents.Count - currentDocumentIndex - 1);
            }

            currentDocumentIndex++;
            documents.Add(document);
        }

        public static void ClearDocuments()
        {
            if (documents != null)
            {
                documents.Clear();
            }
            currentDocumentIndex = -1;
        }

        public static bool CanNavigateBackward()
        {
            if (currentDocumentIndex == -1)
            {
                return false;
            }

            return currentDocumentIndex != 0;
        }

        public static bool CanNavigateForward()
        {
            if (currentDocumentIndex == -1)
            {
                return false;
            }

            return currentDocumentIndex != documents.Count - 1;
        }

        public static DocumentBase PreviousDocument()
        {
            if (!CanNavigateBackward())
            {
                throw new InvalidOperationException();
            }

            currentDocumentIndex--;

            return documents[currentDocumentIndex];
        }

        public static DocumentBase NextDocument()
        {
            if (!CanNavigateForward())
            {
                throw new InvalidOperationException();
            }

            currentDocumentIndex++;

            return documents[currentDocumentIndex];
        }
    }
}