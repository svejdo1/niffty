/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A Rest has a shape, staff step, and duration.
   *
   * @author  user
   */
  public class Rest : MusicSymbol {
    /** Creates a new Rest with the given parameters.
     * 
     * @param shape  the Shape constant.  This must not be null.
     * @param staffStep  see getStaffStep()
     * @param duration  see getDuration()
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Rest(Shape shape, int staffStep, Rational duration, Tags tags)
      : base(tags) {
      _shape = shape;
      _staffStep = staffStep;
      _duration = duration;
    }

    /** A Rest.Shape has constants for the legal shapes.
     *  This is a "typesafe enum" where you can use a static members like
     *  a constant and compare with == .
     */
    public enum Shape {
      BREVE,
      WHOLE,
      HALF,
      QUARTER,
      EIGHTH,
      SIXTEENTH,
      THIRTY_SECOND,
      SIXTY_FOURTH,
      ONE_TWENTY_EIGHTH,
      TWO_FIFTY_SIXTH,
      FOUR_MEASURES,
      MULTIPLE_MEASURE_THICK_HORIZONTAL,
      MULTIPLE_MEASURE_THICK_SLANTED,
      VOCAL_COMMA,
      VOCAL_TWO_SMALL_SLASHES
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

      // DEBUG: should handle multi-measure rests
      // DEBUG: should center half and whole rests
      // override the y position to use the value from staffStepOffsetY
      //   from the parent time slice's position at the top of the staff.
      _screenHotspot = new FinalPoint
        (getAnchor().getScreenHotspot().x,
         getParentTimeSlice().getScreenHotspot().y +
          Notehead.staffStepOffsetY(_staffStep));
      return _screenHotspot;
    }

    private DrawShape graphicShape() {
      if (_shape == Shape.WHOLE)
        return _restWhole;
      if (_shape == Shape.HALF)
        return _restHalf;
      if (_shape == Shape.QUARTER)
        return _restQuarter;
      if (_shape == Shape.EIGHTH)
        return _restEighth;
      if (_shape == Shape.SIXTEENTH)
        return _restSixteenth;
      if (_shape == Shape.THIRTY_SECOND)
        return _restThirtySecond;
      if (_shape == Shape.SIXTY_FOURTH)
        return _restSixtyFourth;
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
    public int lgetLeftEdge() {
      return graphicShape().getLeftEdge();
    }

    public override void draw(IGraphics graphics) {
      if (_tags.invisible())
        return;
      graphicShape().draw(graphics, getScreenHotspot());
    }

    public override string ToString() {
      return "Rest, shape=" + _shape + ", staff step=" + _staffStep +
          ", duration=" + _duration + _tags;
    }

    private Shape _shape;
    private int _staffStep;
    private Rational _duration;

    private static DrawShape _restWhole, _restHalf, _restQuarter, _restEighth,
      _restSixteenth, _restThirtySecond, _restSixtyFourth;
    // (This is automatically called to initialize static fields.)
    static Rest() {
      {
        var points = new int[] { -2, -3, 3, -3, 3, -4, -2, -4, -2, -5, 4, -5 };
        _restWhole = new DrawShape(points);
      }
      {
        var points = new int[] { -2, 0, 3, 0, 3, -1, -2, -1, -2, -2, 4, -2 };
        _restHalf = new DrawShape(points);
      }
      {
        var points = new int[] {0,7, -2,5, -2,3, -1,5, -1,3, 2,3, 1,3, 1,1, 0,1, 0,-1,
                -2,-1, -1,-2, 0,-2, 0,-3, 1,-3, 2,-4, 1,-4, 1,-5, -1,-7, -1,-6};
        _restQuarter = new DrawShape(points);
      }
      {
        var points = new int[] {-2,-3, 0,-3, 0,-2, -2,-2, -1,-1, 1,-1, 2,-2, 2,-3, 2,-2,
                 1,-1, 1,2, 0,3, 0,5};
        _restEighth = new DrawShape(points);
      }
      {
        var points = new int[] {-2,-3, 0,-3, 0,-2, -2,-2, -1,-1, 1,-1, 2,-2, 2,-3, 2,-2,
                 1,-1, 1,1, 0,2, 0,3, -1,4, -2,4, -2,2, -4,2, -4,3, -3,3, -3,4, -1,4,
                 -1,7, -2,8, -2,11};
        _restSixteenth = new DrawShape(points);
      }
      {
        var points = new int[] {4,-8, 3,-8, 3,-6, 1,-6, 1,-8, -1,-8, -1,-7, 0,-7, 0,-6, 
                2,-6, 2,-2, 1,-1, 0,-1, 0,-3, -2,-3, -2,-2, -1,-2, -1,-1, 1,-1, 1,2, 0,2,
                0,4, -2,4, -2,2, -4,2, -4,3, -3,3, -3,4, -1,4, -1,8, -2,9, -2,11};
        _restThirtySecond = new DrawShape(points);
      }
      {
        var points = new int[] {4,-8, 3,-8, 3,-6, 1,-6, 1,-8, -1,-8, -1,-7, 0,-7, 0,-6, 
                2,-6, 2,-2, 1,-1, 0,-1, 0,-3, -2,-3, -2,-2, -1,-2, -1,-1, 1,-1, 1,2, 0,2,
                0,4, -2,4, -2,2, -4,2, -4,3, -3,3, -3,4, -1,4, -1,8, -2,9,
                -3,9, -3,7, -5,7, -5,8, -4,8, -4,9, -2,9, -2,12, -3,13, -3,15};
        _restSixtyFourth = new DrawShape(points);
      }
    }

    private Anchored _anchor;
    private FinalPoint _screenHotspot;
  }
}