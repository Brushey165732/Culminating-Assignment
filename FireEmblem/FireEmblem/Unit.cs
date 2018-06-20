using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.IO;
using System.Windows.Threading;

namespace FireEmblem
{
    class Unit
    {   

        Window window;
        Canvas canvas;

        Combat combat;
        public Rectangle sprite;

        public bool isalive = true;
        public bool isactive = true;
        public bool hasmoved = false;
        public bool firststatgeneration = true;

        public int x_position;
        public int y_position;
        int x_displacement;
        int y_displacement;

        public int hptotal;
        public int hpleft;
        public int str;
        //public int mag;
        public int skill;
        public int spd;
        public int luck;
        public int def;
        //public int res;

        int moverange = 3;
        int playerbuff = 1;

        int team;
        int turn;
        public int type;

        public Unit(Window w, Canvas c, int x, int y, int allegiance, int classnumber)
        {
            window = w;
            canvas = c;
            x_position = x;
            y_position = y;
            team = allegiance;
            type = classnumber;
            combat = new Combat(w, c, turn);

        }
        public void generate()
        {
            sprite = new Rectangle();
            if (team == 0)
            {

                if (type == 0)
                {
                    sprite.Fill = Brushes.CornflowerBlue;
                    //sprite.Fill = new ImageBrush(new BitmapImage(new Uri("Allied_Mercenary_Sprite", UriKind.Relative)));
                }
                else if (type == 1)
                {
                    sprite.Fill = Brushes.Blue;
                    //sprite.Fill = new ImageBrush(new BitmapImage(new Uri("Allied_Fighter_Sprite", UriKind.Relative)));
                }
                else if(type == 2)
                {
                    sprite.Fill = Brushes.DarkBlue;
                    //sprite.Fill = new ImageBrush(new BitmapImage(new Uri("Allied_Soldier_Sprite", UriKind.Relative)));
                }
            }
            else if (team == 1)
            {
                if (type == 0)
                {
                    sprite.Fill = Brushes.IndianRed;
                    //sprite.Fill = new ImageBrush(new BitmapImage(new Uri("Enemy_Mercenary_Sprite", UriKind.Relative)));
                }
                else if (type == 1)
                {
                    sprite.Fill = Brushes.Red;
                    //sprite.Fill = new ImageBrush(new BitmapImage(new Uri("Enemy_Fighter_Sprite", UriKind.Relative)));
                }
                else if (type == 2)
                {
                    sprite.Fill = Brushes.DarkRed;
                    //sprite.Fill = new ImageBrush(new BitmapImage(new Uri("Enemy_Soldier_Sprite", UriKind.Relative)));
                }
            }
            sprite.Height = 30;
            sprite.Width = 30;
            Canvas.SetLeft(sprite, (x_position * 40) + 5);
            Canvas.SetTop(sprite, (y_position * 40) + 5);
            
            canvas.Children.Add(sprite);
        }
        public void update(Rectangle sprite)
        {
            Canvas.SetLeft(sprite, (x_position * 40) + 5);
            Canvas.SetTop(sprite, (y_position*40) + 5);
        }
        public void playermove(int[,] map_array, List<Unit> allied_units, List<Unit> enemy_units, int i, int x_initial, int y_initial, int x, int y, int turn)
        {
            /*if (Keyboard.IsKeyDown(Key.D1) && team == 0 && counter == 0)
            {
                //counter = 0;
                Console.WriteLine("Position: " + x_position + " " + y_position);
                for (int i = -(moverange+1); i <= moverange+1; i++)
                {
                    x_temp = i;
                    for (int j = -(moverange + 1); j <= moverange + 1; j++)
                    {
                        y_temp = j;
                        Console.WriteLine("i,j = (" + i + ", " + j + ")");
                        if (x_temp < 0)
                        {
                            x_temp = -x_temp;
                        }
                        if (y_temp < 0)
                        {
                            y_temp = -y_temp;
                        }
                        Console.WriteLine("x,y = (" + x_temp + ", " + y_temp + ")");
                        if (x_temp + y_temp > 0 && x_temp + y_temp < moverange + 2)
                        {
                            if (x_position + i <= 17 && x_position + i >= 0 && y_position + j <= 14 && y_position + j >= 0)
                            {
                                Rectangle rect = new Rectangle();
                                rect.Width = 40;
                                rect.Height = 40;
                                if (x_temp + y_temp == moverange + 1)
                                {
                                    rect.Fill = Brushes.IndianRed;
                                }
                                else
                                {
                                    rect.Fill = Brushes.CornflowerBlue;
                                }
                                rect.Opacity = 0.33;
                                Canvas.SetLeft(rect, x_position * 40 + (i * 40));
                                Canvas.SetTop(rect, y_position * 40 + (j * 40));
                                canvas.Children.Add(rect);
                                counter++;
                            }
                        }
                    }                   
                }
                Console.WriteLine("Counter: " + counter.ToString());
            }
            if (Keyboard.IsKeyDown(Key.D2) && counter > 0)
            {
                Console.WriteLine(canvas.Children.Count);
                int canvasitems = canvas.Children.Count;
                Console.WriteLine(counter);
                
                for (int i = canvasitems - 1; i >= canvasitems - counter-24; i--)
                {
                    Console.WriteLine("i = " + i);
                    canvas.Children.RemoveAt(i);
                    ///counter = 0;
                }
                counter = 0;
            }*/
            if (team == 0 && turn % 2 == 0 && allied_units[i].isactive == true && allied_units[i].hasmoved == false)
            {
                int x_displacement = allied_units[i].x_position - x;
                int y_displacement = allied_units[i].y_position - y;
                //Console.WriteLine("Displacement by component: " + x_displacement + ", " + y_displacement);

                if (x_displacement < 0)
                {
                    x_displacement = x_displacement * (-1);
                }
                if (y_displacement < 0)
                {
                    y_displacement = y_displacement * (-1);
                }
                //Console.WriteLine("Total Displacement: " + (x_displacement + y_displacement) + " Move: " + moverange);
                if (x_displacement + y_displacement <= moverange)
                {
                    if (x > 0 && x < 19)
                    {
                        if (y > 0 && y < 16)
                        {
                            if (map_array[y, x] == 0 || map_array[y, x] == '-')
                            {
                                map_array[allied_units[i].y_position, allied_units[i].x_position] = '-';
                                allied_units[i].x_position = x;
                                allied_units[i].y_position = y;
                                Canvas.SetLeft(allied_units[i].sprite, allied_units[i].x_position * 40 + 5);
                                Canvas.SetTop(allied_units[i].sprite, allied_units[i].y_position * 40 + 5);
                                map_array[allied_units[i].y_position, allied_units[i].x_position] = 2;

                                //Console.WriteLine("New Coords: (" + allied_units[i].x_position + ", " + allied_units[i].y_position + ")");

                                allied_units[i].hasmoved = true;
                            }
                            else
                            {
                                Console.WriteLine("Impassable Terrain");
                            }
                        }
                        else
                        {
                            Console.WriteLine("Off of Screen, Y");
                        }
                    }
                    else
                    {
                        Console.WriteLine("Off Of Screen, X");
                    }
                }
                else
                {
                    Console.WriteLine("out of range of unit.");
                }
                if (map_array[y_position, x_position + 1] == 3 || map_array[y_position, x_position - 1] == 3 || map_array[y_position + 1, x_position] == 3 || map_array[y_position -1, x_position] == 3)
                {
                    
                }
                else
                {
                    allied_units[i].isactive = false;
                }
            }
            
        }
        public void enemymove(int[,] map_array, List<Unit> allied_units, List<Unit> enemy_units, int j, int x_initial, int y_initial, int turn_num)
        {
            //this method checks to see if any allied units are close enough to attack. If there is one, it is determined if the enemy unit can move there,
            //if, so they will and will call the attack method. If there are multiple, the enemy unit will move to the enemy unit it can deal the most damage to,
            //then call the attack method.
            bool canreachtarget = true;
            turn = turn_num;
            int i;
            if (team == 1 && turn % 2 != 0)
            {
                
                int target_def = 50;
                Point targetlocation = new Point(-100, -100);
                for(i = 0; i <allied_units.Count; i++)
                {
                    if (allied_units[i].x_position <= enemy_units[j].x_position + enemy_units[j].moverange + 1
                        && allied_units[i].x_position >= enemy_units[j].x_position - enemy_units[j].moverange + 1
                        && allied_units[i].y_position <= enemy_units[j].y_position + enemy_units[j].moverange + 1
                        && allied_units[i].y_position >= enemy_units[j].y_position - enemy_units[j].moverange + 1)
                    {
                        x_displacement = enemy_units[j].x_position - allied_units[i].x_position;
                        if (x_displacement < 0)
                        {
                            x_displacement = -x_displacement;
                        }
                        y_displacement = enemy_units[j].y_position - allied_units[i].y_position;
                        if (y_displacement < 0)
                        {
                            y_displacement = -y_displacement;
                        }
                        if (x_displacement + y_displacement <= moverange + 1)
                        {
                            if (allied_units[i].def < target_def)
                            {
                                targetlocation.X = allied_units[i].x_position;
                                targetlocation.Y = allied_units[i].y_position;
                                target_def = allied_units[i].def;
                            }
                        }
                    }
                }
                Console.WriteLine(target_def.ToString());
                Console.WriteLine(targetlocation.ToString());
                if (target_def >= 50)
                {
                    Console.WriteLine("no vaild target");
                    enemy_units[j].isactive = false;
                }
                else
                {
                    Console.WriteLine("Target in Range");
                    int x = enemy_units[j].x_position;
                    int y = enemy_units[j].y_position;
                    Console.WriteLine(targetlocation.X + ", " + targetlocation.Y);


                    //NEED TO DO PATHING BETWEEN START AND END POSITIONS//


                    //adjacent tiles
                    if (x_displacement + y_displacement == 1)
                    {
                        MessageBox.Show("Attacking adjacent target");
                        combat.fight(map_array, allied_units, enemy_units, i, j, turn);
                    }
                    //2 tiles away
                    if (x_displacement + y_displacement == 2)
                    {
                        MessageBox.Show("Attacking 2 range target");
                        if (x_displacement == 2)
                        {
                            if (targetlocation.X < x)
                            {
                                //-2, 0
                                if (map_array[y, x-1] == 0)
                                {
                                    enemy_units[j].x_position = x - 1;
                                }
                                else if (map_array[y - 1, x - 2] == 0)
                                {
                                    enemy_units[j].x_position = x - 2;
                                    enemy_units[j].y_position = y - 1;
                                }
                                else if (map_array[y + 1, x - 2] == 0)
                                {
                                    enemy_units[j].x_position = x - 2;
                                    enemy_units[j].y_position = y + 1;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                            else if (targetlocation.X > x)
                            {
                                //+2, 0
                                if (map_array[y, x + 1] == 0)
                                {
                                    enemy_units[j].x_position = x + 1;
                                }
                                else if (map_array[y - 1, x + 2] == 0)
                                {
                                    enemy_units[j].x_position = x + 2;
                                    enemy_units[j].y_position = y - 1;
                                }
                                else if (map_array[y + 1, x + 2] == 0)
                                {
                                    enemy_units[j].x_position = x + 2;
                                    enemy_units[j].y_position = y + 1;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                        }
                        if (x_displacement == 1)
                        {
                            if (targetlocation.X < x)
                            {
                                //-1, -1
                                if (targetlocation.Y < y)
                                {
                                    if (map_array[y, x - 1] == 0)
                                    {
                                        enemy_units[j].x_position = x - 1;
                                    }
                                    else if (map_array[y-1, x] == 0)
                                    {
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                //-1, +1
                                else if (targetlocation.Y > y)
                                {
                                    if (map_array[y, x-1] == 0)
                                    {
                                        enemy_units[j].x_position = x - 1;
                                    }
                                    else if (map_array[y+1, x] == 0)
                                    {
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                            else if (targetlocation.X > x)
                            {
                                //+1, -1
                                if (targetlocation.Y < y)
                                {
                                    if (map_array[y, x+1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 1;
                                    }
                                    else if (map_array[y-1, x] == 0)
                                    {
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                //+1, +1
                                else if (targetlocation.Y > y)
                                {
                                    if (map_array[y, x + 1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 1;
                                    }
                                    else if (map_array[y+1, x] == 0)
                                    {
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                        }
                        if (x_displacement == 0)
                        {
                            //0, -2
                            if (targetlocation.Y < y)
                            {
                                if (map_array[y-1, x] == 0)
                                {
                                    enemy_units[j].y_position = y - 1;
                                }
                                else if (map_array[y-2, x-1] == 0)
                                {
                                    enemy_units[j].y_position = y - 2;
                                    enemy_units[j].x_position = x - 1;
                                }
                                else if (map_array[y - 2, x+1] == 0)
                                {
                                    enemy_units[j].y_position = y - 2;
                                    enemy_units[j].x_position = x + 1;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                            //0, +2
                            else if (targetlocation.Y > y)
                            {
                                if (map_array[y + 1, x] == 0)
                                {
                                    enemy_units[j].y_position = y + 1;
                                }
                                else if (map_array[y + 2, x - 1] == 0)
                                {
                                    enemy_units[j].y_position = y + 2;
                                    enemy_units[j].x_position = x - 1;
                                }
                                else if (map_array[y + 2, x + 1] == 0)
                                {
                                    enemy_units[j].y_position = y + 2;
                                    enemy_units[j].x_position = x + 1;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                        }

                    }
                    //3 tiles away
                    if (x_displacement  + y_displacement == 3)
                    {
                        MessageBox.Show("Attacking 3 range target");
                        if (x_displacement == 3)
                        {
                            //-3, 0
                            if (targetlocation.X < x)
                            {
                                if (map_array[y, x - 3] == 0)
                                {
                                    enemy_units[i].x_position = x-3;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                            //+3, 0
                            if (targetlocation.X > x)
                            {
                                if (map_array[y, x + 3] == 0)
                                {
                                    enemy_units[i].x_position = x+3;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }

                        }
                        if (x_displacement == 2)
                        {
                            if (targetlocation.X < x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //-2, -1
                                    if (map_array[y, x-2] == 0)
                                    {
                                        enemy_units[j].x_displacement = x - 2;
                                    }
                                    else if (map_array[y-1, x-1] == 0)
                                    {
                                        enemy_units[j].y_position = y - 1;
                                        enemy_units[j].x_position = x - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y > y)
                                {
                                    //-2, +1
                                    if (map_array[y, x - 2] == 0)
                                    {
                                        enemy_units[j].x_position = x - 2;
                                    }
                                    else if (map_array[y+1, x-1] == 0)
                                    {
                                        enemy_units[j].y_position = y + 1;
                                        enemy_units[j].x_position = x - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                            else if (targetlocation.X > x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //+2, -1
                                    if (map_array[y, x+2] == 0)
                                    {
                                        enemy_units[j].x_position = x - 2;
                                    }
                                    else if (map_array[y-1, x+1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 1;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y < y)
                                {
                                    //+2, +1
                                    if (map_array[y, x + 2] == 0)
                                    {
                                        enemy_units[j].x_position = x + 2;
                                    }
                                    else if (map_array[y+1, x + 1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 1;
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                        }
                        if (x_displacement == 1)
                        {
                            if (targetlocation.X < x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //-1, -2
                                    if (map_array[y - 2, x] == 0)
                                    {
                                        enemy_units[j].y_position = y - 2;
                                    }
                                    else if (map_array[y - 1, x - 1] == 0)
                                    {
                                        enemy_units[j].x_position = x - 1;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y > y)
                                {
                                    //-1, +2
                                    if (map_array[y+2, x] == 0)
                                    {
                                        enemy_units[j].y_position = y + 2;
                                    }
                                    else if (map_array[y + 1, x - 1] == 0)
                                    {
                                        enemy_units[j].x_position = x - 1;
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                            else if (targetlocation.X > x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //+1, -2
                                    if (map_array[y-2, x] == 0)
                                    {
                                        enemy_units[j].y_position = y - 2;
                                    }
                                    else if (map_array[y-1, x+1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 1;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y < y)
                                {
                                    //+1, +2
                                    if (map_array[y+2, x] == 0)
                                    {
                                        enemy_units[j].x_position = x + 2;
                                    }
                                    else if (map_array[y+1, x+1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 1;
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                        }
                        if (x_displacement == 0)
                        {
                            if (targetlocation.Y < y)
                            {
                                //0, -3
                                if (map_array[y-3, x] == 0)
                                {
                                    enemy_units[j].y_position = y - 3;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                            if (targetlocation.Y > y_displacement)
                            {
                                //0, +3
                                if (map_array[y+3, x] == 0)
                                {
                                    enemy_units[j].y_position = y + 3;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                        }

                    }
                    //4 tiles away
                    if (x_displacement + y_displacement == 4)
                    {
                        MessageBox.Show("Attacking a 4 range target");
                        if (x_displacement == 4)
                        {
                            if (targetlocation.X < x)
                            {
                                //-4, 0
                                if (map_array[y, x-4] == 0)
                                {
                                    enemy_units[j].x_position = x - 4;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                            else if (targetlocation.X > x)
                            {
                                //4, 0
                                if (map_array[y, x + 4] == 0)
                                {
                                    enemy_units[j].x_position = x + 4;
                                }
                                else
                                {
                                    canreachtarget = false;
                                }
                            }
                        }
                        else if (x_displacement == 3)
                        {
                            if (targetlocation.X < x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //-3, -1
                                    if (map_array[y, x - 3] == 0)
                                    {
                                        enemy_units[j].x_position = x - 3;
                                    }
                                    else if (map_array[y -1, x - 2] == 0)
                                    {
                                        enemy_units[j].x_position = x - 2;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y > y)
                                {
                                    //-3, +1
                                    if (map_array[y, x-3] == 0)
                                    {
                                        enemy_units[j].x_position = x - 3;
                                    }
                                    else if (map_array[y+1, x-2] == 0)
                                    {
                                        enemy_units[j].x_position = x - 2;
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                            else if (targetlocation.X > x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //+3, -1
                                    if (map_array[y, x+3] == 0)
                                    {
                                        enemy_units[j].x_position = x - 3;
                                    }
                                    else if (map_array[y-1, x+2] == 0)
                                    {
                                        enemy_units[j].x_position = x + 2;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                if (targetlocation.Y > y)
                                {
                                    //+3, +1
                                    if (map_array[y, x+3] == 0)
                                    {
                                        enemy_units[j].x_position = x - 3;
                                    }
                                    else if (map_array[y+1, x+2] == 0)
                                    {
                                        enemy_units[j].x_position = x + 2;
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                        }
                        else if (x_displacement == 2)
                        {
                            if (targetlocation.X < x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //-2, -2
                                    if (map_array[y-1, x-2] == 0)
                                    {
                                        enemy_units[j].x_position = x - 2;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else if (map_array[y-2, x-1] == 0)
                                    {
                                        enemy_units[j].x_position = x - 1;
                                        enemy_units[j].y_position = y - 2;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y > y)
                                {
                                    //-2, +2
                                    if (map_array[y+1, x-2] == 0)
                                    {
                                        enemy_units[j].x_position = x - 2;
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else if (map_array[y+2, x-1] == 0)
                                    {
                                        enemy_units[j].x_position = x - 1;
                                        enemy_units[j].y_position = y + 2;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                            else if (targetlocation.X > x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //+2, -2
                                    if (map_array[y - 1, x + 2] == 0)
                                    {
                                        enemy_units[j].x_position = x + 2;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else if (map_array[y - 2, x + 1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 2;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y > y)
                                {
                                    //+2, +2
                                    if (map_array[y+1, x+2] == 0)
                                    {
                                        enemy_units[j].x_position = x + 2;
                                        enemy_units[j].y_position = y + 1;
                                    }
                                    else if (map_array[y+2, x+1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 1;
                                        enemy_units[j].y_position = y + 2;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                        }
                        else if (x_displacement == 1)
                        {
                            if (targetlocation.X < x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //-1, -3
                                    if (map_array[y - 3, x] == 0)
                                    {
                                        enemy_units[j].x_position = x;
                                        enemy_units[j].y_position = y - 3;
                                    }
                                    else if (map_array[y - 2, x - 1] == 0)
                                    {
                                        enemy_units[j].x_position = x - 1;
                                        enemy_units[j].y_position = y - 2;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y > y)
                                {
                                    //-2, +2
                                    if (map_array[y + 3, x] == 0)
                                    {
                                        enemy_units[j].x_position = x;
                                        enemy_units[j].y_position = y + 3;
                                    }
                                    else if (map_array[y + 2, x - 1] == 0)
                                    {
                                        enemy_units[j].x_position = x - 1;
                                        enemy_units[j].y_position = y + 2;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                            else if (targetlocation.X > x)
                            {
                                if (targetlocation.Y < y)
                                {
                                    //+2, -2
                                    if (map_array[y - 3, x] == 0)
                                    {
                                        enemy_units[j].x_position = x;
                                        enemy_units[j].y_position = y - 3;
                                    }
                                    else if (map_array[y - 2, x + 1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 2;
                                        enemy_units[j].y_position = y - 1;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                                else if (targetlocation.Y > y)
                                {
                                    //+2, +2
                                    if (map_array[y + 3, x] == 0)
                                    {
                                        enemy_units[j].x_position = x;
                                        enemy_units[j].y_position = y + 3;
                                    }
                                    else if (map_array[y + 2, x + 1] == 0)
                                    {
                                        enemy_units[j].x_position = x + 1;
                                        enemy_units[j].y_position = y + 2;
                                    }
                                    else
                                    {
                                        canreachtarget = false;
                                    }
                                }
                            }
                        }
                        else if (x_displacement == 0)
                        {
                            if (targetlocation.Y < y)
                            {
                                if (map_array[y-3, x] == 0)
                                {
                                    enemy_units[j].y_position = y - 3;
                                }
                            }
                            else if (targetlocation.X > y)
                            {
                                if (map_array[y+3, x] == 0)
                                {
                                    enemy_units[j].y_position = y + 3;
                                }
                            }
                        }
                    }
                    enemy_units[j].update(enemy_units[j].sprite);
                    if (canreachtarget)
                    {
                        combat.fight(map_array, allied_units, enemy_units, i-1, j, turn);
                    }
                    else if (!canreachtarget)
                    {
                        enemy_units[j].isactive = false;
                    }
                }
            }
            enemy_units[j].hasmoved = true;
            enemy_units[j].isactive = false;
        }
        //calculates stat values for Combat.cs and Window2
        public void getstats(int team, int classnumber)
        {
            hptotal = 0;

            str = 0;
            //int mag = 0;
            skill = 0;
            spd = 0;
            luck = 0;
            def = 0;
            //int res = 0;
            int moverange = 4;

            if (team == 0)
            {
                hptotal += playerbuff;
                str += playerbuff;
                skill += playerbuff;
                spd += playerbuff;
                luck += playerbuff;
                def += playerbuff;
            }
            if (classnumber == 0)//Mercenary//
            {
                //ImageBrush image = new ImageBrush(new BitmapImage(new Uri( , UriKind.Relative)));
                hptotal += 18;
                str += 4;
                spd += 8;
                skill += 8;
                luck += 4;
                def += 4;
            }
            if (classnumber == 1)//Fighter//
            {
                //ImageBrush image = new ImageBrush(new BitmapImage(new Uri( , UriKind.Relative)));
                hptotal += 20;
                str += 6;
                spd += 4;
                skill += 4;
                luck += 6;
                def += 2;
            }
            if (classnumber == 2)//Soldier//
            {
                //ImageBrush image = new ImageBrush(new BitmapImage(new Uri( , UriKind.Relative))); 
                hptotal += 17;
                str += 5;
                spd += 5;
                skill += 6;
                luck += 3;
                def += 6;
                //moverange += 2;
            }
            if (firststatgeneration)
            {
                hpleft = hptotal;
            }
            firststatgeneration = false;
        }
        
        public void remove()
        {
            sprite.Visibility = Visibility.Hidden;
            x_position = 100;
            y_position = 100;
        }
    }
}
