using Xunit;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace WirelessTagClientAppTest.TestHelpers
{
    /// <summary>
    /// Class to verify that a <see cref="PropertyChanged"/> event has been fired.
    /// </summary>
    public class PropertyChangedObserver
    {
        private Dictionary<string, int> propertyTable = new Dictionary<string, int>();

        public PropertyChangedObserver(INotifyPropertyChanged target)
        {
            target.PropertyChanged += OnPropertyChanged;
        }

        private void OnPropertyChanged(object sender, PropertyChangedEventArgs e)
        {
            if (propertyTable.ContainsKey(e.PropertyName))
            {
                propertyTable[e.PropertyName]++;
            }
            else
            {
                propertyTable[e.PropertyName] = 1;
            }
        }

        public void AssertPropertyChangedEvent(string propertyName)
        {
            Assert.True(propertyTable.ContainsKey(propertyName),
                          $"Property '{propertyName}' has not raised a PropertyChanged event");
        }

        public void AssertPropertyChangedEvent(string propertyName, int expectedCount)
        {
            Assert.True((propertyTable.ContainsKey(propertyName) && propertyTable[propertyName] == expectedCount),
                          $"Property '{propertyName}' has not raised a PropertyChanged event the expected number of times (expected {expectedCount}, actual {propertyTable[propertyName]})");
        }

        public void AssertExpectedNumberOfPropertyChangedEventFired(int expectedEventCount)
        {
            int eventCount = propertyTable.Values.Sum();

            Assert.Equal(expectedEventCount, eventCount);
        }
    }
}
