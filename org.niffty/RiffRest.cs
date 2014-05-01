/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffRest provides static methods for encoding/decoding a
   * Rest using RIFF.
   *
   * @author  default
   */
  public class RiffRest {
    static readonly String RIFF_ID = "rest";

    /** Suppress the default constructor.
     */
    private RiffRest() {
    }

    /** Creates new Rest from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Rest newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      return new Rest
         (convertShape(riffInput.readBYTE()),
          riffInput.readSIGNEDBYTE(),
          new Rational(riffInput.readSHORT(), riffInput.readSHORT()),
          RiffTags.newInstance(riffInput));
    }

    /** Return the Rest.Shape for the equivalent RIFF token.
     */
    static Rest.Shape convertShape(int token) {
      if (token == 1)
        return Rest.Shape.BREVE;
      else if (token == 2)
        return Rest.Shape.WHOLE;
      else if (token == 3)
        return Rest.Shape.HALF;
      else if (token == 4)
        return Rest.Shape.QUARTER;
      else if (token == 5)
        return Rest.Shape.EIGHTH;
      else if (token == 6)
        return Rest.Shape.SIXTEENTH;
      else if (token == 7)
        return Rest.Shape.THIRTY_SECOND;
      else if (token == 8)
        return Rest.Shape.SIXTY_FOURTH;
      else if (token == 9)
        return Rest.Shape.ONE_TWENTY_EIGHTH;
      else if (token == 10)
        return Rest.Shape.TWO_FIFTY_SIXTH;
      else if (token == 11)
        return Rest.Shape.FOUR_MEASURES;
      else if (token == 12)
        return Rest.Shape.MULTIPLE_MEASURE_THICK_HORIZONTAL;
      else if (token == 13)
        return Rest.Shape.MULTIPLE_MEASURE_THICK_SLANTED;
      else if (token == 14)
        return Rest.Shape.VOCAL_COMMA;
      else if (token == 15)
        return Rest.Shape.VOCAL_TWO_SMALL_SLASHES;
      else
        throw new RiffFormatException("Illegal value for Rest shape");
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF rest, return a new Rest.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public Rest maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}
