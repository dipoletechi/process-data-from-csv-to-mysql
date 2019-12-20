using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            
        }

        public static void GetLinkedAppleTreeCount(List<AppleTree> appleTree, ref int appleTreeCount)
        {
            foreach (var item in appleTree)
            {
                appleTreeCount += item.linkedTrees.Count();
                if (item.linkedTrees.Count() > 0)
                {
                    GetLinkedAppleTreeCount(item.linkedTrees, ref appleTreeCount);
                }

            }
        }

        public void GetApplesAndLinkedTreesCount(AppleTree appleTree)
        {
            var totalApples = appleTree.apples;
            int appleTreeCount = 0;
            GetLinkedAppleTreeCount(appleTree.linkedTrees, ref appleTreeCount);
            Console.WriteLine("Total apple count:"+ totalApples);
            Console.WriteLine("Total apple tree count:" + appleTreeCount);
        }
    }

   


 

    class AppleTree
    {
      public  int apples { get; set; }
       public List<AppleTree> linkedTrees { get; set; }
    }
}
