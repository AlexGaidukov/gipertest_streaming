namespace VisualEditor.Logic.Commands.Course
{
    internal class AddTestModuleFromOuterCourse : AbstractCommand
    {
        public AddTestModuleFromOuterCourse()
        {
            name = CommandNames.AddTestModuleFromOuterCourse;
            text = CommandTexts.AddTestModuleFromOuterCourse;
            image = Properties.Resources.TestModule;
        }

        public override void Execute(object @object)
        {
            if (!Enabled)
            {
                return;
            }

            CommandManager.Instance.GetCommand(CommandNames.AddTestModuleFromOuterCourseSmall).Execute(null);
        }
    }
}