/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A Placement has a horizontal and vertical placement.  Note
   * that these are not screen pixels but are in the units defined in the
   * ScoreSetup.  A Placement is used to specify the absolute placement,
   * Bezier incoming and Bezier outgoing tags.
   *
   * @author  user
   */
  public class Placement {
    /** Creates new Placement with zero horizontal and vertical */
    public Placement() {
    }

    /** Creates new Placement with the given horizontal and vertical */
    public Placement(int horizontal, int vertical) {
      _horizontal = horizontal;
      _vertical = vertical;
    }

    /** Return the horizontal value
     */
    public int getHorizontal() {
      return _horizontal;
    }
    /** Return the vertical value
     */
    public int getVertical() {
      return _vertical;
    }

    /** Returns string as "(horizontal,vertical)"
     */
    public override string ToString() {
      return "(" + _horizontal + "," + _vertical + ")";
    }

    private int _horizontal;
    private int _vertical;
  }
}