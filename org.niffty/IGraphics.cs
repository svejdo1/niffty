namespace org.niffty {
  public interface IGraphics {
    void DrawArc(int x, int y, int width, int height, int startAngle, int arcAngle);
    void DrawLine(int x1, int y1, int x2, int y2);
    void DrawString(string text, int x, int y);
    void Translate(int x, int y);
    void DrawPolyline(int[] xPoints, int[] yPoints, int nPoints);
  }
}
