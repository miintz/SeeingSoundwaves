class Wave
{ 
  ArrayList<Integer> ExclusionList = new ArrayList<Integer>(); 

  float r = 0.0; 
  float strength;
  PVector position;

  int N = 150;

  Wave(int _x, int _y, int _r)
  {
    smooth();

    this.position = new PVector(_x, _y);

    r = _r;
    this.strength = 1.0;
  }

  void draw()
  {    
    // ellipse(position.x, position.y, r, r);
    float startAngle = 0.0;
    float endAngle = PI+PI;
    float P = (PI * 2) / N;

    int lastExclusion = 0;

    boolean firstExclusion = true;

    if (ExclusionList.size() != 0)
    {
      for (int o = 0; o < N; o++)
      {
        if (ExclusionList.contains(o))
        {
          if (firstExclusion)
          {
            startAngle = P * o;
            firstExclusion = false;
          }

          lastExclusion = o;
        }
      }

      endAngle = P * lastExclusion;
    }         

    if (startAngle != 0.0 && endAngle != PI+PI)
    {  
      //println("end: " + endAngle + " begin: " + startAngle);

      stroke(#D5A500);      
      arc(position.x, position.y, r, r, 0, startAngle, OPEN);
      stroke(#003399);
      arc(position.x, position.y, r, r, endAngle, PI+PI, OPEN);
      stroke(0);
    } else
    {
      //      println(startAngle, endAngle);
      arc(position.x, position.y, r, r, startAngle, endAngle, OPEN);
    }

    if (DRAWPOINTS)
    {
      for (int o = 0; o < N; o++)
      {
        if (!ExclusionList.contains(o))
        {
          float A = P * o;

          float px = position.x + (r/2) * cos(A);
          float py = position.y + (r/2) * sin(A);

          strokeWeight(4);
          point(px, py);
          strokeWeight(2);
        } else
        {
          strokeWeight(0);
          //println("not drawing:" + o);
        }
      }
    }
  }

  void animate()
  {
    r += 5.00;
    decay(50);
  }

  void decay(int str)
  {    
    this.strength = this.strength - (float)(str / 5000.0);
  }

  Object[] collide(ArrayList<Collidable> c)
  {
    int i = 0;
    int j = 0;

    for (Collidable Col : c)
    {       
      ArrayList<PVector> Points = Col.Points;
      ArrayList<Integer> Collisions = new ArrayList<Integer>(); 

      float P = (PI * 2) / N;
      for (int o = 0; o < N; o++)
      {        
        boolean result = false;        
        float A = P * o;

        float px = position.x + (r/2) * cos(A);
        float py = position.y + (r/2) * sin(A);

        PVector test = new PVector(px, py);

        //in polygon?
        for (i = 0, j = Points.size () - 1; i < Points.size(); j = i++) {
          if ((Points.get(i).y > test.y) != (Points.get(j).y > test.y) &&
            (test.x < (Points.get(j).x - Points.get(i).x) * (test.y - Points.get(i).y) / (Points.get(j).y - Points.get(i).y) + Points.get(i).x)) {
            result = !result;
          }
        }

        if (result)
        {
          Collisions.add(o);

          PVector base1 = PVector.random2D();
          PVector base2 = PVector.random2D();

          if (Col.TRIANGLE)
          {
            //get 2 closest points      
            float d1 = dist(px, py, Points.get(0).x, Points.get(0).y);        
            float d2 = dist(px, py, Points.get(1).x, Points.get(1).y);
            float d3 = dist(px, py, Points.get(2).x, Points.get(2).y);          

            if (d1 < d2 && d3 < d2) //linkerzijde
            {                         
              base1 = Points.get(0); 
              base2 = Points.get(2);
            } else if (d1 < d3 && d2 < d3) //onderzijde
            {
              base1 = Points.get(0); 
              base2 = Points.get(1);
            } else if (d3 < d1 && d2 < d1) //rechterzijde
            {            
              base1 = Points.get(2); 
              base2 = Points.get(1);
            }
            //            decay(100);
          } else if (Col.SQUARE) //convex is a bit wonky
          {

            float d1 = dist(px, py, Points.get(0).x, Points.get(0).y);        
            float d2 = dist(px, py, Points.get(1).x, Points.get(1).y);
            float d3 = dist(px, py, Points.get(2).x, Points.get(2).y);
            float d4 = dist(px, py, Points.get(2).x, Points.get(2).y);          

            if (d1 < d2 && d1 < d3 && d4 < d2 && d4 < d3) //linkerzijde
            {           
              base1 = Points.get(0); 
              base2 = Points.get(3);
            } else if (d1 < d3 && d1 < d4 && d2 < d3 && d3 < d4) //onderzijde
            {
              base1 = Points.get(0); 
              base2 = Points.get(1);
            } else if (d2 < d1 && d2 < d4 && d3 < d1 && d3 < d4) //rechterzijde
            {
              base1 = Points.get(2); 
              base2 = Points.get(3);
            } else if (d3 < d1 && d3 < d2 && d4 < d1 && d4 < d2) //bovenkant
            {
              base1 = Points.get(2); 
              base2 = Points.get(3);
            }
            //            decay(100);
          }

          if (!ExclusionList.contains(o))
          {
            float dirx = (this.position.x - px) / dist(this.position.x, this.position.y, px, py);
            float diry = (this.position.y - py) / dist(this.position.x, this.position.y, px, py);

            PVector velocity = new PVector(dirx, diry);
            PVector incidence = PVector.mult(velocity, -1);
            incidence.normalize();

            PVector baseDelta = PVector.sub(base2, base1);
            baseDelta.normalize();
            PVector normal = new PVector(-baseDelta.y, baseDelta.x);

            float dot = incidence.dot(normal);

            velocity.set(2*normal.x*dot - incidence.x, 2*normal.y*dot - incidence.y, 0);

            ExclusionList.add(o);  

            Wave w = new Wave((int)test.x, (int)test.y, 25);

            //exclusielijst moet gemaakt worden..             
            for (int l = 0; l < N; l++)
            {        
              float Aa = P * l;

              float zx = test.x + (25 / 2) * cos(A);
              float zy = test.y + (25 / 2) * sin(A);
              
              for (i = 0, j = Points.size () - 1; i < Points.size(); j = i++) {
                if ((Points.get(i).y > zy) != (Points.get(j).y > zy) &&
                  (zx < (Points.get(j).x - Points.get(i).x) * (zy - Points.get(i).y) / (Points.get(j).y - Points.get(i).y) + Points.get(i).x)) {
                  w.ExclusionList.add(l);
                }
              }
            }

            return new Object[] { 
              true, w
            };
          }
        }
      }
    }
    return new Object[] {
      false
    };
  }
}

