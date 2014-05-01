/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffLyric provides static methods for encoding/decoding a
   * Lyric using RIFF.
   *
   * @author  default
   */
  public class RiffLyric {
    static readonly String RIFF_ID = "lyrc";

    /** Suppress the default constructor.
     */
    private RiffLyric() {
    }

    /** Creates a new Lyric from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Lyric newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      String text = RiffStringTable.decodeString
        (RiffForNiff.getStringTable(parentInput), riffInput.readLONG());
      return new Lyric
        (text, riffInput.readBYTE(), RiffTags.newInstance(riffInput));
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF lyric, return a new Lyric.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Lyric maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}
