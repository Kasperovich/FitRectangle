using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FitRectangle.Enums
{
    /// <summary> Типы прямоугольников </summary>
    public enum ERectangleType
    {
        None = 0,
        /// <summary> Главный прямоугольник </summary>
        Main= 1,
        /// <summary> Второстепенный прямоугольник </summary>
        Secondary = 2,
    }
}
