/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System.IO;
using System;
namespace org.niffty {

  /** This : RIFF to provide helper methods for decoding NIFF files,
   *  such as handling the String Table. This is created as the "root" RIFF
   * object to keep state while decoding the NIFF file.
   */
  public class RiffForNiff : Riff {

    /** Start reading a new RIFF stream as a RIFF form, requiring the
     * chunkID to be "RIFX" and the form ID to be formID.
     * This is the same as the RIFF constructor.  This also creates
     * an empty String Table.  
     *
     * @param input     the RIFF input stream
     * @param formID    the expected form ID
     * @see RIFF#RIFF(InputString, String)
     * @exception IOException   if an I/O error occurs
     * @exception RIFFFormatException   if the chunk ID is not "RIFX"
     */
    public RiffForNiff(Stream input, String formID)
      : base(input, formID) {
    }

    /** Store the entire String Table to be retrieved later by getStringTable()
     *  so that the RiffStringTable class can get a string from it.
     *  We can't decode it now into its component strings because the
     *  stringOffset can be anything.
     *
     *  @param stringTable  the contents of the NIFF String Table as
     *         a byte array.
     */
    public void setStringTable(byte[] stringTable) {
      _stringTable = stringTable;
    }

    /** Return the byte array that was set with setStringTable, or
     *  a zero length byte array if it is not yet set.
     */
    public byte[] getStringTable() {
      return _stringTable;
    }

    /** This is a static method to find the parent of riff which is
     * a RIFFForNIFF and return its getStringTable().
     *
     * @param riff   this, or one of its parents going up the chain
     *      should be an is RIFFForNIFF
     * @return the getStringTable() from the first parent or riff
     *      which is a RIFFForNIFF, or return a zero length byte array
     *      if there is no such parent.
     */
    static public byte[] getStringTable(Riff riff) {
      while (riff != null) {
        if (riff is RiffForNiff)
          return ((RiffForNiff)riff).getStringTable();
        riff = riff.getParent();
      }

      return new byte[0];
    }

    private byte[] _stringTable = new byte[0];
  }
}