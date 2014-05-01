/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffAccidental provides static methods for encoding/decoding an
   * Accidental using RIFF.
   *
   * @author  default
   */
  public class RiffAccidental {
    static readonly String RIFF_ID = "acdl";

    /** Suppress the default constructor.
     */
    private RiffAccidental() {
    }

    /** Creates new Accidental from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Accidental newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      return new Accidental
        (convertShape(riffInput.readBYTE()),
         RiffTags.newInstance(riffInput));
    }

    /** Return the Accidental.Shape for the equivalent RIFF token.
     */
    static Accidental.Shape convertShape(int token) {
      if (token == 1)
        return Accidental.Shape.DOUBLE_FLAT;
      else if (token == 2)
        return Accidental.Shape.FLAT;
      else if (token == 3)
        return Accidental.Shape.NATURAL;
      else if (token == 4)
        return Accidental.Shape.SHARP;
      else if (token == 5)
        return Accidental.Shape.DOUBLE_SHARP;
      else if (token == 6)
        return Accidental.Shape.QUARTER_TONE_FLAT;
      else if (token == 7)
        return Accidental.Shape.THREE_QUARTER_TONES_FLAT;
      else if (token == 8)
        return Accidental.Shape.QUARTER_TONE_SHARP;
      else if (token == 9)
        return Accidental.Shape.THREE_QUARTER_TONES_SHARP;
      else
        throw new RiffFormatException("Illegal value for Accidental shape");
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF accidental, return a new Accidental.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Accidental maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}
