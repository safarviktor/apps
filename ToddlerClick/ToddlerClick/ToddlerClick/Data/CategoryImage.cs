using System.Windows.Input;
using Xamarin.Forms;

namespace ToddlerClick.Data
{
    public class CategoryImage
    {
        public ImageSource ImageSource { get; set; }

        public ICommand Command { get; set; }

        public string SubCategory { get; set; }
    }
}
