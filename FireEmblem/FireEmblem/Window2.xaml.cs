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
using System.Windows.Shapes;

namespace FireEmblem
{
    /// <summary>
    /// Interaction logic for Window2.xaml
    /// </summary>
    public partial class Window2 : Window
    {

        public Window2()
        {
            InitializeComponent();
            
            //MainWindow window = new MainWindow();
        }
        public void stats(int team, int classnumber, int hpleft, int hptotal, int str, int skill, int spd, int luck, int def)
        {
            if (team == 0)
            {
                canvas2.Background = Brushes.CornflowerBlue;
            }
            else if (team == 1)
            {
                canvas2.Background = Brushes.IndianRed;
            }
            if (classnumber == 0)
            {
                lbl_name.Content = "Mercenary";
            }
            else if (classnumber == 1)
            {
                lbl_name.Content = "Fighter";
            }
            else if (classnumber == 2)
            {
                lbl_name.Content = "Soldier";
            }
            
            pbar_hp.Maximum = hptotal;
            pbar_hp.Value = hpleft;
            rect_background.Visibility = Visibility.Visible;

            lbl_hptotal.Content = hptotal;
            lbl_hpleft.Content = hpleft;
            lbl_str.Content = str;
            lbl_skill.Content = skill;
            lbl_spd.Content = spd;
            lbl_luck.Content = luck;
            lbl_def.Content = def;
        }
    }
}
 