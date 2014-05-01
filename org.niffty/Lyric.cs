/*
  This code is part of the Niffty NIFF Music File Viewer.
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A Lyric has a text, a lyric verse ID and optional tags: font ID,
   * absolute placement and part ID.
   *
   * @author  user
   */
  public class Lyric : MusicSymbol {
    /** Creates a new Lyric with the given parameters.
     *
     * @param text  the lyric text
     * @param lyricVerseID  see getLyricVerseID()
     * @param tags  the tags for this music symbol.  If this is null,
     *          then this uses an empty Tags object.
     */
    public Lyric(String text, int lyricVerseID, Tags tags)
      : base(tags) {
      _text = text;
      _lyricVerseID = lyricVerseID;
    }

    /** Returns the text
     */
    public String geText() {
      return _text;
    }

    /** Returns the lyricVerseID
     */
    public int getLyricVerseID() {
      return _lyricVerseID;
    }

    /** This is automatically called after the object is modified to force
     *  this to recompute all its values when the "get"
     *    method is called for the value.
     */
    public override void invalidate() {
      _anchor = null;
      _screenHotspot = null;
    }

    /** Return the anchor as the previous Notehead.
     * Even though this is private, we always use getAnchor() instead of
     * looking at _anchor directly, because this allows us to
     * delay computing it as long as possible.
     */
    private Anchored getAnchor() {
      if (_anchor != null)
        return _anchor;

      MusicSymbol previousNotehead = previousInstanceOf(typeof(Notehead));
      if (previousNotehead == null)
        // DEBUG: maybe this should be an error
        _anchor = findDefaultAnchor();
      else
        _anchor = previousNotehead;
      return _anchor;
    }

    /** Get the hotspot for this object in screen coordinates.
     *  Note that the hotspot for a Lyric is the center of the text.
     */
    public override FinalPoint getScreenHotspot() {
      if (_screenHotspot != null)
        return _screenHotspot;

      // Debug: Maybe we need to find the lowest notehead, not just the anchor.
      Anchored anchor = getAnchor();
      int staffStep;
      if (anchor is Notehead)
        staffStep = ((Notehead)anchor).getStaffStep();
      else
        // Not a Notehead, but staffStep will be adjusted below.
        staffStep = 0;

      // Adjust so we put the text below the note
      staffStep -= 6;

      if (staffStep > -8)
        // Note is high enough that we need to keep the text below the staff
        staffStep = -8;

      _screenHotspot = new FinalPoint
         (anchor.getScreenHotspot().x,
          getParentTimeSlice().getScreenHotspot().y +
            Notehead.staffStepOffsetY(staffStep));

      return _screenHotspot;
    }

    /** Draw this object
     */
    public override void draw(IGraphics graphics) {
      FinalPoint hotspot = getScreenHotspot();
      // The hotspot is defined as the center of the text.
      // Debug: need to properly compute the screen dimensions of the text.
      graphics.DrawString
        (_text, hotspot.x - 3 * _text.Length, hotspot.y + 4);
    }

    public override string ToString() {
      return "Lyric, text=\"" + _text + "\", lyricVerseID=" +
          _lyricVerseID + _tags;
    }

    private Anchored _anchor;
    private FinalPoint _screenHotspot;

    private String _text;
    private int _lyricVerseID;
  }
}