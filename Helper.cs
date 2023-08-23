using System;
using System.Printing;
using System.Threading.Tasks;
using System.Windows;

namespace Guse;

public class Helper
{
    #region Geometrik kontroller
    public void CheckD(double Av, double d)
    {
        if (Av / d > 1) 
        {
            MessageBox.Show("av/d oranı 1 den küçük değil hesap geçersizdir.");
        }
    }

    public bool Check_Alin(double Alin, double D)
    {
        bool result = true;
        if (!(Alin < (D / 2)))
        {
            MessageBox.Show("Gusenin alnı ile ilgili TS500 boyut şartını (Alin > d/2) sağlamıyorsunuz hesap geçersizdir..");
            result = false;
        }
        return result;
    }

    
    #endregion


    #region Kuvvet kontrolleri
    public double Check_Hd(double Hd, double Vd)
    {
        if (Hd < 0.2 * Vd)
        {
            Hd = 0.2 * Vd;
            MessageBox.Show("Hd değeri 0.2*Vd değerinden küçük olamayacak şekilde ayarlanmıştır TS500 şartı..");
        }
        if (Hd > Vd)
        {
            MessageBox.Show("Hd, Vd değerinden büyük olamaz. Bkn. ==> ACI318/16.5.1.1");
        }
        return Hd;
    }

    public double Chechk_Vd(Beam beam, Material material)
    {
        double Vd_sinir = 0.22d * material.Fck / 1.5d * beam.Bw * beam.D;
        if (beam.Vd > Vd_sinir)
        {
            MessageBox.Show("TS500 denklem 8.26 kesme sınır değeri aşılmıştır kesit büyütülmelidir!!! Hesap sınır değer alınarak devam edilmiştir. Hesap geçersizdir.");
            beam.Vd = Vd_sinir;
        }

        double Vsf = 0.2d * beam.H * beam.Av * material.Fck / 1.5;

        if (beam.Vd > Vsf)
        {
            MessageBox.Show("TS500 sürtünme kesmesi sınırı aşılmıştır kesit büyütülmeli...");
            beam.Vd = Vsf;
        }

        return beam.Vd;
    } 
    #endregion


    #region Donatı Hesapları
    public double Calc_As(Beam beam, Material material)
    {
        var a = beam.Vd * beam.Av;
        var b = beam.Hd * (beam.H - beam.D);
        var c = 0.8d * (material.Fyk/100 / 1.15d) * beam.D;
        var As = (a + b) / c;
        return As;
    }

    public double Calc_An(Beam beam, Material material)
    {
        return beam.Hd / (material.Fyk/100 / 1.15d);
    }

    public double Calc_Asf(double Vd, Material material, double mu = 1.0)
    {
        double Asf = Vd / mu / (material.Fyk/100 / 1.15d);
        return Asf;
    }

    public double Calc_Ast(double As, double An, double Asf, Material material, Beam beam)
    {
        double Ast1 = As + An;
        double Ast2 = (2 * Asf / 3) + An;
        double Ast3 = 0.05 * (material.Fck / material.Fyk) * (1.5d / 1.15d) * beam.D;
        double Ast = Math.Max(Ast1, Math.Max(Ast2, Ast3));

        return Ast;
    }

    public double Calc_Asv(double Ast, double An, double Asf)
    {
        double Asv1 = 0.5d * (Ast - An); //Ts500
        double Asv2 = Asf / 3;//ACI318
        return Math.Max(Asv1, Asv2);
    } 
    #endregion

  

}
