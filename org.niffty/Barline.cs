/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A Barline has a type, : to, number of staves and optional 
   * tags: thickness, width and line quality.
   *
   * @author  user
   */
  public class Barline : MusicSymbol {
    /** Creates a new Barline with the given parameters.
     * 
     * @param type  the Type constant.  This must not be null.
     * @param extendsTo  the ExtendsTo constant.  This must not be null.
     * @param numberOfStaves  see getNumberOfStaves()
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Barline(Type type, ExtendsTo extendsTo, int numberOfStaves, Tags tags)
      : base(tags) {
      _type = type;
      _extendsTo = extendsTo;
      _numberOfStaves = numberOfStaves;
    }

    /** A Barline.Type has constants for the legal barline types.
     *  This is a "typesafe enum" where you can use a static members like
     *  a constant and compare with == .
     */
    public enum Type {
      THIN,
      THICK
    }

    /** A Barline.ExtendsTo has constants for the legal "extends to" values.
     *  This is a "typesafe enum" where you can use a static members like
     *  a constant and compare with == .
     */
    public enum ExtendsTo {
      BOTTOM_OF_STAFF,
      NEXT_STAFF,
      BETWEEN_STAVES
    }

    /** Returns the type of barline.
     */
    public Type getType() {
      return _type;
    }
    /** Returns the "extends to" value.
     */
    public ExtendsTo getExtendsTo() {
      return _extendsTo;
    }
    /** Returns the number of staves used if getExtendsTo() is ExtendsTo.BOTTOM_OF_STAFF
     */
    public int getNumberOfStaves() {
      return _numberOfStaves;
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _screenHotspot = null;
      _bottomY = null;
    }

    public override FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      if (getParentTimeSlice().getStartTime().getN() == 0)
        // Usually a Barline is given as the last time slice in a measure,
        //   but this barline is at the time zero time slice in the measure,
        //   so make sure that it appears at the beginning of the whole
        //   measure instead of the place where a note with a time zero
        //   time slice would appear.
        _screenHotspot =
          getParentTimeSlice().getParentMeasureStart().getScreenHotspot();
      else
        // Use the hot spot of the time slice.
        _screenHotspot = getParentTimeSlice().getScreenHotspot();

      return _screenHotspot;
    }

    /** Return the Y screen coordinate of the bottom of the barline.  The
     * top is getScreenHotspot().  Even though this is private, we always
     * call this function to get the value so that invalidate() and caching
     * work properly.
     */
    private int getBottomY() {
      if (_bottomY.HasValue)
        return _bottomY.Value;

      FinalPoint top = getScreenHotspot();

      // Get the default value.
      // Use staffStepOffsetY to get the position at the bottom of the staff.
      int result = top.y + Notehead.staffStepOffsetY(0);

      // Debug: Does not consider extendsTo, etc.
      if (_extendsTo == ExtendsTo.NEXT_STAFF) {
        Staff staff = getParentTimeSlice().getParentMeasureStart().getParentStaff();
        if (staff.getIndex() < staff.getParentSystem().getStaffCount() - 1) {
          // This is not the last staff, so we can extend to the next
          result =
          staff.getParentSystem().getStaff(staff.getIndex() + 1).getScreenHotspot().y;
        }
      }

      _bottomY = result;
      return _bottomY.Value;
    }

    /** Draw this object
     */
    public override void draw(IGraphics graphics) {
      // DEBUG: does not deal with barlines next to each other.
      FinalPoint top = getScreenHotspot();
      int bottomY = getBottomY();

      graphics.DrawLine(top.x, top.y, top.x, bottomY);
      if (_type == Type.THICK) {
        // Thicken by drawing more of the same line to the left
        graphics.DrawLine(top.x - 1, top.y, top.x - 1, bottomY);
        graphics.DrawLine(top.x - 2, top.y, top.x - 2, bottomY);
      }
    }

    public override string ToString() {
      return "Barline, type=" + _type + ", : to=" + _extendsTo +
         ", number of staves=" + _numberOfStaves + _tags;
    }

    private FinalPoint _screenHotspot;
    private int? _bottomY;
    private Type _type;
    private ExtendsTo _extendsTo;
    private int _numberOfStaves;
  }
}