/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A KeySignature has a standard code indicating the number
   * of sharps or flats
   *
   * @author  user
   */
  public class KeySignature : MusicSymbol {
    /** Creates a new KeySignature with the given parameters.
     * 
     * @param standardCode  see getStandardCode()
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public KeySignature(int standardCode, Tags tags)
      : base(tags) {
      _standardCode = standardCode;
    }

    /** Returns the standard code indicating the number of sharps
     * or flats: 
     * 0 = no sharps or flats,
     * 1-7 =1-7 sharps,
     * 8-14 = 1-7 flats,
     * -1 - -7 = 1-7 naturals in the sharp positions,
     * -8 - -14 = 1-7 naturals in the flat positions
     */
    public int getStandardCode() {
      return _standardCode;
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _screenHotspot = null;
      _clefShape = null;
    }

    /** Look backwards in this staff for the previous Clef and return
     * its shape code, or Clef.Shape.G_CLEF if not found.
     * This is needed because the staff step positions for a key
     * signature depend on the clef.
     */
    private Clef.Shape? getClefShape() {
      if (_clefShape.HasValue)
        // already got it.
        return _clefShape;

      Clef clef = (Clef)previousInstanceOfInStaff(typeof(Clef));
      if (clef == null)
        _clefShape = Clef.Shape.G_CLEF;
      else
        _clefShape = clef.getShape();

      return _clefShape;
    }

    /** Get the hotspot for this object in screen coordinates.
     */
    public override FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      // debug: Very preliminary.  Should
      //   take into account the width of this and the previous shape, etc.
      int width = 0;
      // DEBUG: maybe look at actual width of a sharp or flat instead of hardcode to 5
      if (_standardCode >= 1 && _standardCode <= 7)
        // sharps
        width = 5 * _standardCode;
      else if (_standardCode >= 8 && _standardCode <= 14)
        // flats
        width = 5 * (_standardCode - 7);
      // DEBUG: need to handle negative standard code

      // Hotspot is at the left of the key signature
      _screenHotspot = new FinalPoint
        (findLeftPositionedX() - width / 2, getParentTimeSlice().getScreenHotspot().y);

      return _screenHotspot;
    }

    /** Draw this object
     */
    public override void draw(IGraphics graphics) {
      // Look at the clef we are in and shift the staff step given
      //  by SHARP_STEPS, etc. accordingly.  Use G_CLEF as default.
      int clefStepOffset = 0;
      if (getClefShape() == Clef.Shape.F_CLEF)
        clefStepOffset = -2;
      // DEBUG: add support for clefs other than G clef and F clef.

      FinalPoint hotspot = getScreenHotspot();

      // DEBUG: does not handle negative standard code.
      if (_standardCode >= 1 && _standardCode <= 7) {
        // sharps
        for (int i = 0; i < _standardCode; ++i) {
          Accidental._sharp.draw
            (graphics, hotspot.x + i * 5,
             hotspot.y + Notehead.staffStepOffsetY(SHARP_STEPS[i] + clefStepOffset));
        }
      } else if (_standardCode >= 8 && _standardCode <= 14) {
        // flats
        for (int i = 0; i < _standardCode - 7; ++i) {
          Accidental._flat.draw
            (graphics, hotspot.x + i * 5,
             hotspot.y + Notehead.staffStepOffsetY(FLAT_STEPS[i] + clefStepOffset));
        }
      }

    }

    public override string ToString() {
      return "Key-signature, code=" + _standardCode + _tags;
    }

    public override bool isLeftPositionedSymbol() {
      return true;
    }

    private int _standardCode;

    private FinalPoint _screenHotspot;
    private Clef.Shape? _clefShape;

    // Staff steps of sharps and flats for G clef
    private int[] SHARP_STEPS = { 8, 5, 9, 6, 3, 7, 4 };
    private int[] FLAT_STEPS = { 4, 7, 3, 6, 2, 5, 1 };
  }
}