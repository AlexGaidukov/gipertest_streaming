using System;
using System.Drawing;
using VisualEditor.Utils.Helpers;

namespace VisualEditor.Logic.Commands
{
    internal abstract class AbstractCommand
    {
        protected string name;
        protected string text;
        protected bool enabled;
        protected bool _checked;
        protected Image image;

        protected AbstractCommand()
        {
            enabled = false;
        }

        public string Name
        {
            get { return name; }
        }

        public string Text
        {
            get { return text; }
            set { text = value; }
        }

        public Image Image
        {
            get { return image; }
        }

        public bool Enabled
        {
            get { return enabled; }
            set
            {
                enabled = value;
                OnStateChanged();
            }
        }

        public bool Checked
        {
            get { return _checked; }
            set
            {
                _checked = value;
                OnStateChanged();
            }
        }

        public event EventHandler StateChanged;

        public virtual void OnStateChanged()
        {
            if (StateChanged.IsNull())
            {
                return;
            }

            StateChanged(this, EventArgs.Empty);
        }

        public abstract void Execute(object @object);
    }
}