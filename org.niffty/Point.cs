using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace org.niffty {
  public class Point {
    public int x;
    public int y;
    public Point(int x, int y) {
      this.x = x;
      this.y = y;
    }

    public void translate(int dx, int dy) {
      x += dx;
      y += dy;
    }
  }
}
