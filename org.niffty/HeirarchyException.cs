/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A HeirarchyException is thrown by HeirarchyNode if the heirarchical relationship
   * between and parent object and its children is inconsistent.
   * For example, this is thrown if the index of a child in its parent's
   * child list is requrested, but the child has no parent.
   *
   * @see HeirarchyNode
   */
  public class HeirarchyException : Exception {

    /** Creates new HeirarchyException
     * 
     * @param message   the exception detail message
     */
    public HeirarchyException(String message)
      : base(message) {
    }

  }
}
