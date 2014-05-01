/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A PartDescriptionOverride has a MIDI channel, MIDI cable and transpose.  
   * A PartDescriptionOverride's scope depends on the chunk type on which it appears.
   *
   * @author  user
   */
  public class PartDescriptionOverride {
    /** Creates new PartDescriptionOverride with all zero values */
    public PartDescriptionOverride() {
    }

    /** Creates new PartDescriptionOverride with the given values. 
     *
     * @param midiChannel     the MIDI channel. Use -1 to specify none.
     * @param midiCable     the MIDI cable. Use -1 to specify none.
     * @param transpose     Number of halfsteps to transpose this part during playback
     */
    public PartDescriptionOverride(int midiChannel, int midiCable, int transpose) {
      _midiChannel = midiChannel;
      _midiCable = midiCable;
      _transpose = transpose;
    }

    /** Return the MIDI channel value, or -1 specifies none.
     */
    public int getMidiChannel() {
      return _midiChannel;
    }
    /** Return the MIDI cable value, or -1 specifies none.
     */
    public int getMidiCable() {
      return _midiCable;
    }
    /** Return the transpose value.
     */
    public int getTranspose() {
      return _transpose;
    }

    /** Returns string as "(MIDI channel:value, MIDI cable:value, transpose:value)"
     */
    public override string ToString() {
      return "(MIDI channel:" + _midiChannel + ", MIDI cable:" + _midiCable +
        ", transpose:" + _transpose + ")";
    }

    private int _midiChannel;
    private int _midiCable;
    private int _transpose;
  }
}