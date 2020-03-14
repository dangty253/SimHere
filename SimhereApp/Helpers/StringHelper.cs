
using System;
using System.Collections.Generic;
using System.Text;

namespace SimhereApp.Portable.Helpers
{
    public class StringHelper
    {
        public static string DecimalToCurrencyText(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0)
                    return "0 đ";
                else
                    //return String.Format("{0:0,0.00 đ}", input.Value);  co 2 số 0 sau dau chấm.
                    return String.Format("{0:0,0 đ}", input.Value);
            }
            return "";
        }

        public static string DateFormat(DateTime? date)
        {
            if (date.HasValue && date.Value > DateTime.MinValue)
            {
                return date.Value.ToString("dd/MM/yyyy");
            }
            return "";
        }

        public static string DecimalToPercentFormat(decimal? input)
        {
            if (input.HasValue)
            {
                if (input.Value == 0) return "0 %";
                return String.Format("{0:0,0.00}", input.Value) + "%";
            }
            return "";
        }

        

        public static string SubStr(string s, int length)
        {
            if (String.IsNullOrEmpty(s))
                return "";
            if (s.Length <= length) return s;
            var words = s.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
            if (words[0].Length > length)
                throw new ArgumentException("Từ đầu tiên dài hơn chuỗi cần cắt");
            var sb = new StringBuilder();
            foreach (var word in words)
            {
                if ((sb + word).Length > length)
                    return string.Format("{0}...", sb.ToString().TrimEnd(' '));
                sb.Append(word + " ");
            }
            return string.Format("{0}...", sb.ToString().TrimEnd(' '));
        }

    }
}
