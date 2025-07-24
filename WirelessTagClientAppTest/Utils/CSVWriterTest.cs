using System;
using System.Collections.Generic;
using Xunit;
using WirelessTagClientApp.Utils;

namespace WirelessTagClientApp.Test.Utils
{
    
    public class CSVWriterTest
    {
        [Fact]
        public void AddColumn_Null_Expression_Should_Throw_ArgumentNullException()
        {
            var target = new CSVWriter<MyRecord>();
            Assert.Throws<ArgumentNullException>(() => target.AddColumn(null));
        }

        [Fact]
        public void WriteCSV_Null_List_Should_Throw_ArgumentNullException()
        {
            var target = new CSVWriter<MyRecord>();
            Assert.Throws<ArgumentNullException>(() => target.WriteCSV(null));
        }

        [Fact]
        public void WriteCSV_Empty_List_Should_Return_Empty_String()
        {
            // arrange
            var list = new List<MyRecord>();
            var target = new CSVWriter<MyRecord>();

            // act
            var result = target.WriteCSV(list);

            // assert
            Assert.Equal(String.Empty, result);
        }

        [Fact]
        public void WriteCSV_Should_Return_String_With_Values_Separated_By_Comma_Except_For_Last_Value()
        {
            // arrange
            var list = new List<MyRecord>();
            list.Add(new MyRecord() { IntValue = 42, DoubleValue = 3.1415, AnotherDoubleValue = 42.1, Text = "hello" });

            var target = new CSVWriter<MyRecord>();
            target.AddColumn(x => x.IntValue.ToString());
            target.AddColumn(x => x.DoubleValue.ToString("f2"));
            target.AddColumn(x => x.AnotherDoubleValue.ToString("f2"));
            target.AddColumn(x => x.Text);

            // act
            var result = target.WriteCSV(list);

            // assert
            Assert.Equal("42, 3.14, 42.10, hello", result);
        }

        [Fact]
        public void WriteCSV_One_Item_In_List_Should_Return_Expected_String()
        {
            // arrange
            var list = new List<MyRecord>();
            list.Add(new MyRecord() { IntValue = 42, DoubleValue = 3.1415, Text = "hello, world!" });

            var target = new CSVWriter<MyRecord>();
            target.AddColumn(x => x.IntValue.ToString());
            target.AddColumn(x => x.DoubleValue.ToString("f2"));

            // act
            var result = target.WriteCSV(list);

            // assert
            Assert.Equal("42, 3.14", result);
        }

        [Fact]
        public void WriteCSV_Two_Items_In_List_Should_Return_Expected_String()
        {
            // arrange
            var list = new List<MyRecord>();
            list.Add(new MyRecord() { IntValue = 42, DoubleValue = Math.PI, Text = "hello john" });
            list.Add(new MyRecord() { IntValue = 43, DoubleValue = Math.E });

            var target = new CSVWriter<MyRecord>();
            target.AddColumn(x => x.IntValue.ToString());
            target.AddColumn(x => x.DoubleValue.ToString("f2"));

            // act
            var result = target.WriteCSV(list);

            // assert
            Assert.Equal("42, 3.14\r\n43, 2.72", result);
        }

        [Fact]
        public void WriteCSV_With_Special_Delimiters_One_Item_In_List_Should_Return_Expected_String()
        {
            // arrange
            var list = new List<MyRecord>();
            list.Add(new MyRecord() { IntValue = 42, DoubleValue = 3.1415, Text = "hello john" });

            var target = new CSVWriter<MyRecord>("!", " ");
            target.AddColumn(x => x.IntValue.ToString());
            target.AddColumn(x => x.DoubleValue.ToString("f2"));

            // act
            var result = target.WriteCSV(list);

            // assert
            Assert.Equal("42!3.14", result);
        }

        [Fact]
        public void WriteCSV_With_Value_Containing_Separator_Should_Return_Expected_String_Containing_Delimited_Separator()
        {
            // arrange
            var list = new List<MyRecord>();
            list.Add(new MyRecord() { IntValue = 42, DoubleValue = 3.1415, Text = "hello, world!" });

            var target = new CSVWriter<MyRecord>();
            target.AddColumn(x => x.IntValue.ToString());
            target.AddColumn(x => x.Text);

            // act
            var result = target.WriteCSV(list);

            // assert
            Assert.Equal("42, hello\\, world!", result);
        }

        [Fact]
        public void WriteCSV_WithHeader_One_Item_In_List_Should_Return_Expected_String()
        {
            // arrange
            var list = new List<MyRecord>();
            list.Add(new MyRecord() { IntValue = 42, DoubleValue = 3.1415, Text = "hello, world!" });

            var target = new CSVWriter<MyRecord>();
            target.AddColumn(x => x.IntValue.ToString(), "IntValue");
            target.AddColumn(x => x.DoubleValue.ToString("f2"), "DoubleValue");

            // act
            var result = target.WriteCSV(list);

            // assert
            Assert.Equal("#IntValue, DoubleValue\r\n42, 3.14", result);
        }

        [Fact]
        public void WriteCSV_Should_Return_String_With_Lines_Separated_By_Newline_Except_For_Last_Line()
        {
            // arrange
            var list = new List<MyRecord>();
            list.Add(new MyRecord() { IntValue = 42, DoubleValue = 3.1415, Text = "hello, world!" });
            list.Add(new MyRecord() { IntValue = 66, DoubleValue = 1.618, Text = "greetings" });

            var target = new CSVWriter<MyRecord>();
            target.AddColumn(x => x.IntValue.ToString(), "IntValue");
            target.AddColumn(x => x.DoubleValue.ToString("f2"), "DoubleValue");

            // act
            var result = target.WriteCSV(list);

            // assert
            Assert.Equal("#IntValue, DoubleValue\r\n42, 3.14\r\n66, 1.62", result);
        }
    }

    public class MyRecord
    {
        public int IntValue { get; set; }
        public int AnotherValue { get; set; }
        public double DoubleValue { get; set; }
        public double AnotherDoubleValue { get; set; }
        public string Text { get; set; }
    }
}
