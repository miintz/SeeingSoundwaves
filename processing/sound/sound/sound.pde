ArrayList<Dot> Dots = new ArrayList<Dot>();
ArrayList<Collidable> Collidables = new ArrayList<Collidable>();

int framerate = 30;
int N = 40;

void setup()
{
  size(500, 500);
  background(255);
  strokeWeight(0);
  stroke(0);   

  frameRate(framerate);
}

void draw()
{  
  fill(255, 25);       
  rect(0, 0, width, height);
  fill(255); 

  if (Dots.size() != 0)
  {
    for (Dot d : Dots)
    {
      d.animate();
      d.draw();     

      d.collide(Collidables);      
    }

    for (int i = 0; i < Dots.size (); i++)
    {
      if (Dots.get(i).strength < 0)
      {
        Dots.remove(Dots.get(i));
      }
    }
  }

  if (Collidables.size() != 0)
  {
    for (Collidable c : Collidables)
      c.draw();
  }

  fill(50);
  text("F: " + framerate + " N: " + N + " T: triangle S: convex shape R: reset" , 15, 15);
}

void mouseClicked()
{
  if (Dots.size() < N)
  {
    float pia = (PI * 2 / N);

    for (int i = 0; i < N; i++)
    {
      float x = mouseX + 5 * cos(pia*i);
      float y = mouseY + 5 * sin(pia*i);

      float dist = dist(mouseX, mouseY, x, y);

      float difx = (x - mouseX) / dist;
      float dify = (y - mouseY) / dist;

      Dots.add(new Dot(x, y, difx, dify, i));
    }
  }
}

void keyPressed()
{
  switch(key)
  {
  case 'f':
    if (framerate == 180)
      framerate = 15;

    framerate += 15;
    frameRate(framerate);
    break;
  case 'n':
    if (N >= 1800)
      N = 80;

    N += 80;      
    break;
  case 't':
    ArrayList<PVector> P1 = new ArrayList<PVector>();

    P1.add(new PVector(mouseX, mouseY));
    P1.add(new PVector(mouseX + 50, mouseY));
    P1.add(new PVector(mouseX + 25, mouseY - 50));

    Collidable c1 = new Collidable(P1);
    c1.TRIANGLE = true; 

    Collidables.add(c1);
    break;
  case 's':
    ArrayList<PVector> P2 = new ArrayList<PVector>();

    P2.add(new PVector(mouseX, mouseY));
    P2.add(new PVector(mouseX + 50, mouseY + 10));
    P2.add(new PVector(mouseX + 80, mouseY - 30));
    P2.add(new PVector(mouseX - 10, mouseY - 40));
    
    //quad(38, 31, 86, 20, 69, 63, 30, 76);
    
    Collidable c2 = new Collidable(P2);
    c2.SQUARE = true; 

    Collidables.add(c2);
    break;
  case 'r':
    Collidables = new ArrayList<Collidable>();
    break;
 
  case 'p':
    noLoop();
  break;
  
  case 'l':
  loop();
  break;
}
}

