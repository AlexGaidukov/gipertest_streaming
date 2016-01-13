namespace VisualEditor.Logic
{
    internal static class Enums
    {
        public enum ConceptType
        {
            Internal,
            External
        }

        public enum TestType
        {
            InTest,
            OutTest
        }

        /// <summary>
        /// Последовательность вопросов:
        /// естественная,
        /// случайная,
        /// сетевая
        /// </summary>
        public enum QuestionSequence
        {
            Natural,
            Random,
            Network
        }

        public enum WarningType
        {
            MissedProfile,
            ZeroMarks,
            EmptyGroup,
            ZeroChosenQuestionsCount,
            NoResponses,
            NoResponseVariants,
            EmptyTestModule
        }

        public enum LinkTarget
        {
            Bookmark,
            InternalConcept,
            ExternalConcept,
            TrainingModule,
            Hyperlink
        }

        public enum RenderingStyle
        {
            EmptyEnvironment,
            NoActiveDocument,
            TrainingModuleDocument,
            QuestionDocument,
            ResponseDocument,
            MultimediaDocument,
            HintDialog
        }

        /// <summary>
        /// для IMS QTI
        /// режим навигации
        /// пользователь может отвечать на вопросы только последовательно
        /// пользователь может отвечать на вопросы в любом порядке
        /// </summary>
        public enum NavigationMode
        {
            linear,
            nonlinear
        }

        /// <summary>
        /// для IMS QTI
        /// режим прохождения теста
        /// каждый вопрос оценивается сразу после того, как на него ответили
        /// все вопросы оцениваются после прохождения теста
        /// </summary>
        public enum SubmissionMode
        {
            individual,
            simultaneous
        }

        /// <summary>
        /// Типы ответов для формата IMS QTI
        /// </summary>
        public enum ResponseType
        {
            simpleChoice,
            simpleAssociableChoice
        }

        /// <summary>
        /// Типы вопросов:
        /// графическое сопоставление,
        /// неграфическое сопоставление,
        /// выбор,
        /// множественный выбор,
        /// открытый вопрос,
        /// расставление порядка,
        /// истина или ложь
        /// неизвестный тип вопроса
        /// </summary>
        public enum QuestionType
        {
            GraphicalMatching,
            SimpleMatching,
            Multichoice,
            Choice,
            OpenEnded,
            Ordering,
            TrueOrFalse,
            Unknown,
            Interactive
        }

        /// <summary>
        /// для IMS QTI
        /// </summary>
        public enum InteractionType
        {
            associateInteraction,
            choiceInteraction,
            customInteraction,
            drawingInteraction,
            endAttemptInteraction,
            extendedTextInteraction,
            gapMatchInteraction,
            graphicAssociateInteraction,
            graphicGapMatchInteraction,
            graphicOrderInteraction,
            hotspotInteraction,
            hottextInteraction,
            inlineChoiceInteraction,
            matchInteraction,
            orderInteraction,
            positionObjectInteraction,
            selectPointInteraction,
            sliderInteraction,
            textEntryInteraction,
            uploadInteraction
        }




    }
}