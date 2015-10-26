class Dot
{
  float strength;

  PVector position;
  PVector velocity;
  float r = 10;
  float speed = 3.5;

  int id;

  Dot(float _x, float _y, float _dirx, float _diry, int _id)
  {  
    this.id = _id;

    this.position = new PVector(_x, _y);
    this.velocity = new PVector(_dirx, _diry);

    this.strength = 1.0;
  }

  void draw()
  {
    ellipse(position.x, position.y, 8.0 * strength, 8.0 * strength);
  }    

  void decay(int str)
  {    
    this.strength = this.strength - (float)(str / 500.0);
  }

  void animate()
  {    
    decay(1);

    this.position.add(velocity);
  }

  void collide(ArrayList<Collidable> c)
  {
   
    //right
    if (position.x > width-r) {
      position.x = width-r;
      velocity.x *= -1;
      decay(50);
    }
    // left 
    if (position.x < r) {
      position.x = r;
      velocity.x *= -1;
      decay(50);
    }
    // top
    if (position.y < r) {
      position.y = r;
      velocity.y *= -1;
      decay(50);
    }    
      // bottom
    if (position.y > height-r) {
      position.y = height-r;
      velocity.y *= -1;
      decay(50);
    }

    PVector test = this.position;

    int i;
    int j;
    boolean result = false;
    if (c.size() != 0)
    {
      for (int u = 0; u < c.size (); u++)
      {
        result = false;
        
        Collidable Col = c.get(u);
        ArrayList<PVector> Points = Col.Points;
        
        for (i = 0, j = Points.size () - 1; i < Points.size(); j = i++) {
          if ((Points.get(i).y > test.y) != (Points.get(j).y > test.y) &&
            (test.x < (Points.get(j).x - Points.get(i).x) * (test.y - Points.get(i).y) / (Points.get(j).y - Points.get(i).y) + Points.get(i).x)) {
            result = !result;
          }
        }

        if (result)
        {           
           PVector base1 = PVector.random2D();
           PVector base2 = PVector.random2D();
  
          if(Col.TRIANGLE)
          {
            //get 2 closest points      
            float d1 = dist(this.position.x, this.position.y, Points.get(0).x, Points.get(0).y);        
            float d2 = dist(this.position.x, this.position.y, Points.get(1).x, Points.get(1).y);
            float d3 = dist(this.position.x, this.position.y, Points.get(2).x, Points.get(2).y);          
             
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
            
            decay(100);
          }
          else if(Col.SQUARE) //convex is a bit wonky
          {
                       
            float d1 = dist(this.position.x, this.position.y, Points.get(0).x, Points.get(0).y);        
            float d2 = dist(this.position.x, this.position.y, Points.get(1).x, Points.get(1).y);
            float d3 = dist(this.position.x, this.position.y, Points.get(2).x, Points.get(2).y);
            float d4 = dist(this.position.x, this.position.y, Points.get(2).x, Points.get(2).y);          
             
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
            }
            else if (d3 < d1 && d3 < d2 && d4 < d1 && d4 < d2) //bovenkant
            {
              base1 = Points.get(2); 
              base2 = Points.get(3);           
            }
            
            decay(100);
          }          
        
          PVector incidence = PVector.mult(velocity, -1);
          incidence.normalize();

          PVector baseDelta = PVector.sub(base2, base1);
          baseDelta.normalize();
          PVector normal = new PVector(-baseDelta.y, baseDelta.x);

          float dot = incidence.dot(normal);

          velocity.set(2*normal.x*dot - incidence.x, 2*normal.y*dot - incidence.y, 0);                
        }
      }
    }
  }
}

