/*
  This code is part of the Niffty NIFF Music File Viewer.
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** An AugmentationDot has optional tags: logical placement.
   *
   * @author  user
   */
  public class AugmentationDot : MusicSymbol {
    /** Creates a new AugmentationDot with the given parameters.
     *
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public AugmentationDot(Tags tags)
      : base(tags) {
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _previousDot = null;
      _anchor = null;
      _screenHotspot = null;
    }

    /** Return the anchor as the previous Notehead or Rest.
     * Even though this is private, we always use getAnchor() instead of
     * looking at _anchor directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Anchored getAnchor() {
      if (_anchor != null)
        return _anchor;

      MusicSymbol previousNotehead = previousInstanceOf(typeof(Notehead));
      MusicSymbol previousRest = previousInstanceOf(typeof(Rest));
      if (previousNotehead == null && previousRest == null)
        // DEBUG: maybe this should be an error
        _anchor = findDefaultAnchor();
      else if (previousRest == null)
        // There is only a notehead
        _anchor = previousNotehead;
      else if (previousNotehead == null)
        // There is only a rest
        _anchor = previousRest;
      else {
        // Use the most recent notehead or rest
        if (previousNotehead.getIndex() > previousRest.getIndex())
          _anchor = previousNotehead;
        else
          _anchor = previousRest;
      }

      return _anchor;
    }

    /** Return the previous dot for this Notehead, or null if this is the first one.
     * Don't set the anchor to the previous dot, because the anchor should
     * always be the Notehead.
     */
    private AugmentationDot getPreviousDot() {
      if (_gotPreviousDot)
        return _previousDot;

      // Search for the previous AugmentationDot before a Notehead
      _previousDot = null;
      for (int i = getIndex() - 1; i >= 0; --i) {
        MusicSymbol symbol = getParentTimeSlice().getMusicSymbol(i);
        if (symbol is Notehead)
          break;
        if (symbol is AugmentationDot) {
          _previousDot = (AugmentationDot)symbol;
          break;
        }
      }

      _gotPreviousDot = true;
      return _previousDot;
    }

    /** Get the hotspot for this object in screen coordinates.
     */
    public override FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      int deltaY = 0;
      Anchored anchor = getAnchor();
      AugmentationDot previousDot = getPreviousDot();
      if (anchor is Notehead) {
        // Debug: should ensure that anchor is always a Notehead
        if (previousDot == null && ((Notehead)anchor).getStaffStep() % 2 == 0)
          // Notehead is on a line and this is the first dot, so shift up the dot
          deltaY = -3;
      }

      if (previousDot == null)
        // This is the first dot
        _screenHotspot = new FinalPoint(anchor.getScreenHotspot(), 5, deltaY);
      else
        // Shift to the right from the previous dot
        _screenHotspot = new FinalPoint(previousDot.getScreenHotspot(), 3, deltaY);

      return _screenHotspot;
    }

    public override void draw(IGraphics graphics) {
      _dot.draw(graphics, getScreenHotspot());
    }

    public override string ToString() {
      return "Augmentation-dot" + _tags;
    }

    private static DrawShape _dot;
    // (This is automatically called to initialize static fields.)
    static AugmentationDot() {
      {
        var points = new int[] { 0, 0, 1, 0, 1, 1, 0, 1, 0, 0 };
        _dot = new DrawShape(points);
      }
    }

    private FinalPoint _screenHotspot;
    private Anchored _anchor;
    // This defines whether the value of _previousDot is meaningful.
    private bool _gotPreviousDot;
    private AugmentationDot _previousDot;
  }
}

