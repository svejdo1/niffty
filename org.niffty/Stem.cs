/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System.Collections.Generic;
using System;
namespace org.niffty {

  /** A Stem has optional tags: absolute or logical placement,
   * voice ID, part ID, multi-node symbol tags, height, small size,
   * large size, cue note, grace note, silent, number of flags
   *
   * @author  user
   */
  public class Stem : MusicSymbol {
    /** Creates a new Stem with the given parameters.
     * 
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Stem(Tags tags) : base(tags) {
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _anchor = null;
      _screenHotspot = null;
      _noteheads = null;
    }

    /** Return the number of noteheads which have been found for this
     * stem.
     * Even though this is private, we always use getNoteheadCount() instead of
     * looking at _noteheads directly, because this allows us to
     * delay computing it as long as possible.
     */
    private int getNoteheadCount() {
      if (_noteheads == null) {
        // Set _noteheads to all noteheads belonging to this stem
        _noteheads = new List<Notehead>();

        for (int i = getIndex() + 1; i < getParentTimeSlice().getMusicSymbolCount(); ++i) {
          MusicSymbol musicSymbol = getParentTimeSlice().getMusicSymbol(i);
          if (musicSymbol is Stem)
            // We have started a new Stem
            break;

          if (musicSymbol is Notehead)
            _noteheads.Add((Notehead)musicSymbol);
        }
      }

      return _noteheads.Count;
    }

    /** Return the notehead at the given index in the notehead list.
     * Even though this is private, we always use getNotehead() instead of
     * looking at _noteheads directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Notehead getNotehead(int index) {
      // Calling this will force _noteheads to be computed.
      getNoteheadCount();

      return _noteheads[index];
    }

    /** Return the anchor as the parent time slice.
     * Even though this is private, we always use getAnchor() instead of
     * looking at _anchor directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Anchored getAnchor() {
      if (_anchor != null)
        return _anchor;

      _anchor = getParentTimeSlice();
      return _anchor;
    }

    /** Return the screen hotspot, computing it if called for the first time.
     * This also sets _topNotehead and _bottomNotehead used internally.
     *
     * @return  The position of the hotspot in screen coordinates.
     */
    public override FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        // We have already computed it
        return _screenHotspot;

      // Set up _topNotehead and _bottomNotehead.
      _topNotehead = null;
      _bottomNotehead = null;
      for (int i = 0; i < getNoteheadCount(); ++i) {
        Notehead notehead = getNotehead(i);

        if (notehead.getShape() == Notehead.Shape.BREVE ||
            notehead.getShape() == Notehead.Shape.WHOLE)
          // Lime has a bug where it defines a stem for these, so ignore
          continue;
        // DEBUG: should also check height of zero as described in the NIFF spec.

        if (_topNotehead == null) {
          // This is the first one, so just initialize and continue.
          _topNotehead = notehead;
          _bottomNotehead = notehead;
          continue;
        }

        int hotspotY = notehead.getScreenHotspot().y;

        if (hotspotY < _topNotehead.getScreenHotspot().y)
          _topNotehead = notehead;
        if (hotspotY > _bottomNotehead.getScreenHotspot().y)
          _bottomNotehead = notehead;
      }

      if (_topNotehead == null) {
        // There will not be a stem.  Just use the anchor's hotspot.
        _screenHotspot = getAnchor().getScreenHotspot();
        return _screenHotspot;
      }

      _stemDown = false;
      if (_tags.logicalPlacement() != null) {
        if (_tags.logicalPlacement().getVertical() == LogicalPlacement.BELOW)
          _stemDown = true;
      }

      // The hotspot is the tip of the stem
      if (_stemDown) {
        // Make the up stem go from the top notehead past the bottom notehead
        //   on the left side of the noteheads.
        // The notehead's hotspot is in the middle of the notehead, so move
        //   to its left, and move down to the bottom of the stem.
        _screenHotspot = new FinalPoint
        (_bottomNotehead.getScreenHotspot(), _bottomNotehead.getLeftEdge(), 16);
      } else {
        // Stem is up
        // Make the up stem go from the bottom notehead past the top notehead
        //   on the right side of the noteheads.
        // The notehead's hotspot is in the middle of the notehead, so move to
        //    its right, and move up to the top of the stem.
        _screenHotspot = new FinalPoint
        (_topNotehead.getScreenHotspot(), _bottomNotehead.getRightEdge(), -16);
      }

      return _screenHotspot;
    }

    /** Get the top notehead, or null if the stem should not be drawn.
     * Even though this is private, we always use getMultiNode() instead of
     * looking at _multiNodes directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Notehead getTopNotehead() {
      // This will force it to be computed if it isn't already.
      getScreenHotspot();
      return _topNotehead;
    }
    /** Get the bottom notehead, or null if the stem should not be drawn.
     * Even though this is private, we always use getMultiNode() instead of
     * looking at _multiNodes directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Notehead getBottomNotehead() {
      // This will force it to be computed if it isn't already.
      getScreenHotspot();
      return _bottomNotehead;
    }

    /** If there is a logical placement tag and its vertical value is BELOW,
     *    return true, otherwise return false as the default which means
     *    stem up.
     */
    public bool getStemDown() {
      // This will force it to be computed if it isn't already.
      getScreenHotspot();
      return _stemDown;
    }

    /** Draw this object
     */
    public override void draw(IGraphics graphics) {
      // This will set _topNotehead and _bottomNotehead if not already set.
      FinalPoint tip = getScreenHotspot();

      if (getTopNotehead() == null)
        // No noteheads with a stem so don't even draw a stem.
        return;

      bool stemDown = getStemDown();
      if (stemDown) {
        // Make the up stem go from the top notehead past the bottom notehead
        //   on the left side of the noteheads.
        graphics.DrawLine(tip.x, tip.y, tip.x, getTopNotehead().getScreenHotspot().y);
      } else {
        // Step is up
        // Make the up stem go from the bottom notehead past the top notehead
        //   on the right side of the noteheads.
        graphics.DrawLine(tip.x, tip.y, tip.x, getBottomNotehead().getScreenHotspot().y);
      }

      if (_tags.numberOfFlags().HasValue) {
        int numberOfFlags = _tags.numberOfFlags().Value;

        // Draw flags.
        // DEBUG: assume there are not also beams.  Maybe report an error.
        int y = tip.y;
        if (numberOfFlags > 1)
          // Leave one flag at the tip, but for multiple, move closer to notehead
          y += (stemDown ? -3 : 3);

        for (int i = 0; i < numberOfFlags; ++i) {
          if (stemDown)
            _stemDownFlag.draw(graphics, tip.x, y);
          else
            _stemUpFlag.draw(graphics, tip.x, y);

          y += (stemDown ? 4 : -4);
        }
      }
    }

    public override string ToString() {
      return "Stem" + _tags;
    }

    private static DrawShape _stemUpFlag, _stemDownFlag;
    // (This is automatically called to initialize static fields.)
    static Stem() {
      {
        var points = new int[] {0,0, 0,1, 0,2, 0,3, 1,2, 1,3, 2,3, 2,4, 4,6, 4,10,
            3,11, 3,13};
        _stemUpFlag = new DrawShape(points);
      }
      {
        var points = new int[] {0,0, 0,-1, 0,-2, 0,-3, 1,-2, 1,-3, 5,-7, 5,-10,
            4,-11, 5,-10};
        _stemDownFlag = new DrawShape(points);
      }
    }

    private Anchored _anchor;
    private FinalPoint _screenHotspot;
    // The top and bottom notehead from _noteheads on the stem
    // These are computed by getScreenHotspot().
    private Notehead _topNotehead;
    private Notehead _bottomNotehead;
    private bool _stemDown;

    // Vector of Notehead
    private IList<Notehead> _noteheads;
  }
}