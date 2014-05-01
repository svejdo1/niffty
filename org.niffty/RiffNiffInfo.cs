/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.Text;
namespace org.niffty {

  /** A RiffNIFFInfo provides static methods for encoding/decoding a
   * NIFFInfo using RIFF.
   *
   * @author  default
   */
  public class RiffNiffInfo {
    static readonly String RIFF_ID = "nnfo";

    /** Suppress the default constructor.
     */
    private RiffNiffInfo() {
    }

    /** Creates new NIFFInfo from the parentInput's input stream.
     * The next object in the input stream must be of this type.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public NiffInfo newInstance(Riff parentInput) 
           {
        Riff riffInput = new Riff (parentInput, RIFF_ID);

        // Read 8 bytes and convert to string
        byte[] buffer = new byte[8];
        buffer[0] = (byte)riffInput.readBYTE();
        buffer[1] = (byte)riffInput.readBYTE();
        buffer[2] = (byte)riffInput.readBYTE();
        buffer[3] = (byte)riffInput.readBYTE();
        buffer[4] = (byte)riffInput.readBYTE();
        buffer[5] = (byte)riffInput.readBYTE();
        buffer[6] = (byte)riffInput.readBYTE();
        buffer[7] = (byte)riffInput.readBYTE();
        
        return new NiffInfo
          (Encoding.UTF8.GetString(buffer, 0, buffer.Length),
           riffInput.readSIGNEDBYTE(),
           riffInput.readSIGNEDBYTE(),
           riffInput.readSHORT(),
           riffInput.readSHORT());
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF info, return a new NIFFInfo.  Otherwise,
     * return null and leave the input stream unchanged.
     */
    static public NiffInfo maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;

      return newInstance(parentInput);
    }
  }
}
