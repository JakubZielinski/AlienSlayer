using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Diagnostics;
using System.Device.Location;
using System.IO.IsolatedStorage;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Input.Touch;
using Microsoft.Xna.Framework.Media;
using Microsoft.Advertising.Mobile.Xna;
using Microsoft.Advertising.Shared;
using Microsoft.Advertising.Mobile.Resources;
using Microsoft.Advertising.Mobile.UI;
using Microsoft.Advertising.Mobile;



namespace Aliens_Slayer
{
    public class enemymissle : physic
    {
        Game1 game;        
        Vector2 speed;
        int speed1d;
        int type;
        bool roting;
        public float rotation;
        public enemymissle(Game1 game, Vector2 location, int type, float direction, bool rot)
        {
            roting = rot;
            this.type = type;
            center = location;
            this.game = game;
            Random a = new Random();
            speed1d = 100;
            speed = game.methods.getspeed(speed1d,direction);            
            size.X = game.resources.enemybulet[type].Width;
            size.Y = game.resources.enemybulet[type].Height;

        }
        public enemymissle(Game1 game, Vector2 location, int type, bool rot)
        {
            roting=rot;
            this.type = type;
            center = location;
            this.game = game;
            Random a = new Random();
            speed1d = 100;
            speed = game.methods.getspeed(speed1d,game.methods.getdirection(center,game.currentgame.ship.location));
           
            size.X = game.resources.enemybulet[type].Width;
            size.Y = game.resources.enemybulet[type].Height;
        }
        public enemymissle(Game1 game, Vector2 location, float direction, bool rot)
        {
            roting = rot;
            type = 0;
            center = location;
            this.game = game;
            Random a = new Random();
            speed1d = 50 * (game.dificulity + 1) ;
            speed = game.methods.getspeed(speed1d, direction);
            size.X = game.resources.enemybulet[0].Width;
            size.Y = game.resources.enemybulet[0].Height;
        }
        public void move()
        {
            rotate();
            center = center + ((float)(game.time - game.lasttime) / 1000f) * speed;
        }
        public void rotate()
        {
            if (roting)
            {
                if (rotation > MathHelper.Pi * 2)
                {
                    rotation = 0;
                }
                rotation = rotation + (float)(game.time - game.lasttime) / 1000f * (float)MathHelper.Pi * 2f;
            }
        }
        public void draw()
        {
            if (type == 0)
            {
                game.ekran.Draw(game.resources.enemybulet[0], new Rectangle((int)center.X, (int)center.Y, (int)game.resources.enemybulet[0].Width, (int)game.resources.enemybulet[0].Height), null, Color.White, rotation % (MathHelper.Pi * 2), new Vector2(size.X / 2, size.Y / 2), SpriteEffects.None, 1);
            }
            else
            {
                game.ekran.Draw(game.resources.enemybulet[type], new Rectangle((int)center.X, (int)center.Y, (int)game.resources.enemybulet[type].Width, (int)game.resources.enemybulet[type].Height), null, Color.White, rotation % (MathHelper.Pi * 2), new Vector2(size.X / 2, size.Y / 2), SpriteEffects.None, 1);
            }
            }
    }
    public class bonus:physic
    {
        Game1 game;
        Vector2 speed;
        public int type;
        public long lastmovetime;
        public long lastframetime;
        public int currentframe;
        public bonus(Vector2 location, Game1 game)
        {
            this.game = game;
            this.center = location;
            lastmovetime = game.time;
            
            speed=new Vector2(0,game.currentgame.generator.Next(50,100));
            type = game.currentgame.generator.Next(0, 9); //0 universal upgrade 1 missles 2rays 3machine gun, 4random,5shield,6speed,7 health, 8 power
          
            size.X = game.resources.bonus[type,1].Width;
            size.Y = game.resources.bonus[type,1].Height;
        }
        public void move(){
            center = center + speed * ((float)(game.time - game.lasttime) / 1000f);
            lastmovetime = game.time;
        }
        public void draw()
        {
            if (game.time > lastframetime + 200)
            {
                lastframetime = game.time;
                currentframe = currentframe + 1;
                if (currentframe == 5)
                {
                    currentframe = 0;
                }                
            }
            game.ekran.Draw(game.resources.bonus[type, currentframe], center, Color.White);
        }

    }
    public class rubbish : physic
    {
        Game1 game;
        public float rotation;
        public float rotationspeed;        
        public bool todelete;
        public Vector2 speed;
        public byte type;
        public long lastmovetime;
        public short dir;
        public int magicx;

        public rubbish(Game1 game, Vector2 center)
        {
            this.game = game;
            Random generator = new Random();
            type = (byte)generator.Next(0, 2);
            rotationspeed = 0.2f * (float)MathHelper.Pi;
            todelete = false;
            dir = (short)generator.Next(0, 1);
            speed = new Vector2((float)generator.Next(-50, 50), 160);      
            this.center = center;
            lastmovetime = game.time;
            magicx = (int)center.X;
        }
        public bool checkdelete()
        {
            if (center.X > 480 || center.X < 0 || center.Y < 0 || center.Y > 800)
            {
                todelete = true;
                return true;
            }
            return false;
        }
        public void move(){
            center = center + (game.time - game.lasttime) / 1000f * speed;
            if (rotation + (rotationspeed*((float)(game.time-lastmovetime)/100f)) >= 2 * MathHelper.Pi)
            {
                rotation = 0;
            }
            else
            {
                rotation = rotation + (rotationspeed * ((float)(game.time - lastmovetime) / 100f));
            }          
           
            lastmovetime = game.time;
        }
        public void draw()
        {
            game.ekran.Draw(game.resources.rubbish[type], new Rectangle((int)center.X, (int)center.Y, game.resources.rubbish[type].Width, game.resources.rubbish[type].Height), null, Color.White, rotation, new Vector2(game.resources.rubbish[type].Width / 2, game.resources.rubbish[type].Height / 2), SpriteEffects.None, 1);

        }
        
    }
    public class smoke : physic
    {
        int type;
        int currentframe;
        int maxframe;
        long lastframetime;
        int frametime;
        public bool todelete;
        Game1 game;
        public smoke(int type, Vector2 center, Game1 game)
        {
            todelete = false;
            this.type = type;
            this.center = center;
            this.game = game;
            currentframe = 0;
            maxframe = 5;
            frametime = 150;
            if (type == 3 || type == 4 || type == 5)
            {
                maxframe = 17;
                frametime = 50;
            }
            
            if (type == 2)
            {
                frametime = 100;
            }
            lastframetime = game.time;
        }
        public void draw()
        {
            if (lastframetime + frametime < game.time)
            {
                if (currentframe < maxframe-1)
                {
                    currentframe = currentframe + 1;
                    lastframetime = game.time;
                }
                else
                {
                    todelete = true;
                }
            }
            switch (type)
            {
                case 0:
                    game.ekran.Draw(game.resources.greensmoke[currentframe], new Rectangle((int)center.X, (int)center.Y, 25, 25), null, Color.White, 0f, new Vector2(25 / 2, 25 / 2), SpriteEffects.None, 1);
                    break;
                case 1:
                    game.ekran.Draw(game.resources.bigsmoke[currentframe], new Rectangle((int)center.X, (int)center.Y, 50, 50), null, Color.White, 0f, new Vector2(25, 25), SpriteEffects.None, 1);
                    break;
                case 2:
                    game.ekran.Draw(game.resources.bigsmoke[currentframe], new Rectangle((int)center.X, (int)center.Y, 50, 50), null, Color.White, 0f, new Vector2(25, 25), SpriteEffects.None, 1);
                    break;
                case 3:
                    game.ekran.Draw(game.resources.fireworkblue[currentframe], new Rectangle((int)center.X, (int)center.Y, 60, 60), Color.White);
                    break;
                case 4:
                    game.ekran.Draw(game.resources.fireworkred[currentframe], new Rectangle((int)center.X, (int)center.Y, 60, 60), Color.White);
                    break;
                case 5:
                    game.ekran.Draw(game.resources.fireworkorange[currentframe], new Rectangle((int)center.X, (int)center.Y, 60, 60), Color.White);
                    break;
            }
        }
    }
    public class physic
    {
        public Vector2 location;
        public Vector2 center;
        public Vector2 size;       
        
    }
    public struct twovectors{
    public Vector2 AA;
    public Vector2 BB;
}
    public class usefullmethods
    {
        public float getdirection(Vector2 p, Vector2 d)//p-> position, d->dest position
        {
            float dir = new float();
            float pi = MathHelper.Pi;
            float x;
            float y;
            if (p == d)
            {
                dir = 0f;
                return dir;
            }
            if (p.Y == d.Y)
            {
                if (p.X > d.X)
                {
                    dir = 1.5f * pi;
                    return dir;
                }
                else
                {
                    dir = 0.5f * pi;
                    return dir;
                }
            }
            if (p.X == d.X)
            {
                if (d.Y > p.Y)
                {
                    dir = 1f*pi;
                    return dir;
                }
                else
                {
                    dir = 0f*pi;
                    return dir;
                }
            }
            if (p.X < d.X && p.Y > d.Y)
            {
                x = d.X - p.X;
                y = p.Y - d.Y;
                dir = (float)Math.Atan(x / y);
                return dir;
            }
            if (p.X < d.X && p.Y < d.Y)
            {
                x = d.X - p.X;
                y = d.Y - p.Y;
                dir = (float)Math.Atan(y / x);
                return (dir + 0.5f * pi);
            }
            if (p.X > d.X && p.Y < d.Y)
            {
                x = p.X - d.X;
                y = d.Y - p.Y;
                dir = (float)Math.Atan(x / y);
                return (dir+1f*pi);
            }
            if (p.X > d.X && p.Y > d.Y)
            {
                x = p.X - d.X;
                y = p.Y - d.Y;
                dir = (float)Math.Atan(y / x);
                return (dir + 1.5f * pi);
            }
           

            return dir;
        }
        public Vector2 getspeed(float speed1d, float kierunek)
        {
            Vector2 speed=new Vector2();            
            if (speed1d == 0)
            {
                speed.X = 0;
                speed.Y = 0;
                return speed;
            }
            if (kierunek == 0)
            {
                speed.Y = -speed1d;
                speed.X = 0;
                return speed;
            }
            if (kierunek == 0.5f * MathHelper.Pi)
            {
                speed.Y = 0;
                speed.X = speed1d;
                return speed;
            }
            if (kierunek == 1f * MathHelper.Pi)
            {
                speed.Y = speed1d;
                speed.X = 0;
                return speed;
            }
            if (kierunek == 1.5f * MathHelper.Pi)
            {
                speed.Y = 0;
                speed.X = -speed1d;
                return speed;
            }
            if (kierunek > 0 && kierunek < 0.5f * MathHelper.Pi)
            {
                speed.X = speed1d * (float)Math.Cos((0.5f * MathHelper.Pi - kierunek));
                speed.Y = -speed1d * (float)Math.Sin((0.5f * MathHelper.Pi - kierunek));
                return speed;
            }
            if (kierunek > MathHelper.Pi * 0.5f && kierunek < MathHelper.Pi)
            {
                speed.X = speed1d * (float)Math.Cos((kierunek % (0.5f * MathHelper.Pi)));
                speed.Y = speed1d * (float)Math.Sin((kierunek % (0.5f * MathHelper.Pi)));
                return speed;
            }
            if (kierunek > MathHelper.Pi && kierunek < 1.5f * MathHelper.Pi)
            {
                speed.X = -speed1d * (float)Math.Cos((1.5f * MathHelper.Pi - kierunek));
                speed.Y = speed1d * (float)Math.Sin((1.5f * MathHelper.Pi - kierunek));
                return speed;
            }
            if (kierunek > 1.5f * MathHelper.Pi && kierunek < 2f * MathHelper.Pi)
            {
                speed.X = -speed1d * (float)Math.Cos((kierunek - 1.5f * MathHelper.Pi));
                speed.Y = -speed1d * (float)Math.Sin((kierunek - 1.5f * MathHelper.Pi));
                return speed;
            } 
       

            return speed;
        }
        public bool checkcollision(Rectangle a,float ar,Rectangle b, float br)
        {
           
            
            twovectors va =getAABB(a.X, a.Y, ar, a.Width, a.Height);
            twovectors vb =getAABB(b.X, b.Y, br, b.Width, b.Height);
         //  if (((vb.AA.X > va.AA.X && vb.AA.X < va.BB.X) || (va.AA.X > vb.AA.X && va.AA.X < vb.BB.X)) && ((vb.AA.Y > va.AA.Y && vb.AA.Y < va.BB.Y) || (va.AA.Y > vb.AA.Y && va.AA.Y < vb.BB.Y)))
           if(vb.AA.X<va.BB.X && vb.AA.X>va.AA.X && vb.AA.Y<va.BB.Y && vb.AA.Y>va.AA.Y)
           {
               return true;
           }
           if (vb.BB.X > va.AA.X && vb.BB.X < va.BB.X && vb.AA.Y > va.AA.Y && vb.AA.Y < va.BB.Y)
           {
               return true;
           }
           if (vb.BB.X > va.AA.X && vb.BB.X < va.BB.X && vb.BB.Y < va.BB.Y && vb.BB.Y > va.AA.Y)
           {
               return true;
           }
           if (vb.AA.X > va.AA.X && vb.AA.X < va.BB.X && vb.BB.Y < va.BB.Y && vb.BB.Y > va.AA.Y)
           {
               return true;
           }
           if (va.AA.X < vb.AA.X && va.BB.X > vb.BB.X && va.AA.Y < vb.AA.Y && va.BB.Y > vb.BB.Y)
           {
               return true;
           }
           if (va.AA.X > vb.AA.X && va.BB.X < vb.BB.X && va.AA.Y > vb.AA.Y && va.BB.Y < vb.BB.Y)
           {
               return true;
           }
             
           return false;
        }
        public bool checkcollision(Rectangle a, Rectangle b){
            double rayofa = (a.Width + a.Height) / 2 / 2;
            double rayofb = (b.Width + b.Height) / 2 / 2;
            double x = Math.Abs(a.X - b.X);
            double y = Math.Abs(a.Y - b.Y);
            if (x * x + y * y< (rayofa + rayofb) * (rayofa + rayofb))
            {
                return true;
            }


            return false;
        }
        public twovectors getAABB(int x, int y, float r, int w, int h)
        {
            twovectors v = new twovectors();
            float pi = MathHelper.Pi;           
           if (r % pi != 0 && r!=0)
            {
                float boxw = (float)Math.Sin(r % (0.5 * pi)) * w + (float)Math.Cos(r % (0.5 * pi)) * h;
                float boxh = (float)Math.Sin(r % (0.5 * pi)) * h + (float)Math.Cos(r % (0.5 * pi)) * w;
                v.AA.X = x -0.5f * boxw; 
                v.AA.Y = y - 0.5f * boxh;
                v.BB.X = x+ 0.5f * boxw; 
                v.BB.Y = y + 0.5f * boxh;
            }
            else
            {
                v.AA.X = x - (0.5f * w);
                v.AA.Y = y - (0.5f * h);
                v.BB.X = x + (0.5f * w);
                v.BB.Y = y + (0.5f * h);               
            }
            
            return v;
        }
    }    
    public class enemyship
    {
        public Vector2 center;
        public Vector2 size;   
        Game1 game;        
        public int hp;
        public int fullhp;
        public Vector2 speed;
        public int speed1d;
        public Vector2 destlocation;
        public Vector2[] steps;
        public int noofsteps;
        Color color;
        public float direction;
        public float destdirection;
        public long lastmovetime;
        public long lastframetime;
        public int currentframe;
        public int maxframe;
        public int type;
        public int currentstep;
        public float rotationspeed;        
        public bool todelete;
        public long lastshottime;
        public float shotrate;
        public bool kamikadze;
        public bool naprowadzane;

        public enemyship(Game1 game, int type){
            color = new Color(255, 255, 255);
            this.game = game;
            destlocation = new Vector2(240, 150);
            center = new Vector2(240, -100);
            this.type = type+10;
            speed1d = 40;
            hp = 7000+(type*1000);
            fullhp = hp;
            speed1d = 200;
            maxframe = 15;
            currentframe = 1;
            lastframetime = game.time;
            lastmovetime = game.time;
            direction = game.methods.getdirection(center, destlocation);
            destdirection = direction;
            speed = game.methods.getspeed(speed1d, direction);            
            this.size.X = game.resources.enemyship1[this.type, 1].Width;
            this.size.Y = game.resources.enemyship1[this.type, 1].Height-30;
            if (type == 12)
            {
                maxframe = 17;
            }
            if (type == 13)
            {
                maxframe = 19;
            }

    }
        public enemyship(Game1 game, int type, int lvl, Vector2 dest, Vector2 location)
        {
            this.noofsteps = 10;
            todelete = false;
            this.game = game;
            switch (type)
            {
                case 1:
                    rotationspeed = 1f;  //rozowy zwykly
                    shotrate = 0.002f* (1+game.dificulity);
                    speed1d = 50*(1+game.dificulity);
                    kamikadze = false;
                    naprowadzane = false;
                    break;
                case 2: //waz  w zarowce
                    rotationspeed = 1f;
                    shotrate = 0.002f*(1+game.dificulity);
                    speed1d = 50*(1+game.dificulity);
                    kamikadze = false;
                    naprowadzane = false;
                    break;
                case 3://pajak
                    rotationspeed = 0;
                    shotrate = 0.0005f*(1+game.dificulity);
                    speed1d = 70*(1+game.dificulity);;
                    kamikadze = false;
                    naprowadzane = false;
                    break;
                case 4://superman
                    rotationspeed = 0;
                    shotrate = 0;
                    speed1d = 0;
                    kamikadze = false;
                    naprowadzane = false;
                    break;
                case 5://czerwoneobracajace;
                    rotationspeed = 0;
                    shotrate = 0;
                    speed1d = 0;
                    kamikadze = false;
                    naprowadzane = false;
                    break;
                case 6://ufo
                    rotationspeed = 0;
                    shotrate = 0;
                    speed1d = 0;
                    kamikadze = false;
                    naprowadzane = false;
                    break;
                case 7://4katne obracajace
                    rotationspeed = 0;
                    shotrate = 0;
                    speed1d = 0;
                    kamikadze = false;
                    naprowadzane = false;
                    break;
                case 8://czarodzeij w ufo
                    rotationspeed = 0;
                    shotrate = 0;
                    speed1d = 0;
                    kamikadze = false;
                    naprowadzane = false;
                    break;
                case 9:// zolty statek
                    rotationspeed = 0;
                    shotrate = 0;
                    speed1d = 0;
                    kamikadze = false;
                    naprowadzane = false;
                    break;
              

            }

            hp = 100+(lvl*10);
            fullhp = hp;
            this.type = type;            
            color = new Color(255, 255, 255, 255);
            lastmovetime = game.time;
            lastframetime = game.time+game.currentgame.generator.Next(0,850);
            currentframe = game.currentgame.generator.Next(1, 6);
            maxframe = 6;
            center = location;
            currentstep = 1;
            destlocation = dest;
            direction = game.methods.getdirection(center, dest);
            speed = game.methods.getspeed(speed1d, direction);
                  
                this.size.X = game.resources.enemyship1[type, 1].Width;
                this.size.Y = game.resources.enemyship1[type, 1].Height;
                
           
        }
        public void draw(long time)
        {
            if (lastframetime + 199 < time)
            {
                if (currentframe < maxframe)
                {
                    currentframe = currentframe + 1;
                }
                else
                {
                    currentframe =1;
                }
                lastframetime = game.time;
            }
           
            if (type == 8 || type == 2 || type>10)
            {
                game.ekran.Draw(game.resources.enemyship1[type, currentframe], new Rectangle((int)center.X, (int)center.Y, (int)size.X, (int)size.Y), null, color, 0, 0.5f * size, SpriteEffects.None, 1);
            }
            else
            {
                game.ekran.Draw(game.resources.enemyship1[type, currentframe], new Rectangle((int)center.X, (int)center.Y, 50, 50), null, color, direction, 0.5f * size, SpriteEffects.None, 1);
            }
            }
        public void rotate()
        {
            if (type < 11)
            {
                if (Math.Abs(direction - destdirection) < 0.1f * MathHelper.Pi)
                {
                    direction = destdirection;
                }
                if (kamikadze == false)
                {
                    destdirection = game.methods.getdirection(center, destlocation);
                }
                else
                {
                    destdirection = game.methods.getdirection(center, game.currentgame.ship.location);
                }

                if (destdirection < direction)
                {
                    if (direction - destdirection > MathHelper.Pi)
                    {
                        direction = direction + (float)(game.time - game.lasttime) / 1000 * rotationspeed;
                        if (direction >= 2f * MathHelper.Pi)
                        {
                            direction = 0f;
                        }
                    }
                    else
                    {
                        direction = direction - (float)(game.time - game.lasttime) / 1000 * rotationspeed;
                    }
                }
                if (destdirection > direction)
                {
                    if (destdirection - direction > MathHelper.Pi)
                    {
                        direction = direction - (float)(game.time - game.lasttime) / 1000 * rotationspeed;
                        if (direction <= 0f * MathHelper.Pi)
                        {
                            direction = 2f * MathHelper.Pi;
                        }
                    }
                    else
                    {
                        direction = direction + (float)(game.time - game.lasttime) / 1000 * rotationspeed;
                    }
                }
            }
        }
        public void move()
        {
           
            if (center!=destlocation)
            {
                if ((Math.Abs(destlocation.X - center.X) > 5 || Math.Abs(center.Y - destlocation.Y) > 5))
                {
                   
                        speed = game.methods.getspeed(speed1d, direction);
                        center = center + ((float)(game.time - game.lasttime) / 1000) * speed;
                        
                   
                   
                }
                if ((Math.Abs(destlocation.X - center.X) < 5 && Math.Abs(center.Y - destlocation.Y) < 5))
                {
                    center = destlocation;
                }
            }
            if(destlocation==center)
            {
                speed1d = 0;  
            }
            lastmovetime = game.time;
           
        
           
        }
        public void shot()
        {
            if (type < 11)
            {
                Random a = new Random((int)(game.time % (game.currentgame.missles.Count + 1 + (int)Math.Abs(center.X - center.Y))));
                double x = a.NextDouble();
               
                        if (x <shotrate)
                        {
                            if (game.soundvolume > 0)
                            {
                                game.resources.alienshot.Play();
                            }
                            switch(type){
                                    case 1:
                                    game.currentgame.emissles.Add(new enemymissle(game, center, MathHelper.Pi,true));
                                    break;
                                    case 2:
                                    if (game.currentgame.currentstrike % 5 == 3)
                                    {
                                            game.currentgame.emissles.Add(new enemymissle(game,center,MathHelper.Pi*1,false));
                                    }
                                    else
                                    {
                                        x = a.NextDouble();
                                        if (x > 0.5f)
                                        {
                                            game.currentgame.emissles.Add(new enemymissle(game, center, 0.9f * MathHelper.Pi, true));
                                            game.currentgame.emissles.Add(new enemymissle(game, center, 1.1f * MathHelper.Pi, true));
                                        }
                                    }
                                        break;                                   
                                     case 3:
                                        if (game.currentgame.currentstrike % 5 != 3)
                                        {
                                            game.currentgame.emissles.Add(new enemymissle(game, center, 4, game.methods.getdirection(center, game.currentgame.ship.location), true));
                                        }
                                        else
                                        {
                                            game.currentgame.emissles.Add(new enemymissle(game, center, 4, MathHelper.Pi, true));
                                        }
                                    break;
                                     case 4:
                                    game.currentgame.emissles.Add(new enemymissle(game, center, MathHelper.Pi, true));
                                    break;
                                     case 5:
                                    game.currentgame.emissles.Add(new enemymissle(game, center, MathHelper.Pi, true));
                                    break;
                                     case 6:
                                    game.currentgame.emissles.Add(new enemymissle(game, center, MathHelper.Pi, true));
                                    break;
                                     case 7:
                                    game.currentgame.emissles.Add(new enemymissle(game, center, MathHelper.Pi, true));
                                    break;
                                     case 8:
                                    game.currentgame.emissles.Add(new enemymissle(game, center, MathHelper.Pi, true));
                                    break;
                                     case 9:
                                    game.currentgame.emissles.Add(new enemymissle(game, center, MathHelper.Pi, true));
                                    break;

                            }
                        }
                        
                
            }
            else
            {
                switch (type)
                {
                    case 11:
                        if (lastshottime + 3000 < game.time)
                        {
                            game.currentgame.emissles.Add(new enemymissle(game, new Vector2(center.X - 40, center.Y - 75), 1, true));
                            game.currentgame.emissles.Add(new enemymissle(game, new Vector2(center.X + 40, center.Y - 75), 1, true));
                            game.currentgame.emissles.Add(new enemymissle(game, new Vector2(center.X - 205, center.Y + 30), 1, true));
                            game.currentgame.emissles.Add(new enemymissle(game, new Vector2(center.X + 205, center.Y + 30), 1, true));
                            lastshottime = game.time;
                            if (game.soundvolume > 0)
                            {
                                game.resources.alienshot.Play();
                            }
                        }
                        break;
                    case 12:
                        if (lastshottime + 2000 < game.time)
                        {
                            game.currentgame.emissles.Add(new enemymissle(game, new Vector2(center.X - 75, center.Y), (int)2, true));
                            game.currentgame.emissles.Add(new enemymissle(game, center, 2, 1.14f*MathHelper.Pi, true));
                            
                            //game.currentgame.emissles.Add(new enemymissle(game, center, 2, 1.07f*MathHelper.Pi, true));
                            
                            game.currentgame.emissles.Add(new enemymissle(game, center, 2, 1*MathHelper.Pi, true));
                            
                            //game.currentgame.emissles.Add(new enemymissle(game, center, 2, 0.93f*MathHelper.Pi, true));
                           
                            game.currentgame.emissles.Add(new enemymissle(game, center, 2, 0.86f*MathHelper.Pi, true));
                            game.currentgame.emissles.Add(new enemymissle(game, new Vector2(center.X + 75, center.Y), (int)2, true));
                            lastshottime = game.time;
                            if (game.soundvolume > 0)
                            {
                                game.resources.alienshot.Play();
                            }
                        }
                        break;
                    case 13:
                        if (lastshottime + 400 < game.time)
                        {
                            game.currentgame.emissles.Add(new enemymissle(game, new Vector2(game.currentgame.generator.Next(30, 450), center.Y), 3, MathHelper.Pi, false));

                            lastshottime = game.time;
                            if (game.soundvolume > 0)
                            {
                                game.resources.alienshot.Play();
                            }
                        }
                        break;

                }
            }
        }
        public void teleport()
        {
            if (center.X > game.currentgame.maxx && direction>=0 && direction<= MathHelper.Pi)
            {
                center.X = game.currentgame.minx + 1 + game.currentgame.przesunieciex;
            }
            if (center.X < game.currentgame.minx && direction>= MathHelper.Pi && direction <= 2*MathHelper.Pi)
            {
                center.X = game.currentgame.maxx - 1 + game.currentgame.przesunieciex;
            }
            if (center.Y > game.currentgame.maxy && direction >= 0.5f*MathHelper.Pi && direction <= 1.5f * MathHelper.Pi)
            {
                center.Y = game.currentgame.miny + 1 + game.currentgame.przesunieciey;
                center.X = center.X + game.currentgame.przesunieciex;
            }
            if (center.Y < game.currentgame.miny && (direction >= 1.5f* MathHelper.Pi || direction <= 0.5f * MathHelper.Pi))
            {
                center.Y = game.currentgame.maxy - 1 + game.currentgame.przesunieciey;
                center.X = center.X + game.currentgame.przesunieciex;
            }
        }
    }
    public class ship
    {
        public Vector2[] steps;
        public int noofsteps;
        public int type;
        public ship()
        {
            steps = new Vector2[11];
            int l1 = 0;
            do
            {
                steps[l1]=new Vector2();
                l1 = l1 + 1;
            } while (l1 < 11);
        }

    }
    public class strike
    {
        public ship[] ship;        
        public int noofenemies;
        public strike()
        {
            ship = new ship[30];
            int l1 = 0;
            do
            {
                ship[l1] = new ship();
                l1 = l1 + 1;
            } while (l1 < 30);
        }
    }
    public class hiscoremenu
    {
        public button back;
        Game1 game;
        public int[] scores;

        public void drawscores()
        {

            int l = 0;
            int y=300;
            do
            {
                int x = 240;
                int l1 = 0;
                string s = scores[l].ToString();
                int maxl1 = s.Length;
                int center = maxl1 * (40/2);
                x = 240 - center;
                do
                {
                    game.ekran.Draw(game.resources.numbers[Convert.ToInt32(s[l1]) - 48], new Rectangle(x, y, 40, 48), Color.WhiteSmoke);
                    x = x + 40;
                    l1 = l1 + 1;
                } while (l1 < s.Length);
                y = y + 60;
                l = l + 1;
            } while (l != 5);
        }
        public void addscore(int score){
            if (score > scores[0])
            {
                scores[4] = scores[3];
                scores[3] = scores[2];
                scores[2] = scores[1];
                scores[1] = scores[0];
                scores[0] = score;
            }
            else
            {
                if (score > scores[1])
                {
                    scores[4] = scores[3];
                    scores[3] = scores[2];
                    scores[2] = scores[1];
                    scores[1] = score;
                }
                else
                {
                    if (score > scores[2])
                    {
                        scores[4] = scores[3];
                        scores[3] = scores[2];
                        scores[2] = score;
                    }
                    else
                    {
                        if (score > scores[3])
                        {
                            scores[4] = scores[3];
                            scores[3] = score;
                        }
                        else
                        {
                            if (score > scores[4])
                            {
                                scores[4] = score;
                            }
                        }
                    }
                }
            }
            


            string datafolder = "Data";
            string filename = "scores.txt";
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                using (IsolatedStorageFileStream s = file.OpenFile(System.IO.Path.Combine(datafolder, filename), FileMode.Open))
                {
                    StreamWriter writer = new StreamWriter(s);
                    writer.WriteLine(scores[0]);
                    writer.WriteLine(scores[1]);
                    writer.WriteLine(scores[2]);
                    writer.WriteLine(scores[3]);
                    writer.WriteLine(scores[4]);
                    writer.Close();
                }
            }
        }
        public hiscoremenu(Game1 game)
        {
            scores = new int[5];
            this.game = game;
            back = new button(0, game);
            string datafolder = "Data";
            string filename = "scores.txt";
            bool freshfolder = false;
            using(IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string filepath = System.IO.Path.Combine(datafolder, filename);   
                if (file.FileExists(filepath)==false)
                {
                    file.CreateDirectory(datafolder);
                    freshfolder = true;
                }
               
                if (freshfolder==true)
                {
                    using (IsolatedStorageFileStream s = file.CreateFile(filepath))
                    {
                       
                            StreamWriter writer = new StreamWriter(s);
                            writer.WriteLine("0");
                            writer.WriteLine("0");
                            writer.WriteLine("0");
                            writer.WriteLine("0");
                            writer.WriteLine("0");
                            writer.Close();
                     

                    }
                }
                using (IsolatedStorageFileStream s2 = file.OpenFile(filepath, System.IO.FileMode.Open))
                {
                    StreamReader reader = new StreamReader(s2);
                    scores[0] = Convert.ToInt32(reader.ReadLine());
                    scores[1] = Convert.ToInt32(reader.ReadLine());
                    scores[2] = Convert.ToInt32(reader.ReadLine());
                    scores[3] = Convert.ToInt32(reader.ReadLine());
                    scores[4] = Convert.ToInt32(reader.ReadLine());
                    reader.Close();
                }
            }

        }
    }
    public class playership
    {
        Game1 game;
        //moving
        public Vector2 location;
        public int minx,miny,maxx,maxy;
        public Vector2 speed;
        public float maxspeed;
        public float a;
        public float maxa;
        public float rotation;
         //draw
        public bool destroyed;
        long lasttime;
        int currentframe;
        int maxframe;
        //shotting
        public float power;
        public float maxpower;
        public int guntype;
        public int gunlevel;
        long lastshottime;
        public int maxgunlvl;
        //respawn
        public long destroyedtime;
        public long timeofresp;
        ////shield
        public bool suse;
        public bool sactive;
        public long sactivatetime;
        public long susingtime;


        public playership(Game1 game)
        {
            power = 0;
            maxpower = 0.5f;
            suse = true;
            sactivatetime = game.time;
            susingtime = 5000;
            destroyed = false;
            this.game = game;
            timeofresp = 1000;
            speed = new Vector2(0,0);
            location = new Vector2(240, 500);
            minx = 10;
            miny = 10;
            maxx = 480;
            maxy = 600;
            currentframe = 0;
            maxframe = 0;
            a = 20;
            maxa = 70;
            maxspeed = 15;
            guntype = 1;
            gunlevel = 2;
            maxgunlvl = 3;
        }
        public void bonusses()
        {
            if (suse)
            {
                sactivatetime = game.time;
                suse = false;
                sactive = true;
            }
            if (sactive)
            {
                if (sactivatetime + susingtime < game.time)
                {
                    sactive = false;
                }
            }

        }
        public void drawshield()
        {
            if (sactive)
            {
                game.ekran.Draw(game.resources.shield, new Rectangle((int)location.X-37,(int)location.Y-37,75,75),Color.White);
            }
        }
        public void crash()
        {
            if (!sactive)
            {
                foreach (enemyship e in game.currentgame.ships)
                {
                    if (game.methods.checkcollision(new Rectangle((int)location.X, (int)location.Y, 40, 40), 0f, new Rectangle((int)e.center.X, (int)e.center.Y, (int)e.size.X, (int)e.size.Y), e.direction) == true)
                    {
                        game.currentgame.smokes.Add(new smoke(0,location,game));
                        destroyedtime = game.time;
                        destroyed = true;
                        if (game.soundvolume > 0)
                        {
                            game.resources.crash.Play();
                        }
                    }
                }
            }
        }
        public void respawn()
        {
            if (destroyed)
            {
                if (game.currentgame.lifes > 0)
                {
                    if (game.time > destroyedtime + timeofresp)
                    {
                        game.currentgame.lifes = game.currentgame.lifes - 1;
                        game.currentgame.ship.location = new Vector2(230, 600);
                        suse = true;
                        gunlevel = gunlevel - 2;
                        if (gunlevel < 0)
                        {
                            gunlevel = 0;
                        }
                        a = a - 20;
                        if (a < 20)
                        {
                            a = 20;
                        }
                        power = power - 0.2f;
                        if (power < 0)
                        {
                            power = 0f;
                        }
                        destroyed = false;
                    }

                }
                else
                {
                    
                    if (game.time > destroyedtime + timeofresp)
                    {
                        game.statemenager.changestate(4);
                        game.statemenager.laststate = 2;
                        game.scores.addscore(game.currentgame.scores);                        
                        destroyed = false;
                    }
                }
            }
        }
        public void drawship()
        {
            if (game.time >= lasttime + 250)
            {
                lasttime = game.time;
                currentframe = currentframe + 1;
                if (currentframe > maxframe)
                {
                    currentframe = 0;
                }
            }
         //   game.ekran.Draw(game.resources.ship[currentframe], new Rectangle((int)location.X, (int)location.Y, 50, 50), Color.White);
            //rotation = rotation + (float)0.03 * MathHelper.Pi;
            if (destroyed == false)
            {
                Color a = new Color(255, 255, 255);
                game.ekran.Draw(game.resources.ship[currentframe], new Rectangle((int)location.X, (int)location.Y, 50, 50), null, a, rotation, new Vector2(25,25), SpriteEffects.None, 1);
            }
            drawshield();
        }
        public void drawspeed()
        {
            Vector2 x = new Vector2(0);
            x.Y=speed.Y*(1/(float)(game.time-game.lasttime));
            x.X = speed.X * (1 / (float)(game.time - game.lasttime));
            game.ekran.DrawString(game.resources.basicfont, x.ToString(), new Vector2(13), Color.Red);

        }
        public void getbonus(TouchCollection touch)
        {
            foreach (TouchLocation tl in touch)
            {
                if (game.currentgame.adbonus == false)
                {
                    if (tl.Position.X > 0 && tl.Position.X < 480 && tl.Position.Y > 720 && tl.Position.Y < 800 && tl.State == TouchLocationState.Released)
                    {
                        game.currentgame.adbonus = true;
                        if (gunlevel < 5)
                        {
                            gunlevel = gunlevel + 1;
                        }
                       
                    }
                }

            }

        }
        public void shot(TouchCollection touch, long time)
        {
            foreach(TouchLocation tl in touch)
            {
                int movingx;
                int movingy=0;
                if (game.full == false)
                {
                    movingy = -80;
                }
                if (game.fireplace == 1)
                {
                    movingx = 0;
                   
                }
                else
                {
                   
                    movingx = -340;
                }
                if (tl.Position.X > 360+movingx && tl.Position.X < 460+movingx && tl.Position.Y > 680+movingy && tl.Position.Y < 780+movingy)
                {
                    switch (guntype)
                    {
                        case 0 :
                            switch(gunlevel){
                                case 0:
                                    if (time > lastshottime + 1000)
                                    {
                                        lastshottime = time;                                        
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X,location.Y), 0, 0));
                                        if (game.soundvolume > 0)
                                        {
                                            game.resources.missle.Play();
                                        }
                                    }
                                    break;
                                case 1:
                                    if (time > lastshottime + 1000)
                                    {
                                        lastshottime = time;
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X-7, location.Y), 0, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X+7, location.Y), 0, 0));
                                        if (game.soundvolume > 0)
                                        {
                                            game.resources.missle.Play();
                                        }
                                    }
                                    break;

                                case 2:
                                    if (time > lastshottime + 1000)
                                    {
                                        lastshottime = time;
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 7, location.Y), 1, 0));
                                        if (game.soundvolume > 0)
                                        {
                                            game.resources.missle.Play();
                                        }
                                    }
                                    break;
                                case 3:
                                    if (time > lastshottime + 1000)
                                    {
                                        lastshottime = time;
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X -10, location.Y), 0, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 7), 1, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X + 10, location.Y), 0, 0));
                                        if (game.soundvolume > 0)
                                        {
                                            game.resources.missle.Play();
                                        }
                                    }
                                    break;
                                    
                                case 4:
                                     if (time > lastshottime + 1000)
                                    {
                                        lastshottime = time;
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X -10, location.Y), 1, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 7), 0, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X + 10, location.Y), 1, 0));
                                        if (game.soundvolume > 0)
                                        {
                                            game.resources.missle.Play();
                                        }
                                     }
                                    break;

                                case 5:
                                    if (time > lastshottime + 1000)
                                    {
                                        lastshottime = time;
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 20, location.Y-7), 0, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 10, location.Y), 1, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 7), 1, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X + 10, location.Y), 1, 0));
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X + 20, location.Y - 7), 0, 0));
                                        if (game.soundvolume > 0)
                                        {
                                            game.resources.missle.Play();
                                        }
                                    }
                                    break;                              
                             }
                            break;
                        case 1:
                            switch(gunlevel){
                                case 0:
                            if (time > lastshottime + 300)
                            {
                                lastshottime = time;                                
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X , location.Y), 3, 0));
                                if (game.soundvolume > 0)
                                {
                                    game.resources.ray.Play();
                                }
                            }
                            break;
                                case 1:
                            if (time > lastshottime + 300)
                            {
                                lastshottime = time;
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X-9, location.Y), 3, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y+8), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X+9, location.Y), 3, 0));
                                if (game.soundvolume > 0)
                                {
                                    game.resources.ray.Play();
                                }
                            }
                            break;
                                case 2:
                            if (time > lastshottime + 300)
                            {
                                lastshottime = time;                               
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 1.90f*MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 0.1f * MathHelper.Pi));
                                if (game.soundvolume > 0)
                                {
                                    game.resources.ray.Play();
                                }
                            }
                            break;
                                case 3:
                            if (time > lastshottime + 300)
                            {
                                lastshottime = time;
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 1.90f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 3, 1.95f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 3, 0.05f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 0.1f * MathHelper.Pi));
                                if (game.soundvolume > 0)
                                {
                                    game.resources.ray.Play();
                                }
                            }
                            break;
                                case 4:
                            if (time > lastshottime + 300)
                            {
                                lastshottime = time;
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 3, 1.90f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y+10), 3, 1.90f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X-9, location.Y), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X+9, location.Y), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y+10), 3, 0.1f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 3, 0.1f * MathHelper.Pi));
                                if (game.soundvolume > 0)
                                {
                                    game.resources.ray.Play();
                                }
                            }
                            break;
                                case 5:
                            if (time > lastshottime + 300)
                            {
                                lastshottime = time;
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 1.90f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 10), 4, 1.90f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 10), 3, 1.93f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 10), 3, 1.96f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 9, location.Y), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X + 9, location.Y), 4, 0));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 10), 3, 0.04f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 10), 3, 0.07f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y + 10), 4, 0.1f * MathHelper.Pi));
                                game.currentgame.missles.Add(new missle(game, new Vector2(location.X, location.Y), 4, 0.1f * MathHelper.Pi));
                                if (game.soundvolume > 0)
                                {
                                    game.resources.ray.Play();
                                }
                            }
                            break;
                       }
                            break;
                        case 2:
                            switch (gunlevel)
                            {
                            case 0:
                                    if (time > lastshottime + 50)
                                    {
                                        game.currentgame.missles.Add(new missle(game, new Vector2(location.X + game.currentgame.generator.Next(1, 25), location.Y + game.currentgame.generator.Next(1, 25)), 5, 0));
                                        lastshottime = time;
                                        if (game.soundvolume > 0)
                                        {
                                            game.resources.gun.Play();
                                        }
                                    }
                                break;
                            case 1:
                                if (time > lastshottime+50)
                                {
                                    game.currentgame.missles.Add(new missle(game, new Vector2(location.X-25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 25)), 5, 0));
                                    game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 25)), 5, 0));
                                    lastshottime = time;
                                    if (game.soundvolume > 0)
                                    {
                                        game.resources.gun.Play();
                                    }
                                }
                                        break;
                            case 2:
                                        if (time > lastshottime+50)
                                        {
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 25)), 5, 0));
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 25)), 5, 0));
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 25)), 5, 0));
                                            if (game.soundvolume > 0)
                                            {
                                                game.resources.gun.Play();
                                            }
                                        }
                                         break;
                            case 3:
                                         if (time > lastshottime + 50)
                                         {
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 25)), 5, 0));
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                             lastshottime = time;
                                             if (game.soundvolume > 0)
                                             {
                                                 game.resources.gun.Play();
                                             }
                                         }       
                                         break;
                            case 4:
                                         if (time > lastshottime + 50)
                                         {
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                             game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                             lastshottime = time;
                                             if (game.soundvolume > 0)
                                             {
                                                 game.resources.gun.Play();
                                             }
                                         }
                                        break;
                            case 5:
                                        if (time > lastshottime + 50)
                                        {
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                            game.currentgame.missles.Add(new missle(game, new Vector2(location.X - 25 + game.currentgame.generator.Next(1, 50), location.Y + game.currentgame.generator.Next(1, 20)), 5, 0));
                                     lastshottime = time;
                                     if (game.soundvolume > 0)
                                     {
                                         game.resources.gun.Play();
                                     }
                                         }
                                break;

                            }
                    break;

                    }
                }

            }

        }
        public void move(TouchCollection touch)
        {
            bool up = false ;
            bool left = false;
            bool right = false;
            bool down = false;
            bool upright = false;
            bool upleft = false;
            bool downleft = false;
            bool downright = false;
            int movingx = 0;
            int movingy = 0;
            if (game.fireplace == 2)
            {
                movingx = 310;
            }
            if (game.full == false)
            {
                movingy = -80;

            }
            foreach (TouchLocation tl in touch)
            {                
                float x = tl.Position.X;
                float y = tl.Position.Y;
                if (tl.State == TouchLocationState.Moved || tl.State == TouchLocationState.Pressed)
                {
                    if (x > 10 + movingx && x < 60 + movingx && y > 640+movingy && y < 690+movingy)
                    {
                        upleft = true;
                        x = 60 + movingx - x;
                        speed.X = speed.X - (1 / (float)(game.time - game.lasttime) * (x / 50 * a));
                       if (speed.X <= 0 - maxspeed)
                        {
                            speed.X = 0 - maxspeed;
                        } 
                        y = 690 - y+movingy;
                        speed.Y = speed.Y - (1 / (float)(game.time - game.lasttime) * (y / 50 * a));
                        if (speed.Y <= 0 - maxspeed)
                        {
                            speed.Y = 0 - maxspeed;
                        }
                    }

                    if (x > 10 + movingx && x < 60 + movingx && y > 740+movingy && y < 790+movingy)
                    {
                        downleft = true;
                        x = 60 + movingx - x;
                        speed.X = speed.X - (1 / (float)(game.time - game.lasttime) * (x / 50 * a));
                        if (speed.X <= 0 - maxspeed)
                        {
                            speed.X = 0 - maxspeed;
                        }
                        y = 790+movingy - y;
                        speed.Y = speed.Y + (1 / (float)(game.time - game.lasttime) * (y / 50 * a));
                        if (speed.Y >= 0 + maxspeed)
                        {
                            speed.Y = 0 + maxspeed;
                        }
                    }

                    if (x > 110 + movingx && x < 160 + movingx && y > 640+movingy && y < 690+movingy)
                    {
                        upright = true;
                        x = 160 + movingx - x;
                        speed.X = speed.X + (1 / (float)(game.time - game.lasttime) * (x /50 * a));
                        if (speed.X >= 0 + maxspeed)
                        {
                            speed.X = 0 + maxspeed;
                        }
                        y = 690 - y+movingy;
                        speed.Y = speed.Y - (1 / (float)(game.time - game.lasttime) * (y / 50 * a));
                        if (speed.Y <= 0 - maxspeed)
                        {
                            speed.Y = 0 - maxspeed;
                        }
                    }

                    if (x + movingx > 110 && x < 160 + movingx && y > 740+movingy && y < 790+movingy)
                    {
                        downright = true;
                        y = 790 - y+movingy;
                        speed.Y = speed.Y + (1 / (float)(game.time - game.lasttime) * (y / 50 * a));
                        if (speed.Y >= 0 + maxspeed)
                        {
                            speed.Y = 0 + maxspeed;
                        }
                        x = 160 + movingx - x;
                        speed.X = speed.X + (1 / (float)(game.time - game.lasttime) * (x / 50 * a));
                        if (speed.X >= 0 + maxspeed)
                        {
                            speed.X = 0 + maxspeed;
                        }                        
                    }

                    if (x > 10 + movingx && x < 60 + movingx && y > 690+movingy && y < 740+movingy)
                    {
                        left = true;
                        x = 60 + movingx - x;
                        speed.X = speed.X - (1 / (float)(game.time - game.lasttime) * (x / 50* a));
                        if (speed.X <= 0 - maxspeed)
                        {
                            speed.X = 0 - maxspeed;
                        }                        
                    }

                    if (x > 110 + movingx && x < 160 + movingx && y > 690 + movingy && y < 740 + movingy)
                    {
                        right = true;
                        x = 160 + movingx - x;
                        speed.X = speed.X + (1 / (float)(game.time - game.lasttime) * (x / 50 * a));
                        if (speed.X >= 0 + maxspeed)
                        {
                            speed.X = 0 + maxspeed;
                        }

                    }

                    if (x > 60 + movingx && x < 110 + movingx && y > 640+movingy && y < 690+movingy)
                    {
                        up = true;
                        y = 690 - y+movingy;
                        speed.Y = speed.Y - (1 / (float)(game.time - game.lasttime) * (y / 50 * a));
                        if (speed.Y <= 0 - maxspeed)
                        {
                            speed.Y = 0 - maxspeed;
                        }
                    }

                    if (x > 60 + movingx && x < 110 + movingx && y > 740+movingy && y < 790+movingy)
                    {
                        down = true;
                        y = 790 - y+movingy;
                        speed.Y = speed.Y + (1 / (float)(game.time - game.lasttime) * (y /50 * a));
                        if (speed.Y >= 0 + maxspeed)
                        {
                            speed.Y = 0 + maxspeed;
                        }
                    }
                   
                }
            }    
               
                    if (upleft==false && up==false && upright==false && speed.Y<0)
                    {
                        if (speed.Y + a * (1 / (float)(game.time - game.lasttime)) > 0)
                        {
                            speed.Y = 0;
                        }
                        else
                        {
                            speed.Y = speed.Y + a * (1 / (float)(game.time - game.lasttime));
                        }
                    } 
                    if (downleft==false && down==false && downright==false && speed.Y>0)
                    {
                        if (speed.Y - a * (1 / (float)(game.time - game.lasttime)) < 0)
                        {
                            speed.Y = 0;
                        }
                        else
                        {
                            speed.Y = speed.Y - a * (1 / (float)(game.time - game.lasttime));
                        }
                    }
                    if (right ==false && downright == false && upright == false && speed.X>0)
                    {
                        if (speed.X - a * (1 / (float)(game.time - game.lasttime)) < 0)
                        {
                            speed.X = 0;
                        }
                        else
                        {
                            speed.X = speed.X - a * (1 / (float)(game.time - game.lasttime));
                        }
                    }
                    if (left ==false && upleft==false && downleft==false && speed.X<0)
                    {
                        if (speed.X + a * (1 / (float)(game.time - game.lasttime)) > 0)
                        {
                            speed.X = 0;
                        }
                        else
                        {
                            speed.X = speed.X + a * (1 / (float)(game.time - game.lasttime));
                        }
                    }
               
            
            if (location.X + speed.X > maxx)
            {
                location.X = maxx;
            }
            else
            {
            location.X = location.X + speed.X;
            }
            if (location.X + speed.X < minx)
            {
                location.X = minx;
            }
            else
            {
                location.X = location.X + speed.X;
            }
            if (location.Y + speed.Y < miny)
            {
                location.Y = miny;
            }
            else
            {
                location.Y = location.Y + speed.Y;
            }
            if (location.Y + speed.Y > maxy)
            {
                location.Y = maxy;
            }
            else
            {
                location.Y = location.Y + speed.Y;
            }
            bonusses();
            
        }
    }
    public class missle:physic
    {
        Game1 game;
        public Vector2 speed;
        public float speed1d;
        public int maxspeed;
        public float a;
        public int type;
        public int lvl;
        public int power;
        public int currentframe;        
        public long lastframetime;
        public long lastmovetime;
        public float kierunek;
        public Color color;
        public bool todelete;
        
        public missle(Game1 game,Vector2 location,int type, float kierunek)
        {
            speed1d = 0;
            this.type = type;
            a = 40;
            this.kierunek = kierunek;
            this.game = game;
            this.size.X = game.resources.missle1[type].Width;
            this.size.Y = game.resources.missle1[type].Height;
            this.location.X = location.X - 0.5f * size.X;
            this.location.Y = location.Y + 0.5f * size.Y;
            center = location;            
            todelete = false;
            speed = new Vector2();       
            switch (type) //type 0,1,2 missles, 3,4,rays 5 machinegun
            {
                case 0:                  
                    power = 100+(int)(game.currentgame.ship.power*30f);
                    maxspeed = 120;
                    color = Color.White;                   
                    break;
                case 1:
                    power = 150 + (int)(game.currentgame.ship.power * 60f);
                    maxspeed = 110;
                    color = Color.White;
                    break;
                case 2:
                    power = 100 + (int)(game.currentgame.ship.power * 50f);
                    maxspeed = 200;
                    color = Color.White;
                    break;
                case 3:
                    power = 30 + (int)(game.currentgame.ship.power * 10f);
                    maxspeed = 300;
                    color = Color.White;
                    size.X = 5;
                    break;
                case 4:
                    power = 40 + (int)(game.currentgame.ship.power * 10f);
                    maxspeed = 330;
                    color = Color.White;
                    size.X = 5;
                    break;
                case 5:
                    power = 8 + (int)(game.currentgame.ship.power * 10f);
                    maxspeed = 1000;
                    color = Color.White;
                    break;
            }
            speed = game.methods.getspeed(speed1d, kierunek);         
           
                    }
        public void move(long time)
        {
            if (speed1d < maxspeed)
            {
                speed1d = speed1d + ((time - lastframetime) / 1000f) * a;
            }
            else
            {
                speed1d = maxspeed;
            }
            speed = game.methods.getspeed(speed1d, kierunek);           
            center = center + (float)1/(time - lastmovetime) * speed;
            lastmovetime = time;
            this.location.X = center.X - 0.5f * size.X;
            this.location.Y = center.Y - 0.5f * size.Y;
        }
        public void draw(long time)  
        {            
           game.ekran.Draw(game.resources.missle1[type], new Rectangle((int)center.X, (int)center.Y, (int)size.X, (int)size.Y), null, color, kierunek,0.5f*size, SpriteEffects.None,0);
        }
        public bool checkdelete()
        {
            if (center.X > 480 || center.X < 0 || center.Y < 0 || center.Y > 800)
            {
                todelete = true;
                return true;
            }
            return false;
        }
    }
    
    public class gamestates
    {
        public int Game;
        public int Menu;
        public int Start;
        public int About;
        public int Picklevel;
        public int Hiscores;
        public int Settings;

        public gamestates()
        {
            Game = 5;
            Menu = 2;
            Start = 1;
            About = 21;
            Settings = 3;
            Hiscores = 4;
        }

    }
    public class resourcesmgr
    {
        
        Game1 game;
        public Song backgroundsound; 
        public SoundEffect alienshot;
        public SoundEffect crash;
        public SoundEffect boss;
        public SoundEffect aliendie;
        public SoundEffect click;
        public SoundEffect gun;
        public SoundEffect hit;
        public SoundEffect ray;
        public SoundEffect missle;
        public SoundEffect powerup;
        public Texture2D yes;
        public Texture2D no;
        public Texture2D soundstring;
        public Texture2D[] end;
        public Texture2D blvl;
        public Texture2D bottombeam;
        public Texture2D nhelp;
        public Texture2D npause;
        public Texture2D ntap;
        public Texture2D help;
        public Texture2D about;
        public Texture2D wave;
        public Texture2D circle;
        public Texture2D menu;
        public Texture2D settings;       
        public Texture2D volume;
        public Texture2D[] dificulity;
        public Texture2D controls1;
        public Texture2D controls2;
        public Texture2D[] greensmoke;
        public Texture2D[] specialbutton;
        public Texture2D[] numbers;
        public Texture2D[] numbers2;
        public Texture2D[] fireworkblue;
        public Texture2D[] fireworkorange;
        public Texture2D[] fireworkred;
        public Texture2D onclick;
        public Texture2D shield;
        public Texture2D beam;
        public Texture2D gunlvl;
        public Texture2D gunlvlbeam;
        public Texture2D smallship;
        public Texture2D[] smoke;
        public Texture2D[] bigsmoke;
        public strike[] strikes;
        public Texture2D[] rubbish;
        public Texture2D[,] bonus;
        public Texture2D fire;
        public SpriteFont basicfont;
        public Texture2D start;
        public Texture2D[] planet;
        public Texture2D backgroundgame;
        public Texture2D menubutton;
        public Texture2D menupressedbutton;
        public Texture2D[] ship;
        public Texture2D[] enemybulet;
        public Texture2D[,] enemyship1;
        public Texture2D[] missle1;
        public Texture2D pad;
        public Texture2D frame;
        public Texture2D arrow;
      
        public void getgame(Game1 game)
        {
            this.game = game;
            strikes = new strike[3];//no of strikes;
            int l1 = 0;
            do
            {
                strikes[l1] = new strike();
                l1 = l1 + 1;
            } while (l1 < 3);//no of strikes;
        }
        public void loadtextures()
        {
            end = new Texture2D[4];
            end[0] = game.Content.Load<Texture2D>("strings/1");
            end[1] = game.Content.Load<Texture2D>("strings/2");
            end[2] = game.Content.Load<Texture2D>("strings/3");
            end[3] = game.Content.Load<Texture2D>("strings/4");
            blvl = game.Content.Load<Texture2D>("beam/lvl");
            ntap = game.Content.Load<Texture2D>("tap");
            nhelp = game.Content.Load<Texture2D>("help");
            npause = game.Content.Load<Texture2D>("pause");
            help = game.Content.Load<Texture2D>("Menu/help");
            about = game.Content.Load<Texture2D>("Menu/aboutmenu");
            wave = game.Content.Load<Texture2D>("strings/wave");
            soundstring = game.Content.Load<Texture2D>("strings/sound");
            yes = game.Content.Load<Texture2D>("yes");
            no = game.Content.Load<Texture2D>("no");
            dificulity = new Texture2D[3];
            fireworkblue = new Texture2D[17];
            fireworkorange = new Texture2D[17];
            fireworkred = new Texture2D[17];
            int l2 = 0;
            int l3 = 1;
            do
            {

                fireworkorange[l2] = game.Content.Load<Texture2D>("fireworks/orange/" + l3.ToString());
                fireworkred[l2] = game.Content.Load<Texture2D>("fireworks/red/" + l3.ToString());
                l2 = l2 + 1;
                l3 = l3 + 1;
            } while (l2 != 17);
            fireworkblue[0] = game.Content.Load<Texture2D>("fireworks/blue/1");
            fireworkblue[1] = game.Content.Load<Texture2D>("fireworks/blue/2");
            fireworkblue[2] = game.Content.Load<Texture2D>("fireworks/blue/3");
            fireworkblue[3] = game.Content.Load<Texture2D>("fireworks/blue/4");
            fireworkblue[4] = game.Content.Load<Texture2D>("fireworks/blue/5");
            fireworkblue[5] = game.Content.Load<Texture2D>("fireworks/blue/6");
            fireworkblue[6] = game.Content.Load<Texture2D>("fireworks/blue/7");
            fireworkblue[7] = game.Content.Load<Texture2D>("fireworks/blue/8");
            fireworkblue[8] = game.Content.Load<Texture2D>("fireworks/blue/9");
            fireworkblue[9] = game.Content.Load<Texture2D>("fireworks/blue/10");
            fireworkblue[10] = game.Content.Load<Texture2D>("fireworks/blue/11");
            fireworkblue[11] = game.Content.Load<Texture2D>("fireworks/blue/12");
            fireworkblue[12] = game.Content.Load<Texture2D>("fireworks/blue/13");
            fireworkblue[13] = game.Content.Load<Texture2D>("fireworks/blue/14");
            fireworkblue[14] = game.Content.Load<Texture2D>("fireworks/blue/15");
            fireworkblue[15] = game.Content.Load<Texture2D>("fireworks/blue/16");
            fireworkblue[16] = game.Content.Load<Texture2D>("fireworks/blue/17");

            dificulity[0] = game.Content.Load<Texture2D>("Menu/Options/easy");
            dificulity[1] = game.Content.Load<Texture2D>("Menu/Options/medium");
            dificulity[2] = game.Content.Load<Texture2D>("Menu/Options/hard");
            greensmoke = new Texture2D[6];
            greensmoke[0] = game.Content.Load<Texture2D>("smoke/explosion1");
            greensmoke[1] = game.Content.Load<Texture2D>("smoke/explosion2");
            greensmoke[2] = game.Content.Load<Texture2D>("smoke/explosion3");
            greensmoke[3] = game.Content.Load<Texture2D>("smoke/explosion4");
            greensmoke[4] = game.Content.Load<Texture2D>("smoke/explosion5");
            greensmoke[5] = game.Content.Load<Texture2D>("smoke/explosion6");
            settings = game.Content.Load<Texture2D>("Menu/Options/settings");
            circle = game.Content.Load<Texture2D>("Menu/circle");
            volume = game.Content.Load<Texture2D>("Menu/Options/volume_control");
            controls1 = game.Content.Load<Texture2D>("Menu/Options/controls1");
            controls2 = game.Content.Load<Texture2D>("Menu/Options/controls2");
            menu = game.Content.Load<Texture2D>("Menu/background");
            specialbutton = new Texture2D[10];
            bottombeam = game.Content.Load<Texture2D>("beam/bottombeam");
            shield = game.Content.Load<Texture2D>("bonuses/shield");
            beam = game.Content.Load<Texture2D>("beam/beam");
            gunlvl= game.Content.Load<Texture2D>("beam/gunlvl");
            gunlvlbeam = game.Content.Load<Texture2D>("beam/gunlvlbeam");
            smallship = game.Content.Load<Texture2D>("beam/smallship");
            frame = game.Content.Load<Texture2D>("ramka");
            missle1=new Texture2D[9];
            smoke = new Texture2D[6];
            bigsmoke = new Texture2D[6];
            enemybulet = new Texture2D[11];
            enemybulet[0] = game.Content.Load<Texture2D>("enemybulets/yellow");
            enemybulet[1] = game.Content.Load<Texture2D>("enemybulets/1");
            enemybulet[2] = game.Content.Load<Texture2D>("enemybulets/2");
            enemybulet[3] = game.Content.Load<Texture2D>("enemybulets/3");
            enemybulet[4] = game.Content.Load<Texture2D>("enemybulets/4");
            bonus = new Texture2D[9,5];
            bonus[0, 0] = game.Content.Load<Texture2D>("bonuses/universal/multibonus1");
            bonus[0, 1] = game.Content.Load<Texture2D>("bonuses/universal/multibonus2");
            bonus[0, 2] = game.Content.Load<Texture2D>("bonuses/universal/multibonus3");
            bonus[0, 3] = game.Content.Load<Texture2D>("bonuses/universal/multibonus4");
            bonus[0, 4] = game.Content.Load<Texture2D>("bonuses/universal/multibonus5");

            bonus[1, 0] = game.Content.Load<Texture2D>("bonuses/missle/rockets1");
            bonus[1, 1] = game.Content.Load<Texture2D>("bonuses/missle/rockets2");
            bonus[1, 2] = game.Content.Load<Texture2D>("bonuses/missle/rockets3");
            bonus[1, 3] = game.Content.Load<Texture2D>("bonuses/missle/rockets4");
            bonus[1, 4] = game.Content.Load<Texture2D>("bonuses/missle/rockets5");

            bonus[2, 0] = game.Content.Load<Texture2D>("bonuses/ray/laser1");
            bonus[2, 1] = game.Content.Load<Texture2D>("bonuses/ray/laser2");
            bonus[2, 2] = game.Content.Load<Texture2D>("bonuses/ray/laser3");
            bonus[2, 3] = game.Content.Load<Texture2D>("bonuses/ray/laser4");
            bonus[2, 4] = game.Content.Load<Texture2D>("bonuses/ray/laser5");

            bonus[3, 0] = game.Content.Load<Texture2D>("bonuses/machinegun/gun1");
            bonus[3, 1] = game.Content.Load<Texture2D>("bonuses/machinegun/gun2");
            bonus[3, 2] = game.Content.Load<Texture2D>("bonuses/machinegun/gun3");
            bonus[3, 3] = game.Content.Load<Texture2D>("bonuses/machinegun/gun4");
            bonus[3, 4] = game.Content.Load<Texture2D>("bonuses/machinegun/gun5");

            bonus[4, 0] = game.Content.Load<Texture2D>("bonuses/random/losowe1");
            bonus[4, 1] = game.Content.Load<Texture2D>("bonuses/random/losowe2");
            bonus[4, 2] = game.Content.Load<Texture2D>("bonuses/random/losowe3");
            bonus[4, 3] = game.Content.Load<Texture2D>("bonuses/random/losowe4");
            bonus[4, 4] = game.Content.Load<Texture2D>("bonuses/random/losowe5");

            bonus[5, 0] = game.Content.Load<Texture2D>("bonuses/shield/shield1");
            bonus[5, 1] = game.Content.Load<Texture2D>("bonuses/shield/shield2");
            bonus[5, 2] = game.Content.Load<Texture2D>("bonuses/shield/shield3");
            bonus[5, 3] = game.Content.Load<Texture2D>("bonuses/shield/shield4");
            bonus[5, 4] = game.Content.Load<Texture2D>("bonuses/shield/shield5");

            bonus[6, 0] = game.Content.Load<Texture2D>("bonuses/speed/speed1");
            bonus[6, 1] = game.Content.Load<Texture2D>("bonuses/speed/speed2");
            bonus[6, 2] = game.Content.Load<Texture2D>("bonuses/speed/speed3");
            bonus[6, 3] = game.Content.Load<Texture2D>("bonuses/speed/speed4");
            bonus[6, 4] = game.Content.Load<Texture2D>("bonuses/speed/speed5");

            bonus[7, 0] = game.Content.Load<Texture2D>("bonuses/life/health1");
            bonus[7, 1] = game.Content.Load<Texture2D>("bonuses/life/health2");
            bonus[7, 2] = game.Content.Load<Texture2D>("bonuses/life/health3");
            bonus[7, 3] = game.Content.Load<Texture2D>("bonuses/life/health4");
            bonus[7, 4] = game.Content.Load<Texture2D>("bonuses/life/health5");

            bonus[8, 0] = game.Content.Load<Texture2D>("bonuses/power/power1");
            bonus[8, 1] = game.Content.Load<Texture2D>("bonuses/power/power2");
            bonus[8, 2] = game.Content.Load<Texture2D>("bonuses/power/power3");
            bonus[8, 3] = game.Content.Load<Texture2D>("bonuses/power/power4");
            bonus[8, 4] = game.Content.Load<Texture2D>("bonuses/power/power5");
            
            bigsmoke[0] = game.Content.Load<Texture2D>("smoke/dympostatku1");
            bigsmoke[1] = game.Content.Load<Texture2D>("smoke/dympostatku2");
            bigsmoke[2] = game.Content.Load<Texture2D>("smoke/dympostatku3");
            bigsmoke[3] = game.Content.Load<Texture2D>("smoke/dympostatku4");
            bigsmoke[4] = game.Content.Load<Texture2D>("smoke/dympostatku5");
            bigsmoke[5] = game.Content.Load<Texture2D>("smoke/dympostatku6");
            smoke[0] = game.Content.Load<Texture2D>("smoke/dym1");
            smoke[1] = game.Content.Load<Texture2D>("smoke/dym2");
            smoke[2] = game.Content.Load<Texture2D>("smoke/dym3");
            smoke[3] = game.Content.Load<Texture2D>("smoke/dym4");
            smoke[4] = game.Content.Load<Texture2D>("smoke/dym5");
            smoke[5] = game.Content.Load<Texture2D>("smoke/dym6");

            numbers = new Texture2D[10];
            numbers[0] = game.Content.Load<Texture2D>("number/0");
            numbers[1] = game.Content.Load<Texture2D>("number/1");
            numbers[2] = game.Content.Load<Texture2D>("number/2");
            numbers[3] = game.Content.Load<Texture2D>("number/3");
            numbers[4] = game.Content.Load<Texture2D>("number/4");
            numbers[5] = game.Content.Load<Texture2D>("number/5");
            numbers[6] = game.Content.Load<Texture2D>("number/6");
            numbers[7] = game.Content.Load<Texture2D>("number/7");
            numbers[8] = game.Content.Load<Texture2D>("number/8");
            numbers[9] = game.Content.Load<Texture2D>("number/9");

            numbers2 = new Texture2D[10];
            numbers2[0] = game.Content.Load<Texture2D>("number/0a");
            numbers2[1] = game.Content.Load<Texture2D>("number/1a");
            numbers2[2] = game.Content.Load<Texture2D>("number/2a");
            numbers2[3] = game.Content.Load<Texture2D>("number/3a");
            numbers2[4] = game.Content.Load<Texture2D>("number/4a");
            numbers2[5] = game.Content.Load<Texture2D>("number/5a");
            numbers2[6] = game.Content.Load<Texture2D>("number/6a");
            numbers2[7] = game.Content.Load<Texture2D>("number/7a");
            numbers2[8] = game.Content.Load<Texture2D>("number/8a");
            numbers2[9] = game.Content.Load<Texture2D>("number/9a");

            rubbish = new Texture2D[3];
            rubbish[0] = game.Content.Load<Texture2D>("rubbish/srubka");
            rubbish[1] = game.Content.Load<Texture2D>("rubbish/nakretka");
            rubbish[2] = game.Content.Load<Texture2D>("rubbish/tryb");
            missle1[0]= game.Content.Load<Texture2D>("Missles/rocket1");
            missle1[1]= game.Content.Load<Texture2D>("Missles/rocket2");
            missle1[2] = game.Content.Load<Texture2D>("Missles/rocket2");
            missle1[3] = game.Content.Load<Texture2D>("Missles/laser1");
            missle1[4] = game.Content.Load<Texture2D>("Missles/laser2");
            missle1[5] = game.Content.Load<Texture2D>("Missles/smuga_za_pociskiem");
            backgroundgame = game.Content.Load<Texture2D>("background1");
            enemyship1 = new Texture2D[21,21];
            enemyship1[1, 1] = game.Content.Load<Texture2D>("aliens/ufo1/ufo1-1");
            enemyship1[1, 2] = game.Content.Load<Texture2D>("aliens/ufo1/ufo1-2");
            enemyship1[1, 3] = game.Content.Load<Texture2D>("aliens/ufo1/ufo1-3");
            enemyship1[1, 4] = game.Content.Load<Texture2D>("aliens/ufo1/ufo1-4");
            enemyship1[1, 5] = game.Content.Load<Texture2D>("aliens/ufo1/ufo1-5");
            enemyship1[1, 6] = game.Content.Load<Texture2D>("aliens/ufo1/ufo1-6");
            enemyship1[2, 1] = game.Content.Load<Texture2D>("aliens/ufo2/ufo2-1");
            enemyship1[2, 2] = game.Content.Load<Texture2D>("aliens/ufo2/ufo2-2");
            enemyship1[2, 3] = game.Content.Load<Texture2D>("aliens/ufo2/ufo2-3");
            enemyship1[2, 4] = game.Content.Load<Texture2D>("aliens/ufo2/ufo2-4");
            enemyship1[2, 5] = game.Content.Load<Texture2D>("aliens/ufo2/ufo2-5");
            enemyship1[2, 6] = game.Content.Load<Texture2D>("aliens/ufo2/ufo2-6");
            enemyship1[3, 1] = game.Content.Load<Texture2D>("aliens/ufo3/ufo3-1");
            enemyship1[3, 2] = game.Content.Load<Texture2D>("aliens/ufo3/ufo3-2");
            enemyship1[3, 3] = game.Content.Load<Texture2D>("aliens/ufo3/ufo3-3");
            enemyship1[3, 4] = game.Content.Load<Texture2D>("aliens/ufo3/ufo3-4");
            enemyship1[3, 5] = game.Content.Load<Texture2D>("aliens/ufo3/ufo3-5");
            enemyship1[3, 6] = game.Content.Load<Texture2D>("aliens/ufo3/ufo3-6");
            enemyship1[4, 1] = game.Content.Load<Texture2D>("aliens/ufo4/ufo4-1");
            enemyship1[4, 2] = game.Content.Load<Texture2D>("aliens/ufo4/ufo4-2");
            enemyship1[4, 3] = game.Content.Load<Texture2D>("aliens/ufo4/ufo4-3");
            enemyship1[4, 4] = game.Content.Load<Texture2D>("aliens/ufo4/ufo4-4");
            enemyship1[4, 5] = game.Content.Load<Texture2D>("aliens/ufo4/ufo4-5");
            enemyship1[4, 6] = game.Content.Load<Texture2D>("aliens/ufo4/ufo4-6");
            enemyship1[5, 1] = game.Content.Load<Texture2D>("aliens/ufo5/ufo5-1");
            enemyship1[5, 2] = game.Content.Load<Texture2D>("aliens/ufo5/ufo5-2");
            enemyship1[5, 3] = game.Content.Load<Texture2D>("aliens/ufo5/ufo5-3");
            enemyship1[5, 4] = game.Content.Load<Texture2D>("aliens/ufo5/ufo5-4");
            enemyship1[5, 5] = game.Content.Load<Texture2D>("aliens/ufo5/ufo5-5");
            enemyship1[5, 6] = game.Content.Load<Texture2D>("aliens/ufo5/ufo5-6");
            enemyship1[6, 1] = game.Content.Load<Texture2D>("aliens/ufo6/ufo6-1");
            enemyship1[6, 2] = game.Content.Load<Texture2D>("aliens/ufo6/ufo6-2");
            enemyship1[6, 3] = game.Content.Load<Texture2D>("aliens/ufo6/ufo6-3");
            enemyship1[6, 4] = game.Content.Load<Texture2D>("aliens/ufo6/ufo6-4");
            enemyship1[6, 5] = game.Content.Load<Texture2D>("aliens/ufo6/ufo6-5");
            enemyship1[6, 6] = game.Content.Load<Texture2D>("aliens/ufo6/ufo6-6");
            enemyship1[7, 1] = game.Content.Load<Texture2D>("aliens/ufo7/ufo7-1");
            enemyship1[7, 2] = game.Content.Load<Texture2D>("aliens/ufo7/ufo7-2");
            enemyship1[7, 3] = game.Content.Load<Texture2D>("aliens/ufo7/ufo7-3");
            enemyship1[7, 4] = game.Content.Load<Texture2D>("aliens/ufo7/ufo7-4");
            enemyship1[7, 5] = game.Content.Load<Texture2D>("aliens/ufo7/ufo7-5");
            enemyship1[7, 6] = game.Content.Load<Texture2D>("aliens/ufo7/ufo7-6");
            enemyship1[8, 1] = game.Content.Load<Texture2D>("aliens/ufo8/ufo8-1");
            enemyship1[8, 2] = game.Content.Load<Texture2D>("aliens/ufo8/ufo8-2");
            enemyship1[8, 3] = game.Content.Load<Texture2D>("aliens/ufo8/ufo8-3");
            enemyship1[8, 4] = game.Content.Load<Texture2D>("aliens/ufo8/ufo8-4");
            enemyship1[8, 5] = game.Content.Load<Texture2D>("aliens/ufo8/ufo8-5");
            enemyship1[8, 6] = game.Content.Load<Texture2D>("aliens/ufo8/ufo8-6");
            enemyship1[9, 1] = game.Content.Load<Texture2D>("aliens/ufo9/ufo9-1");
            enemyship1[9, 2] = game.Content.Load<Texture2D>("aliens/ufo9/ufo9-2");
            enemyship1[9, 3] = game.Content.Load<Texture2D>("aliens/ufo9/ufo9-3");
            enemyship1[9, 4] = game.Content.Load<Texture2D>("aliens/ufo9/ufo9-4");
            enemyship1[9, 5] = game.Content.Load<Texture2D>("aliens/ufo9/ufo9-5");
            enemyship1[9, 6] = game.Content.Load<Texture2D>("aliens/ufo9/ufo9-6");
            enemyship1[11, 1] = game.Content.Load<Texture2D>("aliens/boss/1/1");
            enemyship1[11, 2] = game.Content.Load<Texture2D>("aliens/boss/1/2");
            enemyship1[11, 3] = game.Content.Load<Texture2D>("aliens/boss/1/3");
            enemyship1[11, 4] = game.Content.Load<Texture2D>("aliens/boss/1/4");
            enemyship1[11, 5] = game.Content.Load<Texture2D>("aliens/boss/1/5");
            enemyship1[11, 6] = game.Content.Load<Texture2D>("aliens/boss/1/6");
            enemyship1[11, 7] = game.Content.Load<Texture2D>("aliens/boss/1/7");
            enemyship1[11, 8] = game.Content.Load<Texture2D>("aliens/boss/1/8");
            enemyship1[11, 9] = game.Content.Load<Texture2D>("aliens/boss/1/9");
            enemyship1[11, 10] = game.Content.Load<Texture2D>("aliens/boss/1/10");
            enemyship1[11, 11] = game.Content.Load<Texture2D>("aliens/boss/1/11");
            enemyship1[11, 12] = game.Content.Load<Texture2D>("aliens/boss/1/12");
            enemyship1[11, 13] = game.Content.Load<Texture2D>("aliens/boss/1/13");
            enemyship1[11, 14] = game.Content.Load<Texture2D>("aliens/boss/1/14");
            enemyship1[11, 15] = game.Content.Load<Texture2D>("aliens/boss/1/15");
            int l1 = 1;
            do
            {
                enemyship1[12, l1] = game.Content.Load<Texture2D>("aliens/boss/2/" + l1.ToString());
                l1 = l1 + 1;
            } while (l1 != 21);
            l1 = 1;
            do{
                enemyship1[13, l1] = game.Content.Load<Texture2D>("aliens/boss/3/" + l1.ToString());
                l1 = l1 + 1;
            } while (l1 != 21) ;

            ship = new Texture2D[6];
            ship[0] = game.Content.Load<Texture2D>("ships/ship");            
            basicfont=game.Content.Load<SpriteFont>("Basicfont");
            menubutton = game.Content.Load<Texture2D>("button");
            menupressedbutton = game.Content.Load<Texture2D>("button");
            start = game.Content.Load<Texture2D>("480x800");
            planet = new Texture2D[1];
            planet[0] = game.Content.Load<Texture2D>("planeta");
            pad = game.Content.Load<Texture2D>("move_button");
            fire = game.Content.Load<Texture2D>("shot");
            onclick = game.Content.Load<Texture2D>("on_click_button");

            arrow = game.Content.Load<Texture2D>("Menu/arrow");

            specialbutton[0] = game.Content.Load<Texture2D>("Menu/back");            
            specialbutton[2] = game.Content.Load<Texture2D>("Menu/play");
            specialbutton[1] = game.Content.Load<Texture2D>("Menu/resume");
            specialbutton[3] = game.Content.Load<Texture2D>("Menu/settings");
            specialbutton[4] = game.Content.Load<Texture2D>("Menu/hight_scores");
            specialbutton[5] = game.Content.Load<Texture2D>("Menu/about");
            specialbutton[6] = game.Content.Load<Texture2D>("Menu/exit");
        }
        public void loadsounds()
        {
            alienshot = game.Content.Load<SoundEffect>("Sounds/alienshot");
            backgroundsound = game.Content.Load<Song>("Sounds/bg");
            crash = game.Content.Load<SoundEffect>("Sounds/playercrasgh");
            boss = game.Content.Load<SoundEffect>("Sounds/boss");
            aliendie = game.Content.Load<SoundEffect>("Sounds/aliendie");
            click = game.Content.Load<SoundEffect>("Sounds/click");
            gun = game.Content.Load<SoundEffect>("Sounds/gun");
            hit = game.Content.Load<SoundEffect>("Sounds/hit");
            ray = game.Content.Load<SoundEffect>("Sounds/Laser");
            missle=game.Content.Load<SoundEffect>("Sounds/missle");
            powerup=game.Content.Load<SoundEffect>("Sounds/powerup");
        }
    
    }
    public class GAME ///center of game logic
    {
        public bool pause;
        public bool adbonus;
        public int maxlevels;
        Game1 game;        
        public int przesunieciex, przesunieciey;
        public int scores;
        public int scorestodraw;
        public int maxy, maxx, miny, minx;
        public Random generator;
        public int lifes;
        public playership ship;
        public int noofbosses;
        public int noofships;
        public int currentplanet;
        public int currentstrike;
        public List<enemymissle> emissles;
        public List<enemyship> ships;
        public List<missle> missles;
        public List<smoke> smokes;
        public List<rubbish> rubbish;
        public List<bonus> bonusses; /// list of existing bonusses
        public int lx;
        public int dificulity;
        bool end = false;
        int[] y;
       
        public void endofgame(TouchCollection touch)
        {
            if (end == true)
            {
                if (y[0] > 200)
                {
                    y[0] = y[0] - 5;
                }
                if (y[1] > 271)
                {
                    y[1] = y[1] - 5;
                }
                if (y[2] > 326)
                {
                    y[2] = y[2] - 5;
                }
                if (y[3] > 395)
                {
                    y[3] = y[3] - 5;
                }


                    if(GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
                    {
                        game.scores.addscore(scores);
                        game.statemenager.currentstate = 4;
                        end = false;
                    }
                
                if (generator.NextDouble() < 0.3)
                {
                    smokes.Add(new smoke(3, new Vector2(generator.Next(0, 480), generator.Next(0, 800)), game));
                    smokes.Add(new smoke(5, new Vector2(generator.Next(0, 480), generator.Next(0, 800)), game));
                    smokes.Add(new smoke(4, new Vector2(generator.Next(0, 480), generator.Next(0, 800)), game));
                }
            }
        }
        public void endofgamedraw()
        {
            int l1=0;
            do{
                game.ekran.Draw(game.resources.end[l1], new Rectangle(240 - (game.resources.end[l1].Width / 2), y[l1], game.resources.end[l1].Width, game.resources.end[l1].Height), Color.White);
                l1 = l1 + 1;
            }while(l1!=4);
            }
        public void drawscores()
        {
            int x = 7;
            int y = 13;            
            int l1=0;               
            string s = scorestodraw.ToString();           
            int maxl1=s.Length;
            do
            {
                game.ekran.Draw(game.resources.numbers[Convert.ToInt32(s[l1]) - 48], new Rectangle(x, y, 20, 24), new Color(25,0,49));
                    x = x + 20;
                    l1 = l1 + 1;
            } while (l1<s.Length);
           
           
               
        }
        public void scoreslogic()
        {

            if (scorestodraw < scores)
            {
                if (scores - scorestodraw > 17)
                {
                    scorestodraw = scorestodraw + 17;
                }
                else
                {
                    scorestodraw = scores;
                }

            }
        }
        public void bonuslogic()
        {
            List<bonus> bonusses2 = new List<bonus>();
            foreach (bonus b in bonusses)
            {
                b.move();
                bonusses2.Add(b);
            }
            foreach(bonus b in bonusses)
            {
                
                if (b.location.Y > 800)
                {
                    bonusses2.Remove(b);
                }
                if (game.methods.checkcollision(new Rectangle((int)ship.location.X, (int)ship.location.Y, game.resources.ship[0].Width, (int)game.resources.ship[0].Height), 0, new Rectangle((int)b.center.X, (int)b.center.Y, (int)b.size.X, (int)b.size.Y), 0) == true)
                {
                    if (game.soundvolume > 0)
                    {
                        game.resources.powerup.Play();
                    }
                    b.center.Y = 900;
                    scores = scores + 500;
                    if (b.type == 4)
                    {
                        do
                        {
                            b.type = generator.Next(0, 9);
                        } while (b.type == 4);
                    }
                    switch (b.type)
                    {
                        case 0:
                            if (ship.maxgunlvl > ship.gunlevel)
                            {
                                ship.gunlevel = ship.gunlevel + 1;
                            }
                            break;
                        case 1:
                            if (ship.guntype == 0)
                            {
                                if (ship.maxgunlvl > ship.gunlevel)
                                {
                                    ship.gunlevel = ship.gunlevel + 1;
                                }
                            }
                            else
                            {
                                ship.guntype = 0;
                            }
                            break;
                        case 2:
                            if (ship.guntype == 1)
                            {
                                if (ship.maxgunlvl > ship.gunlevel)
                                {
                                    ship.gunlevel = ship.gunlevel + 1;
                                }
                            }
                            else
                            {
                                ship.guntype = 1;
                            }
                            break;
                        case 3:
                            if (ship.guntype == 2)
                            {
                                if (ship.maxgunlvl > ship.gunlevel)
                                {
                                    ship.gunlevel = ship.gunlevel + 1;
                                }
                            }
                            else
                            {
                                ship.guntype = 2;
                            }
                            break;
                        case 4:
                           
                            
                            break;
                        case 5:
                            ship.suse = true;
                            break;
                        case 6:
                            if (ship.a < ship.maxa)
                            {
                                ship.a = ship.a + 10;
                            }
                            break;
                        case 7:
                            if (lifes < 3)
                            {
                                lifes = lifes + 1;
                            }
                            else
                            {
                                scores = scores + 5000;
                            }
                            break;
                        case 8:
                            if (ship.power < ship.maxpower)
                            {
                                ship.power = ship.power + 0.1f;                                
                            }                            
                            break;
                    }
                    bonusses2.Remove(b);
                }

            }
            bonusses.Clear();
            foreach (bonus b in bonusses2)
            {
                bonusses.Add(b);
            }
            bonusses2.Clear();
        }
        public void bonusdraw()
        {
            foreach (bonus b in bonusses)
            {
                b.draw();
            }
        }
        public void hits()
        {
            bool deleted=false;
            List<enemyship>ships2 = new List<enemyship>();
            List<missle>missles2= new List<missle>();
            List<enemyship> hitted = new List<enemyship>();
            foreach(missle m in missles){
                missles2.Add(m);
            }

            foreach(enemyship s in ships){
                ships2.Add(s);
            }
            foreach (missle m in missles)
            {
                
                foreach(enemyship s in ships)
                {
                    if (game.methods.checkcollision(new Rectangle((int)s.center.X, (int)s.center.Y, (int)s.size.X, (int)s.size.Y),s.direction, new Rectangle((int)m.center.X, (int)m.center.Y, (int)m.size.X, (int)m.size.Y),m.kierunek) == true)
                    {
                        if (game.soundvolume > 0)
                        {
                            game.resources.hit.Play();
                        }
                        s.hp = s.hp - m.power;
                        missles2.Remove(m);
                        smokes.Add(new smoke(0, m.location, game));                                          
                    }
                    deleted = false;
                    foreach (enemyship h in hitted)
                    {
                        if (s == h)
                        {
                            deleted = true;
                        }
                    }
                    if (s.hp <= 0 && deleted == false)
                    {
                        if (game.soundvolume > 0)
                        {
                            game.resources.aliendie.Play();
                        }
                        if (s.type > 10)
                        {
                            noofbosses = noofbosses - 1;
                            bonusses.Add(new bonus(new Vector2(s.center.X - 240 + generator.Next(0, 480), 150), game));
                            bonusses.Add(new bonus(new Vector2(s.center.X - 240 + generator.Next(0, 480), 150), game));
                            bonusses.Add(new bonus(new Vector2(s.center.X - 240 + generator.Next(0, 480), 150), game));
                            bonusses.Add(new bonus(new Vector2(s.center.X - 240 + generator.Next(0, 480), 150), game));
                            bonusses.Add(new bonus(new Vector2(s.center.X - 240 + generator.Next(0, 480), 150), game));
                        }
                        hitted.Add(s);
                        ships2.Remove(s);
                        switch (dificulity)
                        {
                            case 0:
                                scores = scores + 50 + currentstrike * 2;
                                if (generator.NextDouble() < 0.4)
                                {
                                    bonusses.Add(new bonus(s.center, game));
                                }
                                break;
                            case 1:
                                if (generator.NextDouble() < 0.2)
                                {
                                    bonusses.Add(new bonus(s.center, game));
                                }
                                scores = scores + 100 + currentstrike * 2;
                                break;
                            case 2:
                                if (generator.NextDouble() < 0.07)
                                {
                                    bonusses.Add(new bonus(s.center, game));
                                }
                                scores = scores + 300 + currentstrike * 2;
                                break;

                        }
                        
                        smokes.Add(new smoke(2, s.center, game));           
                        rubbish.Add(new rubbish(game, s.center));
                    }
                }    
               
            }
            int shipstodelete = ships.Count - ships2.Count;
            noofships = noofships - shipstodelete;
            missles = new List<missle>();
            ships = new List<enemyship>();
            foreach (missle m in missles2)
            {
                missles.Add(m);
            }
            foreach (enemyship s in ships2)
            {
                ships.Add(s);
            }
            ships2 = null;
            missles2 = null;
            hitted.Clear();
            List<enemymissle> em = new List<enemymissle>();
            foreach (enemymissle m in emissles)
            {
                em.Add(m);
            }
            if(!ship.sactive){
                foreach (enemymissle m in emissles)
                {
                    if (game.methods.checkcollision(new Rectangle((int)m.center.X, (int)m.center.Y, (int)m.size.X, (int)m.size.Y), 0f, new Rectangle((int)ship.location.X, (int)ship.location.Y, 25, 25), 0) == true)
                    {
                        em.Remove(m);
                        smokes.Add(new smoke(1, ship.location, game));
                        ship.destroyed = true;
                        ship.destroyedtime = game.time;
                        if (game.soundvolume > 0)
                        {
                            game.resources.crash.Play();
                        }
                    }
                }
            }
            emissles = new List<enemymissle>();
            foreach (enemymissle m in em)
            {
                emissles.Add(m);
            }
            }
        public void movemissles()
        {
            foreach (enemymissle m in emissles)
            {
                m.move();
            }
            foreach (missle m in missles)
            {
                m.move(game.time);                
            }
            List<missle> missles2 = new List<missle>();
            foreach(missle m in missles){
                missles2.Add(m);
            }
            foreach(missle m in missles){
                if(m.center.X>480 || m.center.X<0 || m.center.Y>800 || m.center.Y<0)
                {
                    missles2.Remove(m);
                }
            }
            missles= new List<missle>();
            foreach(missle m in missles2){
                missles.Add(m);
            }
            missles2=new List<missle>();


        }
        public void drawmissles()
        {
            foreach (missle m in missles)
            {
                m.draw(game.time);
            }
            foreach (enemymissle m in emissles)
            {
                m.draw();
            }
        }
        public void shipslogic()
        {
            foreach (enemyship e in ships)
            {
                e.teleport();
                e.move();
                e.rotate();
                e.shot();
                
            }            
        }
        public void drawships()
        {
            foreach (enemyship s in ships)
            {
                s.draw(game.time);

            }

        }
        public void SmokesDrawAndLogic()
        {
            List<smoke> smokes2 = new List<smoke>();
            foreach (smoke s in smokes)
            {
                smokes2.Add(s);
            }
            foreach (smoke s in smokes)
            {
                if (s.todelete == true)
                {
                    smokes2.Remove(s);
                }
                s.draw();

            }
            smokes =new List<smoke>();
            foreach (smoke s in smokes2)
            {
                smokes.Add(s);
            }
            
        }
        public void drawrubbish()
        {
            foreach (rubbish r in rubbish)
            {
                r.draw();
            }
        }
        public void rubbishlogic()
        {
            List<rubbish> rubbish2 = new List<rubbish>();
            foreach (rubbish r in rubbish)
            {
                r.move();            
                rubbish2.Add(r);
                if (r.todelete == true)
                {
                    rubbish2.Remove(r);
                }
            }
            rubbish = new List<rubbish>();
            foreach (rubbish r in rubbish2)
            {
                rubbish.Add(r);
            }

        }
        public GAME(Game1 game)
        {
            end = false;
            y = new int[4];
            y[0] = 800;
            y[1] = 900;
            y[2] = 1000;
            y[3] = 1100;
            pause = false;
            adbonus = false;
            maxlevels = 10;
            noofbosses = 0;
            dificulity = game.dificulity;
            lifes = 3;
            generator = new Random();
            lx = 1;
              this.game = game;
              noofships = 0;
            missles= new List<missle>();            
            currentstrike = 0;
            ships = new List<enemyship>();
            smokes = new List<smoke>();
            bonusses = new List<bonus>();
            rubbish = new List<rubbish>();
            emissles = new List<enemymissle>();
            ship = new playership(game);
            scores = 0;
            scorestodraw = 0;
        }
        public void newstrike(){
            int l1=1;

            if (noofships <= 0 && noofbosses <= 0 && currentstrike==maxlevels)
            {
                end = true;
            }
            if (noofships <= 0 && (currentstrike+1)%5!=0 && noofbosses==0 && end==false)
            {
                adbonus = false;
                currentstrike = currentstrike + 1;
                game.hud.shortstrings.Add(new sstring(game, game.resources.wave, true, 480, 350,currentstrike));
                przesunieciex = 0;
                przesunieciey = 0;
                this.miny = 0;
                this.minx = -50;
                this.maxx = 530;
                this.maxy = 850;
                
                noofships = generator.Next(22, 35);
                int type = currentstrike % 5;                
                Vector2 position = new Vector2();
                Vector2 dest = new Vector2();              
                int maxshipsatline;
                int l2;
                int minx;
                int miny;
                int l3;
                int typeofship = (int)(currentstrike / 5);
                typeofship = typeofship + 1;
                switch (type)
                {           
                     case 1:
                        maxx = 550;
                        maxy = 800;
                        this.minx = -100;
                        this.miny = -100;                        
                         maxshipsatline=7;
                        l2=0;
                        minx = 59;
                        miny = 66;
                        l3=0;                                               
                        do
                        {
                            do
                            {
                                switch (generator.Next(1, 3))
                                {
                                    case 1:
                                        position.X = -(generator.Next(1, 50));
                                        position.Y = generator.Next(0, 600);
                                        break;
                                    case 2:
                                        position.X = generator.Next(0, 480);
                                        position.Y = -(generator.Next(0,60));
                                        break;
                                    case 3:
                                         position.X =generator.Next(480, 531);
                                        position.Y = generator.Next(0, 600);
                                        break;
                                }
                                ships.Add(new enemyship(game, typeofship, currentstrike, new Vector2(l2 * 59 + (float)minx, miny), position));
                                l2 = l2 + 1;
                                l1 = l1 + 1;
                            }while (l2 != maxshipsatline && l1!=noofships+1);                          
                            if (l2 == 8)
                            {
                                maxshipsatline = 7;
                                minx = 59;
                            }
                            else
                            {
                                maxshipsatline = 8;
                                minx = 34;                                
                            }
                            
                            miny = miny + 66;
                            l2 = 0;
                            l3=l3+1;
                        } while (l1 != noofships+1);
                        break;

                    case 2:
                        miny = 0;
                        minx = -50;
                        maxx = 530;
                        maxy = 850;
                        int l2a =0;
                        int l1a =0;                       
                        bool side =true;
                        dest.Y=80;
                        position.Y=80;
                        do
                        {
                            do
                            {
                                switch(side){
                                    case true:
                                        position.X= -59*l2a-100;
                                        dest.X=position.X+1700;
                                        break;
                                    case false:
                                        position.X =130+ 59*l2a;
                                        dest.X=position.X-1700;
                                        break;
                                }
                                ships.Add(new enemyship(game,typeofship,currentstrike,dest,position));
                                l2a = l2a + 1;
                                l1a = l1a + 1;
                            } while (l2a != 8 && l1a != noofships);
                            dest.Y=dest.Y+66;
                            position.Y=position.Y+66;                            
                            side = !side;
                            l2a = 0;
                        } while (l1a != noofships);
                        
                        break;
                    case 3:
                        l1 = 0;
                        l2 = 0;
                        do
                        {
                            
                            position.X = generator.Next(25, 465);
                            position.Y = -(l1*25+generator.Next(1, 25));
                            dest.X = position.X;
                            dest.Y = position.Y + 1700;

                            ships.Add(new enemyship(game, typeofship, currentstrike, dest, position));
                            l1 = l1 + 1;
                        } while (l1 != noofships);
                        break;
                    case 4:
                        przesunieciex = 480;
                        miny = 0;
                        minx = -50;
                        maxx = 530;
                        maxy = 850;
                         l2 =0;
                         l1 =0;                 
                         
                        dest.Y=80;
                        position.Y=80;
                        do
                        {
                            do
                            {
                                position.X = generator.Next(480, 600);
                                position.Y = generator.Next(25, 50) + l1 * 25;
                                dest.X = position.X - 1400;
                                dest.Y = position.Y - 1400;
                                ships.Add(new enemyship(game,typeofship,currentstrike,dest,position));
                                l2 = l2 + 1;
                                l1 = l1 + 1;
                            } while (l2 != 5 && l1 != noofships);
                           
                           // typeofships = generator.Next(1, 10);
                           
                            l2 = 0;
                        } while (l1 != noofships);
                        break;
                    case 5:
                        przesunieciex = -480;
                        miny = 0;
                        minx = -50;
                        maxx = 530;
                        maxy = 850;
                        l2 = 0;
                        l1 = 0;                      

                        dest.Y = 80;
                        position.Y = 80;
                        do
                        {
                            do
                            {
                                position.X = -generator.Next(0, 120);
                                position.Y = generator.Next(25, 50) + l1 * 25;
                                dest.X = position.X + 1400;
                                dest.Y = position.Y - 1400;
                                ships.Add(new enemyship(game, typeofship, currentstrike, dest, position));
                                l2 = l2 + 1;
                                l1 = l1 + 1;
                            } while (l2 != 8 && l1 != noofships);                            
                          //  typeofships = generator.Next(1, 10);

                            l2 = 0;
                        } while (l1 != noofships);

                        break;

                }
               
            }
           
            if (noofships <= 0 && (currentstrike+1) % 5 == 0 && noofbosses ==0 && currentstrike!=maxlevels)
            {
                adbonus = false;
                currentstrike = currentstrike + 1;
                int x = currentstrike;
                if(x > 50)
                {
                    x=x-50;
                }
                noofbosses = 1;
                x = x / 5;
                minx = 0;
                miny = 0;
                maxx = 480;
                maxy = 800;
                game.hud.shortstrings.Add(new sstring(game, game.resources.wave, true, 480, 350, currentstrike));
                ships.Add(new enemyship(game,x));
                game.resources.boss.Play();
            }
           
        }
    }
    public class button
    {
        public Game1 game;
        public int x;
        public int y;
        public int width;
        public int height;
        public string name;
        int type; /// 0 back, 1 resume,2 play,3 options, 4 hi scores,5 about,6 exit; 

        public button(int x,int y,int width,int height, Game1 game, string name)
        {
            this.game = game;
            this.x = x;
            this.y = y;
            this.width = width;
            this.height = height;
            this.name = name;
            type = 999;
            
        }
        public button(int type, Game1 game)
        {
            this.type = type;
            name = null;
            this.game = game;            
            width = game.resources.specialbutton[type].Width;
            height = game.resources.specialbutton[type].Height;
            this.x = 460 - width;
            switch (type)
            {
                case 0:
                    this.y = 800-48;
                    this.x = 0;
                    break;
                case 1:
                    this.y = 260;
                    break;
                case 2:
                    this.y = 290 + 60;
                    break;
                case 3:
                    this.y = 290 + 180 ;
                    break;
                case 4:
                    this.y = 290 + 120;
                    break;
                case 5:
                    this.y = 290 + 240;
                    break;
                case 6:
                    this.y = 290 + 300;
                    break;
                
            }

        }
        public bool ispressed(TouchCollection touch)
        {
            if (game.currentgame == null && type==1)
            {
                return false;
            }
            foreach (TouchLocation tl in touch)
            {
                if (tl.Position.X > x && tl.Position.X < x + width && tl.Position.Y > y && tl.Position.Y < y + height && (tl.State == TouchLocationState.Pressed || tl.State == TouchLocationState.Moved))
                {
                    return true;
                }
            }
            return false;
        }
        public bool isreleased(TouchCollection touch)
        {
             
            foreach (TouchLocation tl in touch)
            {
            if (tl.Position.X > x && tl.Position.X < x + width && tl.Position.Y > y && tl.Position.Y < y + height && tl.State == TouchLocationState.Released)
                {
                    return true;
                }
            }
            return false;

        }
        public void draw()
        {
            if (type == 999)
            {
                if (this.ispressed(game.touch) == true)
                {
                    game.ekran.Draw(game.resources.menupressedbutton, new Rectangle(x, y, width, height), Color.White);
                }
                else
                {
                    game.ekran.Draw(game.resources.menubutton, new Rectangle(x, y, width, height), Color.White);
                }
                game.ekran.DrawString(game.resources.basicfont, name, new Vector2(x + 10, y + 10), Color.Gray);
            }
            if (name == null)
            {
                if(type != 1){
                game.ekran.Draw(game.resources.specialbutton[type], new Rectangle(x,y,width,height), Color.White);
                }
                else{
                    if(type==1 && game.currentgame!= null){
                        game.ekran.Draw(game.resources.specialbutton[type], new Rectangle(x, y, width, height), Color.White);
                    }                    
                }

            }
        }
    }
    public class startscreen
    {
        Game1 game;
        public startscreen(Game1 game)
        {
            this.game = game;            
        }
        public void drawstartscreen()
        {
            game.ekran.Draw(game.resources.start, new Rectangle(0, 0, 480, 800), Color.White);
            game.ekran.Draw(game.resources.yes, new Rectangle(0, 720 - game.resources.yes.Height, game.resources.yes.Width, game.resources.yes.Height), Color.White);
            game.ekran.Draw(game.resources.no, new Rectangle(480-game.resources.no.Width, 720 - game.resources.no.Height, game.resources.no.Width, game.resources.no.Height), Color.White);
            game.ekran.Draw(game.resources.soundstring, new Rectangle(240 - (int)(game.resources.soundstring.Width / 2), 600, game.resources.soundstring.Width, game.resources.soundstring.Height), Color.White);
        }
        public void checktouch()
        {
            TouchCollection touch = TouchPanel.GetState();
            foreach (TouchLocation tl in touch)
            {
                if (tl.Position.X > 0 && tl.Position.X < 100 && tl.Position.Y > 620 && tl.Position.Y < 800)
                {
                    game.statemenager.changestate(2);
                    game.soundvolume = 0;
                }
                if (tl.Position.X > 380 && tl.Position.X < 480 && tl.Position.Y > 620 && tl.Position.Y < 800)
                {
                    game.statemenager.changestate(2);
                    game.soundvolume = 1;
                }   
            }

        }
    
    }

    public class sstring
    {
        Game1 game;
        Texture2D napis;
        long time;
        int x;
        int y;        
        Color a;
        bool ruchomy;
        public int todelete;
        int wave;
        int steps;
        public sstring(Game1 game, Texture2D napis, bool ruchomy, int x, int y, int wave)
        {
            time = game.time;
            this.wave = wave;
            this.game = game;
            this.ruchomy = ruchomy;
            this.x = x;
            this.y = y;
            this.napis = napis;
            a = new Color(255, 255, 255, 255);
            todelete = 0;
        }
        public sstring(Game1 game,Texture2D napis, bool ruchomy, int x, int y)
        {
            time = game.time;
            this.wave = 0;
            this.game = game;
            this.ruchomy = ruchomy;
            this.x = x;
            this.y = y;
            this.napis = napis;
            a = new Color(255, 255, 255, 255);
            steps=0;
            todelete = 0;
        }
        public void logic()
        {
            if (ruchomy)
            {
                x = x - (int)((float)(game.time - game.lasttime) / 1000f * 200f);

            }
            if (time + 5000 < game.time)
            {
                todelete = 1;
            }
        }
        public void draw()
        {
            a = new Color(new Vector4(255, 255, 255, (255 - steps * 3)));
            if (wave == 0)
            {
               
                game.ekran.Draw(napis, new Rectangle(x, y, napis.Width, napis.Height), a);
            }
            else
            {
                game.ekran.Draw(napis, new Rectangle(x, y, napis.Width, napis.Height), a);
                string wavenumber = wave.ToString();
                int lmax = wavenumber.Length;
                int l1 = 0;
                int x2 = x+205;
                do
                {
                    game.ekran.Draw(game.resources.numbers2[Convert.ToInt32(wavenumber[l1]) - 48], new Rectangle(x2, y, 40, 60), a);
                    x2 = x2 + 45;
                    l1 = l1 + 1;
                } while (l1 != lmax);
            }
            
        }
    }
    public class gamehud
    {
        Game1 game;
        long lasttime;
        int laptime;
        int backgroundposition;
        int backgroundposition2;
        
        public List<sstring> shortstrings;

        public gamehud(Game1 game)
        {
            
           shortstrings = new List<sstring>();
           this.game = game;
           lasttime = this.game.time;
           laptime = 30;
           backgroundposition = -800;
           backgroundposition2 = 0;           
        }
        public void helplogic(TouchCollection touch)
        {
            foreach (TouchLocation tl in touch)
            {
                if (tl.State == TouchLocationState.Pressed)
                {
                    game.statemenager.currentstate = 5;
                }
            }
        }
        public void logic(TouchCollection touch)
        {
            

            foreach (TouchLocation tl in touch)
            {
                if (game.currentgame.pause == true)
                {
                    game.currentgame.pause = false;
                }
                if (tl.Position.Y > 0 && tl.Position.Y < 100 && tl.Position.X > 380 && tl.Position.X < 480 && tl.State == TouchLocationState.Released)
                {
                    game.currentgame.pause = true;

                }
                if (tl.Position.Y > 0 && tl.Position.Y < 100 && tl.Position.X > 0 && tl.Position.X < 100 && tl.State == TouchLocationState.Released)
                {
                    game.statemenager.currentstate = 10;

                }                

            }

            List<sstring> s = new List<sstring>();
            foreach (sstring ss in shortstrings)
            {
                ss.logic();
                s.Add(ss);                
                if (ss.todelete == 1)
                {
                    s.Remove(ss);
                }
            }
            shortstrings = new List<sstring>();
            foreach (sstring ss in s)
            {
                shortstrings.Add(ss);
            }
        }
        public void drawbackground(){
            if (game.time>= lasttime + laptime)
            {
                lasttime = game.time;
                backgroundposition = backgroundposition + 2;
                backgroundposition2 = backgroundposition2 + 2;
                if (backgroundposition > 799)
                {
                    backgroundposition = -800;
                }
                if (backgroundposition2 > 799)
                {
                    backgroundposition2 = -800;
                }
            }
            game.ekran.Draw(game.resources.backgroundgame, new Vector2(0, (float)backgroundposition), Color.White);
            game.ekran.Draw(game.resources.backgroundgame, new Vector2(0, (float)backgroundposition2), Color.White);
          
        }
        public void drawxy(TouchCollection touch){
            Vector2 xy = new Vector2(0);
            foreach(TouchLocation tl in touch){
                xy.X = tl.Position.X;
                xy.Y = tl.Position.Y;
            }
            game.ekran.DrawString(game.resources.basicfont, xy.ToString(), new Vector2(0, 40), Color.Red);
        }
        public void drawpad()
        {
            int movingy = 0;
            if (game.full == false)
            {
                movingy= -80;                
            }
            if (game.fireplace == 1)
            {
                game.ekran.Draw(game.resources.pad, new Rectangle(10, 640+movingy, 150, 150), Color.White);
            }
            else
            {
                game.ekran.Draw(game.resources.pad, new Rectangle(320, 640+movingy, 150, 150), Color.White);
            }
        }
        public void drawbeam()
        {
            game.ekran.Draw(game.resources.beam,new Rectangle(0,0,480,48),Color.White);            
            switch(game.currentgame.lifes){
                case 0:
            game.ekran.Draw(game.resources.smallship, new Rectangle(320, 8, 32, 32), Color.White);
            game.ekran.Draw(game.resources.smallship, new Rectangle(362, 8, 32, 32), Color.White);
            game.ekran.Draw(game.resources.smallship, new Rectangle(404, 8, 32, 32), Color.White);
            break;
                case 1:
            game.ekran.Draw(game.resources.smallship, new Rectangle(320, 8, 32, 32), Color.Indigo);
            game.ekran.Draw(game.resources.smallship, new Rectangle(362, 8, 32, 32), Color.White);
            game.ekran.Draw(game.resources.smallship, new Rectangle(404, 8, 32, 32), Color.White);
            break;
                case 2:
            game.ekran.Draw(game.resources.smallship, new Rectangle(320, 8, 32, 32), Color.Indigo);
            game.ekran.Draw(game.resources.smallship, new Rectangle(362, 8, 32, 32), Color.Indigo);
            game.ekran.Draw(game.resources.smallship, new Rectangle(404, 8, 32, 32), Color.White);
            break;
                case 3:
            game.ekran.Draw(game.resources.smallship, new Rectangle(320, 8, 32, 32), Color.Indigo);
            game.ekran.Draw(game.resources.smallship, new Rectangle(362, 8, 32, 32), Color.Indigo);
            game.ekran.Draw(game.resources.smallship, new Rectangle(404, 8, 32, 32), Color.Indigo);
            break;
            }
            switch(game.currentgame.ship.gunlevel)
            {
                case 0:
                    game.ekran.Draw(game.resources.gunlvl, new Rectangle(446, 8, 32, 32), Color.White);
                    break;
                case 1:
                    game.ekran.Draw(game.resources.gunlvl, new Rectangle(446, 8, 32, 32), Color.White);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 32, 26, 6), Color.Red);
                    break;
                case 2:
                    game.ekran.Draw(game.resources.gunlvl, new Rectangle(446, 8, 32, 32), Color.White);                    
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 32, 26, 6), Color.Red);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 26, 26, 6), Color.Orange);
                    break;
                case 3:
                    game.ekran.Draw(game.resources.gunlvl, new Rectangle(446, 8, 32, 32), Color.White);                                      
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 32, 26, 6), Color.Red);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 26, 26, 6), Color.Orange);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 20, 26, 6), Color.Yellow);
                    break;
                case 4:
                    game.ekran.Draw(game.resources.gunlvl, new Rectangle(446, 8, 32, 32), Color.White);                                        
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 32, 26, 6), Color.Red);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 26, 26, 6), Color.Orange);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 20, 26, 6), Color.Yellow);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 14, 26, 6), Color.Green);
                    break;
                case 5:
                    game.ekran.Draw(game.resources.gunlvl, new Rectangle(446, 8, 32, 32), Color.White);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 32, 26, 6), Color.Red);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 26, 26, 6), Color.Orange);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 20, 26, 6), Color.Yellow);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 14, 26, 6), Color.Green);
                    game.ekran.Draw(game.resources.gunlvlbeam, new Rectangle(449, 10, 26, 6), Color.LimeGreen);
                    break;

            }
            game.ekran.Draw(game.resources.gunlvl, new Rectangle(446, 8, 32, 32), Color.White);
            game.currentgame.drawscores();
            game.ekran.Draw(game.resources.blvl, new Rectangle(-64, 13, 480, 24), new Color(25, 0, 49)); 
            int x = 210;
            int y = 14;            
            int l1 = 0;
            string s = game.currentgame.currentstrike.ToString();
            int maxl1 = s.Length;
            do
            {
                game.ekran.Draw(game.resources.numbers[Convert.ToInt32(s[l1]) - 48], new Rectangle(x, y, 20, 24), new Color(25,0,49));
                x = x + 20;
                l1 = l1 + 1;
            } while (l1 < s.Length);
            game.ekran.Draw(game.resources.bottombeam, new Rectangle(0, 720, 480, 80), Color.White);
        
        }
        public void drawhelp()
        {
            game.ekran.Draw(game.resources.help, new Rectangle(0, 0, 480, 800), Color.White);

        }
        public void drawbuttons()
        {
            if (game.currentgame.pause == true)
            {
                game.ekran.Draw(game.resources.ntap, new Rectangle(0, 360, 480, 114), Color.White);
            }
            game.ekran.Draw(game.resources.nhelp, new Rectangle(0, 50, 80, 31), Color.White);
            game.ekran.Draw(game.resources.npause, new Rectangle(380, 50, 100, 31), Color.White);
        }
        public void drawfire()
        {
            int movingy = 0;
            if (game.full == false)
            {
                movingy = -80;
            }
            foreach (sstring ss in shortstrings)
            {
                ss.draw();
            }
            if (game.fireplace == 1)
            {
                game.ekran.Draw(game.resources.fire, new Rectangle(360, 680+movingy, 100, 100), Color.White);
            }
            else
            {
                game.ekran.Draw(game.resources.fire, new Rectangle(10, 680+movingy, 100, 100), Color.White);
            }

        }
    }
    public class aboutmenu
    {
        public button back;
        Game1 game;


        public aboutmenu(Game1 game)
        {
            this.game = game;
            back = new button(0, game);  
        }
    }
    public class statemgr
    {
        public int currentstate; // 1 start  2 main menu, 21about, 3 picking lvl 4 game 5
        public int laststate;

        public statemgr(Game1 gra)
        {
            currentstate = 1;
        }
        public void changestate(int changeto)
        {
            laststate = currentstate;
            currentstate = changeto;
        }

    }
    public class settingsmenu:gamestates
    {
        public Game1 game;
        public button back;        
        public int currentplanet;
        public settingsmenu(Game1 game)
        {
            this.game = game;
            back = new button(0, game);         
        }
        public void draw()
        {
            game.ekran.Draw(game.resources.settings, new Rectangle(0, 0, 480, 800), Color.White);
            game.ekran.Draw(game.resources.dificulity[game.dificulity], new Rectangle(277, 442, 162, 38), Color.White);
            if (game.fireplace == 1)
            {
                game.ekran.Draw(game.resources.controls1, new Rectangle(283, 537, 150, 81), Color.White);
            }
            else
            {
                game.ekran.Draw(game.resources.controls2, new Rectangle(283, 537, 150, 81), Color.White);
            }
            game.ekran.Draw(game.resources.volume, new Vector2(277, 324), new Rectangle(0, 0, game.soundvolume * 38, 38), Color.White);

        }
        public void logic(TouchCollection touch)
        {
            foreach (TouchLocation tl in touch)
            {
                if (tl.Position.X > 240 && tl.Position.X < 282 && tl.Position.Y > 320 && tl.Position.Y < 364 && tl.State == TouchLocationState.Released)
                {
                    game.soundvolume = game.soundvolume - 1;
                    if (game.soundvolume < 0)
                    {
                        game.soundvolume = 0;
                    }
                }
                if (tl.Position.X > 436 && tl.Position.X < 482 && tl.Position.Y > 320 && tl.Position.Y < 365 && tl.State == TouchLocationState.Released)
                {
                    game.soundvolume = game.soundvolume + 1;
                    if (game.soundvolume > 4)
                    {
                        game.soundvolume = 4;
                    }
                }
                if (tl.Position.X > 240 && tl.Position.X < 282 && tl.Position.Y > 440 && tl.Position.Y < 480 && tl.State == TouchLocationState.Released)
                {
                    game.dificulity = game.dificulity - 1;
                    if (game.dificulity < 0)
                    {
                        game.dificulity = 0;
                    }
                }
                if (tl.Position.X > 430 && tl.Position.X < 482 && tl.Position.Y > 440 && tl.Position.Y < 480 && tl.State == TouchLocationState.Released)
                {
                    game.dificulity = game.dificulity + 1;
                    if (game.dificulity > 2)
                    {
                        game.dificulity = 2;
                    }
                }
                if (tl.Position.X > 244 && tl.Position.X <284 && tl.Position.Y > 560 && tl.Position.Y < 600 && tl.State == TouchLocationState.Released)
                {
                    game.fireplace = game.fireplace - 1;
                    if (game.fireplace < 1)
                    {
                        game.fireplace = 1;
                    }
                }
                if (tl.Position.X > 430 && tl.Position.X < 480 && tl.Position.Y > 560 && tl.Position.Y < 600 && tl.State == TouchLocationState.Released)
                {
                    game.fireplace = game.fireplace + 1;
                    if (game.fireplace > 2)
                    {
                        game.fireplace = 2;
                    }
                }
            }

        }
    }
    public class Menu : gamestates

    {
        Game1 game;
        button[] button;
        public Menu(Game1 game){
            this.game = game;
            game.aboutMenu = new aboutmenu(game);
            game.settings = new settingsmenu(game);
            game.scores = new hiscoremenu(game);
    }
        public void checkbuttons()
        {
            if(button==null){
                button = new button[6];
            button[0] = new button(1, game);
            button[1] = new button(2, game);
            button[2] = new button(3, game);
            button[3] = new button(4, game);
            button[4] = new button(5, game);
            button[5] = new button(6, game); 
            }
            if (button[0].ispressed(game.touch) == true)
            {
                game.statemenager.changestate(Game);
                
            }
            if (button[1].ispressed(game.touch) == true)
            {
                game.currentgame = new GAME(game);
                game.statemenager.currentstate = 10;
                
            }
            if (button[2].ispressed(game.touch) == true)
            {
                game.statemenager.changestate(Settings);
            }
            if (button[3].ispressed(game.touch) == true)
            {
                game.statemenager.changestate(Hiscores);
            }
            if (button[4].ispressed(game.touch) == true)
            {
                game.statemenager.changestate(About);
            }
            if (button[5].ispressed(game.touch) == true)
            {
                game.Exit();
            }
        }
        public void drawbuttons()
        {
            if (button == null)
            {
                button = new button[6];
                button[0] = new button(1, game);
                button[1] = new button(2, game);
                button[2] = new button(3, game);
                button[3] = new button(4, game);
                button[4] = new button(5, game);
                button[5] = new button(6, game);
            }
            foreach(button b in button){
                b.draw();
            }

        }
    }

    
    public class Game1 : Microsoft.Xna.Framework.Game
    {
        string ApplicationId;
        string AdUnitId;
        DrawableAd bannerAd;
        public long pausetime;
        private GeoCoordinateWatcher gcw;
        public bool full;
        GraphicsDeviceManager graphics;        
        public settingsmenu settings;
        public SpriteBatch ekran;
        public Game1 game;
        public Menu menu;
        public aboutmenu aboutMenu;
        public resourcesmgr resources;
        public gamehud hud;
        public statemgr statemenager;        
        public startscreen start;
        public GAME currentgame;
        public TouchCollection touch;
        public usefullmethods methods;
        public hiscoremenu scores;
        public long time;
        public long soundtime;
        public long lasttime;
        //settings
        public int soundvolume;
        public int dificulity;//0easy 1 medium 2 hard(core);
        public int fireplace;//1 left pad right fire, 2 right pad left fire;
        public Game1()
        {
            ApplicationId = "a522c0b8-602d-445c-af77-94601b8f83a3";
            AdUnitId = "115278";
            pausetime = 0;
            time = 0;
            soundtime = 0;
            soundvolume = 3;
            dificulity = 1;
            fireplace = 1;
            graphics = new GraphicsDeviceManager(this);
            Content.RootDirectory = "Content";
            game = this;               
            start = new startscreen(game);            
            statemenager = new statemgr(game);                        
            hud = new gamehud(game);
            resources = new resourcesmgr();            
            resources.getgame(game);
            methods = new usefullmethods();
            // Frame rate is 30 fps by default for Windows Phone.
            TargetElapsedTime = TimeSpan.FromTicks(333333);
            graphics.PreferredBackBufferWidth = 480;
            graphics.PreferredBackBufferHeight = 800;
            // Extend battery life under lock.
            InactiveSleepTime = TimeSpan.FromSeconds(1);
            using (IsolatedStorageFile file = IsolatedStorageFile.GetUserStoreForApplication())
            {
                string path = "full.txt";
                using (IsolatedStorageFileStream s = new IsolatedStorageFileStream(path, FileMode.OpenOrCreate, file))
                {
                    if (file.FileExists(path))
                    {
                        StreamReader reader = new StreamReader(s);
                        full = Convert.ToBoolean(reader.ReadLine());
                        reader.Close();
                    }
                    else
                    {
                        StreamWriter writer = new StreamWriter(s);
                        writer.WriteLine("0"); /////////////////////// 0 for lite, 1 for full
                        full = false;
                    }


                }


            }
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            AdGameComponent.Initialize(this, ApplicationId);
            // TODO: Add your initialization logic here
            Components.Add(AdGameComponent.Current);
            CreateAd();
            base.Initialize();
        }

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            // Create a new SpriteBatch, which can be used to draw textures.
           ekran = new SpriteBatch(GraphicsDevice);
           resources.loadtextures();
           resources.loadsounds();
            // TODO: use this.Content to load your game content here
        }

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime gameTime)
        {
            /*
            if (soundvolume > 0)
            {
                if (MediaPlayer.State != MediaState.Playing)
                {
                    MediaPlayer.Play(game.resources.backgroundsound);
                    soundtime = time;
                }
                if (soundtime + 108 * 1000 < time || soundtime == 0)
                {

                    MediaPlayer.Play(game.resources.backgroundsound);
                    soundtime = time;
                }
            }
            else
            {
                MediaPlayer.Stop();
            }
            */
            if (menu == null)
            {
                menu = new Menu(game);
            }
            lasttime = time;
            time = (long)gameTime.TotalGameTime.TotalMilliseconds - pausetime; ;
            touch = TouchPanel.GetState();
            
            // TODO: Add your update logic here
            switch(statemenager.currentstate){
                case 1:
            start.checktouch();
            touch = TouchPanel.GetState();
            break;
                case 21:
            if (aboutMenu.back == null)
            {
                aboutMenu.back = new button(0, game);
            }
            if (aboutMenu.back.ispressed(game.touch) == true)
            {
                statemenager.changestate(2);

            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                statemenager.changestate(2);
            }
            break;
                case 2:
            menu.checkbuttons();
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                if (game.currentgame == null)
                {
                    this.Exit();
                }
                else
                {
                    statemenager.currentstate = 5;
                }
            }
            break;
                case 3:
            settings.logic(touch);
            if (settings.back == null)
            {
                settings.back = new button(0, game); 
            }
            if (settings.back.ispressed(touch) == true)
            {
                game.statemenager.changestate(2);
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                statemenager.changestate(2);
            }
            break;
                    case 4:
            if (scores.back == null)
            {
                scores.back = new button(0, game);
            }
            if (scores.back.ispressed(game.touch))
            {
                statemenager.changestate(2);
                
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                statemenager.changestate(2);
            }
            if (currentgame != null)
            {
                if (currentgame.lifes == 0)
                {
                    currentgame = null;
                }
            }
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                statemenager.changestate(2);
            }
            break;
                case 5:
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                statemenager.changestate(2);
            }
            if (game.currentgame.pause == false)
            {
                currentgame.hits();
                currentgame.ship.respawn();
                if (!currentgame.ship.destroyed)
                {
                    currentgame.ship.move(touch);
                    currentgame.ship.shot(touch, time);
                    currentgame.ship.crash();
                    currentgame.ship.getbonus(touch);
                }
                currentgame.movemissles();
                currentgame.shipslogic();
                currentgame.newstrike();
                currentgame.rubbishlogic();
                currentgame.bonuslogic();
                currentgame.scoreslogic();
                currentgame.endofgame(touch);
            }
            hud.logic(touch);
            break;
                default:
            statemenager.changestate(2);
            break;
                case 10:
            if (GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed)
            {
                statemenager.changestate(5);
            }
            hud.helplogic(touch);
            break;

        }
            base.Update(gameTime);
        }

        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            if (menu == null)
            {
                menu = new Menu(game);
            }
            GraphicsDevice.Clear(Color.CornflowerBlue);
            ekran.Begin();
            ekran.Draw(resources.menu, new Rectangle(0, 0, 480, 800), Color.White);
            switch (statemenager.currentstate)
            {
                case 1:
                    start.drawstartscreen();
                    break;
                case 21:
                    aboutMenu.back.draw();
                    ekran.Draw(resources.about, new Rectangle(0, 0, 480, 800), Color.White);
                    break;
                case 2:
                    menu.drawbuttons();
                    break;
                case 3:
                    settings.draw();
                    settings.back.draw();
                    break;
                case 4:
                    scores.back.draw();
                    scores.drawscores();
                    break;
                case 5:
                    hud.drawbackground();
                    currentgame.drawrubbish();
                    currentgame.bonusdraw();
                    currentgame.drawmissles(); 
                    currentgame.ship.drawship();
                    currentgame.drawships();
                    currentgame.ship.drawspeed();
                    currentgame.SmokesDrawAndLogic();
                    hud.drawbeam();                    
                    hud.drawfire();
                    hud.drawpad();
                    hud.drawbuttons();                    
                    currentgame.endofgamedraw();
                    break;
                case 10:
                    hud.drawhelp();
                    
                    break;
            }
            if (statemenager.currentstate != 5)
            {
                foreach(TouchLocation tl in touch){
                    if (soundvolume > 0)
                    {
                        resources.click.Play();
                    }
                    ekran.Draw(resources.circle, new Rectangle((int)tl.Position.X - resources.circle.Width / 2, (int)tl.Position.Y - resources.circle.Height / 2, (int)resources.circle.Width, (int)resources.circle.Height), Color.White);
                    }
            } if (statemenager.currentstate != 1)
            {
                game.ekran.Draw(game.resources.bottombeam, new Rectangle(0, 720, 480, 80), Color.White);
            }
            ekran.End();
            base.Draw(gameTime);            
            
        }
        private void CreateAd()
        {
            
            bannerAd = AdGameComponent.Current.CreateAd(AdUnitId, new Rectangle(0, 720, 480,80), true);
            bannerAd.ErrorOccurred += new EventHandler<Microsoft.Advertising.AdErrorEventArgs>(bannerAd_ErrorOccurred);
            bannerAd.AdRefreshed += new EventHandler(bannerAd_AdRefreshed);


           AdGameComponent.Current.Enabled = false;            
           this.gcw = new GeoCoordinateWatcher();
           this.gcw.PositionChanged += new EventHandler<GeoPositionChangedEventArgs<GeoCoordinate>>(gcw_PositionChanged);
           this.gcw.StatusChanged += new EventHandler<GeoPositionStatusChangedEventArgs>(gcw_StatusChanged);
           this.gcw.Start();
        } 
        private void bannerAd_AdRefreshed(object sender, EventArgs e)
        {
            Debug.WriteLine("Ad received successfully");
        }
        private void bannerAd_ErrorOccurred(object sender, Microsoft.Advertising.AdErrorEventArgs e)
        {
           Debug.WriteLine("Ad error: " + e.Error.Message);
       }
        private void gcw_PositionChanged(object sender, GeoPositionChangedEventArgs<GeoCoordinate> e)
        {
            // Stop the GeoCoordinateWatcher now that we have the device location.
            this.gcw.Stop();

            bannerAd.LocationLatitude = e.Position.Location.Latitude;
            bannerAd.LocationLongitude = e.Position.Location.Longitude;

            AdGameComponent.Current.Enabled = true;

            Debug.WriteLine("Device lat/long: " + e.Position.Location.Latitude + ", " + e.Position.Location.Longitude);
        }
        private void gcw_StatusChanged(object sender, GeoPositionStatusChangedEventArgs e)
        {
            if (e.Status == GeoPositionStatus.Disabled || e.Status == GeoPositionStatus.NoData)
            {
                // in the case that location services are not enabled or there is no data
                // enable ads anyway
                AdGameComponent.Current.Enabled = true;
                Debug.WriteLine("GeoCoordinateWatcher Status :" + e.Status);
            }
        }
         
        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (disposing)
            {
                if (this.gcw != null)
                {
                    this.gcw.Dispose();
                    this.gcw = null;
                }
            }
        }


    }
}
