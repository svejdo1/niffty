/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

namespace org.niffty {



  /** A RiffTags provides static methods for encoding/decoding a
   * Tags using RIFF.
   *
   * @author  default
   */
  public class RiffTags {
    /** Suppress the default constructor.
     */
    private RiffTags() {
    }

    /** Creates new Tags from riffInput's input stream.
     * Note that riffInput is not a parent since the RIFF file
     * doesn't actually have a separate chunk which is the tags.
     * Rather the tags are a section at the end of an existing chunk.
     * This reads from riffInput until its bytesRemaining is zero.
     * Also, after bytesRemaining is zero, this also calls
     * riffInput.skipRemaining() to skip the possible pad byte.
     * Upon return, the "getter" functions height(), width() etc. return
     *   the tag or null if there was no such tag.
     */
    static public Tags newInstance(Riff riffInput) {
      Tags tags = new Tags();
      while (riffInput.getBytesRemaining() > 0) {
        int tag = riffInput.readTag();

        if (tag == 0x01) {
          tags = tags.cloneWithAbsolutePlacement
            (new Placement
             (riffInput.tagShortAt(0), riffInput.tagShortAt(2)));
        } else if (tag == 0x02)
          tags = tags.cloneWithAlternateEnding
            (riffInput.tagByteAt(0));
        else if (tag == 0x03)
          tags = tags.cloneWithAnchorOverride
            (riffInput.tagDataString());
        else if (tag == 0x04)
          tags = tags.cloneWithArticulationDirection
            (convertArticulationDirection(riffInput.tagByteAt(0)));
        else if (tag == 0x05) {
          tags = tags.cloneWithBezierIncoming
          (new Placement
           (riffInput.tagShortAt(0), riffInput.tagShortAt(2)));
        } else if (tag == 0x06) {
          tags = tags.cloneWithBezierOutgoing
          (new Placement
           (riffInput.tagShortAt(0), riffInput.tagShortAt(2)));
        } else if (tag == 0x07)
          tags = tags.cloneWithChordSymbolOffset
            (riffInput.tagShortAt(0));
        // debug: add custom font character
        else if (tag == 0x09)
          tags = tags.cloneWithCustomGraphic
            (riffInput.tagShortAt(0));
        else if (tag == 0x0a)
          tags = tags.cloneWithEndOfSystem(true);
        else if (tag == 0x0b)
          tags = tags.cloneWithFannedBeam
            (convertFannedBeam(riffInput.tagShortAt(0)));
        else if (tag == 0x0c)
          tags = tags.cloneWithFiguredBass
            (riffInput.tagShortAt(0));
        // debug: add font ID
        else if (tag == 0x0e) {
          tags = tags.cloneWithGraceNote
            (new Rational
             (riffInput.tagShortAt(0), riffInput.tagShortAt(2)));
        } else if (tag == 0x0f)
          tags = tags.cloneWithGuitarGridOffset
            (riffInput.tagShortAt(0));
        else if (tag == 0x10)
          tags = tags.cloneWithGuitarTablature(true);
        else if (tag == 0x11)
          tags = tags.cloneWithHeight
            (riffInput.tagShortAt(0));
        else if (tag == 0x12)
          tags = tags.cloneWithId
            (riffInput.tagShortAt(0));
        else if (tag == 0x13)
          tags = tags.cloneWithInvisible(true);
        else if (tag == 0x14)
          tags = tags.cloneWithLargeSize(true);
        else if (tag == 0x15)
          tags = tags.cloneWithLineQuality
            (convertLineQuality(riffInput.tagByteAt(0)));
        else if (tag == 0x16)
          tags = tags.cloneWithLogicalPlacement
            (new LogicalPlacement
             (riffInput.tagByteAt(0), riffInput.tagByteAt(1),
              riffInput.tagByteAt(2)));
        // debug: add lyric verse offset
        else if (tag == 0x18)
          // (Note that NIFF 6a3 has start time and duration as a RATIONAL
          //  but these have been corrected to a LONG.)
          tags = tags.cloneWithMidiPerformance
            (new MidiPerformance
             (riffInput.tagLongAt(0), riffInput.tagLongAt(4),
              riffInput.tagByteAt(8), riffInput.tagByteAt(9)));
        else if (tag == 0x19)
          tags = tags.cloneWithMultiNodeEndOfSystem(true);
        else if (tag == 0x1a)
          tags = tags.cloneWithMultiNodeStartOfSystem(true);
        else if (tag == 0x1b)
          tags = tags.cloneWithNumberOfFlags
            (riffInput.tagByteAt(0));
        else if (tag == 0x1c)
          tags = tags.cloneWithNumberOfNodes
            (riffInput.tagShortAt(0));
        else if (tag == 0x1d)
          tags = tags.cloneWithNumberOfStaffLines
            (riffInput.tagByteAt(0));
        else if (tag == 0x1e)
          tags = tags.cloneWithOssia
            (convertOssia(riffInput.tagByteAt(0)));
        else if (tag == 0x1f)
          tags = tags.cloneWithPartDescriptionOverride
            (new PartDescriptionOverride
             (riffInput.tagSignedByteAt(0), riffInput.tagSignedByteAt(1),
              riffInput.tagSignedByteAt(2)));
        else if (tag == 0x20)
          tags = tags.cloneWithPartID
            (riffInput.tagShortAt(0));
        else if (tag == 0x21)
          tags = tags.cloneWithReferencePointOverride
            (new ReferencePointOverride
             (riffInput.tagByteAt(0), riffInput.tagByteAt(1),
              riffInput.tagByteAt(2), riffInput.tagByteAt(3)));
        else if (tag == 0x22)
          tags = tags.cloneWithRehearsalMarkOffset
            (riffInput.tagShortAt(0));
        else if (tag == 0x23)
          tags = tags.cloneWithRestNumeral
            (riffInput.tagShortAt(0));
        else if (tag == 0x24)
          tags = tags.cloneWithSilent(true);
        else if (tag == 0x25)
          tags = tags.cloneWithSlashedStem(true);
        else if (tag == 0x26)
          tags = tags.cloneWithSmallSize(true);
        else if (tag == 0x27)
          tags = tags.cloneWithSpacingByPart(true);
        else if (tag == 0x28)
          tags = tags.cloneWithSplitStem(true);
        else if (tag == 0x29)
          tags = tags.cloneWithStaffName(true);
        else if (tag == 0x2a)
          tags = tags.cloneWithStaffStep
            (riffInput.tagSignedByteAt(0));
        else if (tag == 0x2b)
          tags = tags.cloneWithThickness
            (riffInput.tagShortAt(0));
        else if (tag == 0x2c)
          tags = tags.cloneWithTieDirection
            (convertTieDirection(riffInput.tagByteAt(0)));
        else if (tag == 0x2d) {
          tags = tags.cloneWithTupletDescription
          (new TupletDescription
           (riffInput.tagShortAt(0), riffInput.tagShortAt(2),
            riffInput.tagShortAt(4), riffInput.tagShortAt(6),
            riffInput.tagByteAt(8)));
        }
          // NOTE: number style tag 0x2e is defined in niff.h but not in the NIFF spec
        else if (tag == 0x2f)
          tags = tags.cloneWithVoiceID
            (riffInput.tagShortAt(0));
        else if (tag == 0x30)
          tags = tags.cloneWithWidth
            (riffInput.tagShortAt(0));
        // else skip the unknown tag ID
      }

      // This skips possible pad byte
      riffInput.skipRemaining();

      return tags;
    }

    /** Return the ArticulationDirection for the equivalent RIFF tag value,
        or null if not recognized.
     */
    static ArticulationDirection? convertArticulationDirection(int tagValue) {
      if (tagValue == 1)
        return ArticulationDirection.POINTED_UP;
      else if (tagValue == 2)
        return ArticulationDirection.POINTED_DOWN;
      else
        return null;
    }

    /** Return the FannedBeam for the equivalent RIFF tag value,
        or null if not recognized.
     */
    static FannedBeam? convertFannedBeam(int tagValue) {
      if (tagValue == 1)
        return FannedBeam.EXPANDING_TOWARD_RIGHT;
      else if (tagValue == 2)
        return FannedBeam.SHRINKING_TOWARD_RIGHT;
      else
        return null;
    }

    /** Return the LineQuality for the equivalent RIFF tag value,
        or null if not recognized.
     */
    static LineQuality? convertLineQuality(int tagValue) {
      if (tagValue == 0)
        return LineQuality.NO_LINE;
      else if (tagValue == 1)
        return LineQuality.DOTTED_LINE;
      else if (tagValue == 2)
        return LineQuality.DASHED_LINE;
      else if (tagValue == 3)
        return LineQuality.WAVY_LINE;
      else
        return null;
    }

    /** Return the Ossia for the equivalent RIFF tag value,
        or null if not recognized.
     */
    static Ossia? convertOssia(int tagValue) {
      if (tagValue == 0)
        return Ossia.DO_NOT_PLAY_BACK;
      else if (tagValue == 1)
        return Ossia.PLAY_BACK;
      else
        return null;
    }

    /** Return the TieDirection for the equivalent RIFF tag value,
        or null if not recognized.
     */
    static TieDirection? convertTieDirection(int tagValue) {
      if (tagValue == 1)
        return TieDirection.ROUNDED_ABOVE;
      else if (tagValue == 2)
        return TieDirection.ROUNDED_BELOW;
      else
        return null;
    }

  }
}
