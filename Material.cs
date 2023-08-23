using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Guse;

public class Material : PropChanger
{
	private double fck;

	public double Fck
	{
		get { return fck; }
		set { fck = value; OnPropertyChanged(nameof(fck)); }
	}

	private double fyk;

	public double Fyk
	{
		get { return fyk; }
		set { fyk = value; OnPropertyChanged(nameof(fyk)); }
	}

    }
