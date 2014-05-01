/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A RiffTimeSlice provides static methods for encoding/decoding a
   * TimeSlice using RIFF.
   *
   * @author  default
   */
  public class RiffTimeSlice {
    static readonly String RIFF_ID = "tmsl";

    /** Suppress the default constructor.
     */
    private RiffTimeSlice() {
    }

    /** Creates new TimeSlice from the parentInput's input stream.
     * The next object in the input stream must be a time slice with 
     * a type any other than MEASURE_START (which means the type should
     * be EVENT).
     * After creating the TimeSlice, you can call addMusicSymbols()
     * to store the music symbols for this time slice.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     * @see #addMusicSymbols
     */
    static public TimeSlice newInstance(Riff parentInput) {
      Riff riffInput = new Riff(parentInput, RIFF_ID);

      int type = riffInput.readBYTE();
      if (type == MEASURE_START)
        throw new RiffFormatException
        ("Did not expect a time slice with type MEASURE_START.");
      else if (type != EVENT)
        throw new RiffFormatException
        ("Expected EVENT for time slice type. Got " + type + ".");

      return new TimeSlice
        (new Rational(riffInput.readSHORT(), riffInput.readSHORT()),
         RiffTags.newInstance(riffInput));
    }

    /** Peek into the parentInput's input stream and if the next item
     * is a NIFF time slice with any type other than MEASURE_START (presumably
     * type EVENT), return a new  TimeSlice.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public TimeSlice maybeNew(Riff parentInput) {
      if (!parentInput.peekFOURCC().Equals(RIFF_ID))
        return null;
      // We must also ensure that this is not a MEASURE_START time slice
      if (parentInput.peekFirstChunkBYTE() == MEASURE_START)
        return null;

      return newInstance(parentInput);
    }
    private const int MEASURE_START = 1;
    private const int EVENT = 2;

    /** This reads chunks from parentInput's input stream until the next
     * NIFF time slice or no more bytesRemaining in the input,
     * adding the chunks to the timeSlice's music symbol list.  If a music
     * symbol is not recognized, this skips it. 
     * This stops at either a measure start or an event time slice.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     * @param timeSlice  the TimeSlice to which MusicSymbol objects are added.
     * @see MusicSymbol
     */
    static public void addMusicSymbols(Riff parentInput, TimeSlice timeSlice) {
      while (parentInput.getBytesRemaining() > 0) {
        if (parentInput.peekFOURCC().Equals(RIFF_ID))
          // encoutered the next time slice, so quit
          // Note that this stops at either a measure start or an event time slice.
          return;

        MusicSymbol musicSymbol = maybeNewAnyMusicSymbol(parentInput);
        if (musicSymbol != null)
          timeSlice.addMusicSymbol(musicSymbol);
        else
          // Did not recognize the music symbol, so skip
          parentInput.skipChunk();
      }
    }

    /** Peek into the parentInput's input stream and if the next item
     * is one of the recognized NIFF music symbols which subclass
     * MusicSymbol, then return a new object of that subclass.  Otherwise,
     * return null and leave the input stream unchanged.
     *
     * @param parentInput    the parent RIFF object being used to read the input stream
     */
    static public MusicSymbol maybeNewAnyMusicSymbol(Riff parentInput) {
      MusicSymbol musicSymbol;

      if ((musicSymbol = RiffAccidental.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffAugmentationDot.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffBarline.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffBeam.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffClef.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffKeySignature.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffLyric.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffNotehead.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffStem.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffRest.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffTie.maybeNew(parentInput)) != null)
        return musicSymbol;
      else if ((musicSymbol = RiffTimeSignature.maybeNew(parentInput)) != null)
        return musicSymbol;
      else
        return null;
    }
  }
}