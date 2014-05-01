/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {

  /** A TimeSlice has a type, start time and optional tags.  The type is EVENT
   * since all time slices of type MEASURE_START are handled by MeasureStartTimeSlice.
   * A TimeSlice also stores the music symbols following the
   * time slice.  
   * Strictly speaking, this is slightly different
   * than the NIFF spec where all the time slice and music symbol chunks are
   * on the same "level" in the Staff, but formally dividing up music symbols into
   * their EVENT time slice, and the EVENT time slices into their
   * MEASURE_START time slice makes the data easier to process.
   *
   * @author  user
   * @see MeasureStartTimeSlice
   */
  public class TimeSlice : HeirarchyNode, Anchored {
    /** Creates a new TimeSlice with the given start time and
     * tags, and an empty MusicSymbol list.
     * The type is EVENT.
     * To create a time slice of type MEASURE_START, see MeasureStartTimeSlice
     * 
     * @param startTime  see getStartTime()
     * @param tags  the tags for this time slice.  If this is null,
     *          then this uses an empty Tags object.
     */
    public TimeSlice(Rational startTime, Tags tags) {
      if (tags == null)
        tags = new Tags();
      _startTime = startTime;
      _tags = tags;
    }

    /** Add the given mysicSymbol to the MusicSymbol list.  
     * This does not call invalidate(), but you may need to before displaying.
     *
     * @param mysicSymbol  the MusicSymbol to add.  It is an error if
     *        this is already the child of an object.
     * @throws HeirarchyException if the mysicSymbol has already
     *      been added as a child to another object.
     * @see #invalidate
     */
    public void addMusicSymbol(MusicSymbol mysicSymbol) {
      addChild(mysicSymbol);
    }

    /** Return the parent MeasureStartTimeSlice.
     */
    public MeasureStartTimeSlice getParentMeasureStart() {
      return (MeasureStartTimeSlice)getParentNode();
    }

    /** Return the ultimate Score object of which this is a child.
     */
    public Score getParentScore() {
      return getParentMeasureStart().getParentScore();
    }

    /** Returns the Tags object containing the optional tags.
     */
    public Tags getTags() {
      return _tags;
    }

    /** Returns the start time Rational.  Since this is an EVENT time slice, 
     * this is the time since the beginning of the measure as given by
     * the parent MeasureStartTimeSlice.
     *
     * @see #getParentMeasureStart
     */
    public Rational getStartTime() {
      return _startTime;
    }

    /** Return the number of music symbols in the music symbol list.
     */
    public int getMusicSymbolCount() {
      return getChildCount();
    }

    /** Return the music symbol at the given index.
     * The actual object returned is a subclass of MusicSymbol
     * such as Notehead, etc.  Use is to determine the type.
     *
     * @throws ArrayIndexOutOfBoundsException if the index is negative or not
     *    less than the number of nodes in the child node list.
     */
    public MusicSymbol getMusicSymbol(int index) {
      return (MusicSymbol)getChild(index);
    }

    /** This is automatically called after the object is modified to force
     *  this and all child objects to recompute their values when the "get"
     *    method is called for the value.
     */
    public void invalidate() {
      for (int i = 0; i < getMusicSymbolCount(); ++i)
        getMusicSymbol(i).invalidate();

      _screenHotspot = null;
    }

    /** Get the hotspot for this object in screen coordinates.
     */
    public FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      //        // Compute the position by locating our start time between the start time
      //        //   of the measure start and the start time of the next measure start.
      //        // DEBUG: This all assumes that the measures in each staff of the staff
      //        //   system have the same beats in each measure.  Maybe check this.
      //        // DEBUG: must compute firstNoteOffset for real!
      //        int firstNoteOffset = 10;
      //        double pixelsPerDuration =
      //          (getParent().getWidth() - firstNoteOffset) / getParent().getDuration().doubleValue();
      //        _screenHotspot = new FinalPoint
      //          (getParent().getScreenHotspot(),
      //           firstNoteOffset + (int)(_startTime.doubleValue() * pixelsPerDuration),
      //           0);
      int position = getParentMeasureStart().getSymbolPosition(_startTime);
      int x = 10 + (getParentMeasureStart().getWidth() - 10) * position /
              (getParentMeasureStart().getSymbolCount() - 1);
      _screenHotspot = new FinalPoint
         (getParentMeasureStart().getScreenHotspot(), x, 0);

      return _screenHotspot;
    }

    /** Return the count of music symbols in this time slice for
     * which isLeftPositionedSymbol() is true.
     */
    public int getLeftPositionedSymbolCount() {
      int count = 0;
      for (int i = 0; i < getMusicSymbolCount(); ++i) {
        if (getMusicSymbol(i).isLeftPositionedSymbol())
          ++count;
      }
      return count;
    }

    public override void draw(IGraphics graphics) {
      // Just draw every MusicSymbol
      for (int i = 0; i < getMusicSymbolCount(); ++i)
        getMusicSymbol(i).draw(graphics);
    }

    /** This only returns the info for this time slice, not the following music symbols.
     */
    public override string ToString() {
      return "Time-slice, type=EVENT, start time=" +
      _startTime + _tags;
    }

    /** This prints the time slice including all following music symbols.
     *
     * @param indent    A string such as "  " to print at the beginning of the line
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(String indent, TextWriter output) {
      output.WriteLine(indent + this);
      for (int i = 0; i < getMusicSymbolCount(); ++i)
        output.WriteLine(indent + getMusicSymbol(i));
    }

    private FinalPoint _screenHotspot;
    // This defines whether the value of _measureStart is meaningful.
    private bool _gotMeasureStart;
    private TimeSlice _measureStart;
    private Double _measurePixelsPerTime;

    private Rational _startTime;

    private Tags _tags;
  }
}