/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffClef provides static methods for encoding/decoding a
   * Clef using RIFF.
   *
   * @author  default
   */
  public class RiffClef {
    static readonly String RIFF_ID = "clef";

    /** Suppress the default constructor.
     */
    private RiffClef() {
    }

    /** Creates new Clef from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Clef newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      return new Clef
         (convertShape(riffInput.readBYTE()),
          riffInput.readSIGNEDBYTE(),
          convertOctaveNumber(riffInput.readBYTE()),
          RiffTags.newInstance(riffInput));
    }

    /** Return the Clef.Shape for the equivalent RIFF token.
     */
    static Clef.Shape convertShape(int token) {
      if (token == 1)
        return Clef.Shape.G_CLEF;
      else if (token == 2)
        return Clef.Shape.F_CLEF;
      else if (token == 3)
        return Clef.Shape.C_CLEF;
      else if (token == 4)
        return Clef.Shape.PERCUSSION;
      else if (token == 5)
        return Clef.Shape.DOUBLE_G_CLEF;
      else if (token == 6)
        return Clef.Shape.TABLATURE;
      else
        throw new RiffFormatException("Illegal value for Clef shape");
    }

    /** Return the Clef.OctaveNumber for the equivalent RIFF token.
     */
    static Clef.OctaveNumber convertOctaveNumber(int token) {
      if (token == 0)
        return Clef.OctaveNumber.NONE;
      else if (token == 1)
        return Clef.OctaveNumber.ABOVE_8;
      else if (token == 2)
        return Clef.OctaveNumber.BELOW_8;
      else if (token == 3)
        return Clef.OctaveNumber.ABOVE_15;
      else if (token == 4)
        return Clef.OctaveNumber.BELOW_15;
      else
        throw new RiffFormatException("Illegal value for Clef octave number");
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF clef, return a new Clef.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Clef maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}