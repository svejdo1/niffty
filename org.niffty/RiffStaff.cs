/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffStaff provides static methods for encoding/decoding a
   * Staff using RIFF.
   *
   * @author  default
   */
  public class RiffStaff {
    static readonly String RIFF_ID = "staf";

    /** Suppress the default constructor.
     */
    private RiffStaff() {
    }

    /** Creates new Staff from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     * This also reads all the enclosed MeasureStartTimeSlice objects
     * which themselves contain the event TimeSlice and MusicSymbol objects.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Staff newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, "LIST");
      riffInput.requireFOURCC(RIFF_ID);

      Staff staff = new Staff(RiffStaffHeader.newInstance(riffInput));

      if (riffInput.getBytesRemaining() <= 0)
        // the staff is empty
        return staff;

      MeasureStartTimeSlice measureStart =
         RiffMeasureStartTimeSlice.maybeNew(riffInput);
      if (measureStart == null) {
        // For some reason, the first chunk in this staff is not a
        //   measure start time slice.  Create a fake one based 
        //   on the previous staff system's end time
        // DEBUG: must implement this!  For now use start time of 0/1
        measureStart = new MeasureStartTimeSlice(new Rational(0, 1), null);
      }

      while (true) {
        if (staff.getMeasureStartCount() > 0 &&
            staff.getMeasureStart
              (staff.getMeasureStartCount() - 1).getStartTime().Equals
            (measureStart.getStartTime()))
          // This new measure start time is the same as the last one,
          // so just continue using the last one.
          // DEBUG: this still doesn't ensure increasing order
          // DEBUG: This discards the tags in the newly read measure start
          measureStart =
            staff.getMeasureStart(staff.getMeasureStartCount() - 1);
        else
          staff.addMeasureStart(measureStart);

        // Read event time slices until the next measure start time slice or 
        //    end of the staff
        RiffMeasureStartTimeSlice.addTimeSlices(riffInput, measureStart);
        if (riffInput.getBytesRemaining() <= 0)
          // reached the end of the staff
          break;

        // The previous call to readTimeSlices has already 
        //   ensured that the next chunk is a measure start time slice.
        measureStart = RiffMeasureStartTimeSlice.newInstance(riffInput);
      }

      return staff;
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF staff, return a new Staff.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Staff maybeNew(Riff parentInput) {
      if (!parentInput.peekListID().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }

  }
}