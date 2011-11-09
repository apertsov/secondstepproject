using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DistanceLessons.Models;

namespace DistanceLessons
{
    public static class TestHelper
    {
        public static void FilterTests(ref List<Test> tests, int count) // робимо в списку tests рівно count елементів
        {
            if (count < 1) return;
            int CountToReplace = tests.Count - count;
            if (CountToReplace < 1) return;
            Random rand = new Random();
            while (CountToReplace != 0)
            {
                tests.Remove(tests[rand.Next(tests.Count)]);
                CountToReplace--;
            }
        }
    }
}