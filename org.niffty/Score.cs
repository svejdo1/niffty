/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System.IO;
namespace org.niffty {



  /** A Score object encapsulates the information in a NIFF file.
   * A Score object has a ScoreSetup and a ScoreData object
   *
   * @author  user
   * @see ScoreSetup
   * @see ScoreData
   */
  public class Score {
    /** Creates a new Score with the given setup and data.
     *
     * @param setup  the ScoreSetup for this score.
     * @param data  the ScoreData for this score.  It is an error if
     *        this already belongs to a Score object.
     * @throws HeirarchyException if the data argument has already
     *      been added to a Score
     */
    public Score(ScoreSetup setup, ScoreData data) {
      if (data.getParentScore() != null)
        throw new HeirarchyException
          ("Cannot add ScoreData to Score because it already belongs to a Score object.");

      _setup = setup;
      _data = data;
      _data.setParentScore(this);
    }

    public ScoreSetup getSetup() {
      return _setup;
    }

    public ScoreData getData() {
      return _data;
    }

    /** This is automatically called after the object is modified to force
     *  this and all child objects to recompute their values when the "get"
     *    method is called for the value.
     */
    public void invalidate() {
      _data.invalidate();

      _stavesScreenHotspot = null;
    }

    /** Return the origin in screen coordinates of the top staff.
     */
    public FinalPoint getStavesScreenHotspot() {
      if (_stavesScreenHotspot != null)
        return _stavesScreenHotspot;

      _stavesScreenHotspot = new FinalPoint(15, 25);
      return _stavesScreenHotspot;
    }

    /** Return the total height of the working area for all the staves
     * on the page in screen pixels.
     */
    public int getStavesHeight() {
      return 800;
    }

    /** Return the width of the working area for the staves
     * on the page in screen pixels.  This is the same as the
     * width of one staff.
     */
    public int getStavesWidth() {
      return 620;
    }

    /** This prints the score info including setup and data sections.
     * 
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(TextWriter output) {
      output.WriteLine("NIFF Score");
      _setup.print(" ", output);
      _data.print(" ", output);
    }

    private ScoreSetup _setup;
    private ScoreData _data;

    private FinalPoint _stavesScreenHotspot;
  }
}