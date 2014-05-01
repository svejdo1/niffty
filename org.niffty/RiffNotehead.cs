/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffNotehead provides static methods for encoding/decoding a
   * Notehead using RIFF.
   *
   * @author  default
   */
  public class RiffNotehead {
    static readonly String RIFF_ID = "note";

    /** Suppress the default constructor.
     */
    private RiffNotehead() {
    }

    /** Creates new Notehead from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Notehead newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      return new Notehead
        (convertShape(riffInput.readBYTE()),
         riffInput.readSIGNEDBYTE(),
         new Rational(riffInput.readSHORT(), riffInput.readSHORT()),
         RiffTags.newInstance(riffInput));
    }

    /** Return the Notehead.Shape for the equivalent RIFF token.
     */
    static Notehead.Shape convertShape(int token) {
      if (token == 1)
        return Notehead.Shape.BREVE;
      else if (token == 2)
        return Notehead.Shape.WHOLE;
      else if (token == 3)
        return Notehead.Shape.HALF;
      else if (token == 4)
        return Notehead.Shape.FILLED;
      else if (token == 5)
        return Notehead.Shape.OPEN_DIAMOND;
      else if (token == 6)
        return Notehead.Shape.SOLID_DIAMOND;
      else if (token == 7)
        return Notehead.Shape.X_NOTEHEAD;
      else if (token == 8)
        return Notehead.Shape.OPEN_X_NOTEHEAD;
      else if (token == 9)
        return Notehead.Shape.FILLED_GUITAR_SLASH;
      else if (token == 10)
        return Notehead.Shape.OPEN_GUITAR_SLASH;
      else if (token == 11)
        return Notehead.Shape.FILLED_SQUARE;
      else if (token == 12)
        return Notehead.Shape.OPEN_SQUARE;
      else if (token == 13)
        return Notehead.Shape.FILLED_TRIANGLE;
      else if (token == 14)
        return Notehead.Shape.OPEN_TRIANGLE;
      else
        throw new RiffFormatException("Illegal value for Notehead shape");
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF notehead, return a new Notehead.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Notehead maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}
