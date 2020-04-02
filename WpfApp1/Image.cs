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

        public Result GetResult(int offset = 0)
        {
            if (XmlIn == null || XmlOut == null)
            {
                return new Result();
            }
            var r = new Result();
            XmlNodeList inBounds = InBound();
            XmlNodeList outBounds = OutBound();
            r.Total = inBounds.Count;
            List<Bound> oBounds = new List<Bound>();
            foreach (XmlNode bound in outBounds)
            {
                oBounds.Add(new Bound(bound));
            }
            foreach (XmlNode inBound in inBounds)
            {
                var bound = new Bound(inBound);
                bool flag = false;
                foreach (var ob in oBounds)
                {
                    if (bound.EqualTo(ob, offset))
                    {
                        flag = true;
                        oBounds.Remove(ob);
                        break;
                    }
                }
                if (flag)
                {
                    r.Correct++;
                }
                else
                {
                    r.Miss++;
                }
            }
            r.Wrong = oBounds.Count;

            return r;
        }

        public XmlNodeList InBound()
        {
            XmlNode root = XmlIn.DocumentElement;
            return root.SelectNodes("//bndbox");
        }

        public XmlNodeList OutBound()
        {
            XmlNode root = XmlOut.DocumentElement;
            return root.SelectNodes("//bndbox");
        }
    }

    public class Bound
    {
        public int x1, y1, x2, y2;

        public int Width
        {
            get
            {
                return Math.Abs(x2 - x1);
            }
        }

        public int Height
        {
            get
            {
                return Math.Abs(y2 - y1);
            }
        }

        public Bound(XmlNode n)
        {
            var bE = (XmlElement)n;
            x1 = int.Parse(bE["xmin"].InnerText);
            x2 = int.Parse(bE["xmax"].InnerText);
            y1 = int.Parse(bE["ymin"].InnerText);
            y2 = int.Parse(bE["ymax"].InnerText);
        }

        public bool EqualTo(Bound b, int offset = 0)
        {
            if (Math.Abs(x1 - b.x1) > (Width * offset / 100))
                return false;
            if (Math.Abs(x2 - b.x2) > (Width * offset / 100))
                return false;
            if (Math.Abs(y1 - b.y1) > (Height * offset / 100))
                return false;
            if (Math.Abs(y2 - b.y2) > (Height * offset / 100))
                return false;

            return true;
        }
    }

    public class Result
    {
        public int Total { get; set; }
        public int Correct { get; set; }
        public int Miss { get; set; }
        public int Wrong { get; set; }

        public Result(int t = 0, int c = 0, int m = 0, int w = 0)
        {
            Total = t;
            Correct = c;
            Miss = m;
            Wrong = w;
        }
    }
}