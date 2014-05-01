/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {

  /** A StaffSystem encapsulates the system information of a Page.
   * A StaffSystem has a StaffSystemHeader, and optional NIFFStaffGrouping,
   * and any number of
   * Staff, NIFFFontSymbol, NIFFCustomGraphicSymbol, NIFFText,
   * or NIFFLine.
   *
   * @author  user
   */
  public class StaffSystem : HeirarchyNode, Anchored {
    /** Creates a new StaffSystem with the given StaffSystemHeader and an 
     *  empty Staff list.
     *  The parent pointer will be set when this is added to a Page.
     *
     * @param systemHeader the header for this StaffSystem
     */
    public StaffSystem(StaffSystemHeader systemHeader) {
      _systemHeader = systemHeader;
    }

    /** Add the given staff to the Staff list.
     * This does not call invalidate(), but you may need to before displaying.
     *
     * @param staff  the Staff to add.  It is an error if
     *        this is already the child of an object.
     * @throws HeirarchyException if the staff has already
     *      been added as a child to another object.
     * @see #invalidate
     */
    public void addStaff(Staff staff) {
      addChild(staff);
    }

    /** Return the parent Page.
     */
    public Page getParentPage() {
      return (Page)getParentNode();
    }

    public StaffSystemHeader getSystemHeader() {
      return _systemHeader;
    }

    /** Return the number of staves in the staff list.
     */
    public int getStaffCount() {
      return getChildCount();
    }

    /** Return the Staff in the staff list at the given index.
     *
     * @throws ArrayIndexOutOfBoundsException if the index is negative or not
     *    less than the number of nodes in the child node list.
     */
    public Staff getStaff(int index) {
      return (Staff)getChild(index);
    }

    /** This is automatically called after the object is modified to force
     *  this and all child objects to recompute their values when the "get"
     *    method is called for the value.
     */
    public void invalidate() {
      for (int i = 0; i < getStaffCount(); ++i)
        getStaff(i).invalidate();

      _screenHotspot = null;
      _startTime = null;
      _duration = null;
    }

    public FinalPoint getScreenHotspot() {
      // debug: should compute this properly relative to the page.
      if (_screenHotspot != null)
        return _screenHotspot;

      // The staff system has a vertical offset from the stavesHotspot
      //   based on how many total staves there are above this system.
      int stavesAbove = 0;
      for (int i = 0; i < getIndex(); ++i)
        stavesAbove += getParentPage().getSystem(i).getStaffCount();

      _screenHotspot = new FinalPoint
        (getParentScore().getStavesScreenHotspot(), 0,
         (int)(stavesAbove * getParentPage().getStaffSpacingY()));
      return _screenHotspot;
    }

    /** Return the global start time of this staff system.  
     * This is the start time of the first measure start time slice in the first
     * staff.
     */
    public Rational getStartTime() {
      if (_startTime != null)
        return _startTime;

      if (getStaffCount() == 0 || getStaff(0).getMeasureStartCount() == 0) {
        // There are no staffs or time slices!  This is unexpected.  Just return 0.
        // DEBUG:  maybe this should be handled more gracefully.
        _startTime = new Rational(0, 1);
        return _startTime;
      }

      _startTime = getStaff(0).getMeasureStart(0).getStartTime();
      return _startTime;
    }

    /** Return the duration of this staff system.  This is the start time
     * difference between the first and last measure start time slices in the
     * first staff plus the duration of the last measure.  (It is assumed that all 
     * staves in the staff system have the same duration.)  
     * This is used to compute the screen hotspots of the measure starts.
     */
    public Rational getDuration() {
      if (_duration != null)
        return _duration;

      if (getStaffCount() == 0 || getStaff(0).getMeasureStartCount() == 0) {
        // There are no staffs or time slices!  This is unexpected.  Just return 0.
        // DEBUG:  maybe this should be handled more gracefully.
        _duration = new Rational(0, 1);
        return _duration;
      }
      Staff staff = getStaff(0);

      MeasureStartTimeSlice lastMeasureStart =
        staff.getMeasureStart(staff.getMeasureStartCount() - 1);

      IntRatio result = new IntRatio(lastMeasureStart.getStartTime());
      result.sub(getStartTime());
      result.add(lastMeasureStart.getDuration());

      _duration = new Rational(result);
      return _duration;
    }

    /** Return the width of this staff system.  Due to staff indentation,
     * this may be different than getParentScore().getStavesWidth().
     */
    public int getWidth() {
      // DEBUG: may want to indent the first staff system
      return getParentScore().getStavesWidth();
    }

    /** Return the ultimate Score object of which this is a child.
     */
    public Score getParentScore() {
      return getParentPage().getParentScore();
    }

    public override void draw(IGraphics graphics) {
      if (getStaffCount() == 0)
        // Nothing to draw
        return;

      // Debug: should check for multistaff barline tags
      FinalPoint top = getScreenHotspot();
      int bottomY =
        getStaff(getStaffCount() - 1).getScreenHotspot().y +
        Notehead.staffStepOffsetY(0);
      graphics.DrawLine(top.x, top.y, top.x, bottomY);

      for (int i = 0; i < getStaffCount(); ++i)
        getStaff(i).draw(graphics);
    }

    /** This prints the staff system including all staves.
     *
     * @param indent    A string such as "  " to print at the beginning of the line
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(String indent, TextWriter output) {
      output.WriteLine(indent + "System");
      output.WriteLine(indent + " " + _systemHeader);

      for (int i = 0; i < getStaffCount(); ++i)
        getStaff(i).print(indent + " ", output);
    }

    private StaffSystemHeader _systemHeader;
    private FinalPoint _screenHotspot;
    private Rational _startTime;
    private Rational _duration;
  }
}