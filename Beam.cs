using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace Guse;

public class Beam : PropChanger, IDataErrorInfo
{
    private readonly BeamValidator _beamValidator;
    

    public Beam()
    {
        _beamValidator = new BeamValidator();
    }

    public string this[string columnName]
    {
        get
        {
            var firstOrDefault = _beamValidator.Validate(this).Errors.FirstOrDefault(lol => lol.PropertyName == columnName);
            if (firstOrDefault != null)
                return _beamValidator != null ? firstOrDefault.ErrorMessage : $"{columnName} is not null or 0";
            return "";
        }
    }

    public string Error
    {
        get
        {
            if (_beamValidator != null)
            {
                var results = _beamValidator.Validate(this);
                if (results != null && results.Errors.Any())
                {
                    var errors = string.Join(Environment.NewLine, results.Errors.Select(x => x.ErrorMessage).ToArray());
                    return errors;
                }
            }
            return string.Empty;
        }
    }


    private double vd;
    public double Vd
    {
        get { return vd; }
        set { vd = value;
            OnPropertyChanged(nameof(vd));
        }
    }
    
    private double hd;
    public double Hd
    {
        get { return hd; }
        set { hd = value;
            OnPropertyChanged(nameof(hd));
        }
    }
    
    private double av;
    public double Av
    {
        get { return av; }
        set { av = value; OnPropertyChanged(nameof(av)); }
    }
    
    private double paspayi;
    public double Paspayi
    {
        get { return paspayi; }
        set { paspayi = value; 
            OnPropertyChanged(nameof(Paspayi));
            OnPropertyChanged(nameof(D)); }
    }

    private double h;
    public double H
    {
        get { return h; }
        set
        {
            if (value == 0 || value == null)
            {
                throw new Exception(nameof(value) +  "is not null or zero");
            }
            else
            {
                h = value;
                OnPropertyChanged(nameof(H));
                OnPropertyChanged(nameof(D));
            }
        }
    }

    private double d;
    public double D
    {
        get { return H-paspayi; }
       
    }

    private double g_alin;
    public double G_alin
    {
        get { return g_alin; }
        set { g_alin = value; OnPropertyChanged(nameof(G_alin)); }
    }

    private double mu;
    public double Mu
    {
        get { return mu; }
        set { mu = value; OnPropertyChanged(nameof(mu)); }
    }

    private double bw;
    public double Bw
    {
        get { return bw; }
        set { bw = value; OnPropertyChanged(nameof(bw)); }
    }

    
}
