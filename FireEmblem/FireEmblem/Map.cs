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

namespace FireEmblem
{
    class Map
    {
        Window window;
        Canvas canvas;
        SolidColorBrush colorBrush = new SolidColorBrush();
        
        ImageBrush forest = new ImageBrush(new BitmapImage(new Uri("Forest.png", UriKind.Relative)));
        ImageBrush grass = new ImageBrush(new BitmapImage(new Uri("Grass.png", UriKind.Relative)));
        ImageBrush horizpath = new ImageBrush(new BitmapImage(new Uri("HorizontalPath.png", UriKind.Relative)));
        ImageBrush vertpath = new ImageBrush(new BitmapImage(new Uri("VerticalPath.png", UriKind.Relative)));
        ImageBrush crosspath = new ImageBrush(new BitmapImage(new Uri("CrossPath.png", UriKind.Relative)));
        ImageBrush ImageBrush = new ImageBrush(new BitmapImage(new Uri("string.st", UriKind.Relative)));

        public Map(Window w, Canvas c)
        {
            window = w;
            canvas = c;
        }

        public int[,] mapArray(string map, List<Unit> a_units, List<Unit> e_units)
        {
            string temp = map;
            List<Unit> allied_units = a_units;
            List<Unit> enemy_units = e_units;
            int[,] maparray = new int[15, 18];
            StreamReader reader = new StreamReader(temp);
            int y = 0;
            while (!reader.EndOfStream)
            {
                string line = reader.ReadLine();
                Console.WriteLine(line);
                for (int x = 0; x < line.Length; x++)
                {
                    if (line[x] == '-')
                    {
                        maparray[y,x] = 0;
                    }
                    else if (line[x] == '|')
                    {
                        maparray[y, x] = 1;
                    }
                    else if (line[x] == 'a')
                    {
                        Unit unit = new Unit(window, canvas, x, y, 0, 0);
                        unit.generate();
                        unit.getstats(0, 0);
                        maparray[y, x] = 2;
                        allied_units.Add(unit);
                    }
                    else if (line[x] == 'b')
                    {
                        Unit unit = new Unit(window, canvas, x, y, 0, 1);
                        unit.generate();
                        unit.getstats(0, 1);
                        maparray[y, x] = 2;
                        allied_units.Add(unit);
                    }
                    else if (line[x] == 'c')
                    {
                        Unit unit = new Unit(window, canvas, x, y, 0, 2);
                        unit.generate();
                        unit.getstats(0, 2);
                        maparray[y, x] = 2;
                        allied_units.Add(unit);
                    }
                    else if (line[x] == 'A')
                    {
                        Unit unit = new Unit(window, canvas, x, y, 1, 0);
                        unit.generate();
                        unit.getstats(1, 0);
                        maparray[y, x] = 3;
                        enemy_units.Add(unit);
                    }
                    else if (line[x] == 'B')
                    {
                        Unit unit = new Unit(window, canvas, x, y, 1, 1);
                        unit.generate();
                        unit.getstats(1, 1);
                        maparray[y, x] = 3;
                        enemy_units.Add(unit);
                    }
                    else if (line[x] == 'C')
                    {
                        Unit unit = new Unit(window, canvas, x, y, 1, 2);
                        unit.generate();
                        unit.getstats(1, 2);
                        maparray[y, x] = 3;
                        enemy_units.Add(unit);
                    }
                }
                y++;
            }
            return maparray;
        }

        public void generateMap(string map)
        {
            StreamReader streamReader = new StreamReader(map);
            colorBrush.Color = Color.FromArgb(255, 84, 166, 74);
            int y = 0;
            while (!streamReader.EndOfStream)
            {
                string row = streamReader.ReadLine();
                for (int x = 0; x < row.Length; x++)
                {
                    Rectangle tile = new Rectangle();
                    tile.Width = 40;
                    tile.Height = 40;
                    tile.StrokeThickness = 0.25;
                    tile.Stroke = Brushes.Gray;
                    Canvas.SetLeft(tile, x * 40);
                    Canvas.SetTop(tile, y * 40);

                    if (row[x] == 'g')
                    {
                        tile.Fill = grass;
                    }
                    if (row[x] == 'w')
                    {
                        tile.Fill = Brushes.SandyBrown;
                    }
                    if (row[x] == 'l')
                    {
                        tile.Fill = Brushes.Blue;
                    }
                    if (row[x] == 'f')
                    {
                        tile.Fill = forest;
                    }
                    if (row[x] == 'p')
                    {
                        tile.Fill = Brushes.PaleGreen;
                    }
                    if (row[x] == '-')
                    {
                        tile.Fill = horizpath;
                    }
                    if (row[x] == '|')
                    {
                        tile.Fill = vertpath;
                    }
                    if (row[x] == '+')
                    {
                        tile.Fill = crosspath;
                    }

                    canvas.Children.Add(tile);
                }
                y++;
            }
        }
    }
}
