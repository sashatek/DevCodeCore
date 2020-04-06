using DevCodeCore.Coders;
using DevCodeCore.DAL;
using DevCodeCore.Model;
using DevCodeCore.Pages;
using DevCodeCore.Shared;
using System;

namespace DevCodeCli
{
    class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Hello World!");

            var testPage = new TestSet();
            // testPage.modelCs();
            testPage.test();
        }
    }
}
