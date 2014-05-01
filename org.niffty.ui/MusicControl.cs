using System.Windows.Forms;

namespace org.niffty.ui {
  public partial class MusicControl : UserControl {
    private Score _score;
    private int _pageIndex;

    public MusicControl() {
      InitializeComponent();
    }

    public int PageIndex {
      get {
        if (_score == null)
          return -1;
        return _pageIndex;
      }
      set {
        if (_score == null)
          return;

        if (value < 0)
          value = 0;
        if (value >= _score.getData().getPageCount())
          value = _score.getData().getPageCount() - 1;

        _pageIndex = value;
        //repaint();
      }
    }

    public Score Score {
      get { return _score; }
      set {
        _score = value;
        if (value != null) {
          _score.invalidate();
        }
        _pageIndex = 0;
      }
    }

    protected override void OnPaint(PaintEventArgs e) {
      if (Score == null) {
        return;
      }
      var wrapper = new GraphicsWrapper(e.Graphics);
      _score.getData().getPage(_pageIndex).draw(wrapper);
    }
  }
}
