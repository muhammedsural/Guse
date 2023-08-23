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

namespace Guse
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            //12.5d,2,20.5d,3,50,28.5d
            this.beam = new Beam() { Vd = 12.5d, Av = 2, Hd = 20.5d, Paspayi = 3, H = 50, G_alin = 28.5d, Bw=50,Mu=1 };
            this.mat = new Material() { Fck = 30, Fyk = 420 };

            this.grb_Malzeme.DataContext = this.mat;
            this.grb_Guse.DataContext = this.beam;

            txt_arka.Text = Math.Round(beam.D * 2 / 3,1).ToString() + "cm";
        }

        Beam beam;Material mat;
        Helper helper = new Helper();


        

        private async void btn_Hesapla_Click(object sender, RoutedEventArgs e)
        {
            bool result = await GeomCheckAsync(beam);

            double An  = helper.Calc_An(beam,mat);
            double As  = helper.Calc_As(beam,mat);
            double Asf = helper.Calc_Asf(beam.Vd, mat, beam.Mu);//shear friction reinforcement
            double Ast = helper.Calc_Ast(As, An, Asf, mat, beam); //Tension reinforcement
            double Asv = helper.Calc_Asv(Ast, An, Asf); //shear reinforcement

            txt_Ast.Text = Math.Round(Ast,2).ToString() + "cm^2";
            txt_Asw.Text = Math.Round(Asv).ToString() + "cm^2";
        }

        public async Task<bool> GeomCheckAsync(Beam beam)
        {
            if (beam.Av / beam.D > 1)
            {
                await Task.Delay(10);
                MessageBox.Show("av/d oranı 1 den küçük değil hesap geçersizdir.");
                return false;

            }
            if (!(beam.G_alin > (beam.D / 2)))
            {
                await Task.Delay(10);
                MessageBox.Show("Gusenin alnı ile ilgili TS500 boyut şartını (Alin > d/2) sağlamıyorsunuz hesap geçersizdir..");
                return false;
            }
            return true;

        }
    }
}
