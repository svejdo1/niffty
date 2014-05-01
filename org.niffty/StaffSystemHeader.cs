/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {



  /** A StaffSystemHeader has optional tags: absolute or logical placement, 
   *    width and height.
   *
   * @author  user
   */
  public class StaffSystemHeader {
    static readonly String RIFF_ID = "syhd";

    /** Creates a new StaffSystemHeader with the given tags.
     *
     * @param tags  the tags for this staff system header.  If this is null,
     *          then this uses an empty Tags object.
     */
    public StaffSystemHeader(Tags tags) {
      if (tags == null)
        tags = new Tags();

      _tags = tags;
    }

    /** Returns the Tags object containing the optional tags.
     */
    public Tags tags() {
      return _tags;
    }

    public override string ToString() {
      return "System-header" + _tags;
    }

    private Tags _tags;
  }
}