/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.Collections.Generic;
namespace org.niffty {

  /** A Tie has optional tags: multi-node symbol tags, tie direction,
   * Bezier incoming and Bezier outgoing.
   *
   * @author  user
   */
  public class Tie : MusicSymbol {
    /** Creates a new Tie with the given parameters.
     * 
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Tie(Tags tags)
      : base(tags) {
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _multiNodes = null;
      _anchor = null;
    }

    /** Get the anchor which is the previous Notehead.
     * Even though this is private, we always use getAnchor() instead of
     * looking at _anchor directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Anchored getAnchor() {
      if (_anchor != null)
        return _anchor;

      var previousNotehead = previousInstanceOf(typeof(Notehead));
      if (previousNotehead == null)
        // DEBUG: should check for multi-node end and start of system
        _anchor = findDefaultAnchor();
      else
        _anchor = previousNotehead;
      return _anchor;
    }

    /** Return the number of nodes which have been found for this 
     * multi-node symbol, or 0 if this is not the defining node
     * of the multi-node symbol.  (If this is the defining node,
     * then the multi-node count is always at least one since the
     * defining node is included.)
     * Even though this is private, we always use getMultiNodeCount() instead of
     * looking at _multiNodes directly, because this allows us to
     * delay computing it as long as possible.
     */
    private int getMultiNodeCount() {
      if (_multiNodes == null)
        _multiNodes = findMultiNodes();
      // DEBUG: Maybe we should sort these to make sure the first is the leftmost.

      return _multiNodes.Count;
    }

    /** Return the node at the given index in the 
     * multi-node symbol.
     * Even though this is private, we always use getMultiNode() instead of
     * looking at _multiNodes directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Tie getMultiNode(int index) {
      // Calling getMultiNodeCount() will force _multiNodes to be found.
      getMultiNodeCount();

      return (Tie)_multiNodes[index];
    }

    /** Return true if the tie should be rounded above, or false if rounded below.
     */
    private bool getRoundedAbove() {
      // DEBUG: this should be cached.
      if (_tags.tieDirection() != null) {
        if (_tags.tieDirection() == TieDirection.ROUNDED_ABOVE)
          return true;
        else if (_tags.tieDirection() == TieDirection.ROUNDED_BELOW)
          return false;

        // Otherwise choose according to stem direction
      }

      // No tie direction tag, so choos according to stem direction.
      Stem stem = (Stem)getMultiNode(0).previousInstanceOf(typeof(Stem));
      if (stem == null)
        // Unexpected.  No stem, so default to rounded above
        return true;
      return stem.getStemDown();
    }

    /** Draw this object
     */
    public override void draw(IGraphics graphics) {
      int multiNodeCount = getMultiNodeCount();
      if (multiNodeCount == 0)
        // Only the defining Beam draws the beams for all in the multi-node group
        return;

      if (multiNodeCount != 2)
        // DEBUG: should handle this case
        return;

      Tie leftNode = getMultiNode(0);
      Tie rightNode = getMultiNode(1);
      FinalPoint leftHotspot = leftNode.getAnchor().getScreenHotspot();
      FinalPoint rightHotspot = rightNode.getAnchor().getScreenHotspot();
      Staff leftNodeParentStaff =
        leftNode.getParentTimeSlice().getParentMeasureStart().getParentStaff();
      if (leftNodeParentStaff !=
          rightNode.getParentTimeSlice().getParentMeasureStart().getParentStaff()) {
        // The two ends of the tie are on different staves. Assume that
        //   the left end should extend to the end of the staff and that
        //   the right end should have a little continuation from the beginning
        int endX = leftNodeParentStaff.getParentSystem().getScreenHotspot().x +
          leftNodeParentStaff.getParentSystem().getWidth() + 10;
        drawTie(graphics, leftHotspot.x + 3, leftHotspot.y, endX);
        drawTie(graphics, rightHotspot.x - 12, rightHotspot.y, rightHotspot.x - 3);
      } else
        drawTie(graphics, leftHotspot.x + 3, leftHotspot.y, rightHotspot.x - 3);
    }

    /** Draw a tie from startX,startY to endX,startY.  Assume the end Y
     * is the same as startY.
     */
    private void drawTie(IGraphics graphics, int startX, int startY, int endX) {
      // deltaY is the distance from the start point to the top of the arc
      int deltaY = (endX - startX) / 6;
      if (deltaY < 5)
        deltaY = 5;
      if (deltaY > 15)
        deltaY = 15;

      int startAngle = 15;
      bool roundedAbove = getRoundedAbove();
      graphics.DrawArc
         (startX, startY - deltaY, endX - startX, 2 * deltaY,
          startAngle + (roundedAbove ? 0 : 180), 2 * (90 - startAngle));
      // Draw again to make thicker
      graphics.DrawArc
         (startX, startY - (deltaY + 1), endX - startX, 2 * (deltaY + 1),
          startAngle + (roundedAbove ? 0 : 180), 2 * (90 - startAngle));
    }

    public override string ToString() {
      return "Tie" + _tags;
    }

    private Anchored _anchor;
    private IList<MusicSymbol> _multiNodes;
  }
}