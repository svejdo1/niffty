/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System.Collections.Generic;
namespace org.niffty {

  /** A HeirarchyNode is used to manage the relationship between a parent and
   * its child nodes in a heirarchy tree.  Strict relationships are enforced.
   * For example, you can't add an object as a child to a parent node if it already
   * has a parent.  All the methods are protected because it is expected that
   * a class will extend this and proved access methods which deal with
   * the correct type.
   *
   */
  public class HeirarchyNode {

    /** Creates new HeirarchyNode with a null parent.  The parent
     * is assigned when this node is added as the child to another node.
     *
     * @see #addChild
     */
    public HeirarchyNode() {
    }

    /** Add the child to the end of the child node list and set its parent to this node.
     * It is expected that the subclass will define an add function which takes
     * a child of the correct type.  
     *
     * @param child     the node to add as a child to this node.  The child's parent
     *          must be null.
     * @throws HeirarchyException if the child's parent is not null.  This ensures
     *          that the child does not have two parents.
     */
    protected void addChild(HeirarchyNode child) {
      if (child._parent != null)
        throw new HeirarchyException("Child node to be added already has a parent.");

      child._parent = this;

      // The child index is the one that will be assigned by the Vector
      child._index = _childNodes.Count;
      _childNodes.Add(child);
    }

    /** Return the number of nodes in the child node list.
     * It is expected that a subclass will define a method with the correct
     * method name for the child type.
     */
    protected int getChildCount() {
      return _childNodes.Count;
    }

    /** Return the child node at the given index in the child node list.
     * It is expected that a subclass will define a "getter" method which
     * returns the correct class for the child.
     *
     * @throws ArrayIndexOutOfBoundsException if the index is negative or not 
     *    less than the number of nodes in the child node list.
     */
    protected HeirarchyNode getChild(int index) {
      return _childNodes[index];
    }

    /** Return the index if this node in the parent node list, or throws
     * a HeirarchyException if there is no parent.  To avoid the exception,
     * you can check the parent first.
     *
     * @throws HeirarchyException if there is no parent
     */
    public virtual int getIndex() {
      if (_parent == null)
        throw new HeirarchyException("Child has no index because parent is null.");

      return _index;
    }

    /** Return the parent in the heirarchy, or null if this is the top node.
     * This is called getParentNode so that a subclass can define a method called
     * getParentClass() which returns the correct type for the class.
     */
    protected HeirarchyNode getParentNode() {
      return _parent;
    }

    /** Return the previous HeirarchyNode before this one at the same level in the 
     * entire heirarchy.  Return null if this is the first one (or if there is
     * no parent).  This proceeds as follows: If there is no parent, return
     * null.  If there is a child before this one in the parent, return it,
     * otherwise ask the parent for the previousInHeirarchy at its level.  If
     * there is none, return null.  If the previous parent has children,
     * return return its last child. Otherwise, keep asking the parent
     * for its previousInHeirarchy until one is found with children.  If none,
     * return null.
     */
    public HeirarchyNode previousInHeirarchy() {
      if (_parent == null)
        return null;

      int previousIndex = _index - 1;
      if (previousIndex >= 0)
        return _parent.getChild(previousIndex);

      for (HeirarchyNode previousParent = _parent.previousInHeirarchy();
           previousParent != null;
           previousParent = previousParent.previousInHeirarchy()) {
        if (previousParent.getChildCount() > 0)
          return previousParent.getChild(previousParent.getChildCount() - 1);
      }

      return null;
    }

    /** Return the next HeirarchyNode after this one at the same level in the 
     * entire heirarchy.  Return null if this is the last one (or if there is
     * no parent).  This proceeds as follows: If there is no parent, return
     * null.  If there is a child after this one in the parent, return it,
     * otherwise ask the parent for the nextInHeirarchy at its level.  If
     * there is none, return null.  If the next parent has children,
     * return return its first child. Otherwise, keep asking the parent
     * for its nextInHeirarchy until one is found with children.  If none,
     * return null.
     */
    public HeirarchyNode nextInHeirarchy() {
      if (_parent == null)
        return null;

      int nextIndex = _index + 1;
      if (nextIndex < _parent.getChildCount())
        return _parent.getChild(nextIndex);

      for (HeirarchyNode nextParent = _parent.nextInHeirarchy();
           nextParent != null; nextParent = nextParent.nextInHeirarchy()) {
        if (nextParent.getChildCount() > 0)
          return nextParent.getChild(0);
      }

      return null;
    }

    public virtual void draw(IGraphics graphics) {
    }

    private HeirarchyNode _parent;
    // _index always has meaning if there is a _parent
    private int _index;
    // Vector of HeirarchyNode
    private IList<HeirarchyNode> _childNodes = new List<HeirarchyNode>();
  }
}

