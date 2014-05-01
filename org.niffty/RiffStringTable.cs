/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.Text;
namespace org.niffty {



  /** A RiffStringTable provides static methods for encoding/decoding a
   * NIFF String Table using RIFF.  The String Table data is stored in the
   * parent RIFFForNIFF to be used later by RiffLyric, etc. for getting
   * the full string.
   *
   * @author  default
   */
  public class RiffStringTable {
    static readonly String RIFF_ID = "stbl";

    /** Suppress the default constructor.
     */
    private RiffStringTable() {
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF String Table, then decode it and store it in the
     * root RIFFForNIFF object.  
     * If the next item is not a NIFF String Table, do nothing
     * and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream.
     *        If parentInput.getParent() is not of type RIFFForNIFF, then this
     *        moves the input past the String Table but does not store it.
     * @return true if the String Table was processed, false if this is
     *         not a String Table.
     */
    static public bool maybeDecode(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return false;

      Riff riffInput = new Riff(parentInput, RIFF_ID);

      if (!(parentInput.getParent() is RiffForNiff)) {
        // There is no place to store the data
        riffInput.skipRemaining();
        return true;
      }
      RiffForNiff riffForNiff = (RiffForNiff)parentInput.getParent();

      // Read the entire string table into a byte array
      byte[] stringTable = new byte[riffInput.getBytesRemaining()];
      for (int i = 0; i < stringTable.Length; ++i)
        stringTable[i] = (byte)riffInput.readBYTE();
      riffForNiff.setStringTable(stringTable);

      // This skips possible pad byte
      riffInput.skipRemaining();

      return true;
    }

    /** Get the null-terminated string from the stringTable at the given offset,
     * decoding using the "ISO-8859-1" latin alphabet.  However, if the
     * first byte after the null terminator is a 1, decode the subsequent
     * null-terminated string as a Unicode string using "UTF-8".
     *
     * @param stringTable  the NIFF stringTable.  Usually, you get this
     *       while decoding from a RIFF object by finding its parent
     *       RIFFForNIFF using the static method 
     *       RIFFForNIFF.getStringTable (riff).
     * @param offset   the offset into the string table, such as
     *      the STROFFSET in a Lyric.  If this is past the end of
     *      the stringTable or less than zero, this returns "".
     * @return  the decoded string
     */
    static public String decodeString(byte[] stringTable, int offset) {
      if (offset < 0)
        return "";

      int length = strlen(stringTable, offset);
      String charsetName = "ISO-8859-1";

      if (offset + length + 1 < stringTable.Length &&
          stringTable[offset + length + 1] == 1) {
        // Use the Unicode string, starting after the 1.  If it starts
        // past the end of the stringTable, strlen will return zero
        // and the String constructor won't try to read stringTable.
        length = strlen(stringTable, offset + length + 2);
        charsetName = "UTF-8";
      }

      try {
        return Encoding.UTF8.GetString(stringTable, offset, length);
      } catch (Exception e) {
        // We don't expect this to happen since we are using
        // standard encodings.
        return "";
      }
    }

    /** Like the C function of the same name, this counts the number
     * of bytes in the array, starting from offset, before a zero,
     * or before the end of the array.
     *
     * @param bytes the byte array
     * @param offset  the index if the first byte to start counting.
     *       if this is past the end of the array, this returns zero.
     * @return  the number of bytes before  a zero or the end of the array.
     */
    static public int strlen(byte[] bytes, int offset) {
      int result = 0;
      while ((offset + result) < bytes.Length &&
             bytes[offset + result] != 0)
        result++;

      return result;
    }
  }
}
