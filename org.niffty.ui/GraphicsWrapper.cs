using System;
using System.Drawing;

namespace org.niffty.ui {
  public class GraphicsWrapper : IGraphics {
    private Graphics m_Graphics;
    public GraphicsWrapper(Graphics graphics) {
      if (graphics == null) {
        throw new ArgumentNullException("graphics");
      }

      m_Graphics = graphics;
    }

    public void DrawArc(int x, int y, int width, int height, int startAngle, int arcAngle) {
      m_Graphics.DrawArc(Pens.Black, x, y, width, height, startAngle, arcAngle);
    }

    public void DrawLine(int x1, int y1, int x2, int y2) {
      m_Graphics.DrawLine(Pens.Black, x1, y1, x2, y2);
    }

    public void DrawString(string text, int x, int y) {
      m_Graphics.DrawString(text, SystemFonts.DefaultFont, Brushes.Black, x, y, StringFormat.GenericDefault);
    }

    public void Translate(int x, int y) {
      m_Graphics.TranslateTransform(x, y);
    }

    public void DrawPolyline(int[] xPoints, int[] yPoints, int nPoints) {
      for (var i = 0; i < nPoints - 1; i++) {
        DrawLine(xPoints[i], yPoints[i], xPoints[i + 1], yPoints[i + 1]);
      }
    }
  }
}
