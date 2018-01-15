using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace JPTCG.Spectrometer
{
    public enum SpectType
    {
        [DescriptionAttribute("No Spectrometer")]
        NoSpectrometer = -1,
        [DescriptionAttribute("Avantes Spectrometer")]
        Avantes = 0,
        [DescriptionAttribute("Ocean Optics Maya")]
        Maya = 1,
        [DescriptionAttribute("IS CAS140")]
        CAS140 = 2,
    }
}
