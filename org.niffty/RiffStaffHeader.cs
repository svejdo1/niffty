/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffStaffHeader provides static methods for encoding/decoding a
   * StaffHeader using RIFF.
   *
   * @author  default
   */
  public class RiffStaffHeader {
    static readonly String RIFF_ID = "sthd";

    /** Suppress the default constructor.
     */
    private RiffStaffHeader() {
    }

    /** Creates new StaffHeader from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public StaffHeader newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      // empty required part
      return new StaffHeader(RiffTags.newInstance(riffInput));
    }
  }
}
