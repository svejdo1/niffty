/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** An exception class for returning format errors in a RIFF file.
   *
   * @author  user
   */
  public class RiffFormatException : Exception {

    /** Creates new RIFFFormatException 
     * 
     * @param message   the exception detail message
     */
    public RiffFormatException(String message)
      : base(message) {
    }
  }
}