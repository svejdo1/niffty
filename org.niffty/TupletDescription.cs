/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A TupletDescription tag has a, b, c, d and grouping symbol values.  A TupletDescription tag
   * is used on the first Tuplet chunk of the tuplet multi-node symbol.
   *
   * @author  user
   */
  public class TupletDescription {
    /** Creates new TupletDescription with zero values */
    public TupletDescription() {
    }

    /** Creates new TupletDescription with the given values
     * 
     * @param a    the a transformation value
     * @param b    the b transformation value
     * @param c    the c transformation value
     * @param d    the d transformation value
     * @param groupingSymbol    grouping symbol. Must be DEFAULT, 
     *          NUMBER_ONLY, NUMBER_WITH_BROKEN_SLUR, NUMBER_OUTSIDE_SLUR,
     *          NUMBER_INSIDE_SLUR, NUMBER_WITH_BROKEN_BRACKET, NUMBER_OUTSIDE_BRACKET,
     *          NUMBER_INSIDE_BRACKET, BRACKET_ONLY, SLUR_ONLY or NO_SYMBOL.
     */
    public TupletDescription(int a, int b, int c, int d, int groupingSymbol) {
      _a = a;
      _b = b;
      _c = c;
      _d = d;
      _groupingSymbol = groupingSymbol;
    }

    /** Return the A value of the AB transformation ratio.
     */
    public int getA() {
      return _a;
    }
    /** Return the B value of the AB transformation ratio.
     */
    public int getB() {
      return _b;
    }
    /** Return the C value of the CD transformation ratio.
     */
    public int getC() {
      return _c;
    }
    /** Return the D value of the CD transformation ratio.
     */
    public int getD() {
      return _d;
    }
    /** Return the grouping symbol which isDEFAULT, 
     *          NUMBER_ONLY, NUMBER_WITH_BROKEN_SLUR, NUMBER_OUTSIDE_SLUR,
     *          NUMBER_INSIDE_SLUR, NUMBER_WITH_BROKEN_BRACKET, NUMBER_OUTSIDE_BRACKET,
     *          NUMBER_INSIDE_BRACKET, BRACKET_ONLY, SLUR_ONLY or NO_SYMBOL.
     */
    public int getGroupingSymbol() {
      return _groupingSymbol;
    }

    public const int DEFAULT = 0;
    public const int NUMBER_ONLY = 1;
    public const int NUMBER_WITH_BROKEN_SLUR = 2;
    public const int NUMBER_OUTSIDE_SLUR = 3;
    public const int NUMBER_INSIDE_SLUR = 4;
    public const int NUMBER_WITH_BROKEN_BRACKET = 5;
    public const int NUMBER_OUTSIDE_BRACKET = 6;
    public const int NUMBER_INSIDE_BRACKET = 7;
    public const int BRACKET_ONLY = 8;
    public const int SLUR_ONLY = 9;
    public const int NO_SYMBOL = 10;

    private String groupingSymbolString() {
      if (_groupingSymbol == DEFAULT) return "DEFAULT";
      else if (_groupingSymbol == NUMBER_ONLY) return "NUMBER_ONLY";
      else if (_groupingSymbol == NUMBER_WITH_BROKEN_SLUR) return "NUMBER_WITH_BROKEN_SLUR";
      else if (_groupingSymbol == NUMBER_OUTSIDE_SLUR) return "NUMBER_OUTSIDE_SLUR";
      else if (_groupingSymbol == NUMBER_INSIDE_SLUR) return "NUMBER_INSIDE_SLUR";
      else if (_groupingSymbol == NUMBER_WITH_BROKEN_BRACKET) return "NUMBER_WITH_BROKEN_BRACKET";
      else if (_groupingSymbol == NUMBER_OUTSIDE_BRACKET) return "NUMBER_OUTSIDE_BRACKET";
      else if (_groupingSymbol == NUMBER_INSIDE_BRACKET) return "NUMBER_INSIDE_BRACKET";
      else if (_groupingSymbol == BRACKET_ONLY) return "BRACKET_ONLY";
      else if (_groupingSymbol == SLUR_ONLY) return "SLUR_ONLY";
      else if (_groupingSymbol == NO_SYMBOL) return "NO_SYMBOL";
      else return "";
    }

    /** Returns string as "(a:value, b:value, c:value, d:value, grouping symbol:value)"
     */
    public override string ToString() {
      return "(a:" + _a + ", b:" + _b + ", c:" + _c + ", d:" + _d +
        ", grouping symbol:" + groupingSymbolString() + ")";
    }

    private int _a;
    private int _b;
    private int _c;
    private int _d;
    private int _groupingSymbol;
  }
}