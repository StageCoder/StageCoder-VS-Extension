using OptionsSample.Options;
using System;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace StageCoder.Settings
{
    internal partial class OptionsProvider
    {
        // Register the options with this attribute on your package class:
        // [ProvideOptionPage(typeof(OptionsProvider.GeneralOptions), "StageCoder.Settings", "General", 0, 0, true, SupportsProfiles = true)]
        [ComVisible(true)]
        public class GeneralOptions : BaseOptionPage<General> { }
    }

    public class General : BaseOptionModel<General>
    {
        private bool _enableHighlighter = false;
        [Category("General")]
        [DisplayName("Enable highlighter")]
        [Description("Enable highlighting the code by blurring the non selected code")]
        [DefaultValue(false)]
        public bool EnableHighlighter
        {
            get { return _enableHighlighter; }
            set { SetProperty(ref _enableHighlighter, value); }
        }

        private void SetProperty(ref bool property, bool value)
        {
            if(property != value)
            {
                property = value;
                EnableHighlighterChanged?.Invoke(this, new PropertyChangedEventArgs(nameof(EnableHighlighter)));
            }
        }

        public event PropertyChangedEventHandler EnableHighlighterChanged;
    }
}
