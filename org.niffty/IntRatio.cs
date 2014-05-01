using System;
namespace org.niffty {

/** IntRatio is a mathematical class for representing a rational number.
 * The ratio is alway kept in its simplified form with the denominator positive.
 * You can modify the value of an IntRatio and do arithmetic on it.
 * A Rational, on the other hand, holds a numerator and denominator but is
 * immutable.
 *
 * @see Rational
 */
public class IntRatio {

    /** Creates new IntRatio with the given numerator and denominator.
     *
     * @throws ArithmeticException if denominator is zero.
     */
    public IntRatio(int numerator, int denominator) {
        if (denominator == 0)
            throw new ArithmeticException ("IntRatio denominator cannot be zero");
        
        n = numerator;
        d = denominator;
        // No need to simplify until necessary.
    }

    /** Creates new IntRatio from the given IntRatio.
     */
    public IntRatio(IntRatio r) {
        n = r.n;
        d = r.d;
        // No need to simplify until necessary.
    }

    /** Creates new IntRatio from the given Rational.
     */
    public IntRatio(Rational r) {
        n = r.getN();
        d = r.getD();
    }

    /** Set this ratio with the given numerator and denominator.
     *
     * @throws ArithmeticException if denominator is zero.
     */
    public void set(int numerator, int denominator) {
        if (denominator == 0)
            throw new ArithmeticException ("IntRatio denominator cannot be zero");
        
        n = numerator;
        d = denominator;
        
        // Must recalculate the double value.
        _gotDoubleValue = false;
        // No need to simplify until necessary.
        _simplified = false;
    }

    /** Set the value of this IntRatio to the given IntRatio.
     */
    public void set(IntRatio r) {
        n = r.n;
        d = r.d;
        
        // Must recalculate the double value.
        _gotDoubleValue = false;
        // No need to simplify until necessary.
        _simplified = false;
    }

    /** Set the value of this IntRatio to the given Ratioanal.
     */
    public void set(Rational r) {
        n = r.getN();
        d = r.getD();
        
        // Must recalculate the double value.
        _gotDoubleValue = false;
        // No need to simplify until necessary.
        _simplified = false;
    }

    /** Return the numerator
     */
    public int getN() {
        // This will simplify the ratio if it is not already.
        simplify();
        return n;
    }
    /** Return the denominator
     */
    public int getD() {
        // This will simplify the ratio if it is not already.
        simplify();
        return d;
    }

    /** Set this ratio to this + r
     */
    public void add (IntRatio r) {
        n = n * r.d + r.n * d;
        d = d * r.d;

        // simplify now to avoid overflow problems
        _simplified = false;
        simplify();
        // Must recalculate the double value.
        _gotDoubleValue = false;
    }
    
    /** Set this ratio to this - r
     */
    public void sub (IntRatio r) {
        n = n * r.d - r.n * d;
        d = d * r.d;
        
        // simplify now to avoid overflow problems
        _simplified = false;
        simplify();
        // Must recalculate the double value.
        _gotDoubleValue = false;
    }
    
    /** Set this ratio to this + r
     */
    public void add (Rational r) {
        n = n * r.getD() + r.getN() * d;
        d = d * r.getD();
        
        // simplify now to avoid overflow problems
        _simplified = false;
        simplify();
        // Must recalculate the double value.
        _gotDoubleValue = false;
    }
    
    /** Set this ratio to this - r
     */
    public void sub (Rational r) {
        n = n * r.getD() - r.getN() * d;
        d = d * r.getD();
        
        // simplify now to avoid overflow problems
        _simplified = false;
        simplify();
        // Must recalculate the double value.
        _gotDoubleValue = false;
    }

    public override int GetHashCode() {
      return 0;
    }
    
    /** Compare this ratio to object and return -1 if this is less than object,
     * 1 if this is greater than object, or zero if they are equal.
     *
     * @throws ClassCastException if object is not an IntRatio or Rational.
     */
    public int compareTo (Object obj) {
        if (obj is Rational)
            // We know Rational.compareTo will call IntRatio on its
            //   own copy of an IntRatio, so use that.
            //   We are reversing the order of the compare, so reverse the result.
            return -((Rational)obj).compareTo(this);
        
        // This will throw the ClassCastException if the object is not IntRatio
        IntRatio r = (IntRatio)obj;
        // Make sure they are simplified and do the quick Equals check first.
        simplify();
        r.simplify();
        if (n == r.n && d == r.d)
            return 0;
        
        // Do this similar to sub, but we are only interested in the numerator.
        // (Since they are simplified, the denominators are positive.
        if ((n * r.d - r.n * d) < 0)
            return -1;
        else
            return 1;
    }
    
    /** Return true of the result of compareTo for the object is zero.
     * If the object is not an IntRatio or Rational, this uses the
     * default Object.Equals().
     * 
     * @see #compareTo
     */
    public override bool Equals (object obj) {
        if (obj is IntRatio || obj is Rational)
            return (compareTo(obj) == 0);

        // Just use the default.
        return base.Equals(obj);
    }
    
    /** If _simplified is true, this does nothing.  
     * Otherwise, simplify this ratio by making the denominator positive and by
     * dividing top and bottom by their gcd.  Also set _simplified true.
     */
    private void simplify () {
        if (_simplified)
            // Already simplified
            return;
        
        if (n == 0) {
            // This is zero, so make it 0/1
            d = 1;
        }
        else {
            if (d < 0) {
                // denominator is negative.
                n = - n;
                d = - d;
            }
            
            int g = gcd (n, d);
            n = n / g;
            d = d / g;
        }
        
        _simplified = true;
    }
    
    /** Return numerator/denominator as a double value.  
     */
    public double doubleValue() {
        if (_gotDoubleValue)
            // Already have it, so no need to recompute.
            return _doubleValue;

        // Note that it is not necessary to simplify to get the double value.
        _doubleValue = ((double)n) / ((double)d);
        _gotDoubleValue = true;
        return _doubleValue;
    }
    
    /** Return the greatest common denomitator of abs(a) and abs(b).
     * If a is zero, return abs(b) or if b is zero, return abs(a).
     * (If both are zero, return zero.)
     */
    public static int gcd (int a, int b) {
        if (a < 0)
            a = -a;
        if (b < 0)
            b = -b;

        if (b == 0)
            return a;
        // (The case where a is zero is checked below.)
        
        // Use Euclid's algorithm.
        while (true) {
            int remainder = a % b;
            if (remainder == 0)
                return b;
            a = b;
            b = remainder;
        }
    }
    
    /** Returns string as "numerator/denominator"
     */
    public override string ToString() {
        // Use getN() instead of n to make sure it is simplified
        return getN() + "/" + getD();
    }

    // Tells whether n and d are in their simplified form
    private bool _simplified = false;
    private int n, d;
    // Tells whether the computed _doubleValue is valid
    private bool _gotDoubleValue = false;
    private double _doubleValue;
}
}
