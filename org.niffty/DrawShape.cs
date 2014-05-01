/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

namespace org.niffty {

  /** A DrawShape holds the points defining a shape to be drawn by the IGraphics class.
   * A DrawShape should not be confused with the types implementing the
   * java.awt.Shape interface.
   *
   * @author  user
   */
  public class DrawShape {
    /** Creates an empty shape where draw does nothing.
     */
    public DrawShape() {
      // Leave _xPoints null.  draw will check for this.
    }

    /** Creates new DrawShape to be drawn by IGraphics drawPolyLine.  NOTE: drawPolyLine
     * does not draw the endpoint of the last line segment, so the
     * supplied points must compensate for this.
     *
     * @param points    An array of size 2 * count, where count is the
     *              number of points.  The array has the format:
     *              {x0, y0, x1, y1, ...} where x0,y0 is the coordinates
     *              of the first points, etc.
     */
    public DrawShape(int[] points) {
      int count = points.Length / 2;
      _xPoints = new int[count];
      _yPoints = new int[count];

      for (int i = 0; i < count; ++i) {
        _xPoints[i] = points[2 * i];
        _yPoints[i] = points[2 * i + 1];
      }

      setEdges();
    }

    private void setEdges() {
      _leftEdge = int.MaxValue;
      _rightEdge = int.MinValue;
      _topEdge = int.MaxValue;
      _bottomEdge = int.MinValue;

      for (int i = 0; i < _xPoints.Length; ++i) {
        if (_xPoints[i] < _leftEdge)
          _leftEdge = _xPoints[i];
        if (_xPoints[i] > _rightEdge)
          _rightEdge = _xPoints[i];
        if (_yPoints[i] < _topEdge)
          _topEdge = _yPoints[i];
        if (_yPoints[i] > _bottomEdge)
          _bottomEdge = _yPoints[i];
      }
    }

    /** Draw the shape with the given offset using IGraphics drawPolyLine.
     *
     * @param graphics  the IGraphics context used to draw the shape
     * @param offsetX   the X amount to offset the shape when drawing
     * @param offsetY   the Y amount to offset the shape when drawing
     */
    public void draw(IGraphics graphics, int offsetX, int offsetY) {
      if (_xPoints == null || _yPoints == null)
        // Nothing to draw
        return;
      graphics.Translate(offsetX, offsetY);
      graphics.DrawPolyline(_xPoints, _yPoints, _xPoints.Length);
      graphics.Translate(-offsetX, -offsetY);
    }

    /** Same as draw (IGraphics, int, int) but takes the offset as a FinalPoint.
     *
     * @see #draw(IGraphics, int, int)
     */
    public void draw(IGraphics graphics, FinalPoint offset) {
      draw(graphics, offset.x, offset.y);
    }

    /** Draw this shape so that it touches the left edge of the target shape,
     * or if gap is > 0 then place that many pixels of gap between the two shapes.
     *
     * @param graphics  the IGraphics context used to draw the shape
     * @param target    the target shape on whose left edge this is drawn
     * @param targetOffsetX   the X amount to offset the target shape. Note that
     *              the target shape is placed with the offset and this shape
     *              is placed to the left of it.
     * @param targetOffsetY   the Y amount to offset the target shape
     * @param gap       amount of space between the two shapes.  If zero,
     *                  then the two shapes touch.  If 1, then there is one
     *                  pixel between them, etc.
     * @see #draw
     */
    public void drawLeftOf
      (IGraphics graphics, DrawShape target, int targetOffsetX, int targetOffsetY, int gap) {
      // Note that to get the two shapes to just touch with zero gap, the
      //   value of _rightEdge must be one left of the value of target._leftEdge
      int offsetX = targetOffsetX + target._leftEdge - (_rightEdge + gap + 1);

      graphics.Translate(offsetX, targetOffsetY);
      graphics.DrawPolyline(_xPoints, _yPoints, _xPoints.Length);
      graphics.Translate(-offsetX, -targetOffsetY);
    }

    /** Same as drawLeftOf (IGraphics, DrawShape, int, int, int) but takes the 
     * offset as a FinalPoint.
     *
     * @see #drawLeftOf (IGraphics, DrawShape, int, int, int)
     */
    public void drawLeftOf(IGraphics graphics, DrawShape target,
              FinalPoint targetOffset, int gap) {
      drawLeftOf(graphics, target, targetOffset.x, targetOffset.y, gap);
    }

    /** Return the left edge of the shape which is the minimum X coordinate.
     */
    public int getLeftEdge() {
      return _leftEdge;
    }
    /** Return the right edge of the shape which is the maximum X coordinate.
     */
    public int getRightEdge() {
      return _rightEdge;
    }
    /** Return the top edge of the shape which is the minimum Y coordinate.
     */
    public int getTopEdge() {
      return _topEdge;
    }
    /** Return the bottom edge of the shape which is the maximum Y coordinate.
     */
    public int getBottomEdge() {
      return _bottomEdge;
    }

    int[] _xPoints;
    int[] _yPoints;

    int _leftEdge, _rightEdge, _topEdge, _bottomEdge;
  }
}