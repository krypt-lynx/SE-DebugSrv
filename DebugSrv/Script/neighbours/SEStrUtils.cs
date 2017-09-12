using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Script
{

    #region ingame script start

    static class StrUtils
    {
        static string cRef =
                "\n\n\n\n\n\n'|¦ˉ‘’‚\nј\n !I`ijl ¡¨¯´¸ÌÍÎÏìíîïĨĩĪīĮįİıĵĺļľłˆˇ˘˙˚˛˜˝ІЇії‹›∙\n" +
                "(),.1:;[]ft{}·ţťŧț\n\"-rª­ºŀŕŗř\n*²³¹\n\\°“”„\nґ\n/ĳтэє\nL_vx«»ĹĻĽĿŁГгзлхчҐ–" +
                "•\n7?Jcz¢¿çćĉċčĴźżžЃЈЧавийнопсъьѓѕќ\n3FKTabdeghknopqsuy£µÝàáâãäåèéêëðñòóôõö" +
                "øùúûüýþÿāăąďđēĕėęěĝğġģĥħĶķńņňŉōŏőśŝşšŢŤŦũūŭůűųŶŷŸșȚЎЗКЛбдекруцяёђћўџ\n+<=>E" +
                "^~¬±¶ÈÉÊË×÷ĒĔĖĘĚЄЏЕНЭ−\n#0245689CXZ¤¥ÇßĆĈĊČŹŻŽƒЁЌАБВДИЙПРСТУХЬ€\n$&GHPUVY§Ù" +
                "ÚÛÜÞĀĜĞĠĢĤĦŨŪŬŮŰŲОФЦЪЯжы†‡\nABDNOQRSÀÁÂÃÄÅÐÑÒÓÔÕÖØĂĄĎĐŃŅŇŌŎŐŔŖŘŚŜŞŠȘЅЊЖф□\n" +
                "љ\nю\n%ĲЫ\n@©®мшњ\nMМШ\nmw¼ŵЮщ\n¾æœЉ\n½Щ\n™\nWÆŒŴ—…‰\n\n\n\n\n\n\n\n"  +
                "\n\n";

        static string kRef =
                "\nЖв?ж?\nъв>\nьв>\nҐ,?-?.?‚?„?…?–?—?š>œ>à>á>â>ã>ä>å>æ>ç>è>é>ê>ë>ð>ñ?ò>ó>ô>õ" +
                ">ö>ø>ù?ú?û?ü?ž?ā>ă>ą>ć>ĉ>č>ď>đ>ě>ē>ĕ>ę>ĝ>ğ>ń?ň?ō>ŏ>ŕ?ř?ś>ŝ>ş>ș>ũ?ū?ŭ?ů?ų?ŵ?" +
                "ź?Ц>Ш>Ы?Ю?б>в?ж?н>";

        static Dictionary<char, int> cW;
        static Dictionary<char, Dictionary<char, int>> kP;

        static StrUtils()
        {
            cW = new Dictionary<char, int>();
            int w = 0;
            foreach (char c in cRef)
            {
                if (c == '\n')
                    w++;
                else
                    cW[c] = w;
            }


            kP = new Dictionary<char, Dictionary<char, int>>();
            Dictionary<char, int> cur = null;

            for (var e = ((IList<char>)kRef.ToCharArray()).GetEnumerator(); e.MoveNext();)
            {
                char c1 = e.Current;
                e.MoveNext();
                char c2 = e.Current;

                if (c1 == '\n')
                    kP[c2] = cur = new Dictionary<char, int>();
                else
                    cur[c1] = c2 - 64;
            }

        }

        public static int Kern(this char left, char right)
        {
            if (kP.ContainsKey(left))
            {
                int w;
                kP[left].TryGetValue(right, out w);
                return w;
            }
            return 0;
        }

        public static int Width(this char ch)
        {
            int w;
            cW.TryGetValue(ch, out w);
            return w;
        }

        public static int Width(this string str, char lead = '\0')
        {
            char lc = lead;
            int wd = 0, w;
            foreach (var c in str)
            {
                cW.TryGetValue(c, out w);
                wd += w + 1;
                if (kP.ContainsKey(lc))
                {
                    kP[lc].TryGetValue(c, out w);
                    wd += w;
                }
                lc = c;
            }

            return wd;
        }

        public static string[] Warp(int width) // todo: implement
        {
            return new string[] { "line1", "line2", "line3" };
        }
    }
    #endregion // ingame script end
}
