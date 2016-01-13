namespace VisualEditor.Logic.Course.Items.Questions
{
    internal class CorrespondenceQuestion : Question
    {
        public override void WriteQti(string fileName)
        { }
        public override bool ReadQti(string qfPath)
        { return true; }
        // POSTPONE: Реализовать функционал для вопросов на сопоставление.
    }
}