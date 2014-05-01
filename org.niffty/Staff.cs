/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {

  /** A Staff encapsulates the staff information of a StaffSystem.
   * A Staff has a StaffHeader, and any number of
   * MeasureStartTimeSlice objects. All event time slices and music symbols in 
   * the Staff are within their respective MeasureStartTimeSlice.
   * Strictly speaking, this is slightly different
   * than the NIFF spec where all the time slice and music symbol chunks are
   * on the same "level", but formally dividing up music symbols into
   * their EVENT time slice, and the EVENT time slices into their
   * MEASURE_START time slice makes the data easier to process.
   *
   * @author  user
   */
  public class Staff : HeirarchyNode, Anchored {
    /** Creates a new Staff with the given StaffHeader and an 
     *  empty MeasureStartTimeSlice list.
     *  The parent pointer will be set when this is added to a StaffSystem.
     *
     * @param staffHeader the header for this Staff
     */
    public Staff(StaffHeader staffHeader) {
      _staffHeader = staffHeader;
    }

    /** Add the given measure start timeslice to the MeasureStartTimeSlice list.  
     * This does not call invalidate(), but you may need to before displaying.
     *
     * @param measureStart  the MeasureStartTimeSlice to add.  It is an error if
     *        this is already the child of an object.
     * @throws HeirarchyException if the measureStart has already
     *      been added as a child to another object.
     * @see #invalidate
     */
    public void addMeasureStart(MeasureStartTimeSlice measureStart) {
      addChild(measureStart);
    }

    /** Return the parent StaffSystem.
     */
    public StaffSystem getParentSystem() {
      return (StaffSystem)getParentNode();
    }

    public StaffHeader getStaffHeader() {
      return _staffHeader;
    }

    /** Return the number of measure start time slices in the 
     * measure start time slice list.
     * All event time slices and music symbols in this Staff are contained within their
     * respective MeasureStartTimeSlice object.
     */
    public int getMeasureStartCount() {
      return getChildCount();
    }

    /** Return the MeasureStartTimeSlice in the measure start time slice list at 
     * the given index.
     * All event time slices and music symbols in this Staff are contained within their
     * respective MeasureStartTimeSlice object.
     *
     * @throws ArrayIndexOutOfBoundsException if the index is negative or not 
     *    less than the number of nodes in the child node list.
     */
    public MeasureStartTimeSlice getMeasureStart(int index) {
      return (MeasureStartTimeSlice)getChild(index);
    }

    /** This is automatically called after the object is modified to force
     *  this and all child objects to recompute their values when the "get"
     *    method is called for the value.
     */
    public void invalidate() {
      for (int i = 0; i < getMeasureStartCount(); ++i)
        getMeasureStart(i).invalidate();

      _screenHotspot = null;
    }

    public FinalPoint getScreenHotspot() {
      // debug: should compute this properly.
      if (_screenHotspot != null)
        return _screenHotspot;

      _screenHotspot = new FinalPoint
        (getParentSystem().getScreenHotspot(), 0,
         (int)(getIndex() * getParentSystem().getParentPage().getStaffSpacingY()));
      return _screenHotspot;
    }

    /** Return the ultimate Score object of which this is a child.
     */
    public Score getParentScore() {
      return getParentSystem().getParentScore();
    }

    public override void draw(IGraphics graphics) {
      // This will force the hotspot to be computed if it isn't
      FinalPoint hotspot = getScreenHotspot();
      int width = getParentSystem().getWidth();

      // Draw the staff lines
      for (int y = 0; y <= 20; y += 5)
        graphics.DrawLine(hotspot.x, hotspot.y + y,
          hotspot.x + width, hotspot.y + y);

      // Draw all time slices.
      for (int i = 0; i < getMeasureStartCount(); ++i)
        getMeasureStart(i).draw(graphics);
    }

    /** This prints the staff including all time slices.
     *
     * @param indent    A string such as "  " to print at the beginning of the line
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(String indent, TextWriter output) {
      output.WriteLine(indent + "Staff");
      output.WriteLine(indent + " " + _staffHeader);

      for (int i = 0; i < getMeasureStartCount(); ++i)
        getMeasureStart(i).print(indent + " ", output);
    }

    private StaffHeader _staffHeader;
    private FinalPoint _screenHotspot;
  }
}