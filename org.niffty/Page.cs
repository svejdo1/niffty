/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {

  /** A Page encapsulates the page information of a ScoreData.
   * A Page has a PageHeader and any number of
   * StaffSystem, NIFFFontSymbol, NIFFCustomGraphicSymbol, NIFFText,
   * or NIFFLine.
   *
   * @author  user
   * @see ScoreData
   */
  public class Page : HeirarchyNode {
    /** Creates a new Page with the given PageHeader and an empty StaffSystem list.
     *  The parent pointer will be set when this is added to a ScoreData.
     *
     * @param pageHeader the header for this Page
     */
    public Page(PageHeader pageHeader) {
      _pageHeader = pageHeader;
    }

    /** Add the given staffSystem to the StaffSystem list.
     * This does not call invalidate(), but you may need to before displaying.
     *
     * @param staffSystem  the StaffSystem to add.  It is an error if
     *        this is already the child of an object.
     * @throws HeirarchyException if the staffSystem has already
     *      been added as a child to another object.
     * @see #invalidate
     */
    public void addSystem(StaffSystem staffSystem) {
      addChild(staffSystem);
    }

    /** Return the parent ScoreData.
     */
    public ScoreData getParentData() {
      return (ScoreData)getParentNode();
    }

    public PageHeader getPageHeader() {
      return _pageHeader;
    }

    /** Return the number of staff systems in the staff system list.
     */
    public int getSystemCount() {
      return getChildCount();
    }

    /** Return the StaffSystem in the staff system list at the given index.
     *
     * @throws ArrayIndexOutOfBoundsException if the index is negative or not 
     *    less than the number of nodes in the child node list.
     */
    public StaffSystem getSystem(int index) {
      return (StaffSystem)getChild(index);
    }

    /** Return the Y distance in screen coordinates between one staff and
     * the next.  This is a double instead of an int so that we
     * don't lose precision.
     */
    public double getStaffSpacingY() {
      if (_staffSpacingY != 0.0)
        // We never allow 0 as a valid value, so we know this is good.
        return _staffSpacingY;

      // Count the total number of staves on this page.
      int staffCount = 0;
      for (int i = 0; i < getSystemCount(); ++i)
        staffCount += getSystem(i).getStaffCount();
      if (staffCount == 0) {
        // This is unexpected. No staves will be drawn.  Just set to 1.
        _staffSpacingY = 1.0;
        return _staffSpacingY;
      }

      _staffSpacingY = ((double)getParentScore().getStavesHeight()) / staffCount;
      // This prevents spreading out the staves if there are just a few.
      _staffSpacingY = Math.Min(_staffSpacingY, 80.0);
      return _staffSpacingY;
    }

    /** This is automatically called after the object is modified to force
     *  this and all child objects to recompute their values when the "get"
     *    method is called for the value.
     */
    public void invalidate() {
      for (int i = 0; i < getSystemCount(); ++i)
        getSystem(i).invalidate();

      _staffSpacingY = 0.0;
    }

    /** Return the ultimate Score object of which this is a child.
     */
    public Score getParentScore() {
      // The parent of the ScoreData is the Score.
      return getParentData().getParentScore();
    }

    public override void draw(IGraphics graphics) {
      for (int i = 0; i < getSystemCount(); ++i)
        getSystem(i).draw(graphics);
    }

    /** This prints the page info including all staff systems.
     *
     * @param indent    A string such as "  " to print at the beginning of the line
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(String indent, TextWriter output) {
      output.WriteLine(indent + "Page");
      output.WriteLine(indent + " " + _pageHeader);

      for (int i = 0; i < getSystemCount(); ++i)
        getSystem(i).print(indent + " ", output);
    }

    private PageHeader _pageHeader;
    private double _staffSpacingY;
  }
}