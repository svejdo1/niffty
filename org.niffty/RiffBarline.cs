/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffBarline provides static methods for encoding/decoding a
   * Barline using RIFF.
   *
   * @author  default
   */
  public class RiffBarline {
    static readonly String RIFF_ID = "barl";

    /** Suppress the default constructor.
     */
    private RiffBarline() {
    }

    /** Creates a new Barline from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Barline newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      return new Barline
        (convertType(riffInput.readBYTE()),
         convertExtendsTo(riffInput.readBYTE()),
         riffInput.readSHORT(),
         RiffTags.newInstance(riffInput));
    }

    /** Return the Barline.Type for the equivalent RIFF token.
     */
    static Barline.Type convertType(int token) {
      if (token == 1)
        return Barline.Type.THIN;
      else if (token == 2)
        return Barline.Type.THICK;
      else
        throw new RiffFormatException("Illegal value for Barline type");
    }

    /** Return the Barline.ExtendsTo for the equivalent RIFF token.
     */
    static Barline.ExtendsTo convertExtendsTo(int token) {
      if (token == 1)
        return Barline.ExtendsTo.BOTTOM_OF_STAFF;
      else if (token == 2)
        return Barline.ExtendsTo.NEXT_STAFF;
      else if (token == 3)
        return Barline.ExtendsTo.BETWEEN_STAVES;
      else
        throw new RiffFormatException("Illegal value for Barline extendsTo");
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF barline, return a new Barline.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Barline maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}