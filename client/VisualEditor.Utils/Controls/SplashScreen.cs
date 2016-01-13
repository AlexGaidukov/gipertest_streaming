using System.Windows.Forms;

namespace VisualEditor.Utils.Controls
{
    public partial class SplashScreen : Form
    {
        private static SplashScreen instance;
        
        private SplashScreen()
        {
            InitializeComponent();
        }

        public string Message
        {
            set
            {
                messageLabel.Text = value;
                Update();
            }
        }

        public string BuildVersion
        {
            set
            {
                buildVersionLabel.Text = value;
                Update();
            }
        }

        public static SplashScreen Instance
        {
            get
            {
                return instance ?? (instance = new SplashScreen());
            }
        }
    }
}