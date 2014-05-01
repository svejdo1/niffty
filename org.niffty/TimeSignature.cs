/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {


  /** A TimeSignature has a top number and bottom number and optional
   * tags: absolute or logical placement, large size, small size.
   *
   * @author  user
   */
  public class TimeSignature : MusicSymbol {
    /** Creates a new TimeSignature with the given parameters.
     * 
     * @param topNumber see getTopNumber()
     * @param bottomNumber see getBottomNumber()
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public TimeSignature(int topNumber, int bottomNumber, Tags tags)
      : base(tags) {
      _topNumber = topNumber;
      _bottomNumber = bottomNumber;
    }

    /** Returns the top number of the time signature.
     * For common time, top number = -1, bottom number = -1
     * For cut time, top number = -2, bottom number = -1
     */
    public int getTopNumber() {
      return _topNumber;
    }

    /** Returns the bottom number of the time signature.
     * For common time, top number = -1, bottom number = -1
     * For cut time, top number = -2, bottom number = -1
     */
    public int getBottomNumber() {
      return _bottomNumber;
    }

    public override bool isLeftPositionedSymbol() {
      return true;
    }

    /** This is automatically called after the object is modified to to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _screenHotspot = null;
    }

    /** Get the hotspot for this object in screen coordinates.
     */
    public override FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      // debug: Very preliminary.  Should
      //   take into account the width of this and the previous shape, etc.
      _screenHotspot = new FinalPoint
        (findLeftPositionedX(), getParentTimeSlice().getScreenHotspot().y);

      return _screenHotspot;
    }

    public override void draw(IGraphics graphics) {
      // DEBUG: should use shapes instead of drawString and handle cut time, etc.
      FinalPoint hotspot = getScreenHotspot();
      graphics.DrawString("" + _topNumber, hotspot.x, hotspot.y + 10);
      graphics.DrawString("" + _bottomNumber, hotspot.x, hotspot.y + 20);
    }

    public override string ToString() {
      return "Time-signature, top=" + _topNumber + ", bottom=" + _bottomNumber + _tags;
    }

    private int _topNumber;
    private int _bottomNumber;

    private FinalPoint _screenHotspot;
  }
}