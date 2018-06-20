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
    class Combat
    {
        Window window;
        Canvas canvas;

        Unit attacker;
        Unit defender;
        Random rnd = new Random();
        public int targets;
        int[,] map_array;
        int classnumber;

        int atk_hptotal;
        int atk_hpleft;
        int atk_str;
        int atk_skill;
        int atk_spd;
        int atk_luck;
        int atk_def;
        bool atk_isalive;

        int def_hptotal;
        int def_hpleft;
        int def_str;
        int def_skill;
        int def_spd;
        int def_luck;
        int def_def;
        bool def_isalive;

        public Combat(Window w, Canvas c, int turn)
        {
            window = w;
            canvas = c;
        }
        public void close_targets(int[,] map_array, List<Unit> allied_units, List<Unit> enemy_units, int i)
        {
            int targets = 0;
            int x = 0;
            int y = 0;
            if (map_array[allied_units[i].y_position + 1, allied_units[i].x_position] == '3')
            {
                targets += 1;
            }
            if (map_array[allied_units[i].y_position - 1, allied_units[i].x_position] == '3')
            {
                targets += 1;
            }
            if (map_array[allied_units[i].y_position, allied_units[i].x_position + 1] == '3')
            {
                targets += 1;
            }
            if (map_array[allied_units[i].y_position, allied_units[i].x_position - 1] == '3')
            {
                targets += 1;
            }
            //Console.WriteLine("Targets: " + targets);
        }
        public void combat_preview()
        {

        }
        public void fight(int[,] map_array, List<Unit> allied_units, List<Unit> enemy_units, int i, int j, int turn)
        {
            //Importing the stats for each unit, and determining the odds of landing hits or critical hits on the opposing unit
            Console.WriteLine(i);
            if (allied_units[i].hptotal == 18)
            {
                classnumber = 0;
            }
            else if (allied_units[i].hptotal == 20)
            {
                classnumber = 1;
            }
            else if (allied_units[i].hptotal == 17)
            {
                classnumber = 2;
            }
            if (turn % 2 == 0)
            {
                allied_units[i].getstats(0, classnumber);

                //Unit attacker = allied_units[i];
                atk_hpleft = allied_units[i].hpleft;
                atk_str = allied_units[i].str;
                atk_skill = allied_units[i].skill;
                atk_spd = allied_units[i].spd;
                atk_luck = allied_units[i].luck;
                atk_def = allied_units[i].def;

                //Unit defender = enemy_units[j];
                def_hpleft = enemy_units[j].hpleft;
                def_str = enemy_units[j].str;
                def_skill = enemy_units[j].skill;
                def_spd = enemy_units[j].spd;
                def_luck = enemy_units[j].luck;
                def_def = enemy_units[j].def;
            }
            if (turn %2 != 0)
            {
                Unit attacker = enemy_units[j];
                Unit defender = allied_units[i];
            }

            bool atk_isalive = true;
            bool def_isalive = true;

            int atk_number = 1;
            int def_number = 1;

            //If one unit's speed is 5 greater than another, they may attack twice.
            if (atk_spd >= def_spd + 5)
            {
                atk_number *= 2;
            }
            if (def_spd >= atk_spd + 5)
            {
                def_number *= 2;
            }
            //A number between 0 and 99 is generated for determining if a hit lands. It the atk_hit value is higher than
            //the number generated, a hit occurs.

            //Chance of attacker landing a hit
            int atk_hit = (3 * atk_skill / 2 + atk_luck / 2) + 70;
            int def_hit = (3 * def_skill / 2 + def_luck / 2) + 70;

            //Chance of a critical attack
            int atk_crit = (atk_skill / 4 + 20);
            int def_crit = (def_skill / 4 + 20);

            //Evade bonus
            int atk_avoid = (3 * atk_spd / 2 + atk_luck / 2) + 10;
            int def_avoid = (3*atk_spd / 2 + atk_luck / 2) + 10;

            MessageBox.Show(atk_number + " " + def_number);
            MessageBox.Show(allied_units[i].isalive + " " + enemy_units[j].isalive);

            if (atk_number > 0 && def_number > 0 /*&& atk_isalive == true && def_isalive == true*/)
            {
                //Combat Loop//
                //Uses values determined in previous section of method and randomly generated numbers to determine damage dealt to each unit.
                while (atk_number > 0 && def_number > 0 /*&& atk_isalive && def_isalive*/)
                {
                    if (atk_number > 0)
                    {
                        Console.WriteLine("Attacker Turn");
                        if (rnd.Next(0, 99) < atk_hit - def_avoid)
                        {
                            Console.WriteLine("attacker hits");
                            if (rnd.Next(0, 99) < atk_crit - 5)
                            {
                                Console.WriteLine("critical attack");
                                //if the defender's defense stat is high enough, damage will not be dealt
                                if (atk_str + 3 <= def_def)
                                {
                                    Console.WriteLine("no damage");
                                }
                                else
                                {
                                    def_hpleft = def_hpleft - 3 * (atk_str + 3 - def_def);
                                    //it a unit's hpleft drops below 0, they are defeated and removed from the game
                                    if (def_hpleft <= 0)
                                    {
                                        Console.WriteLine("defender defeated");
                                        def_isalive = false;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("defender has " + atk_hpleft + "hp left");
                                    }
                                }
                            }
                            else
                            {
                                if (atk_str + 3 <= def_def)
                                {
                                    Console.WriteLine("no damage");
                                }
                                else
                                {
                                    def_hpleft = def_hpleft - (atk_str + 3 - def_def);
                                    if (def_hpleft <= 0)
                                    {
                                        Console.WriteLine("defender defeated");
                                        def_isalive = false;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("defender has " + def_hpleft + "hp left");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Misses");
                        }
                        atk_number -= 1;
                    }
                    if (def_number > 0)
                    {
                        Console.WriteLine("Defender Turn");
                        if (rnd.Next(0, 99) < def_hit - atk_avoid)
                        {
                            Console.WriteLine("defender hits");
                            if (rnd.Next(0, 99) < def_crit - 5)
                            {
                                Console.WriteLine("critical attack");
                                if (def_str + 3 <= atk_def)
                                {
                                    Console.WriteLine("no damage");
                                }
                                else
                                {
                                    atk_hpleft = atk_hpleft - 3 * (def_str + 3 - atk_def);
                                    if (atk_hpleft <= 0)
                                    {
                                        Console.WriteLine("attacker defeated");
                                        atk_isalive = false;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Attacker has " + atk_hpleft + " hp remaining.");
                                    }
                                }
                            }
                            else
                            {
                                if (def_str + 2 <= atk_def)
                                {
                                    Console.WriteLine("no damage");
                                }
                                else
                                {
                                    atk_hpleft = atk_hpleft - (def_str + 3 - atk_def);
                                    if (atk_hpleft <= 0)
                                    {
                                        Console.WriteLine("attacker defeated");
                                        atk_isalive = false;
                                        break;
                                    }
                                    else
                                    {
                                        Console.WriteLine("Attacker has " + atk_hpleft + " hp remaining.");
                                    }
                                }
                            }
                        }
                        else
                        {
                            Console.WriteLine("Miss");
                        }
                        def_number -= 1;
                    }
                }
            }
            if (turn %2 == 0)
            {
                MessageBox.Show("hp before " + allied_units[i].hpleft.ToString());
                allied_units[i].isactive = false;
                allied_units[i].hpleft = atk_hpleft;
                enemy_units[j].hpleft = def_hpleft;
                if (atk_isalive == false)
                {
                    allied_units[i].isalive = false;
                }
                if (def_isalive == false)
                {
                    enemy_units[j].isalive = false;
                }
                MessageBox.Show("hp after " + allied_units[i].hpleft.ToString());
            }
            if (turn % 2 != 0)
            {
                enemy_units[j].isactive = false;
                allied_units[i].hpleft = def_hpleft;
                enemy_units[j].hpleft = atk_hpleft;
                if (atk_isalive == false)
                {
                    enemy_units[j].isalive = false;
                }
                if (def_isalive == false)
                {
                    allied_units[i].isalive = false;
                }
            }
            MessageBox.Show("Fight Over");
        }
    }
}
