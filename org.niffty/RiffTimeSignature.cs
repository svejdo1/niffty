/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffTimeSignature provides static methods for encoding/decoding a
   * TimeSignature using RIFF.
   *
   * @author  default
   */
  public class RiffTimeSignature {
    static readonly String RIFF_ID = "time";

    /** Suppress the default constructor.
     */
    private RiffTimeSignature() {
    }

    /** Creates new TimeSignature from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public TimeSignature newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      return new TimeSignature
        (riffInput.readSIGNEDBYTE(),
         riffInput.readSIGNEDBYTE(),
         RiffTags.newInstance(riffInput));
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF time signature, return a new TimeSignature.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public TimeSignature maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }

  }
}