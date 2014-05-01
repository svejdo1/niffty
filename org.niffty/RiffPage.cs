/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffPage provides static methods for encoding/decoding a
   * Page using RIFF.
   *
   * @author  default
   */
  public class RiffPage {
    static readonly String RIFF_ID = "page";

    /** Suppress the default constructor.
     */
    private RiffPage() {
    }

    /** Creates new Page from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Page newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, "LIST");
      riffInput.requireFOURCC(RIFF_ID);

      Page page = new Page(RiffPageHeader.newInstance(riffInput));

      while (riffInput.getBytesRemaining() > 0) {
        StaffSystem staffSystem;
        if ((staffSystem = RiffStaffSystem.maybeNew(riffInput)) != null)
          page.addSystem(staffSystem);
        // debug: must check for NIFFFontSymbol, etc.
        else
          // Did not recognize the chunk type, so skip
          riffInput.skipChunk();
      }

      return page;
    }
  }
}
