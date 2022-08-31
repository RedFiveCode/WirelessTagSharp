using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
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
            Assert.IsTrue(propertyTable.ContainsKey(propertyName),
                          String.Format("Property '{0}' has not raised a PropertyChanged event", propertyName));
        }

        public void AssertExpectedNumberOfPropertyChangedEventFired(int expectedEventCount)
        {
            int eventCount = propertyTable.Values.Sum();

            Assert.AreEqual(expectedEventCount, eventCount);
        }
    }
}
