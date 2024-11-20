/////////////////////////////////////////////////////////////////////////////
//
//  Script   : BigNumber.cs
//  Info     : BigNumber 大数显示辅助类
//  Author   : ls9512
//  E-mail   : ls9512@vip.qq.com
//
//  Copyright : Aya Game Studio 2018
//
/////////////////////////////////////////////////////////////////////////////
using System;
using System.Globalization;
using System.Text;
using UnityEngine;

namespace Aya.Maths
{
    public static class BigNumber
    {
        // Kilo
        // Meg
        // Giga
        // Tera
        // Peta
        // Era
        // Zetta
        // Yotta

        public static string[] Unit = { "K", "M", "B", "T", "P", "E", "Z", "Y", "A", "D", "F" };

        private static readonly StringBuilder Builder = new StringBuilder();

        public static string Format(string number)
        {
            if (number.Length <= 3) return number;

            try
            {
                var value = decimal.Parse(number);
                value = decimal.Round(value);
                if (value < 1000) return value.ToString(CultureInfo.InvariantCulture);
                Builder.Remove(0, Builder.Length);

                var unitIndex = 0;
                decimal current = 1;
                while (unitIndex < Unit.Length)
                {
                    current *= 1000;
                    var next = current * 1000;
                    if (value >= current && value < next)
                    {
                        value /= current;
                        break;
                    }

                    unitIndex++;
                }

                var unit = Unit[unitIndex];
                var str = value.ToString(CultureInfo.InvariantCulture);
                if (str.Contains(".") && str.Length >= 4) str = str.Substring(0, 4);
                else if (str.Length > 3) str = str.Substring(0, 3);
                while (str.Contains(".") && str.EndsWith("0"))
                {
                    str = str.Substring(0, str.Length - 1);
                }

                if (str.EndsWith(".")) str = str.Substring(0, str.Length - 1);

                str += unit;
                return str;
            }
            catch (Exception e)
            {
                Debug.LogWarning("BigNumber Error" + number + "\n" + e);
                return number;
            }
        }
    }
}
