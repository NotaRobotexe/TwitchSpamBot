using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TwitchSpamBot
{
    class StringOperations
    {
        public static bool ContainWord(string[] array, string word)
        {
            foreach (var item in array)
            {
                if (item == word)
                {
                    return true;
                }
            }
            return false;
        }

        public static int IsNumericAndReturnMinutes(string[] array)
        {
            foreach (var item in array)
            {
                var isNumeric = int.TryParse(item, out int minutes);

                if (isNumeric == true)
                {
                    return minutes;
                }

            }
            return 0;
        }

    }
}
