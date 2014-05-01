/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffMeasureStartTimeSlice provides static methods for encoding/decoding a
   * MeasureStartTimeSlice using RIFF.
   *
   * @author  default
   */
  public class RiffMeasureStartTimeSlice {
    static readonly String RIFF_ID = "tmsl";

    /** Suppress the default constructor.
     */
    private RiffMeasureStartTimeSlice() {
    }

    /** Creates new MeasureStartTimeSlice from the parentInput's input stream.
     * The next object in the input stream must be a time slice with type MEASURE_START.
     * After creating the MeasureStartTimeSlice, you can call addTimeSlices()
     * to store the event time slices for this measure start time slice.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     * @see #addTimeSlices
     */
    static public MeasureStartTimeSlice newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      int type = riffInput.readBYTE();
      if (type != MEASURE_START)
        throw new RiffFormatException
        ("Expected MEASURE_START for time slice type. Got " + type + ".");

      return new MeasureStartTimeSlice
        (new Rational(riffInput.readSHORT(), riffInput.readSHORT()),
         RiffTags.newInstance(riffInput));
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF time slice with type MEASURE_START, return a new
     * MeasureStartTimeSlice.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public MeasureStartTimeSlice maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;
      // We must also ensure that this is a MEASURE_START time slice
      if (parentInput.peekFirstChunkBYTE() != MEASURE_START)
        return null;

      return newInstance(parentInput);
    }

    /** This reads event TimeSlice chunks from parentInput's input stream until the next
     * measure start time slice or no more bytesRemaining in the parentInput,
     * adding the TimeSlice objects to the measureStart's time slice list.
     * When this reads the TimeSlice, it also adds the subsequent music
     * symbols to it.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     * @param measureStart  the MeasureStartTimeSlice to which TimeSlice
     *      objects are added.
     * @see TimeSlice
     */
    static public void addTimeSlices
       (Riff parentInput, MeasureStartTimeSlice measureStart) {
      if (parentInput.getBytesRemaining() <= 0)
        // This is a measure start time slice with nothing after it in the staff
        return;

      if (parentInput.peekFOURCC().Equals(RIFF_ID) &&
          parentInput.peekFirstChunkBYTE() == MEASURE_START)
        // The next chunk is already the start of a new measure
        return;

      TimeSlice timeSlice = RiffTimeSlice.maybeNew(parentInput);
      if (timeSlice == null)
        // For some reason, the first chunk after the measure start
        //   time slice is not an event time slice.  Create a fake one with
        //   start time of 0/1.
        // DEBUG: should mock up an input file and test this.
        timeSlice = new TimeSlice(new Rational(0, 1), null);

      while (true) {
        if (measureStart.getTimeSliceCount() > 0 &&
            measureStart.getTimeSlice
              (measureStart.getTimeSliceCount() - 1).getStartTime().Equals
            (timeSlice.getStartTime()))
          // This new time slice start time is the same as the last one,
          // so just continue using the last one.
          // DEBUG: this still doesn't ensure increasing order
          // DEBUG: This discards the tags in the newly read time slice
          timeSlice =
            measureStart.getTimeSlice(measureStart.getTimeSliceCount() - 1);
        else
          measureStart.addTimeSlice(timeSlice);

        // Read music symbols until the next time slice or 
        //    end of the staff.
        RiffTimeSlice.addMusicSymbols(parentInput, timeSlice);
        if (parentInput.getBytesRemaining() <= 0)
          // reached the end of the staff
          break;
        // We know the next chunk is a time slice.  Check whether it is
        //    a measure start.
        if (parentInput.peekFirstChunkBYTE() == MEASURE_START)
          // The next chunk is the start of a new measure
          break;

        // We have now ensured that the next chunk is an event time slice.
        timeSlice = RiffTimeSlice.newInstance(parentInput);
      }
    }

    private const int MEASURE_START = 1;
  }
}
