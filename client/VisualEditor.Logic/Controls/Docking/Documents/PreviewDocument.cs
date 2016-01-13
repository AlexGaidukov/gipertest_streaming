using System.Drawing;
using System.Windows.Forms;

namespace VisualEditor.Logic.Controls.Docking.Documents
{
  public partial class PreviewDocument : Form
  {
    public PreviewDocument(Bitmap iconResource, string content = null)
    {
      InitializeComponent();
      if (!string.IsNullOrEmpty(content))
      {
        webBrowser1.DocumentText = content;
        webBrowser1.Select();
      }
      Text = "Предварительный просмотр";
      MainForm.Instance.Text = string.Concat("Предварительный просмотр - ", Application.ProductName);
      Icon = Icon.FromHandle(iconResource.GetHicon());
    }

    public string Content
    {
      get
      {
        return webBrowser1.DocumentText;
      }
      set
      {
        if (!string.IsNullOrEmpty(value))
        {
          webBrowser1.DocumentText = value;
          webBrowser1.Select();
        }
      }
    }
  }
}
