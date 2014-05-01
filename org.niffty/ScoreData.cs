/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {

  /** A ScoreData object encapsulates the data section of a NIFF file.
   * A ScoreData has zero or more of Page.
   *
   * @author  user
   * @see Page
   */
  public class ScoreData : HeirarchyNode {
    /** Creates a new ScoreData with an empty page list.
     *  The parent Score pointer will be set when this is added to a Score.
     */
    public ScoreData() {
      // The parent Score is not a HeirarchyNode, so HeirarchyNode._parent
      //  remains null.
    }

    /** Add the given Page to the page list.  After adding, this also calls
     * invalidate().
     *
     * @param page  the Page to add.  It is an error if
     *        this is already the child of an object.
     * @throws HeirarchyException if the page has already
     *      been added as a child to another object.
     * @see #invalidate
     */
    public void addPage(Page page) {
      addChild(page);
      invalidate();
    }

    /** Set the parent score.  This is a protected method because only
     *  Score (which is in the same package) should call this.
     *
     * @param parentScore  the parentScore.getData() must already
     *      be this ScoreData, and this ScoreData cannot already have
     *      a parent.  In this way, we ensure that only the Score can
     *      call this.
     * @param HierarchyException  if this already has a parent, or if
     *      parentScore does not already have this as the child.
     */
    public void setParentScore(Score parentScore) {
      if (_parentScore != null)
        throw new HeirarchyException
          ("Cannot add the ScoreData to a parent Score because it already has a parent");
      if (parentScore.getData() != this)
        throw new HeirarchyException
          ("Cannot add the ScoreData to a parent Score because the Score does not have this as its ScoreData");

      _parentScore = parentScore;
    }

    /** Return the parent Score, or null if this hasn't been added to a 
     *  Score object yet.
     */
    public Score getParentScore() {
      // There is only one ScoreData, so the parent Score is not a HeirarchyNode,
      //  so we keep track of the _parent ourselves, and 
      //  HeirarchyNode._parent is null.
      return _parentScore;
    }

    /** Return the number of pages in the page list.
     */
    public int getPageCount() {
      return getChildCount();
    }

    /** Return the Page in the page list at the given index.
     *
     * @throws ArrayIndexOutOfBoundsException if the index is negative or not 
     *    less than the number of nodes in the child node list.
     */
    public Page getPage(int index) {
      return (Page)getChild(index);
    }

    /** This is automatically called after the object is modified to force
     *  this and all child objects to recompute their values when the "get"
     *    method is called for the value.
     */
    public void invalidate() {
      for (int i = 0; i < getPageCount(); ++i)
        getPage(i).invalidate();

      // We have no locally cached data
    }

    /** This prints the score data info including all pages.
     *
     * @param indent    A string such as "  " to print at the beginning of the line
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(String indent, TextWriter output) {
      output.WriteLine(indent + "Data");

      for (int i = 0; i < getPageCount(); ++i)
        getPage(i).print(indent + " ", output);
    }

    // There is only one ScoreData, so the parent Score does not keep
    // a list of ScoreData objects, so it does not need to be a HeirarchyNode.
    private Score _parentScore;
  }
}