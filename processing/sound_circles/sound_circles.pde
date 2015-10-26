//ArrayList<Dot> Dots = new ArrayList<Dot>();
ArrayList<Wave> Waves = new ArrayList<Wave>();
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
  fill(255, 25); 

  if (Waves.size() != 0)
  {
    ArrayList<Wave> newWave = new ArrayList<Wave>();

    for (Wave d : Waves)
    {
      d.animate();

      fill(255, 25);       
      rect(0, 0, width, height);
      fill(255, 25); 

      strokeWeight(2);

      d.draw();     

      Object[] results = d.collide(Collidables);
      if ((Boolean)results[0] == true)
      {
        if (Waves.size() < WAVECOUNT)
        {                   
          newWave.add(((Wave)results[1]));
        }
      }
    }

    //add new waves
    for (Wave w : newWave)
    {
      Waves.add(w);
    }


    for (int i = 0; i < Waves.size (); i++)
    {
      if (Waves.get(i).strength < 0)
      {
        Waves.remove(Waves.get(i));
      }
    }
  }

  if (Collidables.size() != 0)
  {
    for (Collidable c : Collidables)
      c.draw();
  }

  fill(50);
  text("F: " + framerate + " N: " + N + " T: triangle S: convex shape R: reset WAVES: " + Waves.size(), 15, 15);
}

void mouseClicked()
{
  Wave w = new Wave(mouseX, mouseY, 25);
  Waves.add(w);
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
  case 'z':
    if (framerate >= 15)
      framerate = 1;
    
    framerate ++ ;
    frameRate(framerate);
    
    background(255);

    break;
  case 'n':
    if (N >= 1800)
      N = 80;

    N += 80;      
    break;
  case 'm':
    if (N >= 1800)
      N = 80;

    N ++;      
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
  }
}

