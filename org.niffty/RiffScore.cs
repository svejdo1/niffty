/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {



  /** A RiffScore provides static methods for encoding/decoding a
   * Score using RIFF.
   *
   * @author  default
   */
  public class RiffScore {
    static readonly String RIFF_ID = "NIFF";

    /** Suppress the default constructor.
     */
    private RiffScore() {
    }

    /** Creates new Score object from the input stream.
     * This uses the InputStream to initialize a RIFF object
     * which is used to read the input as a RIFF file.
     *
     * @param input     the InputStream containing the NIFF info
     */
    static public Score newInstance(Stream input) {
      Riff riffInput = new RiffForNiff(input, RIFF_ID);

      return new Score
         (RiffScoreSetup.newInstance(riffInput),
          RiffScoreData.newInstance(riffInput));
    }

  }
}
