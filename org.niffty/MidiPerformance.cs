/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A MIDIPerformance has a start time, duration, pitch and velocity.  
   * A MIDIPerformance is used as a tag on a Notehead.
   *
   * @author  user
   */
  public class MidiPerformance {
    /** Creates new MIDIPerformance with all zero values */
    public MidiPerformance() {
    }

    /** Creates new MIDIPerformance with the given values. 
     *
     * @param startTime     start time, given in MIDI ticks, as an offset from the 
     *           time-slice (Note that NIFF Spec 6.a.3 has this as a RATIONAL
     *           but this has been corrected to a LONG.)
     * @param duration      duration given in MIDI ticks
     *          (Note that NIFF Spec 6.a.3 has this as a RATIONAL
     *           but this has been corrected to a LONG.)
     * @param pitch     MIDI pitch (0-127)
     * @param velocity  MIDI velocity (0-127)
     */
    public MidiPerformance(int startTime, int duration, int pitch, int velocity) {
      _startTime = startTime;
      _duration = duration;
      _pitch = pitch;
      _velocity = velocity;
    }

    /** Return the start time value. (Note that NIFF Spec 6.a.3 has this as a RATIONAL
     *           but this has been corrected to a LONG.)
     */
    public int getStartTime() {
      return _startTime;
    }
    /** Return the duration value.  (Note that NIFF Spec 6.a.3 has this as a RATIONAL
     *           but this has been corrected to a LONG.)
     */
    public int getDuration() {
      return _duration;
    }

    /** Return the pitch value
     */
    public int getPitch() {
      return _pitch;
    }

    /** Return the velocity value
     */
    public int getVelocity() {
      return _velocity;
    }

    /** Returns string as "(start time:value, duration:value, pitch:value,
     *      velocity:value)"
     */
    public override string ToString() {
      return "(start time:" + _startTime + ", duration:" + _duration +
        ", pitch:" + _pitch + ", velocity:" + _velocity + ")";
    }

    private int _startTime;
    private int _duration;
    private int _pitch;
    private int _velocity;
  }
}