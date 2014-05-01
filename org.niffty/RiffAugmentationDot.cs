/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A RiffAugmentationDot provides static methods for encoding/decoding an
   * AugmentationDot using RIFF.
   *
   * @author  default
   */
  public class RiffAugmentationDot {
    static readonly String RIFF_ID = "augd";

    /** Suppress the default constructor.
     */
    private RiffAugmentationDot() {
    }

    /** Creates a new AugmentationDot from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public AugmentationDot newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      return new AugmentationDot(RiffTags.newInstance(riffInput));
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF augmentation dot, return a new AugmentationDot.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public AugmentationDot maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}
