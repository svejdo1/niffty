/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A Clef has a shape, staff step, and octave number and optional
   * tags: logical or absolute placement, voice ID
   *
   * @author  user
   */
  public class Clef : MusicSymbol {
    /** Creates a new Clef with the given parameters.
     * 
     * @param shape  the Shape constant.  This must not be null.
     * @param staffStep  see getStaffStep()
     * @param shape  the OctaveNumber constant.  This must not be null.
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Clef
      (Shape shape, int staffStep, OctaveNumber octaveNumber, Tags tags) : base(tags) {
      _shape = shape;
      _staffStep = staffStep;
      _octaveNumber = octaveNumber;
    }

    /** A Clef.Shape has constants for the legal shapes.
     *  This is a "typesafe enum" where you can use a static members like
     *  a constant and compare with == .
     */
    public enum Shape {
      G_CLEF,
      F_CLEF,
      C_CLEF,
      PERCUSSION,
      DOUBLE_G_CLEF,
      TABLATURE
    }

    /** A Clef.OctaveNumber has constants for the legal octave number indicatiors.
     *  This is a "typesafe enum" where you can use a static members like
     *  a constant and compare with == .
     */
    public enum OctaveNumber {
      NONE,
      ABOVE_8,
      BELOW_8,
      ABOVE_15,
      BELOW_15
    }

    /** Returns the shape.
     */
    public Shape getShape() {
      return _shape;
    }
    /** Returns the staff step for the clef, which is 2 for a
     * G_CLEF, etc.
     */
    public int getStaffStep() {
      return _staffStep;
    }
    /** Returns the octave number.
     */
    public OctaveNumber getOctaveNumber() {
      return _octaveNumber;
    }

    public override bool isLeftPositionedSymbol() {
      return true;
    }

    /** This is automatically called after the object is modified to force
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
      // Use the Y value from staffStepOffsetY
      //   from the parent time slice's position at the top of the staff.
      _screenHotspot = new FinalPoint
        (findLeftPositionedX(),
         getParentTimeSlice().getScreenHotspot().y +
           Notehead.staffStepOffsetY(_staffStep));

      return _screenHotspot;
    }

    /** Draw this object
     */
    public override void draw(IGraphics graphics) {
      // This will force the hotspot to be computed if it isn't
      FinalPoint hotspot = getScreenHotspot();

      if (_shape == Shape.G_CLEF) {
        if (_tags.smallSize())
          _gClefSmall.draw(graphics, hotspot);
        else
          _gClef.draw(graphics, hotspot);
      } else if (_shape == Shape.F_CLEF) {
        if (_tags.smallSize()) {
          _fClefSmall.draw(graphics, hotspot);
          _fClefDot1.draw(graphics, hotspot.x, hotspot.y + 1);
          _fClefDot2.draw(graphics, hotspot);
        } else {
          _fClef.draw(graphics, hotspot);
          _fClefDot1.draw(graphics, hotspot);
          _fClefDot2.draw(graphics, hotspot);
        }
      }
      // Debug: must add other shapes
    }

    public override string ToString() {
      return "Clef, shape=" + _shape + ", staff step=" + _staffStep +
         ", octave=" + _octaveNumber + _tags;
    }

    private static DrawShape _gClef, _fClef, _fClefDot1, _fClefDot2;
    private static DrawShape _gClefSmall, _fClefSmall;
    // (This is automatically called to initialize static fields.)
    static Clef() {
      {
        var points = new int[]
            {-2,8, 0,8, 0,9, -3,9, -3,10, -1,10, -3,10, -3,11, -2,12, 1,12, 3,10,
                3,5, -1,-12, -1,-19, 0,-18, 0,-21, 1,-21, 1,-23, 2,-23, 2,-21, 3,-21,
                3,-16, 2,-15, 2,-12, 1,-15, 1,-11, 0,-11, 0,-13, -1,-12, -1,-10,
                -2,-11, -2,-9, -3,-10, -3,-8, -4,-9, -4,-7, -5,-7, -5,-5, -6,-5,
                -6,0, -5,0, -5,2, -4,2, -4,3, -3,3, -2,4, 2,4, 5,1, 5,-3, 4,-2, 4,-4,
            -2,-4, 3,-3, -3,-3, -2,-2, -3,-2, -3,0, -1,2, -3,0};
        _gClef = new DrawShape(points);
      }

      {
        var points = new int[]
            {-3,1, -5,1, -5,0, -3,0, -3,-1, -6,-1, -6,-2, -4,-4, 0,-4, 0,-3, 2,-3,
                1,-2, 3,-2, 3,2, 2,-2, 2,4, 1,3, 1,5, 0,6, -1,6, -1,7, -2,7, -4,9,
            -5,9, -4,9};
        _fClef = new DrawShape(points);
      }
      {
        var points = new int[] { 6, -3, 7, -3, 7, -2, 6, -2, 6, -3 };
        _fClefDot1 = new DrawShape(points);
      }
      {
        var points = new int[] { 6, 2, 7, 2, 7, 3, 6, 3, 6, 2 };
        _fClefDot2 = new DrawShape(points);
      }
      {
        var points = new int[]
          {-1,8, -3,8, -3,9, -2,9, -2,10, 0,10, 1,9, 1,4, 0,3, 0,-1, -1,-1,
           -1,-15, 0,-16, 1,-15, 1,-9, 0,-9, 0,-7, -2,-6, -3,-4, -4,-4, -4,-3,
           -5,-2, -5,1, -4,2, -4,3, -3,3, -3,4, -1,4, -1,5, 0,5, 3,2, 3,-1,
           2,-1, 2,-2, 0,-2, 1,-3, 0,-3, -2,-1, -2,2, -1,3, -1,4};
        _gClefSmall = new DrawShape(points);
      }
      {
        var points = new int[]
            {0,1, -1,1, 0,0, -1,0, -1,-1, 0,-2, 3,-2, 3,-1, 4,-1, 5,0, 5,2, 4,0,
            4,4, 3,4, 2,6, 1,6, 0,7, -1,8, -2,8};
        _fClefSmall = new DrawShape(points);
      }
    }

    private Shape _shape;
    private int _staffStep;
    private OctaveNumber _octaveNumber;

    private FinalPoint _screenHotspot;
  }
}
