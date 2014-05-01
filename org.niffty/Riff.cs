/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
using System.Text;
namespace org.niffty {

  /** Class for reading RIFF files.
   */
  public class Riff {
    /** Start reading a new RIFF stream as a RIFF form, requiring the
     * chunkID to be "RIFX" and the form ID to be formID.
     * Upon return, the input file is ready to read the content.
     *
     * @param input     the RIFF input stream
     * @param formID    the expected form ID
     * @see #requireFOURCC
     * @exception IOException   if an I/O error occurs
     * @exception RIFFFormatException   if the chunk ID is not "RIFX"
     */
    public Riff(Stream input, String formID) {
      _input = input;
      _parent = null;

      beginChunk();
      if (!_chunkID.Equals("RIFX")) {
        throw new RiffFormatException
        ("Expected RIFX at start of RIFF stream. Got " + _chunkID + ".");
      }

      String formIDRead = readFOURCC();
      if (!formIDRead.Equals(formID)) {
        throw new RiffFormatException
        ("Expected form ID " + formID + ". Got " + formIDRead + ".");
      }
    }

    /** Begin reading a new RIFF chunk or list which is part of a parent chunk or list.
     * Upon return, the input file is ready to read the content.  The chunk ID can
     * be checked with getChunkID().  If this is a list, you can read
     * the list ID with readFOURCC().
     * This will update the bytesRemaining for this and parent RIFF objects.
     *
     * @param parent     the parent RIFF object
     * @see #getChunkID
     * @see #readFOURCC
     * @exception IOException   if an I/O error occurs
     */
    public Riff(Riff parent) {
      _parent = parent;
      _input = _parent._input;

      beginChunk();
    }

    /** Begin reading a new RIFF chunk or list which is part of a parent chunk or list,
     * requiring the chunkID to be chunkID. To begin a list, use "LIST" for chunkID.
     * Upon return, the input file is ready to read the content.  To check
     * the list ID, call requireFOURCC.
     * This will update the bytesRemaining for this and parent RIFF objects.
     *
     * @param parent     the parent RIFF object
     * @param chunkID   the required chunk ID
     * @see #requireFOURCC
     * @exception IOException   if an I/O error occurs
     * @exception RIFFFormatException   if the chunk ID read is not chunkID
     */
    public Riff(Riff parent, String chunkID) {
      _parent = parent;
      _input = _parent._input;

      beginChunk();
      if (!_chunkID.Equals(chunkID)) {
        throw new RiffFormatException
        ("Expected chunk ID " + chunkID + ". Got " + _chunkID + ".");
      }
    }

    /** Read the chunkID and length from _input, and set up
     * _bytesRemaining and _hasPadByte.
     */
    private void beginChunk() {
      // Note that we will override _bytesRemaining when these advance it.
      _chunkID = readFOURCC();
      _bytesRemaining = readDWORD();
      // There is a pad byte if the chunk size is odd
      _hasPadByte = (_bytesRemaining % 2 != 0);
    }

    /** Return the chunk ID which was read in the constructor.
     */
    public String getChunkID() {
      return _chunkID;
    }
    /** Return the bytes remaining in this chunk or list.  This value
     * is automatically decremented as this or any child objects
     * reads from the input stream.
     */
    public int getBytesRemaining() {
      return _bytesRemaining;
    }

    /** Read one byte from _input as a BYTE (unsigned one-byte)
     * and return as an int.
     * This decrements bytesRemaining for this and all parents.
     */
    public int readBYTE() {
      // read() already returns unsigned
      int result = _input.ReadByte();
      advanceBytesRemaining(1);

      return result;
    }

    /** Read four bytes from _input as a DWORD (unsigned,
     * most significant byte first)
     * and return as an int.
     * This decrements bytesRemaining for this and all parents.
     */
    public int readDWORD() {
      // readBYTE() decrements bytesRemaining
      int result = readBYTE() << 24;
      result += readBYTE() << 16;
      result += readBYTE() << 8;
      result += readBYTE();

      return result;
    }

    /** Read one byte from _input as a SIGNEDBYTE (signed one-byte)
     * and return as an int.
     * This decrements bytesRemaining for this and all parents.
     */
    public int readSIGNEDBYTE() {
      // readBYTE decrements bytesRemaining
      int result = readBYTE();
      if (result >= 128)
        // This converts bytes 128 to 255 to -128 to -1
        result -= 256;

      return result;
    }

    /** Read two bytes from _input as a SHORT (signed, most
     * significant byte first) and return as an int.
     * This decrements bytesRemaining for this and all parents.
     */
    public int readSHORT() {
      int result = readSIGNEDBYTE();
      result = (result << 8) + readBYTE();
      return result;
    }

    /** Read four bytes from _input as a LONG (signed, most
     * significant byte first) and return as an int.
     * This decrements bytesRemaining for this and all parents.
     */
    public int readLONG() {
      int result = readSIGNEDBYTE();
      result = (result << 8) + readBYTE();
      result = (result << 8) + readBYTE();
      result = (result << 8) + readBYTE();
      return result;
    }

    /** Read four bytes from input and return as a string.
     * This decrements bytesRemaining for this and all parents.
     */
    public String readFOURCC() {
      byte[] fourcc = new byte[4];

      // readBYTE() decrements bytesRemaining.
      fourcc[0] = (byte)readBYTE();
      fourcc[1] = (byte)readBYTE();
      fourcc[2] = (byte)readBYTE();
      fourcc[3] = (byte)readBYTE();

      return Encoding.UTF8.GetString(fourcc, 0, fourcc.Length);
    }

    /** Read four bytes from input and return as a string.
     * This does not change bytesRemaining.
     */
    private String readFOURCCNoAdvance() {
      byte[] fourcc = new byte[4];

      fourcc[0] = (byte)_input.ReadByte();
      fourcc[1] = (byte)_input.ReadByte();
      fourcc[2] = (byte)_input.ReadByte();
      fourcc[3] = (byte)_input.ReadByte();

      return Encoding.UTF8.GetString(fourcc, 0, fourcc.Length);
    }

    /** Read four bytes from input and return as a string, and
     * restore the input stream to before the bytes were read.
     * This does not change bytesRemaining.
     */
    public String peekFOURCC() {
      var position = _input.Position;
      String result = readFOURCCNoAdvance();
      // Restore the input stream
      _input.Seek(position, SeekOrigin.Begin);

      return result;
    }

    /** First, read four bytes from input stream.  If it is not "LIST"
     * then return "".  Otherwise, skip four chunk length bytes
     * are read the next four bytes, returning them as a String.
     * Finally, restore the input stream to before the bytes were read.
     * This does not change bytesRemaining.
     *
     * @return the list ID or "" if the chunkID is not "LIST"
     */
    public String peekListID() {
      // Default result to not found.
      String result = "";

      long position = _input.Position;
      if (readFOURCCNoAdvance().Equals("LIST")) {
        // skip the chunk length.  Use read() because I don't trust skip().
        _input.ReadByte();
        _input.ReadByte();
        _input.ReadByte();
        _input.ReadByte();
        result = readFOURCCNoAdvance();
      }
      // Restore the input stream
      _input.Seek(position, SeekOrigin.Begin);

      return result;
    }

    /** First, read four bytes from input stream (the chunk ID) and 
     * ignore them.  Next, skip four chunk length bytes
     * are return the first BYTE  (unsigned one-byte) of the chunk data.
     * Finally, restore the input stream to before the bytes were read.
     * Although not very general (it can only get the first BYTE, not
     * any other byte), this is useful if the first byte of the chunk
     * has important identifying information.
     * This does not change bytesRemaining.
     *
     * @return the the first BYTE of the chunk body.
     */
    public int peekFirstChunkBYTE() {
      long position = _input.Position;
      // skip the chunk ID.  Use read() because I don't trust skip().
      _input.ReadByte();
      _input.ReadByte();
      _input.ReadByte();
      _input.ReadByte();
      // skip the chunk length.  Use read() because I don't trust skip().
      _input.ReadByte();
      _input.ReadByte();
      _input.ReadByte();
      _input.ReadByte();

      int result = _input.ReadByte();

      // Restore the input stream
      _input.Seek(position, SeekOrigin.Begin);

      return result;
    }

    /** Read four bytes from input as a FOURCC and check against value,
     * and if not equal, throw a RIFFFormatException.  For success, simply return.
     * This decrements bytesRemaining for this and all parents.
     *
     * @param value     the expected FOURCC value
     * @exception IOException   if an I/O error occurs
     * @exception RIFFFormatException   if the chunk ID is not "RIFX"
     */
    public void requireFOURCC(String value) {
      String valueRead = readFOURCC();
      if (!valueRead.Equals(value)) {
        throw new RiffFormatException
        ("Expected " + value + " in " + _chunkID + " chunk. Got " +
        valueRead + ".");
      }
    }

    /** Skip past the next chunk in the input stream.
     */
    public void skipChunk() {
      Riff child = new Riff(this);
      child.skipRemaining();
    }

    /** Skip over and discard the remaining bytes in this chunk or list.
     * This also skips past a possible pad byte at the end of the chunk.
     */
    public void skipRemaining() {
      while (_bytesRemaining > 0)
        // readBYTE() decrements _bytesRemaining
        readBYTE();

      if (_hasPadByte) {
        // Skip the pad byte which is not part of _bytesRemaining
        // Use readBYTE() which decrements the _bytesRemaining for
        //   all parents
        readBYTE();
        // But we want the _bytesRemaining for this to be 0, not -1
        _bytesRemaining = 0;
        _hasPadByte = false;
      }
    }

    /** Read a tag from the input stream and return the tag ID.
     * A tag has a one byte tag ID, a one byte tag size and data
     * of the given size.  On return, call getTagData() to get the tag data.
     * The tag data is an array of int where each int is between 0 and 255.
     * The data length is getTagData().length .
     * This decrements bytesRemaining for this and all parents.
     * Even though getTagData() does not include the possible pad byte,
     * this will advance the input stream past the pad byte.
     *
     * @return  the tag ID (also available from getTagID())
     * @see #getTagData
     * @see #getTagID
     */
    public int readTag() {
      // readBYTE() decrements bytesRemaining
      _tagID = readBYTE();
      int tagLength = readBYTE();

      _tagData = new int[tagLength];
      for (int i = 0; i < tagLength; ++i) {
        _tagData[i] = readBYTE();
      }

      if (tagLength % 2 != 0) {
        // advance past the pad byte
        readBYTE();
      }

      return _tagID;
    }

    /** Returns the tag ID from the most recent call to readTag.
     * @see #readTag
     */
    public int getTagID() {
      return _tagID;
    }

    /** Returns the tag data from the most recent call to readTag.
     * The tagData is an array of int where each int is between 0 and 255.
     *
     * @see #readTag
     */
    public int[] getTagData() {
      return _tagData;
    }

    /** Returns the tag data from the most recent call to readTag as a String.
     * This first converts the tagData() to an array of byte and then uses
     * the String constructor String(bytes).
     *
     * @see #readTag
     */
    public String tagDataString() {
        byte[] bytes = new byte[_tagData.Length];
        for (int i = 0; i < _tagData.Length; ++i)
            bytes[i] = (byte)_tagData[i];

        return Encoding.UTF8.GetString(bytes, 0, bytes.Length);
    }

    /** Interpret tagData[index] and tagData[index+1] as a two-byte signed integer,
     * most significant byte first and return as an int.
     *
     * @param index the position in tagData of the first byte of the SHORT
     * @return      the two-byte integer starting at tagData[index]
     * @exception RIFFFormatException   if the SHORT at index extends
     *                                  beyond the length of tagData.
     */
    public int tagShortAt(int index) {
      if ((index + 1) >= _tagData.Length)
        throw new RiffFormatException
        ("Tag " + _tagID + " is too short in chunk " + _chunkID + ".");

      // Get most significant byte.
      int result = _tagData[index];
      if (result >= 128)
        // This converts bytes 128 to 255 to -128 to -1
        result -= 256;

      result = (result << 8) + _tagData[index + 1];
      return result;
    }

    /** Interpret tagData[index] through tagData[index+3] as a four-byte signed integer,
     * most significant byte first and return as an int.
     *
     * @param index the position in tagData of the first byte of the LONG
     * @return      the four-byte integer starting at tagData[index]
     * @exception RIFFFormatException   if the LONG at index extends
     *                                  beyond the length of tagData.
     */
    public int tagLongAt(int index) {
      if ((index + 3) >= _tagData.Length)
        throw new RiffFormatException
        ("Tag " + _tagID + " is too short in chunk " + _chunkID + ".");

      // Get most significant byte.
      int result = _tagData[index];
      if (result >= 128)
        // This converts bytes 128 to 255 to -128 to -1
        result -= 256;

      result = (result << 8) + _tagData[index + 1];
      result = (result << 8) + _tagData[index + 2];
      result = (result << 8) + _tagData[index + 3];
      return result;
    }

    /** Interpret tagData[index] as a one-byte unsigned integer,
     * and return as an int.
     *
     * @param index the position in tagData of the BYTE
     * @return      the one-byte integer at tagData[index]
     * @exception RIFFFormatException   if the index is
     *                                  beyond the length of tagData.
     */
    public int tagByteAt(int index) {
      if (index >= _tagData.Length)
        throw new RiffFormatException
        ("Tag " + _tagID + " is too short in chunk " + _chunkID + ".");

      return _tagData[index];
    }

    /** Interpret tagData[index] as a one-byte signed integer,
     * and return as an int.
     *
     * @param index the position in tagData of the SIGNEDBYTE
     * @return      the one-byte integer at tagData[index]
     * @exception RIFFFormatException   if the index is
     *                                  beyond the length of tagData.
     */
    public int tagSignedByteAt(int index) {
      if (index >= _tagData.Length)
        throw new RiffFormatException
        ("Tag " + _tagID + " is too short in chunk " + _chunkID + ".");

      int result = _tagData[index];
      if (result >= 128)
        // This converts bytes 128 to 255 to -128 to -1
        result -= 256;

      return result;
    }

    /** Return the parent which was given to the constructor, or
     *  null if there is no parent.
     */
    public Riff getParent() {
      return _parent;
    }

    /** Decrease _bytesRemaining for this and all parent RIFF objects.
     * This does not read anything from _input.
     *
     * @param bytes     the amount to decrease _bytesRemaining
     */
    private void advanceBytesRemaining(int bytes) {
      Riff riff = this;

      while (riff != null) {
        riff._bytesRemaining -= bytes;
        riff = riff._parent;
      }
    }

    private Stream _input;
    private Riff _parent;

    // _bytesRemaining is decreased as bytes are read so that skipRemainder()
    //   will function.  _bytesRemaining doesn't include a possible pad
    //   byte which also must be skipped, as indicated by _hasPadByte.
    private int _bytesRemaining;
    private bool _hasPadByte;

    private String _chunkID;
    private int _tagID;
    private int[] _tagData;
  }
}
