using Prism.Commands;
using Prism.Navigation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using ToddlerClick.Data;
using Xamarin.Forms;

namespace ToddlerClick.ViewModels
{
    public class MainPageViewModel : ViewModelBase
    {
        private List<CategoryImage> categoryImages;
        private bool categoryGridIsVisible;
        private bool isSubcategoryItemsControlVisible;
        private bool isShownImageVisible;
        private ImageSource shownImageSource;
        private List<string> _availableImages;

        public MainPageViewModel(INavigationService navigationService)
            : base(navigationService)
        {
            Title = "Main Page";

            // NOTE: use for debugging, not in released app code!
            //var assembly = typeof(MainPageViewModel).Assembly;
            //foreach (var res in assembly.GetManifestResourceNames())
            //{
            //    System.Diagnostics.Debug.WriteLine("found resource: " + res);
            //}

            MenuCommand = new Command<CategoryTypes>(ExecuteMenuCommand);
            ShownImageCommand = new Command(ExecuteShownImageCommand);

            var assembly = typeof(MainPageViewModel).Assembly;
            _availableImages = assembly.GetManifestResourceNames().Where(i => i.StartsWith("ToddlerClick.pics")).ToList() ;
        }

        private void ExecuteMenuCommand(CategoryTypes selectedCategory)
        {
            IsShownImageVisible = false;
            IsSubcategoryItemsControlVisible = false;

            CategoryImages = new List<CategoryImage>();

            var categoryKey = $"ToddlerClick.pics.{selectedCategory.ToString().ToLower()}";

            var newImages = new List<CategoryImage>();
            var categoryImages = _availableImages.Where(x => x.StartsWith(categoryKey));
            var subCategories = categoryImages.Select(x => x.Substring(0, x.IndexOf(".", categoryKey.Length + 2))).Distinct();

            foreach (var subCategory in subCategories)
            {
                var subCategoryRepre = categoryImages.First(x => x.StartsWith(subCategory));

                newImages.Add(
                    new CategoryImage()
                    {
                        ImageSource = ImageSource.FromResource(subCategoryRepre),
                        Command = new Command<string>(ShowImageFromSubcategory),
                        SubCategory = subCategory
                    }); ;
            }

            CategoryImages = newImages;
            IsSubcategoryItemsControlVisible = true;
        }

        private void ExecuteShownImageCommand()
        {
            IsShownImageVisible = false;
            IsSubcategoryItemsControlVisible = true;
        }

        private void ShowImageFromSubcategory(string subCategory)
        {
            var subCategoryImages = _availableImages.Where(x => x.StartsWith(subCategory)).ToList();            
            var newIndex = new Random().Next(subCategoryImages.Count);
            var selectedImage = subCategoryImages.Skip(newIndex).First();
            ShownImageSource = ImageSource.FromResource(selectedImage);
            IsSubcategoryItemsControlVisible = false;
            IsShownImageVisible = true;
        }

        public ImageSource ShownImageSource 
        { 
            get => shownImageSource; 
            set
            {
                shownImageSource = value;
                RaisePropertyChanged(nameof(ShownImageSource));
            }
        }

        public bool IsShownImageVisible
        {
            get => isShownImageVisible;
            set
            {
                isShownImageVisible = value;
                RaisePropertyChanged(nameof(IsShownImageVisible));
            }
        }

        public bool IsSubcategoryItemsControlVisible
        {
            get => isSubcategoryItemsControlVisible;
            set
            {
                isSubcategoryItemsControlVisible = value;
                RaisePropertyChanged(nameof(IsSubcategoryItemsControlVisible));
            }
        }

        public ICommand MenuCommand { get; set; }
        public ICommand ShownImageCommand { get; set; }

        public bool CategoryGridIsVisible
        {
            get => categoryGridIsVisible;
            set
            {
                categoryGridIsVisible = value;
                RaisePropertyChanged(nameof(CategoryGridIsVisible));
            }
        }

        public List<CategoryImage> CategoryImages
        {
            get => categoryImages;
            set
            {
                categoryImages = value;
                RaisePropertyChanged(nameof(CategoryImages));
            }
        }
    }
}
