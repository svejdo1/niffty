/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffPartsList provides static methods for encoding/decoding a
   * PartsList using RIFF.
   *
   * @author  default
   */
  public class RiffPartsList {
    static readonly String RIFF_ID = "prts";

    /** Suppress the default constructor.
     */
    private RiffPartsList() {
    }

    /** Creates new PartsList from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public PartsList newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, "LIST");
      riffInput.requireFOURCC(RIFF_ID);

      // Debug. Just skip remaining for now.
      riffInput.skipRemaining();
      return new PartsList();
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF parts list, return a new PartsList.  Otherwise,
     * return null and leave the input stream unchanged.
     */
    static public PartsList maybeNew(Riff parentInput) {
      if (!parentInput.peekListID().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}
