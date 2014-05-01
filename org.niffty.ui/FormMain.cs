using System.Windows.Forms;

namespace org.niffty.ui {
  public partial class FormMain : Form {
    public FormMain() {
      InitializeComponent();
    }

    public Score Score {
      get { return musicControl.Score; }
      set { musicControl.Score = value; }
    }
  }
}
