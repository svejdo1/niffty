/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.Collections.Generic;
namespace org.niffty {

  /** MusicSymbol is a base class for music symbol classes
   * used in a Staff. It is usually not constructed directly.
   * MusicSymbol is used as a type for the objects which
   * follow a TimeSlice in a Staff.  Every MusicSymbol has
   * a parent TimeSlice which is the object in which the MusicSymbol
   * is listed.
   *
   * @author  user
   */
  public class MusicSymbol : HeirarchyNode, Anchored {

    /** Creates a new MusicSymbol with the given tags.
     * The parent will be set when this is added as a child to the parent.
     * 
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public MusicSymbol(Tags tags) {
      if (tags == null)
        tags = new Tags();

      _tags = tags;
    }

    /** Return the parent TimeSlice.  The parent is the TimeSlice object which 
     * created this Music Symbol
     * and to which this MusicSymbol belongs.  This is not to be confused
     * with the _anchor which could be another MusicSymbol.
     */
    public TimeSlice getParentTimeSlice() {
      return (TimeSlice)base.getParentNode();
    }

    /** Returns the Tags object containing the optional tags.
     */
    public Tags getTags() {
      return _tags;
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     * The invalidate() method for MusicSymbol does nothing.
     * A subclass deriving from this MusicSymbol should override this to
     * do its own invalidate operations
     */
    public virtual void invalidate() {
    }

    /** This is a utility function to return the anchor
     * of the previous MusicSymbol
     * in the parent TimeSlice, or to return the parent time slice if there
     * are no previous music symbols.
     */
    protected Anchored findDefaultAnchor() {
      if (getIndex() == 0)
        return getParentTimeSlice();
      else
        return getParentTimeSlice().getMusicSymbol(getIndex() - 1);
    }

    /** This is a utility function to return the screen X position of
     * a left-positioned symbol (as defined by isLeftPositioned()).
     * This finds the hotspot of the next symbol in the time slice
     * which isLeftPositioned() (or the hotspot of the time slice itself
     * if there are no more) and subtracts from its X value
     * the (measure width - 10) / (the measure symbol count - 1).
     */
    protected int findLeftPositionedX() {
      FinalPoint nextHotspot = null;
      for (int i = getIndex() + 1; i < getParentTimeSlice().getMusicSymbolCount(); ++i) {
        MusicSymbol nextSymbol = getParentTimeSlice().getMusicSymbol(i);
        if (nextSymbol.isLeftPositionedSymbol()) {
          nextHotspot = nextSymbol.getScreenHotspot();
          break;
        }
      }
      if (nextHotspot == null)
        // No next left positioned symbol, so use the time slice
        nextHotspot = getParentTimeSlice().getScreenHotspot();

      MeasureStartTimeSlice measure = getParentTimeSlice().getParentMeasureStart();
      // Note that we know measure.getSymbolCount() is at least one
      return nextHotspot.x -
        (measure.getWidth() - 10) / (measure.getSymbolCount() - 1);
    }

    /** This is a method of the Anchored interface.
        As a default, just return the hotspot of the parent time slice.
        It is expected that the subclass will override this.
     */
    public virtual FinalPoint getScreenHotspot() {
      return getParentTimeSlice().getScreenHotspot();
    }

    /** Return true if this is a symbol, like a cleff change,
     * which appears to the left of the main symbol in the time slice,
     * like the stem.  The default return value is false but Cleff,
     * TimeSignature, etc. should override and return true.
     */
    public virtual bool isLeftPositionedSymbol() {
      return false;
    }

    /** Return the previous instance of the given class type looking
     * backwards from this MusicSymbol in the parent TimeSlice.
     * For example, previousInstanceOf(typeof(Notehead)) gets the
     * previous Notehead.
     *
     * @param desiredClass  Class object for the MusicSymbol being sought
     * @return the previous MusicSymbol of the desired class in the
     *          parent TimeSlice, or null if not found.
     */
    public MusicSymbol previousInstanceOf(Type desiredClass) {
      for (int i = getIndex() - 1; i >= 0; --i) {
        var result = getParentTimeSlice().getMusicSymbol(i);
        if (desiredClass.IsInstanceOfType(result))
          return result;
      }

      return null;
    }

    /** Return the previous instance of the given class type looking
     * backwards from this MusicSymbol in the entire score.
     * For example, previousInstanceOfInScore(Beam.class) gets the
     * previous Beam.  
     *
     * @param desiredClass  Class object for the MusicSymbol being sought
     * @return the previous MusicSymbol of the desired class in the
     *          entire Score, or null if not found.
     */
    public MusicSymbol previousInstanceOfInScore(Type desiredClass) {
      for (HeirarchyNode previous = previousInHeirarchy(); previous != null;
           previous = previous.previousInHeirarchy()) {
        if (desiredClass.IsInstanceOfType(previous))
          return (MusicSymbol)previous;
      }

      return null;
    }

    /** Return the previous instance of the given class type looking
     * backwards from this MusicSymbol at all previous MusicSymbols
     * in the same staff.
     * For example, previousInstanceOfInStaff(Notehead.class) gets the
     * previous Notehead in the staff.
     *
     * @param desiredClass  Class object for the MusicSymbol being sought
     * @return the previous MusicSymbol of the desired class in the
     *          same staff, or null if not found.
     */
    public MusicSymbol previousInstanceOfInStaff(Type desiredClass) {
      MusicSymbol previous = previousInstanceOfInScore(desiredClass);
      // Check if this is the same staff
      if (getParentTimeSlice().getParentMeasureStart().getParentStaff() ==
          previous.getParentTimeSlice().getParentMeasureStart().getParentStaff())
        return previous;

      // There was none in the same staff.
      return null;
    }

    /** Return the next instance of the given class type looking
     * forwards from this MusicSymbol in the entire score.
     * For example, nextInstanceOfInScore(Beam.class) gets the
     * next Beam.  To find the next instance of the same class,
     * see nextInstanceOfInScore()
     *
     * @param desiredClass  Class object for the MusicSymbol being sought
     * @return the next MusicSymbol of the desired class in the
     *          entire Score, or null if not found.
     */
    public MusicSymbol nextInstanceOfInScore(Type desiredClass) {
      for (HeirarchyNode next = nextInHeirarchy(); next != null;
           next = next.nextInHeirarchy()) {
        if (desiredClass.IsInstanceOfType(next))
          return (MusicSymbol)next;
      }

      return null;
    }

    /** This is a utility method which returns a list of nodes in this ID group for 
     * objects of the same type.  If there is no numberOfNodes tag
     * (which defines the first node in the multi-node symbol), this returns
     * a new empty Vector. If there is a numberOfNodes tag, this creates a new Vector 
     * and adds the given number of nodes of the same type and with the
     * same ID tag, including this first node, and stops searching after that
     * number have been found.  If this reaches the end of the Score before
     * the required number have been found, this also stops, and the returned
     * Vector will not be complete. If a subsequent node of
     * the same type and ID, which also has a numberOfNodes tag, is encountered
     * before the number of nodes are found, this does not add it and stops searching.
     */
    protected IList<MusicSymbol> findMultiNodes() {
      var result = new List<MusicSymbol>();

      if (_tags.numberOfNodes() == null)
        // This is not a starting node.
        return result;
      if (_tags.id() == null)
        // This shouldn't really happen, so just return null.
        return result;

      int numberOfNodes = _tags.numberOfNodes().Value;
      int id = _tags.id().Value;

      // Add this as the first one.
      result.Add(this);

      // Note that nextInstanceOfInScore(getClass()) only returns the same type
      for (MusicSymbol node = nextInstanceOfInScore(GetType());
           node != null && result.Count < numberOfNodes;
           node = node.nextInstanceOfInScore(GetType())) {
        if (!node.getTags().id().HasValue || node.getTags().id().Value != id)
          continue;

        if (node.getTags().numberOfNodes() != null)
          // Unexpectedly starting a new multi-node symbol.  Just 
          //   return what we have so far.
          return result;

        result.Add(node);
      }

      return result;
    }

    // This is usually loaded by a subclass during its constructor.
    protected Tags _tags;
  }
}