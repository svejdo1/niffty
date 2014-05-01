/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** An LineQuality has constants for the legal types of line quality used in Tags.
   *  This is a "typesafe enum" where you can use a static members like
   *  a constant and compare with == .
   */
  public enum LineQuality {
    NO_LINE,
    DOTTED_LINE,
    DASHED_LINE,
    WAVY_LINE
  }
}