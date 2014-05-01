/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A NIFFInfo has a version string, writing program type, standard units,
   *   absolute units, and MIDI ticks per quarter.
   *
   * @author  user
   */
  public class NiffInfo {
    /** Creates a new NIFFInfo with the given values.
     */
    public NiffInfo
      (String version, int writingProgramType, int standardUnits,
      int absoluteUnits, int midiTicksPerQuarter) {
      _version = version;
      _writingProgramType = writingProgramType;
      _standardUnits = standardUnits;
      _absoluteUnits = absoluteUnits;
      _midiTicksPerQuarter = midiTicksPerQuarter;
    }

    // for writing program type
    public const int OTHER = -1;
    public const int ENGRAVING_PROGRAM = 1;
    public const int SCANNING_PROGRAM = 2;
    public const int MIDI_INTERPRETER = 3;
    public const int SEQUENCER = 4;
    public const int RESEARCH_PROGRAM = 5;
    public const int EDUCATIONAL_PROGRAM = 6;

    // for standard units
    public const int NONE = -1;
    public const int INCHES = 1;
    public const int CENTIMETERS = 2;
    public const int POINTS = 3;

    /** Returns the version which is a string of 8 characters.  Because the string
     * length is always8 characters, some of the characters at the end of the
     * string may be zeroes.
     */
    public String getVersion() {
      return _version;
    }

    /** Returns the writing program type, which is OTHER, ENGRAVING_PROGRAM,
     *    SCANNING_PROGRAM, MIDI_INTERPRETER, SEQUENCER, RESEARCH_PROGRAM,
     *    or EDUCATIONAL_PROGRAM.
     */
    public int getWritingProgramType() {
      return _writingProgramType;
    }

    /** Returns the standard units, which is NONE, INCHES, CENTIMETERS or POINTS.
     */
    public int getStandardUnits() {
      return _standardUnits;
    }

    /** Returns the absolute units, or -1 for none.
     */
    public int getAbsoluteUnits() {
      return _absoluteUnits;
    }

    /** Returns the MIDI ticks per quarter, or -1 for none.
     */
    public int getMidiTicksPerQuarter() {
      return _midiTicksPerQuarter;
    }

    private String writingProgramTypeString() {
      if (_writingProgramType == OTHER) return "OTHER";
      else if (_writingProgramType == ENGRAVING_PROGRAM) return "ENGRAVING_PROGRAM";
      else if (_writingProgramType == SCANNING_PROGRAM) return "SCANNING_PROGRAM";
      else if (_writingProgramType == MIDI_INTERPRETER) return "MIDI_INTERPRETER";
      else if (_writingProgramType == SEQUENCER) return "SEQUENCER";
      else if (_writingProgramType == RESEARCH_PROGRAM) return "RESEARCH_PROGRAM";
      else if (_writingProgramType == EDUCATIONAL_PROGRAM) return "EDUCATIONAL_PROGRAM";
      else return "";
    }

    private String standardUnitsString() {
      if (_writingProgramType == NONE) return "NONE";
      else if (_writingProgramType == INCHES) return "INCHES";
      else if (_writingProgramType == CENTIMETERS) return "CENTIMETERS";
      else if (_writingProgramType == POINTS) return "POINTS";
      else return "";
    }

    public override string ToString() {
        // Need to get a version string without zeroes at the end.
        int zeroIndex = _version.IndexOf ('\0');
        if (zeroIndex < 0)
          // There are no zeroes, so use the whole string
          zeroIndex = _version.Length;

        return "NIFF-Info, version=" + _version.Substring(0, zeroIndex) +
          ", writing program type=" +
          writingProgramTypeString() + ", standard units=" + standardUnitsString() +
          ", absolute units=" + _absoluteUnits + ", midi ticks per quarter=" +
          _midiTicksPerQuarter;
    }

    private String _version;
    private int _writingProgramType;
    private int _standardUnits;
    private int _absoluteUnits;
    private int _midiTicksPerQuarter;
  }
}