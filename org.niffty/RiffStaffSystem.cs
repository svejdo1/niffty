/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffStaffSystem provides static methods for encoding/decoding a
   * StaffSystem using RIFF.
   *
   * @author  default
   */
  public class RiffStaffSystem {
    static readonly String RIFF_ID = "syst";

    /** Suppress the default constructor.
     */
    private RiffStaffSystem() {
    }

    /** Creates new StaffSystem from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public StaffSystem newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, "LIST");
      riffInput.requireFOURCC(RIFF_ID);

      StaffSystem staffSystem = new StaffSystem
         (RiffStaffSystemHeader.newInstance(riffInput));
      // debug: check for NIFFStaffGrouping

      while (riffInput.getBytesRemaining() > 0) {
        Staff staff;
        if ((staff = RiffStaff.maybeNew(riffInput)) != null)
          // Call addChild() which will set the child's parent to this.
          staffSystem.addStaff(staff);
        // debug: must check for NIFFFontSymbol, etc.
        else
          // Did not recognize the chunk type, so skip
          riffInput.skipChunk();
      }

      return staffSystem;
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF system, return a new StaffSystem.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public StaffSystem maybeNew(Riff parentInput) {
      if (!parentInput.peekListID().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }

  }
}