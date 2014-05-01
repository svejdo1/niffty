/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {



  /** ChunkLengthTable
   *
   * @author  user
   */
  public class ChunkLengthTable {
    /** Create a new empty ChunkLengthTable
     */
    public ChunkLengthTable() {
    }

    /** This prints the chunk length table including all table entries.
     *
     * @param indent    A string such as "  " to print at the beginning of the line
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(String indent, TextWriter output) {
      output.WriteLine(indent + "Chunk-length-table");

      // debug: should print entries
    }
  }
}