/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A StaffHeader has optional tags: part ID, absolute or logical
   * placement tags, width, height, number of staff lines, 
   * silent (used for ossias).
   *
   * @author  user
   */
  public class StaffHeader {
    /** Creates a new StaffHeader with the given tags.
     *
     * @param tags  the tags for this staff header.  If this is null,
     *          then this uses an empty Tags object.
     */
    public StaffHeader(Tags tags) {
      if (tags == null)
        tags = new Tags();

      _tags = tags;
    }

    /** Returns the Tags object containing the optional tags.
     */
    public Tags getTags() {
      return _tags;
    }

    public override string ToString() {
      return "Staff-header" + _tags;
    }

    private Tags _tags;
  }
}