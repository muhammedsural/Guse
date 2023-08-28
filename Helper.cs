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
    public async Task<double> Check_Hd(double Hd, double Vd)
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
        await Task.Delay(10);
        return Hd;
    }

    public async Task<double> Check_Vd(double H,double Av, double Vd,double D,double Bw, double Fck)
    {
        double Vsf = 0.2d * H * Av * Fck/100/ 1.5;

        if (Vd > Vsf)
        {
            MessageBox.Show($"TS500 sürtünme kesmesi sınırı aşılmıştır (Vd = {Vd} > Vsf = {Vsf}) kesit büyütülmeli...");
            Vd = Vsf;
        }

        if (Fck > 25)
        {
            MessageBox.Show("Beton basınç dayanımı 25 MPa dan büyüktür. Sürtünme kesmesi kontrolünde 25 MPa dan büyük basınç dayanımı alınmaz.Bu nedenle bu hesapta 25 MPa değeri kullanılmıştır.");
            
            
            
            Fck = 25;
        }
        double Vd_sinir = 0.22d * (Fck / 1.5d/100) * Bw * D;
        if (Vd > Vd_sinir)
        {
            MessageBox.Show($"TS500 denklem 8.26 kesme sınır değeri aşılmıştır (Vd = {Vd} > Vd_sınır = {Vd_sinir}) kesit büyütülmelidir!!! Hesap sınır değer alınarak devam edilmiştir. Hesap geçersizdir.");
            Vd = Vd_sinir;
        }
        await Task.Delay(10);
        return Vd;
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
        double Ast3 = 0.05 * beam.D * (material.Fck/ 1.5d) / (material.Fyk / 1.15d);
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
