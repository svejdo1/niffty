/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A LogicalPlacement tag has horizontal, vertical and proximity values.
   * Note that these are not screen pixels but values like "left" and "right".
   *
   * @author  user
   */
  public class LogicalPlacement {
    /** Creates new LogicalPlacement with zero horizontal, vertical and proximity */
    public LogicalPlacement() {
    }

    /** Creates new LogicalPlacement with the given horizontal, vertical and proximity 
     * 
     * @param horizontal    horizontal placement. Must be DEFAULT, LEFT, RIGHT, STEM_SIDE, NOTE_SIDE or CENTERED.
     * @param vertical    vertical placement. Must be DEFAULT, ABOVE, BELOW, STEM_SIDE, NOTE_SIDE or CENTERED.
     * @param proximity    proximity. Must be DEFAULT, TOUCHING, OFFSET_SLIGHTLY.
     */
    public LogicalPlacement(int horizontal, int vertical, int proximity) {
      _horizontal = horizontal;
      _vertical = vertical;
      _proximity = proximity;
    }

    /** Return the horizontal value which is 
     *    DEFAULT, LEFT, RIGHT, STEM_SIDE, NOTE_SIDE or CENTERED
     */
    public int getHorizontal() {
      return _horizontal;
    }
    /** Return the vertical value which is
     *    DEFAULT, ABOVE, BELOW, STEM_SIDE, NOTE_SIDE or CENTERED.
     */
    public int getVertical() {
      return _vertical;
    }
    /** Return the proximity value which is DEFAULT, TOUCHING, OFFSET_SLIGHTLY.
     */
    public int getProximity() {
      return _proximity;
    }

    private String horizontalString() {
      if (_horizontal == DEFAULT) return "DEFAULT";
      else if (_horizontal == LEFT) return "LEFT";
      else if (_horizontal == RIGHT) return "RIGHT";
      else if (_horizontal == STEM_SIDE) return "STEM_SIDE";
      else if (_horizontal == NOTE_SIDE) return "NOTE_SIDE";
      else if (_horizontal == CENTERED) return "CENTERED";
      else return "";
    }
    private String verticalString() {
      if (_vertical == DEFAULT) return "DEFAULT";
      else if (_vertical == ABOVE) return "ABOVE";
      else if (_vertical == BELOW) return "BELOW";
      else if (_vertical == STEM_SIDE) return "STEM_SIDE";
      else if (_vertical == NOTE_SIDE) return "NOTE_SIDE";
      else if (_vertical == CENTERED) return "CENTERED";
      else return "";
    }
    private String proximityString() {
      if (_proximity == DEFAULT) return "DEFAULT";
      else if (_proximity == TOUCHING) return "TOUCHING";
      else if (_proximity == OFFSET_SLIGHTLY) return "OFFSET_SLIGHTLY";
      else return "";
    }

    /** Returns string as "(horizontal:value, vertical:value, proximity:value)"
     */
    public override string ToString() {
      return "(horizontal:" + horizontalString() + ", vertical:" + verticalString() +
        ", proximity:" + proximityString() + ")";
    }

    // for all
    public const int DEFAULT = 0;
    public const int STEM_SIDE = 3;
    public const int NOTE_SIDE = 4;
    public const int CENTERED = 5;

    // for horizontal
    public const int LEFT = 1;
    public const int RIGHT = 2;

    // for vertical
    public const int ABOVE = 1;
    public const int BELOW = 2;

    // for proximity
    public const int TOUCHING = 1;
    public const int OFFSET_SLIGHTLY = 2;

    private int _horizontal;
    private int _vertical;
    private int _proximity;
  }
}