/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.Collections.Generic;
namespace org.niffty {

  /** A Beam has a parts to left and parts to right and optional 
   * tags: multi-node symbol tags
   *
   * @author  user
   */
  public class Beam : MusicSymbol {
    static readonly String RIFF_ID = "beam";

    /** Creates a new Beam with the given parameters.
     * 
     * @param partsToLeft see getPartsToLeft()
     * @param partsToRight see getPartsToRight()
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Beam(int partsToLeft, int partsToRight, Tags tags) : base(tags) {
      _partsToLeft = partsToLeft;
      _partsToRight = partsToRight;
    }

    /** Returns the parts to left for the beam.
     */
    public int getPartsToLeft() {
      return _partsToLeft;
    }
    /** Returns the parts to right for the beam.
     */
    public int getPartsToRight() {
      return _partsToRight;
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _multiNodes = null;
      _screenHotspot = null;
      _anchor = null;
    }

    /** Get the anchor which is the previous Stem.
     * Even though this is private, we always use getAnchor() instead of
     * looking at _anchor directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Anchored getAnchor() {
      if (_anchor != null)
        return _anchor;

      MusicSymbol previousStem = previousInstanceOf(typeof(Stem));
      if (previousStem == null)
        // DEBUG: maybe this should be an error
        _anchor = findDefaultAnchor();
      else
        _anchor = previousStem;
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
    private Beam getMultiNode(int index) {
      // Calling getMultiNodeCount() will force _multiNodes to be found.
      getMultiNodeCount();

      return (Beam)_multiNodes[index];
    }

    /** If this is the defining node, return the screen hotspot, computing it if 
     * called for the first time.  The screen hotspot for the beam group is
     * the position of the left end of the beam group at the beam farthest from
     * the notehead.  If not the defining node, return the
     * anchor hotspot.
     * Strictly speaking, this is the hotspot of the left end of the beam, which
     * is not necessarily the defining node.
     * This also sets _maxParts and other values used internally.
     *
     * @return  The position of the hotspot in screen coordinates.
     */
    public override FinalPoint getScreenHotspot() {
        if (_screenHotspot != null)
            // We have already computed it
            return _screenHotspot;
        
        int multiNodeCount = getMultiNodeCount();
        if (multiNodeCount == 0) {
            // Not the defining node. Just use the anchor's hot spot
            _screenHotspot = getAnchor().getScreenHotspot();
            return _screenHotspot;
        }
        
        if (multiNodeCount == 1) {
            // This is an unexpected case with the beam on only one stem.
            _screenHotspot = getAnchor().getScreenHotspot();
            return _screenHotspot;
        }
        
        // A beam is either horizontal (if the first and last stem tip are
        //   at the same y value) or slanted with one end of the beam
        //   5 points higher or lower than the beginning, based on whether
        //   the last stem tip is higher or lower.
        // Don't assume that this defining node is the leftmost, but do
        //   assume multiNode(0) is leftmost.
        // Assume the anchor for a beam is the stem whos hotspot is its tip.
        Point hotspot = getMultiNode(0).getAnchor().getScreenHotspot().newPoint();
        FinalPoint rightmost = getMultiNode (multiNodeCount - 1).getAnchor().getScreenHotspot();
        if (rightmost.y == hotspot.y)
            _slope = 0.0;
        else if (rightmost.y > hotspot.y)
            _slope = 5.0 / (rightmost.x - hotspot.x);
        else
            _slope = -5.0 / (rightmost.x - hotspot.x);

        // DEBUG: this assumes all stems in the same direction.
        _stemDirection = ((Stem)getMultiNode(0).getAnchor()).getStemDown() ? 1 : -1;
        
        // The Y position along the beam at x is 
        //   hotspot.y + _slope * (x - hotspot.x).
        // Check each stem tip and adjust the hotspot.y as necessary
        //  to make sure the beam does not cross the stem.
        for (int i = 1; i < multiNodeCount; ++i) {
            FinalPoint stemTip = getMultiNode(i).getAnchor().getScreenHotspot();
            int beamY = (int)(hotspot.y + _slope * (stemTip.x - hotspot.x));
            
            // Note that larger Y is lower on the screen!
            // _stemDirection > 0 means stems down.
            if (_stemDirection > 0 && stemTip.y > beamY)
                hotspot.translate (0, stemTip.y - beamY);
            else if (_stemDirection < 0 && stemTip.y < beamY)
                hotspot.translate (0, -(beamY - stemTip.y));
        }
        
        // Compute _maxParts to be used later.
        _maxParts = 0;
        for (int i = 0; i < multiNodeCount; ++i) {
            Beam beam = getMultiNode(i);
            if (beam._partsToLeft > _maxParts)
                _maxParts = beam._partsToLeft;
            if (beam._partsToRight > _maxParts)
                _maxParts = beam._partsToRight;
        }
        
        // Move the hotspot to the beam farthest from the notehead.
        hotspot.translate (0, (_maxParts - 1) * _stemDirection * SPACING);
        
        if (_maxParts > 1)
            // As a special case for 16th notes and higher, move the beams
            //  closer to the notehead.
            hotspot.translate (0, - _stemDirection * SPACING);
        
        _screenHotspot = new FinalPoint (hotspot);
        return _screenHotspot;
    }

    /** Even though this is private, we always use getSlope() instead of
     * looking at _slope directly, because this allows us to
     * delay computing it as long as possible.
     */
    private double getSlope() {
      // This will force it to be computed if it isn't already.
      getScreenHotspot();
      return _slope;
    }
    /** Even though this is private, we always use getMaxParts() instead of
     * looking at _maxParts directly, because this allows us to
     * delay computing it as long as possible.
     */
    private int getMaxParts() {
      // This will force it to be computed if it isn't already.
      getScreenHotspot();
      return _maxParts;
    }
    /** Even though this is private, we always use getStemDirection() instead of
     * looking at _stemDirection directly, because this allows us to
     * delay computing it as long as possible.
     */
    private int getStemDirection() {
      // This will force it to be computed if it isn't already.
      getScreenHotspot();
      return _stemDirection;
    }

    /** Draw this object
     */
    public override void draw(IGraphics graphics) {
      int multiNodeCount = getMultiNodeCount();
      if (multiNodeCount == 0)
        // Only the defining Beam draws the beams for all in the multi-node group
        return;

      if (multiNodeCount == 1)
        // DEBUG: should handle this case
        return;

      FinalPoint left = getScreenHotspot();
      double slope = getSlope();
      int maxParts = getMaxParts();
      int stemDirection = getStemDirection();

      for (int i = 0; i < multiNodeCount; ++i) {
        Beam beam = getMultiNode(i);
        Beam previousBeam = null, nextBeam = null;
        if (i > 0)
          previousBeam = getMultiNode(i - 1);
        if (i < (multiNodeCount - 1))
          nextBeam = getMultiNode(i + 1);

        FinalPoint stemTip = beam.getAnchor().getScreenHotspot();
        // Get X and Y for this beam
        int beamX = stemTip.x;
        int beamY = (int)(left.y + slope * (beamX - left.x));

        // First, extend the stem.
        graphics.DrawLine(beamX, beamY, stemTip.x, stemTip.y);

        // For each of the possible parts, draw as needed to the left or right
        for (int part = 1; part <= maxParts; part++) {
          if (i > 0) {
            // Only do parts to left if we are not the leftmost stem
            if (part <= beam._partsToLeft && part > previousBeam._partsToRight)
              // Draw a stub to the left
              // DEBUG: should make sure this doesn't run into the other stem
              drawBeam(graphics, beamX, beamY, beamX - 6);
            // The case where the beam would extend all the way was handled
            // by the previousBeam drawing to here.
          }
          if (i < (multiNodeCount - 1)) {
            // Only do parts to right if we are not the rightmost stem
            if (part <= beam._partsToRight && part > nextBeam._partsToLeft)
              // Draw a stub to the right
              // DEBUG: should make sure this doesn't run into the other stem
              drawBeam(graphics, beamX, beamY, beamX + 6);
            if (part <= beam._partsToRight && part <= nextBeam._partsToLeft)
              // Draw beam extended over to the other
              drawBeam(graphics, beamX, beamY,
                        nextBeam.getAnchor().getScreenHotspot().x);
          }

          beamY += -stemDirection * SPACING;
        }
      }
    }

    /** Draw a beam starting at the beamX, beamY and
     * extending to endX.  The vertical location of the end is determined
     * by getSlope().
     */
    private void drawBeam(IGraphics graphics, int beamX, int beamY, int endX) {
      int endY = (int)(beamY + getSlope() * (endX - beamX));

      // Make the beam three thick vertically.
      for (int i = 0; i < 3; ++i)
        graphics.DrawLine(beamX, beamY - i, endX, endY - i);
    }

    public override string ToString() {
      return "Beam, parts to left=" + _partsToLeft + ", parts to right=" +
          _partsToRight + _tags;
    }

    // The vertical distance between the start of each beam.
    private static int SPACING = 4;

    private int _partsToLeft;
    private int _partsToRight;

    private Anchored _anchor;
    private IList<MusicSymbol> _multiNodes;
    private FinalPoint _screenHotspot;
    // These are defined by screenHotspot().
    private double _slope;
    private int _maxParts;
    // _stemDirection is 1 for stems down because that is increasing Y,
    //   otherwise -1.
    private int _stemDirection;
  }
}