using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;
using VisualEditor.Logic.Commands;
using VisualEditor.Logic.Controls.Docking;
using VisualEditor.Logic.Controls.Ribbon;
using VisualEditor.Logic.Controls.Trees;
using VisualEditor.Logic.Course.Items;
using VisualEditor.Logic.Helpers.AppSettings;
using VisualEditor.Utils.ExceptionHandling;

namespace VisualEditor.Logic.Warehouse
{
    internal class Warehouse
    {
        private static Warehouse instance;
        private readonly Timer autosavingTimer;
        public static readonly string RelativeImagesDirectory = "Images";
        public static readonly string RelativeFlashesDirectory = "Flashes";
        public static readonly string RelativeAudiosDirectory = "Audios";
        public static readonly string RelativeVideosDirectory = "Videos";

        public static readonly string FlashPlayerRelativeDirectory = "FlashPlayer";
        public static readonly string FlashPlayerRelativePath = "FlashPlayer\\uppod.swf";
        public static readonly string FlashPlayerRelativeAudioSettingsPath = "FlashPlayer\\audio.txt";
        public static readonly string FlashPlayerRelativeVideoSettingsPath = "FlashPlayer\\video.txt";
        public static readonly string InteractiveFlashRelativePath = "FlashPlayer\\picregion.swf";

        private Warehouse()
        {
            Bookmarks = new List<Bookmark>();
            TrainingModules = new List<TrainingModule>();
            LinksToObjects = new List<LinkToObject>();
            RecentProjects = new List<string>();
            IsProjectSavedToFile = false;

            autosavingTimer = new Timer();
            int autosavingInterval = Convert.ToInt32(AppSettingsHelper.GetAutosavingInterval())*60000;
            if (autosavingInterval != 0)
            {
                autosavingTimer.Interval = autosavingInterval;
                autosavingTimer.Tick += autosavingTimer_Tick;
                autosavingTimer.Enabled = (AppSettingsHelper.GetAutosavingInterval() != 0);
            }
        }

        #region Свойства
        
        public static Warehouse Instance
        {
            get { return instance ?? (instance = new Warehouse()); }
        }

        public CourseTree CourseTree
        {
            get { return DockContainer.Instance.CourseWindow.CourseTree; }
        }

        public ConceptTree ConceptTree
        {
            get { return DockContainer.Instance.ConceptWindow.ConceptTree; }
        }

        public WarningTree WarningTree
        {
            get { return DockContainer.Instance.WarningWindow.WarningTree; }
        }

        public List<Bookmark> Bookmarks { get; private set; }
        public List<TrainingModule> TrainingModules { get; private set; }
        public List<LinkToObject> LinksToObjects { get; private set; }
        public List<string> RecentProjects { get; private set; }

        public static bool IsProjectBeingDesigned
        {
            get { return Instance.CourseTree.Nodes.Count != 0; }
        }

        public static bool IsProjectSavedToFile { get; set; }
        public static bool IsHtmlSourceMode { get; set; }
        public static bool IsProjectModified { get; set; }

        public List<Concept> InternalConcepts
        {
            get
            {
                return ConceptTree.Nodes.Cast<Concept>().Where(concept => concept.Type == Enums.ConceptType.Internal).ToList();
            }
        }

        public List<Concept> ExternalConcepts
        {
            get
            {
                return ConceptTree.Nodes.Cast<Concept>().Where(concept => concept.Type == Enums.ConceptType.External).ToList();
            }
        }

        // Каталог, в котором размещается редактируемый проект.
        // C:\Documents and Settings\dissonant\Application Data\VisualEditor\0b48c88f-a0ba-4415-9feb-086284c8e31b\
        public static string ProjectEditorLocation { get; private set; }
        public static string AbsoluteEditorImagesDirectory { get; private set; }
        public static string AbsoluteEditorFlashesDirectory { get; private set; }
        public static string AbsoluteEditorAudiosDirectory { get; private set; }
        public static string AbsoluteEditorVideosDirectory { get; private set; }

        // Истинное расположение проекта.
        public static string ProjectTrueLocation { get; set; }
        public static string ProjectFileName { get; set; }
        public static string ProjectFileType { get; set; }
        public static string ProjectArchiveName { get; set; }

        // Для загрузки контролей/вопросов из внешних учебных курсов.
        public static string OuterProjectTrueLocation { get; set; }
        public static string OuterProjectFileName { get; set; }
        public static string OuterProjectEditorLocation { get; private set; }
        public static string OuterAbsoluteEditorImagesDirectory { get; private set; }
        public static string OuterAbsoluteEditorFlashesDirectory { get; private set; }
        public static string OuterProjectArchiveName { get; set; }

        #endregion

        #region GetBookmarkNameById

        public static string GetBookmarkNameById(Guid id)
        {
            foreach (var bookmark in Instance.Bookmarks)
            {
                if (bookmark.Id.Equals(id))
                {
                    return bookmark.Text;
                }
            }

            return string.Empty;
        }

        #endregion

        #region CreateDirectories

        public static void CreateDirectories()
        {
            var appDataDirectory = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            ProjectEditorLocation = Path.Combine(appDataDirectory, Application.ProductName);
            ProjectEditorLocation = Path.Combine(ProjectEditorLocation, Guid.NewGuid().ToString());
            try
            {
                Directory.CreateDirectory(ProjectEditorLocation);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }

            AbsoluteEditorImagesDirectory = Path.Combine(ProjectEditorLocation, RelativeImagesDirectory);
            try
            {
                Directory.CreateDirectory(AbsoluteEditorImagesDirectory);
                Properties.Resources.Pic.Save(Path.Combine(AbsoluteEditorImagesDirectory, "Pic.png"));
                Properties.Resources.Anim.Save(Path.Combine(AbsoluteEditorImagesDirectory, "Anim.png"));
                Properties.Resources.Aud.Save(Path.Combine(AbsoluteEditorImagesDirectory, "Aud.png"));
                Properties.Resources.Vid.Save(Path.Combine(AbsoluteEditorImagesDirectory, "Vid.png"));
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }

            AbsoluteEditorFlashesDirectory = Path.Combine(ProjectEditorLocation, RelativeFlashesDirectory);
            try
            {
                Directory.CreateDirectory(AbsoluteEditorFlashesDirectory);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }

            AbsoluteEditorAudiosDirectory = Path.Combine(ProjectEditorLocation, RelativeAudiosDirectory);
            try
            {
                Directory.CreateDirectory(AbsoluteEditorAudiosDirectory);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }

            AbsoluteEditorVideosDirectory = Path.Combine(ProjectEditorLocation, RelativeVideosDirectory);
            try
            {
                Directory.CreateDirectory(AbsoluteEditorVideosDirectory);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }
        }

        // Для загрузки контролей/вопросов из внешних учебных курсов.
        public static void CreateOuterDirectories()
        {
            var d = Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData);
            OuterProjectEditorLocation = Path.Combine(d, Application.ProductName);
            OuterProjectEditorLocation = Path.Combine(OuterProjectEditorLocation, Guid.NewGuid().ToString());
            try
            {
                Directory.CreateDirectory(OuterProjectEditorLocation);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }
            OuterAbsoluteEditorImagesDirectory = Path.Combine(OuterProjectEditorLocation, RelativeImagesDirectory);
            try
            {
                Directory.CreateDirectory(OuterAbsoluteEditorImagesDirectory);
                Properties.Resources.Pic.Save(Path.Combine(OuterAbsoluteEditorImagesDirectory, "Pic.png"));
                Properties.Resources.Anim.Save(Path.Combine(OuterAbsoluteEditorImagesDirectory, "Anim.png"));                
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }
            OuterAbsoluteEditorFlashesDirectory = Path.Combine(OuterProjectEditorLocation, RelativeFlashesDirectory);
            try
            {
                Directory.CreateDirectory(OuterAbsoluteEditorFlashesDirectory);
            }
            catch (Exception exception)
            {
                ExceptionManager.Instance.LogException(exception);
                throw;
            }
        }

        #endregion

        #region DeleteDirectories

        public static void DeleteDirectories()
        {
            if (!Directory.Exists(ProjectEditorLocation))
            {
                return;
            }

            // Когда пользователь использует редактор формул, процесс, создающий изображение
            // не сразу освобождает файл-изображение формулы. Поэтому если в этот момент удалить 
            // каталог проекта с этим файлом, то возникнет исключение.
            // Следующая логика проверяет, прошло ли удаление корректно. Иногда это занимает 
            // продолжительное время.
            var isDirectoryDeleted = false;
            while (!isDirectoryDeleted)
            {
                Application.DoEvents();

                try
                {
                    Directory.Delete(ProjectEditorLocation, true);
                    isDirectoryDeleted = true;
                }
                catch (Exception exception)
                {

                }
            }

            ProjectEditorLocation = AbsoluteEditorImagesDirectory = 
                AbsoluteEditorFlashesDirectory = string.Empty;
            ProjectTrueLocation = ProjectFileName = string.Empty;
        }

        public static void DeleteOuterDirectories()
        {
            if (!Directory.Exists(OuterProjectEditorLocation))
            {
                return;
            }

            // Когда пользователь использует редактор формул, процесс, создающий изображение
            // не сразу освобождает файл-изображение формулы. Поэтому если в этот момент удалить 
            // каталог проекта с этим файлом, то возникнет исключение.
            // Следующая логика проверяет, прошло ли удаление корректно. Иногда это занимает 
            // продолжительное время.
            var isDeleted = false;
            while (!isDeleted)
            {
                Application.DoEvents();

                try
                {
                    Directory.Delete(OuterProjectEditorLocation, true);
                    isDeleted = true;
                }
                catch (Exception)
                {

                }
            }

            OuterProjectEditorLocation = OuterAbsoluteEditorImagesDirectory = 
                OuterAbsoluteEditorFlashesDirectory = string.Empty;
            OuterProjectTrueLocation = OuterProjectFileName = string.Empty;
        }

        #endregion

        #region GetTrainingModuleIdByObjectId

        public Guid GetTrainingModuleIdByObjectId(Guid objectId)
        {
            foreach (var b in Bookmarks)
            {
                if (b.Id.Equals(objectId))
                {
                    return b.ModuleId;
                }
            }

            foreach (var ic in InternalConcepts)
            {
                if (ic.Id.Equals(objectId))
                {
                    return ic.ModuleId;
                }
            }

            foreach (var ec in ExternalConcepts)
            {
                if (ec.Id.Equals(objectId))
                {
                    return Guid.Empty;
                }
            }

            foreach (var tm in TrainingModules)
            {
                if (tm.Id.Equals(objectId))
                {
                    return tm.Id;
                }
            }

            return Guid.Empty;
        }

        #endregion

        #region WalkTree

        private static void WalkTree(TreeNodeCollection tnc, Guid id)
        {
            for (var i = 0; i < tnc.Count; i++)
            {
                var tn = tnc[i];
                if (tn.Nodes.Count != 0)
                {
                    WalkTree(tnc[i].Nodes, id);
                }

                if (tn is TrainingModule)
                {
                    if ((tn as TrainingModule).Id.Equals(id))
                    {
                        if (trainingModule == null)
                        {
                            trainingModule = tn as TrainingModule;

                            return;
                        }
                    }
                }

                if (tn is Question)
                {
                    if ((tn as Question).Id.Equals(id))
                    {
                        if (question == null)
                        {
                            question = tn as Question;

                            return;
                        }
                    }
                }
            }
        }

        #endregion

        #region GetQuestionById

        private static Question question;

        public static Question GetQuestionById(Guid id)
        {
            question = null;

            WalkTree(Instance.CourseTree.Nodes, id);

            return question;
        }

        #endregion

        #region GetTrainingModuleById

        private static TrainingModule trainingModule;

        public static TrainingModule GetTrainingModuleById(Guid id)
        {
            trainingModule = null;

            WalkTree(Instance.CourseTree.Nodes, id);

            return trainingModule;
        }

        #endregion

        #region GetLinkObjectIdByText

        public static string GetLinkObjectIdByText(Enums.LinkTarget linkTarget, string linkObjectText)
        {
            if (linkTarget.Equals(Enums.LinkTarget.Bookmark))
            {
                var bs = Instance.Bookmarks;
                foreach (var b in bs)
                {
                    if (b.Text.Equals(linkObjectText))
                    {
                        return b.Id.ToString();
                    }
                }
            }

            if (linkTarget.Equals(Enums.LinkTarget.InternalConcept))
            {
                var ics = Instance.InternalConcepts;
                foreach (var c in ics)
                {
                    if (c.Text.Equals(linkObjectText))
                    {
                        return c.Id.ToString();
                    }
                }
            }

            if (linkTarget.Equals(Enums.LinkTarget.ExternalConcept))
            {
                var ecs = Instance.ExternalConcepts;
                foreach (var c in ecs)
                {
                    if (c.Text.Equals(linkObjectText))
                    {
                        return c.Id.ToString();
                    }
                }
            }

            if (linkTarget.Equals(Enums.LinkTarget.TrainingModule))
            {
                var tms = Instance.TrainingModules;
                foreach (var tm in tms)
                {
                    if (tm.Text.Equals(linkObjectText))
                    {
                        return tm.Id.ToString();
                    }
                }
            }

            return string.Empty;
        }

        #endregion

        #region InvalidateRecentProjects

        public static void InvalidateRecentProjects()
        {
            RibbonHelper.ClearOrbRecentButtons(MainRibbon.Instance);

            foreach (var rp in Instance.RecentProjects)
            {
                RibbonHelper.AddOrbRecentButton(MainRibbon.Instance, rp);
            }
        }

        #endregion        

        #region Автосохранение

        private static void autosavingTimer_Tick(object sender, EventArgs e)
        {
            if (!IsProjectBeingDesigned)
            {
                return;
            }

            if (!IsProjectSavedToFile)
            {
                return;
            }

            if (ProjectFileType.Equals(".htp"))
            {
                CommandManager.Instance.GetCommand(CommandNames.SaveToHtp).Execute(null);
            }
        }

        #endregion
    }
}