/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffScoreData provides static methods for encoding/decoding a
   * ScoreData using RIFF.
   *
   * @author  default
   */
  public class RiffScoreData {
    static readonly String RIFF_ID = "data";

    /** Suppress the default constructor.
     */
    private RiffScoreData() {
    }

    /** Creates new ScoreData from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public ScoreData newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, "LIST");
      riffInput.requireFOURCC(RIFF_ID);

      ScoreData scoreData = new ScoreData();

      while (riffInput.getBytesRemaining() > 0) {
        // Create the child and call addChild() which will set the child's parent
        //   to this.
        scoreData.addPage(RiffPage.newInstance(riffInput));
      }

      return scoreData;
    }
  }
}
