/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
using System.IO;
namespace org.niffty {



  /** A ScoreSetup has a NIFF info, chunk length table and parts list.
   *
   * @author  user
   */
  public class ScoreSetup {
    /** Creates a new ScoreSetup with the given values.
     */
    public ScoreSetup
      (NiffInfo niffInfo, ChunkLengthTable chunkLengthTable,
       PartsList partsList) {
      _niffInfo = niffInfo;
      _chunkLengthTable = chunkLengthTable;
      _partsList = partsList;
    }

    public NiffInfo getNIFFInfo() {
      return _niffInfo;
    }

    public ChunkLengthTable getChunkLengthTable() {
      return _chunkLengthTable;
    }

    public PartsList getPartsList() {
      return _partsList;
    }

    /** This prints the score setup info.
     *
     * @param indent    A string such as "  " to print at the beginning of the line
     * @param output    the PrintStream to print to, such as System.out
     */
    public void print(String indent, TextWriter output) {
      output.WriteLine(indent + "Setup");
      _chunkLengthTable.print(indent + " ", output);
      output.WriteLine(indent + " " + _niffInfo);
      _partsList.print(indent + " ", output);

      // debug: should check for and print optional chunks
    }

    private NiffInfo _niffInfo;
    private ChunkLengthTable _chunkLengthTable;
    private PartsList _partsList;
  }
}