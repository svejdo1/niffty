/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffScoreSetup provides static methods for encoding/decoding a
   * ScoreSetup using RIFF.
   *
   * @author  default
   */
  public class RiffScoreSetup {
    static readonly String RIFF_ID = "setp";

    /** Suppress the default constructor.
     */
    private RiffScoreSetup() {
    }

    /** Creates new ScoreSetup from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public ScoreSetup newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, "LIST");
      riffInput.requireFOURCC(RIFF_ID);

      NiffInfo niffInfo = null;
      ChunkLengthTable chunkLengthTable = null;
      PartsList partsList = null;

      while (riffInput.getBytesRemaining() > 0) {
        Object obj;
        if ((obj = RiffChunkLengthTable.maybeNew(riffInput)) != null)
          chunkLengthTable = (ChunkLengthTable)obj;
        else if ((obj = RiffNiffInfo.maybeNew(riffInput)) != null)
          niffInfo = (NiffInfo)obj;
        else if ((obj = RiffPartsList.maybeNew(riffInput)) != null)
          partsList = (PartsList)obj;
        else if (RiffStringTable.maybeDecode(riffInput)) {
          // Do nothing.  The String Table was stored for later use.
        }
          // debug: check for other optional chunks
        else
          // Did not recognize the chunk type, so skip
          riffInput.skipChunk();
      }

      // Make sure required chunks are present.
      if (niffInfo == null)
        throw new RiffFormatException("Can't find NIFF info chunk in Setup section.");
      if (chunkLengthTable == null)
        throw new RiffFormatException("Can't find chunk length table in Setup section.");
      if (partsList == null)
        throw new RiffFormatException("Can't find parts list chunk in Setup section.");

      return new ScoreSetup(niffInfo, chunkLengthTable, partsList);
    }

  }
}
