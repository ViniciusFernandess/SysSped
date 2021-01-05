using System;
using System.Collections.Generic;

namespace SysSped.Domain.Core
{
    public static class MetodoExtensao
    {
        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static DateTime ToDateTime(this string source)
        {
            var dd = source.Substring(0, 2);
            var mm = source.Substring(2, 2);
            var yy = source.Substring(4, 4);

            return DateTime.Parse($@"{dd}/{mm}/{yy}");
        }

        public static string ToAliquotaDecimalDomain(this string source)
        {
            var valor = source;

            var temVirgulaEPonto = source.Contains(",") && source.Contains(".");
            if (temVirgulaEPonto)
            {
                var virgulaVemAntesDoPonto = source.IndexOf(",") < source.IndexOf(".");

                if (virgulaVemAntesDoPonto)
                    source = source.Replace(",", "");
                else
                    source = source.Replace(".", "");
            }


            if (decimal.TryParse(source.Replace(".", ","), out var valorFloat))
            {
                valor = valorFloat.ToString("0.000");
                valor = valor.Remove(valor.Length - 1);
            }

            if (valor == "0,00")
                valor = "0";

            return valor;
        }

        public static double ToAliquotaDoubleDomain(this double source)
        {
            var valor = source;
            double.TryParse(source.ToString().ToAliquotaDecimalDomain(), out valor);
            return valor;
        }
    }
}
