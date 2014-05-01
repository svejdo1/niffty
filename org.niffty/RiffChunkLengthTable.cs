/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffChunkLengthTable provides static methods for encoding/decoding a
   * ChunkLengthTable using RIFF.
   *
   * @author  default
   */
  public class RiffChunkLengthTable {
    static readonly String RIFF_ID = "clt ";

    /** Suppress the default constructor.
     */
    private RiffChunkLengthTable() {
    }

    /** Creates new ChunkLengthTable from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public ChunkLengthTable newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      // Debug. Just skip remaining for now.
      riffInput.skipRemaining();

      return new ChunkLengthTable();
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF chunk length table, return a new ChunkLengthTable.  Otherwise,
     * return null and leave the input stream unchanged.
     */
    static public ChunkLengthTable maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}
