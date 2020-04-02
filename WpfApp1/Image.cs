using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace BoundingChecker
{
    public class BoundingImage
    {
        public string Path { get; set; }
        public XmlDocument XmlIn { get; set; }
        public XmlDocument XmlOut { get; set; }

        public Result GetResult()
        {
            if (XmlIn == null || XmlOut == null)
            {
                return new Result();
            }

            return new Result();
        }
    }

    public class Result
    {
        private int Total { get; set; }
        private int Correct { get; set; }
        private int Miss { get; set; }
        private int Wrong { get; set; }

        public Result(int t = 0, int c = 0, int m = 0, int w = 0)
        {
            Total = t;
            Correct = c;
            Miss = m;
            Wrong = w;
        }
    }
}