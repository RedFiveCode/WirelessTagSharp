using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using WirelessTagClientApp.ViewModels;
using WirelessTagClientLib.DTO;

namespace WirelessTagClientApp.Common
{
    /// <summary>
    /// Creates view-model objects from DTO objects
    /// </summary>
    public class ViewModelFactory
    {
        public static TagViewModel CreateTagViewModel(TagInfo tag)
        {
            var tagVM = new TagViewModel()
            {
                Id = tag.SlaveId,
                Name = tag.Name,
                Description = tag.Comment,
                Uuid = tag.Uuid,
                LastCommunication = tag.LastCommunication,
                Temperature = tag.Temperature,
                SignalStrength = tag.SignalStrength,
                BatteryVoltage = tag.BatteryVoltage,
                BatteryRemaining = tag.BatteryRemaining,
                RelativeHumidity = tag.RelativeHumidity
            };

            return tagVM;
        }

        public static ObservableCollection<TagViewModel> CreateTagViewModelList(IEnumerable<TagInfo> tags)
        {
            var collection = new ObservableCollection<TagViewModel>();

            if (tags != null && tags.Any())
            {
                foreach (var tag in tags)
                {
                    collection.Add(CreateTagViewModel(tag));
                }
            }

            return collection;
        }
    }
}
