using System;
using System.Diagnostics;
using System.IO;
using System.Reflection;
using System.Windows.Input;
using WirelessTagClientApp.Commands;
using WirelessTagClientApp.Common;

namespace WirelessTagClientApp.ViewModels
{
    public class AboutViewModel : ViewModelBase
    {
        private NavigateHyperlinkCommand navigateCommand;

        public AboutViewModel()
        {
            AppName = String.Empty;
            Version = String.Empty;
            Copyright = String.Empty;
            BuildDate = DateTime.MinValue;
            CompanyURL = String.Empty;
            Credits = String.Empty;

            navigateCommand = new NavigateHyperlinkCommand();
        }

        public void Initialise()
        {
            Assembly assembly = Assembly.GetExecutingAssembly();

            FileInfo info = new FileInfo(assembly.Location);

            FileVersionInfo fvi = FileVersionInfo.GetVersionInfo(assembly.Location);

            AppName = fvi.ProductName;
            Version = assembly.GetName().Version.ToString();

            Copyright = fvi.LegalCopyright;

            BuildDate = info.LastWriteTime;

            CompanyURL = fvi.CompanyName;

            // Credits is an embedded markdown file
            Credits = ReadResourceFile("WirelessTagClientApp.Resources.Credits.md");
        }

        // properties don't need to fire INotifyPropertyChanged as they do not change after the initial binding
        public string AppName { get; private set; }

        public string Version { get; private set; }

        public string Copyright { get; private set; }

        public DateTime BuildDate { get; private set; }

        public string CompanyURL { get; private set; }

        public string Credits { get; set; }

        public ICommand NavigateCommand
        {
            get { return navigateCommand.Command; }
        }

        private string ReadResourceFile(string resourceName)
        {
            var result = String.Empty;

            try
            {
                var assembly = Assembly.GetExecutingAssembly();

                using (Stream stream = assembly.GetManifestResourceStream(resourceName))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        result = reader.ReadToEnd();
                    }
                }
            }
            catch { }

            return result;
        }
    }
}
