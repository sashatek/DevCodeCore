using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DevGen.Coder
{
    internal class CodeWriter
    {

        public int identSize { get; set; }
        private bool isFirstSmbolB = true;
        private StringBuilder buffer = new StringBuilder(1024);
        private int nestLevel = 0;

        public CodeWriter()
        {
            identSize = 4;
        }

        private void writeIdent()
        {
            buffer.Append(' ', nestLevel*identSize);
        }

        public void newLine()
        {
            buffer.AppendLine();
        }

        /**
     * Increase nest level by 1
     * 
     */

        public void nest()
        {
            nestLevel++;
        }
        public void nest(int n)
        {
            nestLevel = n;
        }

        public void unNest()
        {
            if (nestLevel > 0)
            {
                nestLevel--;
            }
        }

        public void openCurly()
        {
            writeLine("{");
            nest();
        }

        public void closeCurly()
        {
            unNest();
            writeLine("}");
        }

        public void writeLine(String s)
        {
            if (isFirstSmbolB)
            {
                writeIdent();
            }
            buffer.Append(s);
            buffer.AppendLine();
            isFirstSmbolB = true;
        }
        public void writeMultiLine(String s)
        {
            string[] lines = s.Split(new string[] { "\r\n", "\n" }, StringSplitOptions.None);
            foreach(var line in lines)
            {
                writeLine(line);
            }
  
        }

        public void write(String s)
        {
            if (isFirstSmbolB)
            {
                writeIdent();
                isFirstSmbolB = false;
            }
            buffer.Append(s);
        }
        public void writeAsIs(String s)
        {
            buffer.Append(s);
        }

        public string toString()
        {
            return buffer.ToString();
        }

    }
}