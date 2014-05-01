/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A ReferencePointOverride has an anchor horizontal, dependent horizontal,
   *    anchor vertical and dependent vertical.  
   * Always used in combination with either Absolute Placement or Logical Placement tags.
   *
   * @author  user
   */
  public class ReferencePointOverride {
    /** Creates new ReferencePointOverride with all zero values */
    public ReferencePointOverride() {
    }

    /** Creates new ReferencePointOverride with the given values. 
     *
     * @param anchorHorizontal     anchor horizontal. Must be DEFAULT, LEFT, RIGHT or HORIZONTAL_CENTER.
     * @param dependentHorizontal     dependent horizontal. Must be DEFAULT, LEFT, RIGHT or HORIZONTAL_CENTER.
     * @param anchorVertical     anchor vertical. Must be DEFAULT, TOP, BOTTOM or VERTICAL_CENTER.
     * @param dependentVertical     anchor vertical. Must be DEFAULT, TOP, BOTTOM or VERTICAL_CENTER.
     */
    public ReferencePointOverride(int anchorHorizontal, int dependentHorizontal,
        int anchorVertical, int dependentVertical) {
      _anchorHorizontal = anchorHorizontal;
      _dependentHorizontal = dependentHorizontal;
      _anchorVertical = anchorVertical;
      _dependentVertical = dependentVertical;
    }

    /** Return the anchor horizontal value, which is DEFAULT, LEFT, RIGHT or HORIZONTAL_CENTER.
     */
    public int getAnchorHorizontal() {
      return _anchorHorizontal;
    }
    /** Return the dependent horizontal value, which is DEFAULT, LEFT, RIGHT or HORIZONTAL_CENTER.
     */
    public int getDependentHorizontal() {
      return _dependentHorizontal;
    }
    /** Return the anchor vertical value, which is DEFAULT, TOP, BOTTOM or VERTICAL_CENTER.
     */
    public int getAnchorVertical() {
      return _anchorVertical;
    }
    /** Return the dependent vertical value, which is DEFAULT, TOP, BOTTOM or VERTICAL_CENTER.
     */
    public int getDependentVertical() {
      return _dependentVertical;
    }

    public const int DEFAULT = 0;

    // For horizontal
    public const int LEFT = 1;
    public const int RIGHT = 2;
    public const int HORIZONTAL_CENTER = 3;

    // For vertical
    public const int TOP = 1;
    public const int BOTTOM = 2;
    public const int VERTICAL_CENTER = 3;

    private String horizontalString(int h) {
      if (h == DEFAULT) return "DEFAULT";
      else if (h == LEFT) return "LEFT";
      else if (h == RIGHT) return "RIGHT";
      else if (h == HORIZONTAL_CENTER) return "HORIZONTAL_CENTER";
      else return "";
    }
    private String verticalString(int v) {
      if (v == DEFAULT) return "DEFAULT";
      else if (v == TOP) return "TOP";
      else if (v == BOTTOM) return "BOTTOM";
      else if (v == VERTICAL_CENTER) return "VERTICAL_CENTER";
      else return "";
    }

    /** Returns string as "(anchor horizontal:value, dependent horizontal:value, 
         anchor vertical:value, dependent vertical:value)"
     */
    public override string ToString() {
      return "(anchor horizontal:" + horizontalString(_anchorHorizontal) +
        ", dependent horizontal:" + horizontalString(_dependentHorizontal) +
        ", anchor vertical:" + verticalString(_anchorVertical) +
        ", dependent vertical:" + verticalString(_dependentVertical) + ")";
    }

    private int _anchorHorizontal;
    private int _dependentHorizontal;
    private int _anchorVertical;
    private int _dependentVertical;
  }
}