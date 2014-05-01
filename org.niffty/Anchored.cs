/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

namespace org.niffty {

  /** The Anchored interface is for music elements such as Staff, Notehead, etc.
   * whose position is relative to an anchor.
   *
   * @author  user
   */
  public interface Anchored {
    /** Get the hotspot for this object in screen coordinates.
     *
     * @return  The position of the hotspot in screen coordinates from top-left
     *          of screen (not relative to an anchor).
     *          This is a FinalPoint and not a Point so that the class can
     *          give access to its screen hotspot object knowing that the
     *          caller will not modify it.  This is more efficient that returning
     *          a new Point every time getScreenHotspot() is called.
     * @see FinalPoint#newPoint
     */
    FinalPoint getScreenHotspot();
  }
}
