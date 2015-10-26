class Collidable
{
  boolean TRIANGLE = false;
  boolean SQUARE = false;
  boolean ELLIPSE = false;

  public ArrayList<PVector> Points;

  Collidable(ArrayList<PVector> points)
  {
    this.Points = points;
  }

  void draw()
  {
    fill(0);
    if (Points.size() != 0)
    {
      if (TRIANGLE)
      {
        triangle(Points.get(0).x, Points.get(0).y, Points.get(1).x, Points.get(1).y, Points.get(2).x, Points.get(2).y);
      } else if (SQUARE)
      {
        quad(Points.get(0).x, Points.get(0).y, Points.get(1).x, Points.get(1).y, Points.get(2).x, Points.get(2).y, Points.get(3).x, Points.get(3).y);
      } else if (ELLIPSE)
      {
        ellipse(Points.get(0).x, Points.get(0).y, Points.get(1).x, Points.get(1).y);
      }
    }
  }  
}

