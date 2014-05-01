/*
  This code is part of the Niffty NIFF Music File Viewer. 
  It is distributed under the GNU Public Licence (GPL) version 2.  See
  http://www.gnu.org/ for further details of the GPL.
 */

using System;
namespace org.niffty {

  /** A Rational in an immutable object for representing a rational number
   * with a numerator and denominator.  The ration is always kept in its
   *   simplified form, with the denominator positive.
   * A Rational is immutable.  Therefore you can pass a Rational to another 
   *   method and know that it won't modify it.
   * An IntRatio, on the other hand, can be modified and can do more arithmetic.
   *
   * @see IntRatio
   */
  public class Rational {
    /** Creates new Rational with the given numerator and denominator. 
     * If denominator is zero (which doesn't make sense), this sets
     * numerator to zero and denominator to 1.
     */
    public Rational(int numerator, int denominator) {
      if (denominator == 0) {
        numerator = 0;
        denominator = 1;
      }

      _ratio = new IntRatio(numerator, denominator);
    }

    /** Creates new Rational with the numerator and denominator
     * from the given IntRatio. 
     */
    public Rational(IntRatio r) {
      _ratio = new IntRatio(r);
    }

    public override int GetHashCode() {
      return 0;
    }

    /** Compare this Rational to object and return -1 if this is less than object,
     * 1 if this is greater than object, or zero if they are equal.
     *
     * @throws ClassCastException if object is not an IntRatio or Rational.
     */
    public int compareTo(Object obj) {
      // Just use the compareTo on the IntRatio.
      if (obj is IntRatio)
        return _ratio.compareTo(obj);

      // This will throw the ClassCastException if the object is not Rational
      return _ratio.compareTo(((Rational)obj)._ratio);
    }

    /** Return true of the result of compareTo for the object is zero.
     * If the object is not an IntRatio or Rational, this uses the
     * default Object.Equals().
     * 
     * @see #compareTo
     */
    public override bool Equals(object obj) {
      if (obj is IntRatio || obj is Rational)
        return (_ratio.compareTo(obj) == 0);

      // Just use the default.
      return base.Equals(obj);
    }

    /** Return numerator/denominator as a double value.  
     */
    public double doubleValue() {
      return _ratio.doubleValue();
    }

    /** Returns string as "numerator/denominator"
     */
    public override string ToString() {
      return _ratio.ToString();
    }

    /** Return the numerator
     */
    public int getN() {
      return _ratio.getN();
    }
    /** Return the denominator
     */
    public int getD() {
      return _ratio.getD();
    }

    private IntRatio _ratio;
  }
}