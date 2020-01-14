using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Demo.Util
{
    
    public class StringUtil
    {
        public static (string beforeStringMatched, string stringMatched, string afterStringMatched, int startIndex, int lastIndex) getBetween(string strSource, string strStart, string strEnd)
        {
            int Start, End;
            if (strSource.Contains(strStart) && strSource.Contains(strEnd))
            {
                Start = strSource.IndexOf(strStart, 0) + strStart.Length;
                End = strSource.IndexOf(strEnd, Start);
                return ( strSource.Substring(0, Start), strSource.Substring( Start, End - Start), strSource.Substring(End, strSource.Length - (Start +(End - Start))), Start, End);
            }
            else
            {
                return ("","","",0,0);
            }
        }
    }
}
