/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A Notehead has a shape, staff step, and duration and optional
   * tags: logical or absolute placement, part ID, voice ID, MIDI performance,
   * grace note, cue note, small size, large size, invisible, split stem,
   * silent
   *
   * @author  user
   */
  public class Notehead : MusicSymbol {
    static readonly String RIFF_ID = "note";

    /** Creates a new Notehead with the given parameters.
     * 
     * @param shape  the Shape constant.  This must not be null.
     * @param staffStep  see getStaffStep()
     * @param duration  see getDuration()
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Notehead(Shape shape, int staffStep, Rational duration, Tags tags)
      : base(tags) {
      _shape = shape;
      _staffStep = staffStep;
      _duration = duration;
    }

    /** A Notehead.Shape has constants for the legal shapes.
     *  This is a "typesafe enum" where you can use a static members like
     *  a constant and compare with == .
     */
    public enum Shape {
      BREVE,
      WHOLE,
      HALF,
      FILLED,
      OPEN_DIAMOND,
      SOLID_DIAMOND,
      X_NOTEHEAD,
      OPEN_X_NOTEHEAD,
      FILLED_GUITAR_SLASH,
      OPEN_GUITAR_SLASH,
      FILLED_SQUARE,
      OPEN_SQUARE,
      FILLED_TRIANGLE,
      OPEN_TRIANGLE
    }

    /** Returns the shape.
     */
    public Shape getShape() {
      return _shape;
    }
    /** Returns the staff step for the notehead.  The bottom line is 0,
     *    1 is the space above it, 2 is the line above that, etc.
     */
    public int getStaffStep() {
      return _staffStep;
    }
    /** Returns the duration RATIONAL
     */
    public Rational getDuration() {
      return _duration;
    }

    /** Convert a staff step to vertical offset from hotspot at top of staff.
     *
     * @param staffStep 0 is the bottom line,
     *    1 is the space above it, 2 is the line above that, etc.
     * @return  the screen offset from the top line.  Note that while a greater
     *          staff step moves upward on the staff, the return value is in
     *          screen coordinates where a greater value moves down the screen.
     */
    public static int staffStepOffsetY(int staffStep) {
      // Get a value where zero is the top line and it increases going down.
      int staffStepFromTopLine = 8 - staffStep;

      // Distance between lines is 5 screen coordinates and this is 2 staff steps.
      if (staffStepFromTopLine % 2 == 0)
        // We are on a line
        return 5 * (staffStepFromTopLine / 2);
      else
        // We are in a space. We handle this explicitly instead of relying
        //  on integer division to round a fraction in the way we want.
        return 2 + 5 * ((staffStepFromTopLine - 1) / 2);
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _anchor = null;
      _screenHotspot = null;
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

    /** Override the y position to use the value from staffStepOffsetY
     * from the parent time slice's position at the top of the staff.
     */
    public override FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      // override the y position to use the value from staffStepOffsetY
      //   from the parent time slice's position at the top of the staff.
      _screenHotspot = new FinalPoint
      (getAnchor().getScreenHotspot().x,
      getParentTimeSlice().getScreenHotspot().y + staffStepOffsetY(_staffStep));

      return _screenHotspot;
    }

    private DrawShape graphicShape() {
      if (_shape == Shape.WHOLE)
        return _noteheadWhole;
      else if (_shape == Shape.HALF)
        return _noteheadHalf;
      else if (_shape == Shape.FILLED)
        return _noteheadFilled;
      // Debug: must add other shapes
      else
        // unrecognized.  Shouldn't happen, so return an empty shape
        return new DrawShape();
    }

    /** Return the right edge of the shape which is the offset from the screenHotspot().
     */
    public int getRightEdge() {
      return graphicShape().getRightEdge();
    }

    /** Return the left edge of the shape which is the offset from the screenHotspot().
     */
    public int getLeftEdge() {
      return graphicShape().getLeftEdge();
    }

    /** Draw this object
     */
    public override void draw(IGraphics graphics) {
      // Draw the notehead
      graphicShape().draw(graphics, getScreenHotspot());

      // Draw extension to the staff as necessary
      if (_staffStep >= 10) {
        // Notehead is above the staff, so draw staff line extensions.
        FinalPoint staffTop = getAnchor().getScreenHotspot();
        for (int step = 10; step <= _staffStep; step += 2) {
          int y = staffTop.y + staffStepOffsetY(step);
          graphics.DrawLine(staffTop.x - 4, y, staffTop.x + 6, y);
        }
      } else if (_staffStep <= -2) {
        // Notehead is below the staff, so draw staff line extensions.
        FinalPoint staffTop = getAnchor().getScreenHotspot();
        for (int step = -2; step >= _staffStep; step -= 2) {
          int y = staffTop.y + staffStepOffsetY(step);
          graphics.DrawLine(staffTop.x - 4, y, staffTop.x + 6, y);
        }
      }
    }

    public override string ToString() {
      return "Notehead, shape=" + _shape + ", staff step=" + _staffStep +
      ", duration=" + _duration + _tags;
    }

    private static DrawShape _noteheadWhole, _noteheadHalf, _noteheadFilled;
    // (This is automatically called to initialize static fields.)
    static Notehead() {
      {
        var points = new int[] {-1,0, -3,0, -3,-1, -1,-1, -1,-2, 2,-2, 2,-1, 4,-1, 4,0, 2,0, 2,1,
            4,1, 4,2, 2,2, 2,3, -1,3, -1,2, -3,2, -3,1, -1,1, -1,0};
        _noteheadWhole = new DrawShape(points);
      }
      {
        var points = new int[] {-2,2, -2,0, -1,0, -1,-1, 0,-1, 0,-2, 2,-2, 3,-1, 3,1, 2,1, 2,2,
            1,2, 1,3, -1,3, -2,2, -1,3};
        _noteheadHalf = new DrawShape(points);
      }
      {
        var points = new int[] {-1,3, 1,3, 2,2, -2,2, -2,1, 3,1, 3,0, -2,0, -1,-1, 3,-1, 2,-2,
            0,-2, 1,-2};
        _noteheadFilled = new DrawShape(points);
      }
    }

    private Anchored _anchor;
    private FinalPoint _screenHotspot;

    private Shape _shape;
    private int _staffStep;
    private Rational _duration;
  }
}