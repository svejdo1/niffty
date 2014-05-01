/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

namespace org.niffty {

  /** A FannedBeam has constants for the legal types of fanned beam used in Tags.
   *  This is a "typesafe enum" where you can use a static members like
   *  a constant and compare with == .
   */
  public enum FannedBeam {
    EXPANDING_TOWARD_RIGHT,
    SHRINKING_TOWARD_RIGHT
  }
}