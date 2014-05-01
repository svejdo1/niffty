/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** This is an immutable version of Point where x and y cannot change.
   * This is so that a class can give access to a point and know that
   * other classes will not change it.  To convert this to a Point which you
   * can change, call newPoint.
   *
   * @see #newPoint
   */
  public class FinalPoint {
    /** The x coordinate.
     */
    public readonly int x;
    /** The y coordinate.
     */
    public readonly int y;

    /** Creates new FinalPoint with the given x and y. */
    public FinalPoint(int x, int y) {
      this.x = x;
      this.y = y;
    }

    public FinalPoint(Point point)
      : this(point.x, point.y) {
    }

    /** Creates a new FinalPoint with the coordinates of point added to
     *    deltaX and deltaY.
     */
    public FinalPoint(FinalPoint point, int deltaX, int deltaY) :
      this(point.x + deltaX, point.y + deltaY) {
    }

    public Point newPoint() {
      return new Point(x, y);
    }

    /** Return String of the form "(x,y)"
     */
    public override string ToString() {
      return "(" + x + "," + y + ")";
    }
  }
}
