using AnagramSolver.BusinessLogic;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace AnagramSolver.Tests
{
    public class HelpersTests
    {
        [Fact]
        public void Swap_Ints_SwapsInts()
        {
            int a = 1, b = 2;
            Helpers.Swap(ref a, ref b);
            Assert.Equal(2, a);
            Assert.Equal(1, b);
        }

        [Fact]
        public void Swap_Strings_SwapsStrings()
        {
            string a = "a", b = "b";
            Helpers.Swap(ref a, ref b);
            Assert.Equal("b", a);
            Assert.Equal("a", b);
        }

        public class Person
        {
            public string Name { get; set; }
        }

        [Fact]
        public void Swap_Classes_SwapsClasses()
        {
            var person1 = new Person { Name = "Alex" };
            var person2 = new Person { Name = "Steve" };

            Helpers.Swap(ref person1, ref person2);

            Assert.Equal("Steve", person1.Name);
            Assert.Equal("Alex", person2.Name);
        }

        [Fact]
        public void Where_FilterWordLengthMoreThanFour_CorrectWords()
        {
            var wordArray = new[] { "vienas", "du", "trys" , "keturi"};
            var result = Helpers.Where(wordArray, w => w.Length > 4);
            Assert.Equal(["vienas", "keturi"], result);
        }

        [Fact]
        public void Where_FilterEvenNumbers_CorrectNumbers()
        {
            var numberArray = new[] { -2, 1, 2, 3, 4, 5 };
            var result = Helpers.Where(numberArray, n => n % 2 == 0);
            Assert.Equal([-2, 2, 4], result);
        }

    }
}
