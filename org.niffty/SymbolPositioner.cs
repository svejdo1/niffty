/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System.Collections.Generic;
using System;
namespace org.niffty {

  /**
   *
   */
  public class SymbolPositioner {

    /** Creates new SymbolPositioner */
    public SymbolPositioner() {
    }

    /** Add a new entry to the positions list for the given startTime, with
     * the given symbolsToLeft.  If a position already exists for the given
     * startTime, then set its symbols to left value to the max of the existing
     * value and the given symbolsToLeft.
     */
    public void add(Rational startTime, int symbolsToLeft) {
      _positionTester.startTime = startTime;
      int index = _positions.IndexOf(_positionTester);

      if (index < 0) {
        // No entry yet for the startTime, so add
        _positions.Add(new SymbolPosition(startTime, symbolsToLeft));
        _isSorted = false;
        _symbolCount += 1 + symbolsToLeft;
      } else {
        // A position already exists for this startTime
        var symbolPosition = _positions[index];
        int maxSymbolsToLeft =
          (symbolsToLeft > symbolPosition.symbolsToLeft) ?
             symbolsToLeft : symbolPosition.symbolsToLeft;

        // Increment _symbolCount if necessary
        _symbolCount += (maxSymbolsToLeft - symbolPosition.symbolsToLeft);
        symbolPosition.symbolsToLeft = maxSymbolsToLeft;
      }
    }

    /** Return the total symbol count for all the positions, where for each
     * position, there is 1 for the center symbol plus the symbols to left.
     */
    public int getSymbolCount() {
      return _symbolCount;
    }

    /** Get the position of the symbol for the given startTime.  The
     * position is between 0 and getSymbolCount() and includes all of
     * the previous symbols plus their corresponding symbolsToLeft.
     * If there is no symbol in the symbol position list with the
     * given start time, return -1.
     */
    public int getSymbolPosition(Rational startTime) {
      // Sort if not already sorted.  This also sets the position for each.
      sort();

      _positionTester.startTime = startTime;
      int index = _positions.IndexOf(_positionTester);
      if (index < 0)
        return -1;

      return _positions[index].position;
    }

    /** Sort the entries in _positions using the entry's compareTo,
     * and calculate the position value for each SymbolPosition.
     * If _isSorted is true, this does nothing.
     * We should be using a Java sort function, but these are all
     * post Java 1.1 and we want this to work with browsers which have
     * only 1.1.
     */
    private void sort() {
      if (_isSorted)
        return;

      for (int i = 0; i < _positions.Count - 1; ++i) {
        for (int j = i + 1; j < _positions.Count; ++j) {
          if (_positions[i].compareTo(_positions[j]) > 0) {
            // swap
            var temp = _positions[i];
            _positions[i] = _positions[j];
            _positions[j] = temp;
          }
        }
      }

      // Now that we are sorted, we can calculate the position values.
      int position = 0;
      for (int i = 0; i < _positions.Count; ++i) {
        var symbolPosition = _positions[i];

        position += symbolPosition.symbolsToLeft;
        symbolPosition.position = position;
        // Add one for the symbolPosition we just set.
        position += 1;
      }

      _isSorted = true;
    }

    // Vector of SymbolPosition
    private IList<SymbolPosition> _positions = new List<SymbolPosition>();
    // True if _positions has been sorted
    private bool _isSorted;
    // This is a total count of symbols in _positions including symbols to left.
    private int _symbolCount;
    // This is used to make _positions.indexOf() work
    private SymbolPosition _positionTester = new SymbolPosition(null, 0);

    private class SymbolPosition {
      public SymbolPosition(Rational startTime, int symbolsToLeft) {
        this.startTime = startTime;
        this.symbolsToLeft = symbolsToLeft;
      }

      public override int GetHashCode() {
        return 0;
      }

      /** Compare this SymbolPosition's startTime to the object's startTime
       * (if the object is a SymbolPosition) or to the object (if the object
       * itself is a Rational) and return true if they are equal.
       */
      public override bool Equals(object obj) {
        if (obj is Rational)
          return startTime.Equals(obj);
        if (obj is SymbolPosition)
          return startTime.Equals(((SymbolPosition)obj).startTime);

        // else use the default
        return base.Equals(obj);
      }

      public int compareTo(Object obj) {
        return startTime.compareTo(((SymbolPosition)obj).startTime);
      }

      // It is OK to make these public since this class is not public.
      public Rational startTime;
      public int symbolsToLeft;
      // This is set when the _positions is sorted.  This is the 
      //   symbolsToLeft for this plus 1 for all previous symbols plus
      //   their symbolsToLeft values.
      public int position;
    }
  }
}