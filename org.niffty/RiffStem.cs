/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffStem provides static methods for encoding/decoding a
   * Stem using RIFF.
   *
   * @author  default
   */
  public class RiffStem {
    static readonly String RIFF_ID = "stem";

    /** Suppress the default constructor.
     */
    private RiffStem() {
    }

    /** Creates new Stem from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Stem newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      // empty required part
      return new Stem(RiffTags.newInstance(riffInput));
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF stem, return a new Stem.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Stem maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }

  }
}
