/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {

  /** A MeasureStartTimeSlice has a start time and optional tags and is always type MEASURE_START.
   * A MeasureStartTimeSlice also stores the EVENT TimeSlice objects following the measure start
   * time slice. 
   * Strictly speaking, this is slightly different
   * than the NIFF spec where all the time slice and music symbol chunks are
   * on the same "level" in the Staff, but formally dividing up music symbols into
   * their EVENT time slice, and the EVENT time slices into their
   * MEASURE_START time slice makes the data easier to process.
   *
   * @see TimeSlice
   */
  public class MeasureStartTimeSlice : HeirarchyNode, Anchored {
    /** Creates a new MeasureStartTimeSlice with the given start time and
     * tags, and an empty TimeSlice list.
     * The type is MEASURE_START.
     * 
     * @param startTime  see getStartTime()
     * @param tags  the tags for this time slice.  If this is null,
     *          then this uses an empty Tags object.
     */
    public MeasureStartTimeSlice(Rational startTime, Tags tags) {
      if (tags == null)
        tags = new Tags();
      _startTime = startTime;
      _tags = tags;
    }

    /** Add the given timeslice to the TimeSlice list.  
     * This does not call invalidate(), but you may need to before displaying.
     *
     * @param timeSlice  the TimeSlice to add.  It is an error if
     *        this is already the child of an object.
     * @throws HeirarchyException if the timeSlice has already
     *      been added as a child to another object.
     * @see #invalidate
     */
    public void addTimeSlice(TimeSlice timeSlice) {
      addChild(timeSlice);
    }

    /** Return the parent Staff.
     */
    public Staff getParentStaff() {
      return (Staff)getParentNode();
    }

    /** Return the ultimate Score object of which this is a child.
     */
    public Score getParentScore() {
      return getParentStaff().getParentScore();
    }

    /** Returns the Tags object containing the optional tags.
     */
    public Tags getTags() {
      return _tags;
    }

    /** Returns the start time Rational.  Since this is a measure start
     * time slice, this is the time since the beginning of the score.
     */
    public Rational getStartTime() {
      return _startTime;
    }

    /** Return the number of event time slices in the time slice list.
     */
    public int getTimeSliceCount() {
      return getChildCount();
    }

    /** Return the event time slice at the given index.
     *
     * @throws ArrayIndexOutOfBoundsException if the index is negative or not
     *    less than the number of nodes in the child node list.
     */
    public TimeSlice getTimeSlice(int index) {
      return (TimeSlice)getChild(index);
    }

    /** This is automatically called after the object is modified to force
     *  this and all child objects to recompute their values when the "get"
     *    method is called for the value.
     */
    public void invalidate() {
      for (int i = 0; i < getTimeSliceCount(); ++i)
        getTimeSlice(i).invalidate();

      _screenHotspot = null;
      _duration = null;
      _width = null;
      _symbolPositioner = null;
    }

    /** Return the duration of this measure.  If this MeasureStartTimeSlice has
     * a following MeasureStartTimeSlice in the Staff, the duration is the start time
     * difference of this measure start and the following measure start.
     * If no following measure start, the duration of this measure is
     * the start time of the last event TimeSlice.  Note that this assumes
     * the last event TimeSlice has zero duration, but this is reasonable
     * since the last event TimeSlice usually just holds a bar line.
     */
    public Rational getDuration() {
      if (_duration != null)
        return _duration;

      if ((getIndex() + 1) >= getParentStaff().getMeasureStartCount()) {
        // This is the last measure start
        if (getTimeSliceCount() == 0)
          // No time slices, just set duration to zero
          _duration = new Rational(0, 1);
        else
          // DEBUG: This assumes the event time slices are ordered. Maybe check this.
          _duration = getTimeSlice(getTimeSliceCount() - 1).getStartTime();
      } else {
        // Compute the start time difference between the next measure and this
        // DEBUG: This assumes that the measure start time slices are ordered. Maybe check this.
        IntRatio result = new IntRatio
          (getParentStaff().getMeasureStart(getIndex() + 1).getStartTime());
        result.sub(getStartTime());
        _duration = new Rational(result);
      }

      return _duration;
    }

    /** Return the width of this measure in screen pixels.  
     * If this MeasureStartTimeSlice has
     * a following MeasureStartTimeSlice in the Staff, the width is the screen hotspot X
     * difference of this measure start and the following measure start.
     * If no following measure start, the width of this measure is the X coordinate difference
     * between the end of the staff and this screen hotspot.  
     */
    public int getWidth() {
      if (_width.HasValue)
        return _width.Value;

      if ((getIndex() + 1) >= getParentStaff().getMeasureStartCount()) {
        // This is the last measure start
        int stavesEndX = getParentStaff().getParentSystem().getScreenHotspot().x +
          getParentStaff().getParentSystem().getWidth();
        _width = stavesEndX - getScreenHotspot().x;
      } else {
        // Compute the hotspot X difference between the next measure and this
        // DEBUG: This assumes that the measure start time slices are ordered. Maybe check this.
        int nextMeasureStartX =
          getParentStaff().getMeasureStart(getIndex() + 1).getScreenHotspot().x;
        _width = nextMeasureStartX - getScreenHotspot().x;
      }

      return _width.Value;
    }

    public int getSymbolCount() {
      if (_symbolPositioner != null)
        return _symbolPositioner.getSymbolCount();

      // Create a new SymbolPositioner and add a position at each quarter note.
      _symbolPositioner = new SymbolPositioner();
      Rational quarter = new Rational(1, 4);
      for (IntRatio r = new IntRatio(0, 1);
           r.compareTo(getDuration()) < 0;
           r.add(quarter)) {
        _symbolPositioner.add(new Rational(r), 0);
      }

      // Go through the entire staff system and for every measure which
      //   has the same start time and duration as this, set its
      //   SymbolPositioner to the new one and call its addToSymbolPositioner
      StaffSystem system = getParentStaff().getParentSystem();
      for (int staffIndex = 0; staffIndex < system.getStaffCount(); ++staffIndex) {
        Staff staff = system.getStaff(staffIndex);
        for (int measureIndex = 0;
             measureIndex < staff.getMeasureStartCount();
             ++measureIndex) {
          MeasureStartTimeSlice measure = staff.getMeasureStart(measureIndex);
          if (measure.getStartTime().Equals(getStartTime()) &&
              measure.getDuration().Equals(getDuration())) {
            // This is an equivalent measure to this one in another
            //   staff (or it is this same measure.
            measure._symbolPositioner = _symbolPositioner;
            measure.addToSymbolPositioner();

            // There should not be any more measures in this staff
            //   with the same start time.
            break;
          }
        }
      }

      return _symbolPositioner.getSymbolCount();
    }

    /** Assume that _symbolPositioner has already been set.
     * Add all the time slices in this measure to the _symbolPositioner.
     * This is called by getSymbolCount() when it is initializing all the
     * similar measures in the staff system.
     */
    private void addToSymbolPositioner() {
      for (int i = 0; i < getTimeSliceCount(); ++i) {
        TimeSlice timeSlice = getTimeSlice(i);

        _symbolPositioner.add
          (timeSlice.getStartTime(), timeSlice.getLeftPositionedSymbolCount());
      }
    }

    public int getSymbolPosition(Rational startTime) {
      // This will compute _symbolPositioner if it is not already.
      getSymbolCount();

      return _symbolPositioner.getSymbolPosition(startTime);
    }

    /** Get the hotspot for this object in screen coordinates.
     */
    public FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      // Compute the position by locating the start time within the staff duration.
      // DEBUG: This all assumes that the measures in each staff of the staff
      //   system have the same number of measures.  Maybe check this.
      StaffSystem parentSystem = getParentStaff().getParentSystem();
      _screenHotspot = new FinalPoint
         (getParentStaff().getScreenHotspot(),
          (int)((_startTime.doubleValue() - parentSystem.getStartTime().doubleValue())
                 * parentSystem.getWidth() /
                parentSystem.getDuration().doubleValue()), 0);

      return _screenHotspot;
    }

    public override void draw(IGraphics graphics) {
      // Just draw every TimeSlice
      for (int i = 0; i < getTimeSliceCount(); ++i)
        getTimeSlice(i).draw(graphics);
    }

    /** This only returns the info for this time slice, not the following music symbols.
     */
    public override string ToString() {
      return "Time-slice, type=MEASURE_START, start time=" +
      _startTime + _tags;
    }

    /** This prints the time slice including all following music symbols.
     *
     * @param indent    A string such as "  " to print at the beginning of the line
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(String indent, TextWriter output) {
      output.WriteLine(indent + this);
      for (int i = 0; i < getTimeSliceCount(); ++i) {
        getTimeSlice(i).print(indent + " ", output);
        output.WriteLine("");
      }
    }

    private FinalPoint _screenHotspot;
    private Rational _duration;
    private int? _width;
    private SymbolPositioner _symbolPositioner;

    private Rational _startTime;

    private Tags _tags;
  }
}