using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace WirelessTagClientApp.Utils
{
    public class CSVWriter<T>
    {
        private string headerDelimiter = "#";
        private string separator = ", ";
        private string newline = Environment.NewLine;

        private List<Func<T, string>> _expressionList;
        private string _columnHeader;

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVWriter"/> class.
        /// </summary>
        public CSVWriter()
        {
            _expressionList = new List<Func<T, string>>();
            _columnHeader = null;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CSVWriter"/> class.
        /// </summary>
        /// <param name="separator">Separator between values; usually comma.</param>
        /// <param name="newline">Separator between records; usually newline.</param>
        public CSVWriter(string separator, string newline) : this()
        {
            this.separator = separator;
            this.newline = newline;
        }

        /// <summary>
        /// Register an expression (an action or lambda taking an instance of the templated type, and returning a string)
        /// </summary>
        /// <remarks>
        /// Expressions are evaluated in the order added; defines the column order.
        /// </remarks>
        /// <param name="expression"></param>
        public void AddColumn(Func<T, string> expression)
        {
            AddColumn(expression, null);
        }

        /// <summary>
        /// Register an expression (an action or lambda taking an instance of the templated type, and returning a string)
        /// </summary>
        /// <remarks>
        /// Expressions are evaluated in the order added; defines the column order.
        /// </remarks>
        /// <param name="expression"></param>
        /// <param name="columnName">Optional column name for header.</param>
        public void AddColumn(Func<T, string> expression, string columnName)
        {
            ThrowIf.Argument.IsNull(expression, nameof(expression));
            _expressionList.Add(expression);

            // append optional column name for header
            if (columnName != null)
            {
                if (_columnHeader == null)
                {
                    _columnHeader = headerDelimiter + columnName;
                }
                else
                {
                    // append separator and next column name
                    _columnHeader += separator;
                    _columnHeader += columnName;
                }
            }
        }

        public string WriteCSV(IList<T> list)
        {
            ThrowIf.Argument.IsNull(list, nameof(list));

            var builder = new StringBuilder();

            // add optional header
            if (!String.IsNullOrEmpty(_columnHeader))
            {
                builder.Append(_columnHeader);
                builder.Append(newline);
            }

            foreach(var item in list)
            {
                foreach(var expression in _expressionList)
                {
                    var value = expression(item); 

                    if (value == null)
                    {
                        value = "(null)";
                    }

                    // check if value already contains a comma separator
                    if (value.Contains(separator))
                    {
                        value = value.Replace(separator, @"\" + separator);
                    }

                    builder.Append(value);

                    // append separator for all values except the last
                    if (expression != _expressionList.Last())
                    {
                        builder.Append(separator);
                    }
                }
                builder.Append(newline);
            }

            return builder.ToString();
        }
    }
}