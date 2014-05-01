/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {


  /** An Accidental has a shape and optional tags: small size, parenthesis
   *
   * @author  user
   */
  public class Accidental : MusicSymbol {
    /** Creates a new Accidental with the given parameters.
     * 
     * @param shape  the Shape constant.  This must not be null.
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Accidental(Shape shape, Tags tags) : base(tags) {
      _shape = shape;
    }

    /** An Accidental.Shape has constants for the legal shapes.
     *  This is a "typesafe enum" where you can use a static members like
     *  a constant and compare with == .
     */
    public enum Shape {
      DOUBLE_FLAT,
      FLAT,
      NATURAL,
      SHARP,
      DOUBLE_SHARP,
      QUARTER_TONE_FLAT,
      THREE_QUARTER_TONES_FLAT,
      QUARTER_TONE_SHARP,
      THREE_QUARTER_TONES_SHARP
    }

    /** Returns the shape.
     */
    public Shape getShape() {
      return _shape;
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _anchor = null;
      _screenHotspot = null;
    }

    /** Return the anchor as the previous Notehead.
     * Even though this is private, we always use getAnchor() instead of
     * looking at _anchor directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Anchored getAnchor() {
      if (_anchor != null)
        return _anchor;

      MusicSymbol previousNotehead = previousInstanceOf(typeof(Notehead));
      if (previousNotehead == null)
        // DEBUG: maybe this should be an error
        _anchor = findDefaultAnchor();
      else
        _anchor = previousNotehead;
      return _anchor;
    }

    /** Get the hotspot for this object in screen coordinates.  
     */
    public override FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      Anchored anchor = getAnchor();
      if (!(anchor is Notehead)) {
        // We never found a previous Notehead in the TimesSlice, which shouldn't
        //   happen.  Just use whatever anchor was found
        _screenHotspot = anchor.getScreenHotspot();
        return _screenHotspot;
      }

      Notehead notehead = (Notehead)anchor;
      // Debug: very preliminary.  This does not account for the
      //  possibility of bumping into offset 2nds from the Stem, etc.
      int offsetX = notehead.getLeftEdge() - (graphicShape().getRightEdge() + 3);
      // offsetX should be negative
      _screenHotspot = new FinalPoint(notehead.getScreenHotspot(), offsetX, 0);
      return _screenHotspot;
    }

    private DrawShape graphicShape() {
      if (_shape == Shape.FLAT)
        return _flat;
      else if (_shape == Shape.NATURAL)
        return _natural;
      else if (_shape == Shape.SHARP)
        return _sharp;
      // Debug: must add other shapes
      else
        // unrecognized.  Shouldn't happen, so return an empty shape
        return new DrawShape();
    }

    public override void draw(IGraphics graphics) {
      graphicShape().draw(graphics, getScreenHotspot());
    }

    public override string ToString() {
      return "Accidental, shape=" + _shape + _tags;
    }

    public static readonly DrawShape _flat, _natural, _sharp;
    // (This is automatically called to initialize static fields.)
    static Accidental() {
      {
        var points = new int[] { -1, -8, -1, 3, 2, 0, 2, -2, -1, -2 };
        _flat = new DrawShape(points);
      }
      {
        var points = new int[] { -1, -5, -1, 3, 2, 3, 2, 6, 2, -2, -1, -2, -1, 2, 2, 2, 2, -1, -1, -1 };
        _natural = new DrawShape(points);
      }
      {
        var points = new int[] {-1,7, -1,-5, -1,-2, -2,-2, 2,-2, 2,-1, -2,-1, 1,-1, 1,-5, 1,6,
            1,2, -1,2, 2,2, 2,3, -2,3, -2,4, -1,4};
        _sharp = new DrawShape(points);
      }
    }

    private Shape _shape;

    private Anchored _anchor;
    private FinalPoint _screenHotspot;
  }
}