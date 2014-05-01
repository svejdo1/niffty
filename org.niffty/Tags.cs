/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {


  /** A Tags is a collection of the optional tags at the end of a chunk.
   * Because Tags is immutable, it doesn't have "set" methods.
   * But you can call cloneWithAbsolutePlacement(), etc.
   * to get a copy of the Tags object with the modified particular field.
   * For example to make a Tags with height of 10 and the endOfSystem tag
   * true, use: 
  <code>
    new Tags().cloneWithHeight (new int? (10)).cloneWithEndOfSystem (true)
  </code>
   *
   * @author  user
   */
  public class Tags {
    /** Creates a new empty Tags object.
     */
    public Tags() {
    }

    /** Return a copy of this Tags object with all the same fields.
     *  This is only called by the "cloneWith" methods.
     */
    protected Tags clone() {
      return (Tags)this.MemberwiseClone();
    }

    /** Returns the absolute placement tag as a Placement, or null if the tag is not present.
     */
    public Placement absolutePlacement() { return _absolutePlacement; }
    /** Returns a copy of this Tags, with the absolute placement changed to
     *  the given value, which may be null if the tag is not present.
     */
    public Tags cloneWithAbsolutePlacement(Placement absolutePlacement) {
      var tags = clone();
      tags._absolutePlacement = absolutePlacement;
      return tags;
    }
    private Placement _absolutePlacement;

    /** Returns the alternate ending tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? alternateEnding() { return _alternateEnding; }
    /** Returns a copy of this Tags, with alternate ending changed to
     *  the given value, which may be null if the tag is not present.
     */
    public Tags cloneWithAlternateEnding(int? alternateEnding) {
      var tags = clone();
      tags._alternateEnding = alternateEnding;
      return tags;
    }
    private int? _alternateEnding;

    /** Returns the anchorOverride tag, or null if the tag is not present.
     */
    public String anchorOverride() { return _anchorOverride; }
    /** Returns a copy of this Tags, with anchorOverride
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithAnchorOverride(String anchorOverride) {
      var tags = clone();
      tags._anchorOverride = anchorOverride;
      return tags;
    }
    private String _anchorOverride;

    /** Returns the articulation direction tag
     * or null if the tag is not present.
     */
    public ArticulationDirection? articulationDirection() { return _articulationDirection; }
    /** Returns a copy of this Tags, with articulationDirection
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithArticulationDirection(ArticulationDirection? articulationDirection) {
      var tags = clone();
      tags._articulationDirection = articulationDirection;
      return tags;
    }
    private ArticulationDirection? _articulationDirection;

    /** Returns the Bezier incoming tag as a Placement, or null if the tag is not present.
     */
    public Placement bezierIncoming() { return _bezierIncoming; }
    /** Returns a copy of this Tags, with bezierIncoming
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithBezierIncoming(Placement bezierIncoming) {
      Tags tags = (Tags)clone();
      tags._bezierIncoming = bezierIncoming;
      return tags;
    }
    private Placement _bezierIncoming;

    /** Returns the Bezier outgoing tag as a Placement, or null if the tag is not present.
     */
    public Placement bezierOutgoing() { return _bezierOutgoing; }
    /** Returns a copy of this Tags, with bezierOutgoing
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithBezierOutgoing(Placement bezierOutgoing) {
      Tags tags = (Tags)clone();
      tags._bezierOutgoing = bezierOutgoing;
      return tags;
    }
    private Placement _bezierOutgoing;

    /** Returns the chord symbol offset tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? chordSymbolOffset() { return _chordSymbolOffset; }
    /** Returns a copy of this Tags, with chordSymbolOffset
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithChordSymbolOffset(int? chordSymbolOffset) {
      Tags tags = (Tags)clone();
      tags._chordSymbolOffset = chordSymbolOffset;
      return tags;
    }
    private int? _chordSymbolOffset;

    /** Returns the custom graphic tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? customGraphic() { return _customGraphic; }
    /** Returns a copy of this Tags, with customGraphic
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithCustomGraphic(int? customGraphic) {
      Tags tags = (Tags)clone();
      tags._customGraphic = customGraphic;
      return tags;
    }
    private int? _customGraphic;

    /** Returns true if the end of system tag is present, otherwise false.
     */
    public bool endOfSystem() { return _endOfSystem; }
    /** Returns a copy of this Tags, with the endOfSystem
     *  flag set to the given value.
     */
    public Tags cloneWithEndOfSystem(bool endOfSystem) {
      Tags tags = (Tags)clone();
      tags._endOfSystem = endOfSystem;
      return tags;
    }
    private bool _endOfSystem;

    /** Returns the fanned beam tag
     * or null if the tag is not present.
     */
    public FannedBeam? fannedBeam() { return _fannedBeam; }
    /** Returns a copy of this Tags, with fannedBeam
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithFannedBeam(FannedBeam? fannedBeam) {
      Tags tags = (Tags)clone();
      tags._fannedBeam = fannedBeam;
      return tags;
    }
    private FannedBeam? _fannedBeam;

    /** Returns the figured bass tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? figuredBass() { return _figuredBass; }
    /** Returns a copy of this Tags, with figuredBass
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithFiguredBass(int? figuredBass) {
      Tags tags = (Tags)clone();
      tags._figuredBass = figuredBass;
      return tags;
    }
    private int? _figuredBass;

    /** Returns the grace note tag as a Rational, or null if the tag is not present.
     */
    public Rational graceNote() { return _graceNote; }
    /** Returns a copy of this Tags, with graceNote
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithGraceNote(Rational graceNote) {
      Tags tags = (Tags)clone();
      tags._graceNote = graceNote;
      return tags;
    }
    private Rational _graceNote;

    /** Returns the guitar grid offset tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? guitarGridOffset() { return _guitarGridOffset; }
    /** Returns a copy of this Tags, with guitarGridOffset
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithGuitarGridOffset(int? guitarGridOffset) {
      Tags tags = (Tags)clone();
      tags._guitarGridOffset = guitarGridOffset;
      return tags;
    }
    private int? _guitarGridOffset;

    /** Returns true if the guitar tablature tag is present, otherwise false.
     */
    public bool guitarTablature() { return _guitarTablature; }
    /** Returns a copy of this Tags, with the guitarTablature
     *  flag set to the given value.
     */
    public Tags cloneWithGuitarTablature(bool guitarTablature) {
      Tags tags = (Tags)clone();
      tags._guitarTablature = guitarTablature;
      return tags;
    }
    private bool _guitarTablature;

    /** Returns the height tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? height() { return _height; }
    /** Returns a copy of this Tags, with height
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithHeight(int? height) {
      Tags tags = (Tags)clone();
      tags._height = height;
      return tags;
    }
    private int? _height;

    /** Returns the id tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? id() { return _id; }
    /** Returns a copy of this Tags, with id
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithId(int? id) {
      Tags tags = (Tags)clone();
      tags._id = id;
      return tags;
    }
    private int? _id;

    /** Returns true if the invisible tag is present, otherwise false.
     */
    public bool invisible() { return _invisible; }
    /** Returns a copy of this Tags, with the invisible
     *  flag set to the given value.
     */
    public Tags cloneWithInvisible(bool invisible) {
      Tags tags = (Tags)clone();
      tags._invisible = invisible;
      return tags;
    }
    private bool _invisible;

    /** Returns true if the large size tag is present, otherwise false.
     */
    public bool largeSize() { return _largeSize; }
    /** Returns a copy of this Tags, with the largeSize
     *  flag set to the given value.
     */
    public Tags cloneWithLargeSize(bool largeSize) {
      Tags tags = (Tags)clone();
      tags._largeSize = largeSize;
      return tags;
    }
    private bool _largeSize;

    /** Returns the line quality tag
     * or null if the tag is not present.
     */
    public LineQuality? lineQuality() { return _lineQuality; }
    /** Returns a copy of this Tags, with lineQuality
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithLineQuality(LineQuality? lineQuality) {
      Tags tags = (Tags)clone();
      tags._lineQuality = lineQuality;
      return tags;
    }
    private LineQuality? _lineQuality;

    /** Returns the logical placement tag, or null if the tag is not present.
     */
    public LogicalPlacement logicalPlacement() { return _logicalPlacement; }
    /** Returns a copy of this Tags, with logicalPlacement
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithLogicalPlacement(LogicalPlacement logicalPlacement) {
      Tags tags = (Tags)clone();
      tags._logicalPlacement = logicalPlacement;
      return tags;
    }
    private LogicalPlacement _logicalPlacement;

    /** Returns the MIDI performance tag, or null if the tag is not present.
     */
    public MidiPerformance midiPerformance() { return _midiPerformance; }
    /** Returns a copy of this Tags, with midiPerformance
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithMidiPerformance(MidiPerformance midiPerformance) {
      Tags tags = (Tags)clone();
      tags._midiPerformance = midiPerformance;
      return tags;
    }
    private MidiPerformance _midiPerformance;

    /** Returns true if the multi node end of system tag is present, otherwise false.
     */
    public bool multiNodeEndOfSystem() { return _multiNodeEndOfSystem; }
    /** Returns a copy of this Tags, with the multiNodeEndOfSystem
     *  flag set to the given value.
     */
    public Tags cloneWithMultiNodeEndOfSystem(bool multiNodeEndOfSystem) {
      Tags tags = (Tags)clone();
      tags._multiNodeEndOfSystem = multiNodeEndOfSystem;
      return tags;
    }
    private bool _multiNodeEndOfSystem;

    /** Returns true if the multi node start of system tag is present, otherwise false.
     */
    public bool multiNodeStartOfSystem() { return _multiNodeStartOfSystem; }
    /** Returns a copy of this Tags, with the multiNodeStartOfSystem
     *  flag set to the given value.
     */
    public Tags cloneWithMultiNodeStartOfSystem(bool multiNodeStartOfSystem) {
      Tags tags = (Tags)clone();
      tags._multiNodeStartOfSystem = multiNodeStartOfSystem;
      return tags;
    }
    private bool _multiNodeStartOfSystem;

    /** Returns the number of flags tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? numberOfFlags() { return _numberOfFlags; }
    /** Returns a copy of this Tags, with numberOfFlags
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithNumberOfFlags(int? numberOfFlags) {
      Tags tags = (Tags)clone();
      tags._numberOfFlags = numberOfFlags;
      return tags;
    }
    private int? _numberOfFlags;

    /** Returns the number of nodes tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? numberOfNodes() { return _numberOfNodes; }
    /** Returns a copy of this Tags, with numberOfNodes
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithNumberOfNodes(int? numberOfNodes) {
      Tags tags = (Tags)clone();
      tags._numberOfNodes = numberOfNodes;
      return tags;
    }
    private int? _numberOfNodes;

    /** Returns the number of staff lines tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? numberOfStaffLines() { return _numberOfStaffLines; }
    /** Returns a copy of this Tags, with numberOfStaffLines
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithNumberOfStaffLines(int? numberOfStaffLines) {
      Tags tags = (Tags)clone();
      tags._numberOfStaffLines = numberOfStaffLines;
      return tags;
    }
    private int? _numberOfStaffLines;

    /** Returns the ossia tag
     * or null if the tag is not present.
     */
    public Ossia? ossia() { return _ossia; }
    /** Returns a copy of this Tags, with ossia
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithOssia(Ossia? ossia) {
      Tags tags = (Tags)clone();
      tags._ossia = ossia;
      return tags;
    }
    private Ossia? _ossia;

    /** Returns the part description override tag, or null if the tag is not present.
     */
    public PartDescriptionOverride partDescriptionOverride() { return _partDescriptionOverride; }
    /** Returns a copy of this Tags, with partDescriptionOverride
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithPartDescriptionOverride(PartDescriptionOverride partDescriptionOverride) {
      Tags tags = (Tags)clone();
      tags._partDescriptionOverride = partDescriptionOverride;
      return tags;
    }
    private PartDescriptionOverride _partDescriptionOverride;

    /** Returns the part ID tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? partID() { return _partID; }
    /** Returns a copy of this Tags, with partID
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithPartID(int? partID) {
      Tags tags = (Tags)clone();
      tags._partID = partID;
      return tags;
    }
    private int? _partID;

    /** Returns the reference point override tag, or null if the tag is not present.
     */
    public ReferencePointOverride referencePointOverride() { return _referencePointOverride; }
    /** Returns a copy of this Tags, with referencePointOverride
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithReferencePointOverride(ReferencePointOverride referencePointOverride) {
      Tags tags = (Tags)clone();
      tags._referencePointOverride = referencePointOverride;
      return tags;
    }
    private ReferencePointOverride _referencePointOverride;

    /** Returns the rehearsal mark offset tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? rehearsalMarkOffset() { return _rehearsalMarkOffset; }
    /** Returns a copy of this Tags, with rehearsalMarkOffset
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithRehearsalMarkOffset(int? rehearsalMarkOffset) {
      Tags tags = (Tags)clone();
      tags._rehearsalMarkOffset = rehearsalMarkOffset;
      return tags;
    }
    private int? _rehearsalMarkOffset;

    /** Returns the rest numeral tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? restNumeral() { return _restNumeral; }
    /** Returns a copy of this Tags, with restNumeral
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithRestNumeral(int? restNumeral) {
      Tags tags = (Tags)clone();
      tags._restNumeral = restNumeral;
      return tags;
    }
    private int? _restNumeral;

    /** Returns true if the silent tag is present, otherwise false.
     */
    public bool silent() { return _silent; }
    /** Returns a copy of this Tags, with the silent
     *  flag set to the given value.
     */
    public Tags cloneWithSilent(bool silent) {
      Tags tags = (Tags)clone();
      tags._silent = silent;
      return tags;
    }
    private bool _silent;

    /** Returns true if the slashed stem tag is present, otherwise false.
     */
    public bool slashedStem() { return _slashedStem; }
    /** Returns a copy of this Tags, with the slashedStem
     *  flag set to the given value.
     */
    public Tags cloneWithSlashedStem(bool slashedStem) {
      Tags tags = (Tags)clone();
      tags._slashedStem = slashedStem;
      return tags;
    }
    private bool _slashedStem;

    /** Returns true if the small size tag is present, otherwise false.
     */
    public bool smallSize() { return _smallSize; }
    /** Returns a copy of this Tags, with the smallSize
     *  flag set to the given value.
     */
    public Tags cloneWithSmallSize(bool smallSize) {
      Tags tags = (Tags)clone();
      tags._smallSize = smallSize;
      return tags;
    }
    private bool _smallSize;

    /** Returns true if the spacing by part tag is present, otherwise false.
     */
    public bool spacingByPart() { return _spacingByPart; }
    /** Returns a copy of this Tags, with the spacingByPart
     *  flag set to the given value.
     */
    public Tags cloneWithSpacingByPart(bool spacingByPart) {
      Tags tags = (Tags)clone();
      tags._spacingByPart = spacingByPart;
      return tags;
    }
    private bool _spacingByPart;

    /** Returns true if the split stem tag is present, otherwise false.
     */
    public bool splitStem() { return _splitStem; }
    /** Returns a copy of this Tags, with the splitStem
     *  flag set to the given value.
     */
    public Tags cloneWithSplitStem(bool splitStem) {
      Tags tags = (Tags)clone();
      tags._splitStem = splitStem;
      return tags;
    }
    private bool _splitStem;

    /** Returns true if the staff name tag is present, otherwise false.
     */
    public bool staffName() { return _staffName; }
    /** Returns a copy of this Tags, with the staffName
     *  flag set to the given value.
     */
    public Tags cloneWithStaffName(bool staffName) {
      Tags tags = (Tags)clone();
      tags._staffName = staffName;
      return tags;
    }
    private bool _staffName;

    /** Returns the staff step tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? staffStep() { return _staffStep; }
    /** Returns a copy of this Tags, with staffStep
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithStaffStep(int? staffStep) {
      Tags tags = (Tags)clone();
      tags._staffStep = staffStep;
      return tags;
    }
    private int? _staffStep;

    /** Returns the thickness tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? thickness() { return _thickness; }
    /** Returns a copy of this Tags, with thickness
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithThickness(int? thickness) {
      Tags tags = (Tags)clone();
      tags._thickness = thickness;
      return tags;
    }
    private int? _thickness;

    /** Returns the tie direction tag
     * or null if the tag is not present.
     */
    public TieDirection? tieDirection() { return _tieDirection; }
    /** Returns a copy of this Tags, with tieDirection
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithTieDirection(TieDirection? tieDirection) {
      Tags tags = (Tags)clone();
      tags._tieDirection = tieDirection;
      return tags;
    }
    private TieDirection? _tieDirection;

    /** Returns the tuplet description tag as a TupletDescription, or null if the tag is not present.
     */
    public TupletDescription tupletDescription() { return _tupletDescription; }
    /** Returns a copy of this Tags, with tupletDescription
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithTupletDescription(TupletDescription tupletDescription) {
      Tags tags = (Tags)clone();
      tags._tupletDescription = tupletDescription;
      return tags;
    }
    private TupletDescription _tupletDescription;

    /** Returns the voice ID tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? voiceID() { return _voiceID; }
    /** Returns a copy of this Tags, with voiceID
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithVoiceID(int? voiceID) {
      Tags tags = (Tags)clone();
      tags._voiceID = voiceID;
      return tags;
    }
    private int? _voiceID;

    /** Returns the width tag as an int? whose value() has the value,
     * or null if the tag is not present.
     */
    public int? width() { return _width; }
    /** Returns a copy of this Tags, with width
     *  changed to the given value, which may be null if the tag
     *  is not present.
     */
    public Tags cloneWithWidth(int? width) {
      Tags tags = (Tags)clone();
      tags._width = width;
      return tags;
    }
    private int? _width;

    /** This return the string representation.  Each tag is preceded by
     * a comma.
     */
    public override string ToString() {
      String result = "";

      if (_absolutePlacement != null)
        result += ", absolute placement=" + _absolutePlacement;
      if (_alternateEnding != null)
        result += ", alternate ending=" + _alternateEnding;
      if (_anchorOverride != null)
        result += ", anchor override=" + _anchorOverride;
      if (_articulationDirection.HasValue)
        result += ", articulation direction=" + _articulationDirection;
      if (_bezierIncoming != null)
        result += ", bezier incoming=" + _bezierIncoming;
      if (_bezierOutgoing != null)
        result += ", bezier outgoing=" + _bezierOutgoing;
      if (_chordSymbolOffset != null)
        result += ", chord symbol offset=" + _chordSymbolOffset;
      if (_customGraphic != null)
        result += ", custom graphic=" + _customGraphic;
      if (_endOfSystem)
        result += ", end of system";
      if (_fannedBeam.HasValue)
        result += ", fanned beam=" + _fannedBeam;
      if (_figuredBass != null)
        result += ", figured bass=" + _figuredBass;
      if (_graceNote != null)
        result += ", grace note=" + _graceNote;
      if (_guitarGridOffset != null)
        result += ", guitar grid offset=" + _guitarGridOffset;
      if (_guitarTablature)
        result += ", guitar tablature";
      if (_height != null)
        result += ", height=" + _height;
      if (_id != null)
        result += ", ID=" + _id;
      if (_invisible)
        result += ", invisible";
      if (_largeSize)
        result += ", large size";
      if (_lineQuality != null)
        result += ", line quality=" + _lineQuality;
      if (_logicalPlacement != null)
        result += ", logical placement=" + _logicalPlacement;
      if (_midiPerformance != null)
        result += ", MIDI performance=" + _midiPerformance;
      if (_multiNodeEndOfSystem)
        result += ", multi node end of system";
      if (_multiNodeStartOfSystem)
        result += ", multi node start of system";
      if (_numberOfFlags != null)
        result += ", number of flags=" + _numberOfFlags;
      if (_numberOfNodes != null)
        result += ", number of nodes=" + _numberOfNodes;
      if (_numberOfStaffLines != null)
        result += ", number of staff lines=" + _numberOfStaffLines;
      if (_ossia != null)
        result += ", ossia=" + _ossia;
      if (_partDescriptionOverride != null)
        result += ", part description override=" + _partDescriptionOverride;
      if (_partID != null)
        result += ", part ID=" + _partID;
      if (_referencePointOverride != null)
        result += ", reference point override=" + _referencePointOverride;
      if (_rehearsalMarkOffset != null)
        result += ", rehearsal mark offset=" + _rehearsalMarkOffset;
      if (_restNumeral != null)
        result += ", rest numeral=" + _restNumeral;
      if (_silent)
        result += ", silent";
      if (_slashedStem)
        result += ", slashed stem";
      if (_smallSize)
        result += ", small size";
      if (_spacingByPart)
        result += ", spacing by part";
      if (_splitStem)
        result += ", split stem";
      if (_staffName)
        result += ", staff name";
      if (_staffStep != null)
        result += ", staff step=" + _staffStep;
      if (_thickness != null)
        result += ", thickness=" + _thickness;
      if (_tieDirection != null)
        result += ", tie direction=" + _tieDirection;
      if (_tupletDescription != null)
        result += ", tuplet description=" + _tupletDescription;
      if (_voiceID != null)
        result += ", voice ID=" + _voiceID;
      if (_width != null)
        result += ", width=" + _width;

      return result;
    }
  }
}