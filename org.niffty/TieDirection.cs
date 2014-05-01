/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

namespace org.niffty {

  /** An TieDirection has constants for the legal types of tie direction use in Tags.
   *  This is a "typesafe enum" where you can use a static members like
   *  a constant and compare with == .
   */
  public enum TieDirection {
    ROUNDED_ABOVE,
    ROUNDED_BELOW
  }
}